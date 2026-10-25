using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System.Windows;
using System.Windows.Input;

namespace CulturalCenterWpf.Views.Manager
{
    /// <summary>
    /// Логика взаимодействия для MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        private User currentUser;
        private Employee currentEmployee;
        private readonly EmployeeApiService _employeeApiService = new();

        public MainManagerWindow(User user)
        {
            InitializeComponent();

            currentUser = user; //устанавливаем данные текущего пользователя в локальную переменную

            SetEmployeeData(currentUser); //устанавливаем данные сотрудуника на форме
        }

        //скрываем все границы на форме
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBorder();
        }

        //показываем таблицу мероприятий
        private async void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBorder(events: Visibility.Visible);
            
            try
            {
                await SetEventsDataAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        public async Task SetEventsDataAsync()
        {
            var eventApiService = new EventApiService();
            EventsDataGrid.ItemsSource = await eventApiService.GetEventsAsync();
        }

        //показываем таблицу организаторов
        private async void OrganizersButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBorder(organizer: Visibility.Visible);

            try
            {
                await SetOrganizersDataAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        public async Task SetOrganizersDataAsync()
        {
            var organizerApiService = new OrganizerApiService();
            OrganizersDataGrid.ItemsSource = await organizerApiService.GetOrganizerAsync();
        }

        //показываем таблицу сотрудников
        private async void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBorder(employee: Visibility.Visible);

            try
            {
                await SetEmployeesDataAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //устанавливаем данные сотрудника как источник данных в таблице
        public async Task SetEmployeesDataAsync()
        {
            var employeeApiService = new EmployeeApiService();
            EmployeesDataGrid.ItemsSource = await employeeApiService.GetEmployeesAsync();
        }

        //вернуться к окну авторизации
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            Close();
        }

        //открываем личный кабинет пользователя заполняя данные о нем
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBorder(profile: Visibility.Visible);
            loginUserTextBlock.Text = currentUser.Login;
            nameUserTextBlock.Text = currentEmployee.FirstName + " " + currentEmployee.LastName;
        }

        //показываем только нажные границы, другие скрываем
        private void ShowBorder(Visibility profile = Visibility.Collapsed, Visibility events = Visibility.Collapsed, 
                                Visibility organizer = Visibility.Collapsed, Visibility employee = Visibility.Collapsed)
        {
            profileBorder.Visibility = profile;
            eventBorder.Visibility = events;
            organizerBorder.Visibility = organizer;
            employeeBorder.Visibility = employee;
        }

        //открытие формы для добавления нового мероприятия
        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            var addEventWindow = new AddNewEventManagerWindow(currentUser.IdUser, this, add:Visibility.Visible);
            addEventWindow.ShowDialog();
        }

        //выбор определенного мероприятия из таблицы мероприятия
        private void EventsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EventsDataGrid.SelectedItem is EventDTO selectedEvent)
            {
                var addEventWindow = new AddNewEventManagerWindow(currentUser.IdUser, this, delete:Visibility.Visible, save:Visibility.Visible, @event:selectedEvent);
                addEventWindow.ShowDialog();
            }
        }

        //открытие формы для добавления нового организатора
        private void AddOrganizerButton_Click(object sender, RoutedEventArgs e)
        {
            var addOrganizerWindow = new AddNewOrganizerManagerWindow(currentUser.IdUser, this, add:Visibility.Visible);
            addOrganizerWindow.ShowDialog();
        }

        //выбор определенного организатора из таблицы организаторы
        private void OrganizersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrganizersDataGrid.SelectedItem is Organizer selectedOrganizer)
            {
                var addOrganizerWindow = new AddNewOrganizerManagerWindow(currentUser.IdUser, this, delete:Visibility.Visible, save:Visibility.Visible, organizer:selectedOrganizer);
                addOrganizerWindow.ShowDialog();
            }
        }

        //выбор определенного сотрудника из таблицы сотрудники
        private void EmployeesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is Employee selectedEmployee)
            {
                var addEmployeeWindow = new AddNewEmployeeManagerWindow(currentUser.IdUser, this, delete: Visibility.Visible, save: Visibility.Visible, employee: selectedEmployee, createUser: Visibility.Collapsed);
                addEmployeeWindow.ShowDialog();
            }
        }

        //открытие формы для добавления нового сотрудника
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddNewEmployeeManagerWindow(currentUser.IdUser, this, add:Visibility.Visible);
            addEmployeeWindow.ShowDialog();
        }

        //устанавливаем данные сотрудника на форме пользователя
        private async void SetEmployeeData(User user)
        {
            currentEmployee = await _employeeApiService.GetEmployeeUser(user);

            currentUserNameTextBlock.Text = currentEmployee.FirstName + " " + currentEmployee.LastName;
        }

        //нажатие на кнопку изменения данных в личном кабинете пользователя
        private void ChangeDataButton_Click(object sender, RoutedEventArgs e)
        {
            var checkUserPasswordWindow = new CheckUserPasswordWindow(currentUser);
            checkUserPasswordWindow.ShowDialog();
        }
        
        //добавление ошибки в базу данных и вывод ее пользователю
        private async Task AddAndShowErrorAsync(Exception ex)
        {
            LogsErrorService logsErrorService = new();
            await logsErrorService.AddLogsError(currentUser.IdUser, ex.Message);

            LogsErrorService.ShowErrorMessage(ex.Message);
        }
    }
}
