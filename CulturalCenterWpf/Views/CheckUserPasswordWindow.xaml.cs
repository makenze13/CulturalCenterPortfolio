using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System.Windows;

namespace CulturalCenterWpf.Views
{
    /// <summary>
    /// Логика взаимодействия для CheckUserPasswordWindow.xaml
    /// </summary>
    public partial class CheckUserPasswordWindow : Window
    {
        private readonly User _user;

        public CheckUserPasswordWindow(User user)
        {
            InitializeComponent();

            //устанавливаем текущего пользователя
            _user = user;
        }

        private async void ConfirmPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //проверка чтобы поле с паролем не было пустым
                if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
                {
                    LogsErrorService.ShowWarningMessage("Введите пароль", "Пустые поля");
                    return;
                }

                //создаем запрос авторизации с логином и паролем
                var loginRequest = new LoginRequest
                {
                    Login = _user.Login,
                    Password = PasswordTextBox.Text.Trim()
                };

                var userApiService = new UserApiService();
                bool check = await userApiService.CheckPasswordAsync(loginRequest); //проверяем корректность ввода пароля пользователем

                if (check)
                {
                    //закрываем текущее окно и открываем окно для изменения данных пользователя
                    Close();
                    var changeUserDataWindow = new ChangeUserDataWindow(_user);
                    changeUserDataWindow.Show();
                }
                else
                    MessageBox.Show("Пароль неверный", "Подтверждение пароля", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                LogsErrorService logsErrorService = new();
                await logsErrorService.AddLogsError(_user.IdUser, ex.Message); //добавляем ошибку в базу данных

                LogsErrorService.ShowErrorMessage(ex.Message); //выводим ошибку пользователю
            }
        }
    }
}
