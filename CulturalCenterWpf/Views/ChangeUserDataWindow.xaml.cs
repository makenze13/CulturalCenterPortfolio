using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CulturalCenterWpf.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangeUserDataWindow.xaml
    /// </summary>
    public partial class ChangeUserDataWindow : Window
    {
        private readonly User _user;

        public ChangeUserDataWindow(User userData)
        {
            InitializeComponent();

            //устанавливаю текущего пользователя и логин в текстовое поле
            _user = userData;
            LoginTextBox.Text = _user.Login;
        }

        //закрытие окна по кнопке
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        //нажатие на кнопку сохранения
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = LoginTextBox.Text.Trim();

                //проверка чтобы поле с логином не было пустым
                if (string.IsNullOrWhiteSpace(login))
                {
                    LogsErrorService.ShowWarningMessage("Заполните логин", "Пустые поля");
                    return;
                }

                //создаем модель с обновленными данными пользователя
                var newUser = new AccountChangeModel
                {
                    IdUser = _user.IdUser,
                    Login = login,
                    Role = _user.Role,
                    IdEmployee = _user.IdEmployee
                };

                string? password = null;

                if (PasswordBox.Visibility == Visibility.Visible)
                    password = PasswordBox.Password;
                else
                    password = PasswordTextBox.Text.Trim();

                //не изменяем пароль если пользователь не ввел новый
                if (!string.IsNullOrEmpty(password))
                    newUser.NewPassword = password;

                var userApiService = new UserApiService();

                //обновляем данные пользователя
                await userApiService.ChangeDataUserAsync(newUser);

                //добавляем действия пользователя в базу данных
                LogsActionService logsActionService = new();
                await logsActionService.AddLogsAction(newUser.IdUser, "Изменение данных пользователя", $"Учётная запись пользователя {_user.Login}");
            }
            catch(Exception ex)
            {
                //добавляем ошибку в базу данных
                LogsErrorService logsErrorService = new();
                await logsErrorService.AddLogsError(_user.IdUser, ex.Message);

                LogsErrorService.ShowErrorMessage(ex.Message); //выводим ошибку пользователю
            }
        }
    }
}
