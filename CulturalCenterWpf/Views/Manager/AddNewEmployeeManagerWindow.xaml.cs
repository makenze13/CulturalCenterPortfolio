using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System.Windows;

namespace CulturalCenterWpf.Views.Manager
{
    /// <summary>
    /// Логика взаимодействия для AddNewEventManagerWindow.xaml
    /// </summary>
    public partial class AddNewEmployeeManagerWindow : Window
    {
        private readonly EmployeeApiService _employeeApiService = new();
        private readonly int _idEmployee;
        private readonly int _idUser;
        private readonly MainManagerWindow _mainManagerWindow;

        //показываем необходимые кнопки, получаем id пользователя системы, заполняем поля данными в случае если был выбран определенный сотрудник
        public AddNewEmployeeManagerWindow(int idUSer, MainManagerWindow mainManagerWindow, Visibility delete = Visibility.Collapsed, Visibility add = Visibility.Collapsed, 
                                            Visibility save = Visibility.Collapsed, Employee employee = null, Visibility createUser = Visibility.Visible)
        {
            InitializeComponent();

            DeleteButton.Visibility = delete;
            AddButton.Visibility = add;
            SaveButton.Visibility = save;
            CreateUserCheckBox.Visibility = createUser;

            _idUser = idUSer;
            _mainManagerWindow = mainManagerWindow;

            if (employee != null)
            {
                SetData(employee);
                _idEmployee = employee.IdEmployee;
            }   
        }

        //добавление нового пользователя
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employee = GetData();

                if (employee == null)
                    return;

                if (CreateUserCheckBox.IsChecked == true)
                {
                    await _employeeApiService.PostEmployeeWithUserAsync(employee, RoleComboBox.Text);
                }
                else
                    await _employeeApiService.PostEmployeeAsync(employee);

                await AddActionAsync("Добавление нового сотрудника", "Таблица сотрудники");

                //очищаем поля на форме
                SetData(new Employee());

                //обновляем таблицу в окне менеджера
                await _mainManagerWindow.SetEmployeesDataAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //закрываем форму
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //показать панель по нажатию не CheckBox
        private void CreateUserCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RolePanel.Visibility = Visibility.Visible;
        }

        //скрыть панель при повторном нажатии на CheckBox
        private void CreateUserCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RolePanel.Visibility = Visibility.Collapsed;
        }

        //удаление сотрудника
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _employeeApiService.DeleteEmployeeAsync(_idEmployee);

                await AddActionAsync($"Удаление сотрудника id:{_idEmployee}", "Таблица пользователи");

                await CloseAndUpdateAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //сохранить изменения сотрудника
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employee = GetData();

                if (employee == null)
                    return;

                await _employeeApiService.PutEmployeeAsync(_idEmployee, employee);

                await AddActionAsync($"Изменены данные сотрудника id:{_idEmployee}", "Таблица пользователи");

                await CloseAndUpdateAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //получение данныех сотрудника с формы
        private Employee GetData()
        {
            var phone = PhoneTextBox.Text.Trim();
            var lastName = LastNameTextBox.Text.Trim();
            var firstName = FirstNameTextBox.Text.Trim();
            var middleName = MiddleNameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) || DateBirthDatePicker.SelectedDate == null)
            {
                LogsErrorService.ShowWarningMessage("Заполните пустые поля!", "Пустые поля");
                return null;
            }
            else if (phone.Length > 11 || !phone.All(char.IsDigit))
            {
                LogsErrorService.ShowWarningMessage("Введен некорректный номер телефона", "Некорректный ввод данных");
                return null;
            }
            else if (CreateUserCheckBox.IsChecked == true && RoleComboBox.SelectedIndex == -1)
            {
                LogsErrorService.ShowWarningMessage("Выберите роль сотрудника!", "Пустые поля");
                return null;
            }

            var employee = new Employee
            {
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName,
                NumberPhone = phone,
                DateBirth = DateOnly.FromDateTime(DateBirthDatePicker.SelectedDate.Value)
            };

            return employee;
        }

        //устанавливаем данные на форму
        private void SetData(Employee employee)
        {
            LastNameTextBox.Text = employee.LastName;
            FirstNameTextBox.Text = employee.FirstName;
            MiddleNameTextBox.Text = employee.MiddleName;
            PhoneTextBox.Text = employee.NumberPhone;
            DateBirthDatePicker.SelectedDate = employee.DateBirth.ToDateTime(TimeOnly.MinValue);
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
            await _mainManagerWindow.SetEmployeesDataAsync();
        }
    }
}
