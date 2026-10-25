using CulturalCenterWpf.Services;
using System.Windows;
using System.Net.Mail;
using CulturalCenterWpf.Models;

namespace CulturalCenterWpf.Views.Manager
{
    /// <summary>
    /// Логика взаимодействия для AddNewOrganizerManagerWindow.xaml
    /// </summary>
    public partial class AddNewOrganizerManagerWindow : Window
    {
        private readonly OrganizerApiService organizerApiService = new();
        private readonly int _idOrganizer;
        private readonly int _idUser;
        private readonly MainManagerWindow _mainManagerWindow;

        //показываем необходимые кнопки, получаем id пользователя системы, заполняем поля данными в случае если был выбран определенный организатор
        public AddNewOrganizerManagerWindow(int idUser, MainManagerWindow mainManagerWindow, Visibility delete = Visibility.Collapsed, Visibility add = Visibility.Collapsed, Visibility save = Visibility.Collapsed, Organizer organizer = null)
        {
            InitializeComponent();

            DeleteButton.Visibility = delete;
            AddButton.Visibility = add;
            SaveButton.Visibility = save;

            _idUser = idUser;
            _mainManagerWindow = mainManagerWindow;

            if (organizer != null)
            {
                SetData(organizer);
                _idOrganizer = organizer.IdOrganizer;
            }
        }

        //закрытие формы
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //добавление нового организатора
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Organizer organizer = GetData();

            try
            {
                if (organizer == null)
                    return;

                await organizerApiService.PostOrganizerAsync(organizer);

                await AddActionAsync("Добавление нового организатора", "Таблица организаторы");

                //очищаем поля на форме
                SetData(new Organizer());

                //обновляем таблицу в окне менеджера
                await _mainManagerWindow.SetOrganizersDataAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
            
            
        }

        //удаление организатора
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await organizerApiService.DeleteOrganizerAsync(_idOrganizer);

                await AddActionAsync($"Удаление организатора id:{_idOrganizer}", "Таблица организаторы");

                await CloseAndUpdateAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //сохранить изменения организатора
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Organizer organizer = GetData();

            try
            {
                if (organizer == null)
                    return;

                await organizerApiService.PutOrganizerAsync(_idOrganizer, organizer);

                await AddActionAsync($"Изменены данные организатора id:{_idOrganizer}", "Таблица организаторы");

                await CloseAndUpdateAsync();
            }
            catch (Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //получение данных с формы
        private Organizer GetData()
        {
            var phone = PhoneTextBox.Text.Trim();
            var organizerName = NameTextBox.Text.Trim();
            var organizerDescription = DescriptionTextBox.Text.Trim();
            var organizerEmail = EmailTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(organizerName) || string.IsNullOrWhiteSpace(organizerEmail))
            {
                LogsErrorService.ShowWarningMessage("Заполните пустые поля!", "Пустые поля");
                return null;
            }
            else if (phone.Length > 11 || !phone.All(char.IsDigit))
            {
                LogsErrorService.ShowWarningMessage("Введен некорректный номер телефона", "Некорректный ввод данных");
                return null;
            }
            else if (!IsValidEmail(organizerEmail))
            {
                LogsErrorService.ShowWarningMessage("Введен некорректный адрес электронной почты", "Некорректный ввод данных");
                return null;
            }

            var organizer = new Organizer
            {
                OrganizerName = NameTextBox.Text.Trim(),
                OrganizerNumberPhone = phone,
                OrganizerEmail = EmailTextBox.Text.Trim(),
                OrganizerDescription = DescriptionTextBox.Text.Trim()
            };

            return organizer;
        }

        //проверка корректного ввода почты
        private bool IsValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        //устанавливаем данные на форму
        private void SetData(Organizer organizer)
        {
            NameTextBox.Text = organizer.OrganizerName;
            PhoneTextBox.Text = organizer.OrganizerNumberPhone;
            EmailTextBox.Text = organizer.OrganizerEmail;
            DescriptionTextBox.Text = organizer.OrganizerDescription;
        }

        //добавление действия в логи действий в базу данных
        private async Task AddActionAsync(string action, string logsObject)
        {
            LogsActionService logsActionService = new();
            await logsActionService.AddLogsAction(_idUser, action, logsObject);
        }

        //добавление ошибки в базу данных и вывод ее пользователю
        private async Task AddAndShowErrorAsync(Exception ex)
        {
            LogsErrorService logsErrorService = new();
            await logsErrorService.AddLogsError(_idUser, ex.Message);

            LogsErrorService.ShowErrorMessage(ex.Message);
        }

        //закрываем форму и обновляем таблицу сотрудники
        private async Task CloseAndUpdateAsync()
        {
            Close();
            await _mainManagerWindow.SetOrganizersDataAsync();
        }
    }
}
