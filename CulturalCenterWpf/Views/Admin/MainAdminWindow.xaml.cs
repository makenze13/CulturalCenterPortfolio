using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System.Windows;
using System.Windows.Input;

namespace CulturalCenterWpf.Views.Admin
{
    /// <summary>
    /// Логика взаимодействия для MainAdminWindow.xaml
    /// </summary>
    public partial class MainAdminWindow : Window
    {
        private readonly User? _currentUser;
        private Employee currentEmployee;
        private readonly EmployeeApiService _employeeApiService = new();
        
        public MainAdminWindow(User user)
        {
            InitializeComponent();

            _currentUser = user; //устанавливаем данные в локальную переменную для текущего пользователя

            SetEmployeeData(_currentUser); //устанавливаем данные сотрудника на форме
        }

        //получаем и показываем пользователей
        private async void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowBorder(user: Visibility.Visible); //показываем блок с пользователями

                await GetUsersAsync();
            }
            catch (Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //получаем пользователей и выводим их в таблицу
        public async Task GetUsersAsync()
        {
            UserApiService userApiService = new();
            UsersDataGrid.ItemsSource = await userApiService.GetUsersAsync(); //получаем пользователь в таблицу
        }

        //получаем и показываем логи действий
        private async void LogsActionsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowBorder(actionLogs: Visibility.Visible); // показываем блок с логами действий

                LogsActionApiService logsActionApiService = new();
                LogsActionDataGrid.ItemsSource = await logsActionApiService.GetLogsActionAsync(); //получаем логи действий в таблицу
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //показываем и получаем логи ошибок
        private async void LogsErrorsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowBorder(errorLogs: Visibility.Visible); //показываем блок с логами ошибок

                LogsErrorApiService logsErrorApiService = new();
                LogsErrorDataGrid.ItemsSource = await logsErrorApiService.GetLogsErrorAsync(); //получаем логи ошибок в таблицу
            }
            catch (Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            } 
        }

        //показываем и получаем настройки системы
        private async void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBorder(settings: Visibility.Visible);

            SettingService settingService = new();
            AppSettings appSettings = await settingService.LoadJsonAsync();

            ApiAddressTextBox.Text = appSettings.UrlApi;
            RootLoginTextBox.Text = appSettings.RootLogin;
            RootPasswordBox.Password = appSettings.RootPassword;
        }

        //скрыть все блоки
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBorder();
        }

        //вернутся к окну авторизации
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new();
            authorizationWindow.Show();
            this.Close();
        }

        //открыть профиль пользователя
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            //показываем блок личного кабинета пользователя и устанавливаем данные пользователя
            ShowBorder(profile: Visibility.Visible);
            loginUserTextBlock.Text = _currentUser.Login;
            nameUserTextBlock.Text = currentEmployee.FirstName + " " + currentEmployee.LastName;
        }

        //уникальный метод для открытия только нужного блока
        private void ShowBorder(Visibility user = Visibility.Collapsed, Visibility actionLogs = Visibility.Collapsed, 
                                Visibility errorLogs = Visibility.Collapsed, Visibility settings = Visibility.Collapsed, 
                                Visibility profile = Visibility.Collapsed)
        {
            usersBorder.Visibility = user;
            actionLogsBorder.Visibility = actionLogs;
            errorLogsBorder.Visibility = errorLogs;
            settingsBorder.Visibility = settings;
            profileBorder.Visibility = profile;
        }

        //устанавливаем данные сотрудника на форме
        private async void SetEmployeeData(User user)
        {
            try
            {
                if (user.IdUser != 0)
                {
                    currentEmployee = await _employeeApiService.GetEmployeeUser(user);

                    currentUserNameTextBlock.Text = currentEmployee.FirstName + " " + currentEmployee.LastName;
                }
            }
            catch(Exception ex)
            {
               await AddAndShowErrorAsync(ex);
            } 
        }

        //нажатие на изменить данные пользователя в личном кабинете
        private void ChangeDataButton_Click(object sender, RoutedEventArgs e)
        {
            var checkUserPasswordWindow = new CheckUserPasswordWindow(_currentUser);
            checkUserPasswordWindow.ShowDialog();
        }

        //выбрать определенного пользователя в системе
        private async void UsersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (UsersDataGrid.SelectedItem is User selectedUser)
                {
                    var addNewUserWindow = new AddNewUserAdminWindow(save: Visibility.Visible, delete: Visibility.Visible, reset:Visibility.Visible, 
                                                                    user: selectedUser, userEmployee: _currentUser, mainAdminWindow: this);
                    addNewUserWindow.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //добавить нового пользователя в систему
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var addNewUserWindow = new AddNewUserAdminWindow(add: Visibility.Visible, userEmployee: _currentUser, mainAdminWindow: this);
            addNewUserWindow.ShowDialog();
        }

        //добавление и вывод ошибок
        private async Task AddAndShowErrorAsync(Exception ex)
        {
            LogsErrorService logsErrorService = new LogsErrorService();
            await logsErrorService.AddLogsError(_currentUser.IdUser, ex.Message); //добавляем ошибку в базу данных

            LogsErrorService.ShowErrorMessage(ex.Message); //выводим ошибку пользователю
        }

        //сохранить настройки системы
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //создаем объект приложения загружая данные с формы
                AppSettings appSettings = new AppSettings
                {
                    UrlApi = ApiAddressTextBox.Text,
                    RootLogin = RootLoginTextBox.Text,
                    RootPassword = RootPasswordBox.Password
                };

                SettingService settingService = new();
                await settingService.SaveJsonAsync(appSettings); //сохраняем данные в файл
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
            
        }
    }
}
