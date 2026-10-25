using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System.Runtime.InteropServices;
using System.Windows;

namespace CulturalCenterWpf.Views.Admin
{
    /// <summary>
    /// Логика взаимодействия для AddNewUserAdminWindow.xaml
    /// </summary>
    public partial class AddNewUserAdminWindow : Window
    {
        private int idUser;
        private readonly UserApiService _userApiService = new UserApiService();
        private readonly User _userEmployee;
        private readonly MainAdminWindow _mainAdminWindow;
        private readonly User _recievedUser;

        public AddNewUserAdminWindow(Visibility delete = Visibility.Collapsed, Visibility add = Visibility.Collapsed, Visibility save = Visibility.Collapsed, 
                                    Visibility reset = Visibility.Collapsed, User? user = null, User? userEmployee = null, MainAdminWindow? mainAdminWindow = null)
        {
            InitializeComponent();

            //показываем кнопки в соответствии с запросом (добавить нового или открыть форму с данынми уже существующего пользователя)
            DeleteButton.Visibility = delete;
            AddButton.Visibility = add;
            SaveButton.Visibility = save;
            ResetPasswordButton.Visibility = reset;

            //получаем пользователя текущего сотрудника для обработки действий и основное окно для обновления таблицы
            _userEmployee = userEmployee;
            _mainAdminWindow = mainAdminWindow;

            if (user != null)
            {
                SetData(user);
                idUser = user.IdUser;
                _recievedUser = user;
            }
        }

        //закрыть форму
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //добавить нового пользователя
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = await CheckUserInSystemAsync();

                if (user == null)
                    return;

                await _userApiService.PostUserAsync(user); //добавление нового пользователя

                await AddActionAsync("Добавление нового пользователя", "Таблица пользователи");

                SetData(new User());
                PasswordTextBox.Text = "";

                await _mainAdminWindow.GetUsersAsync();
            }
            catch (Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //удаление пользователя
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _userApiService.DeleteUserAsync(idUser); //удаление выбранного из таблицы пользователя

                await AddActionAsync($"Удаление пользователя id:{idUser}", "Таблица пользователи");

                await UpdateAndCloseAsync();
            }
            catch (Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //сохранение изменений
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = await CheckUserInSystemAsync(); //проверка что у пользователя уникальный логин

                if (user == null)
                    return;

                //создаем модель для изменения пользователя
                var accountChangeModel = new AccountChangeModel
                {
                    IdUser = idUser,
                    IdEmployee = user.IdEmployee,
                    Login = user.Login,
                    Role = user.Role
                };

                //проверяем поле пароля, если поле пустое, пароль остается прежним, иначе устанавливаем новый пароль
                if (!string.IsNullOrEmpty(user.Password))
                    accountChangeModel.NewPassword = user.Password;

                await _userApiService.ChangeAccountAsync(accountChangeModel); //заменяем измененные данные пользователя и меняем пароль если тот был заменен

                await AddActionAsync($"Изменены данные пользователя id:{idUser}", "Таблица пользователи");

                await UpdateAndCloseAsync();
            }
            catch (Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //получение данных из полей формы в объект User
        private User GetData()
        {
            var user = new User
            {
                Login = LoginTextBox.Text.Trim(),
                Role = RoleComboBox.Text.Trim(),
                Password = PasswordTextBox.Text.Trim(),
                IdEmployee = int.Parse(EmployeeTextBox.Text.Trim())
            };

            return user;
        }

        //установить данные в форму из объекта User
        private void SetData(User user)
        {
            LoginTextBox.Text = user.Login;
            RoleComboBox.Text = user.Role;
            EmployeeTextBox.Text = user.IdEmployee.ToString();
        }

        //добавление действия в логи действий в базу данных
        private async Task AddActionAsync(string action, string logsObject)
        {
            LogsActionService logsActionService = new();
            await logsActionService.AddLogsAction(_userEmployee.IdUser, action, logsObject);
        }

        //добавление ошибки в базу данных и вывод ее пользователю
        private async Task AddAndShowErrorAsync(Exception ex)
        {
            LogsErrorService logsErrorService = new();
            await logsErrorService.AddLogsError(_userEmployee.IdUser, ex.Message);

            LogsErrorService.ShowErrorMessage(ex.Message);
        }

        //обновляем таблицу с пользователями и закрываем форму
        private async Task UpdateAndCloseAsync()
        {
            await _mainAdminWindow.GetUsersAsync();

            Close();
        }

        //проверяем есть ли в системе пользователь с таким логином
        private async Task<User?> CheckUserInSystemAsync()
        {
            var user = GetData();

            var userExists = await _userApiService.GetUserCheckLogin(user.Login, idUser); //проверяем есть ли пользователь с таким же логином

            //проверка что логин уникальный
            if (userExists)
            {
                MessageBox.Show($"Пользователь с логином {user.Login} есть в системе!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            return user;
        }

        private async void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _userApiService.ResetPasswordUserAsync(idUser); //сброс пароля пользователя на стандартный пароль "1234"

                await AddActionAsync($"Сброшен пароль пользователя id:{idUser}", "Таблица пользователи");

                await UpdateAndCloseAsync();
            }
            catch (Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }
    }
}
