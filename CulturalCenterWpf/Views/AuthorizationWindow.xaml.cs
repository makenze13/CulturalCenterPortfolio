using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using CulturalCenterWpf.Views.Admin;
using CulturalCenterWpf.Views.Cashier;
using CulturalCenterWpf.Views.Manager;
using System.IO.Packaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CulturalCenterWpf
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        //нажатие на кнопку войти в систему
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string password, login = LoginTextBox.Text.Trim();
                SettingService settingService = new();
                AppSettings appSettings = await settingService.LoadJsonAsync();


                if (PasswordBox.Visibility == Visibility.Visible)
                    password = PasswordBox.Password.ToString().Trim();
                else
                    password = PasswordTextBox.Text.Trim();

                //проверка на пустые поля
                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(login))
                {
                    MessageBox.Show("Заполните поля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                    

                var userApiService = new UserApiService();

                //проверка корректности ввода данных пользователем
                var user = await userApiService.LoginAsync(login, password);
                
                if (login == appSettings.RootLogin && password == appSettings.RootPassword)
                {
                    var mainAdminWindow = new MainAdminWindow(new User());
                    mainAdminWindow.Show();
                    this.Close();
                }
                else if (user == null)
                    MessageBox.Show("Неверный логин или пароль!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    //открытие формы в соответствии с ролью пользователя
                    switch (user.Role)
                    {
                        case "Менеджер":
                            var mainManagerWindow = new MainManagerWindow(user);
                            mainManagerWindow.Show();
                            break;
                        case "Администратор":
                            var mainAdminWindow = new MainAdminWindow(user);
                            mainAdminWindow.Show();
                            break;
                        case "Кассир":
                            var mainCashierWindow = new MainCashierWindow(user);
                            mainCashierWindow.Show();
                            break;
                    }

                    //добавление действия пользователем в базу данных
                    LogsActionService logsActionService = new LogsActionService();
                    await logsActionService.AddLogsAction(user.IdUser, "Вход", "Система");

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogsErrorService logsErrorService = new LogsErrorService();
                await logsErrorService.AddLogsError(null, ex.Message); //добавляем ошибку в базу данных

                LogsErrorService.ShowErrorMessage(ex.Message); //вывод ошибки пользователю
            }
        }

        //показать/скрыть пароль
        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                PasswordTextBox.Text = PasswordBox.Password;

                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;

                PasswordTextBox.CaretIndex = PasswordTextBox.Text.Length;
            }
            else
            {
                PasswordBox.Password = PasswordTextBox.Text;

                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;

            }
        }
    }
}