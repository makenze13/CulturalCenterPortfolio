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

namespace CulturalCenterWpf.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для MainCashierWindow.xaml
    /// </summary>
    public partial class MainCashierWindow : Window
    {
        public readonly User _currentUser;
        private Employee currentEmployee;
        private readonly EventApiService _eventApiService = new();
        private readonly TypeEventApiService _typeEventApiService = new();
        private readonly EmployeeApiService _employeeApiService = new();

        public EventDTO eventDTODetail;

        public MainCashierWindow(User user)
        {
            InitializeComponent();

            _currentUser = user; //устанавливаем текущего пользователя
        }

        //открываем список мероприятий
        private async void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            //показываем события, загружаем их в таблицу и сбрасываем источник данных в схеме зала
            ShowBorder(events: Visibility.Visible);
            await LoadDataEvent();
            SectorC1.ItemsSource = null;
            SectorC2.ItemsSource = null;
            SectorC3.ItemsSource = null;
        }

        //возврат к форме авторизации
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            Close();
        }

        //показать профиль пользователя
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBorder(profile: Visibility.Visible);
            loginUserTextBlock.Text = _currentUser.Login;
            nameUserTextBlock.Text = currentEmployee.FirstName + " " + currentEmployee.LastName;
        }

        //показываем определенную границу скрывая другие
        private void ShowBorder(Visibility events = Visibility.Collapsed, Visibility profile = Visibility.Collapsed, Visibility eventDetails = Visibility.Collapsed)
        {
            eventsBorder.Visibility = events;
            eventDetailsBorder.Visibility = eventDetails;
            profileBorder.Visibility = profile;
        }

        //создаем места в схеме зала
        public async Task CreateSeats(int idEvent)
        {
            try
            {
                SectorC1.ItemsSource = await _eventApiService.CreateSectorAsync(idEvent, "C1", eventDTODetail);
                SectorC2.ItemsSource = await _eventApiService.CreateSectorAsync(idEvent, "C2", eventDTODetail);
                SectorC3.ItemsSource = await _eventApiService.CreateSectorAsync(idEvent, "C3", eventDTODetail);
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }
        
        //загружаем меропрятия
        private async Task LoadDataEvent()
        {
            try
            {
                //устанавливаем параметры фильтрации и сортировки
                var search = SearchTextBox.Text.Trim();
                var filterType = EventTypeComboBox.Text.Trim();
                var dateTime = EventDatePicker.SelectedDate;
                var sortBy = SortComboBox.Text.Trim() switch
                {
                    "По дате" => "date",
                    "По названию" => "name",
                    "По цене" => "price",
                    _ => null
                };
                var sortDirection = SortDirectionComboBox.Text.Trim() == "По возрастанию" ? "asc" : "desc";
                bool isRelevant = isRelevantCheckBox.IsChecked == true;

                //получаем мероприятия с учетом необходимых параметров
                List<EventDTO> @event = await _eventApiService.GetEventsWithParameters(search, filterType, sortBy, dateTime, sortDirection, isRelevant);

                EventsItemsControl.ItemsSource = @event; //устанавливаем в качестве источника данных полученные мероприятия
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //загружаем данные для формы
        private async Task LoadDataForm()
        {
            try
            {
                List<TypesEvent> typesEvents = await _typeEventApiService.GetTypesEventsAsync(); //получаем типы мероприятий
                EventTypeComboBox.ItemsSource = typesEvents; //добавляем типы мероприятий в комбо бокс
                await LoadDataEvent(); //вызываем метод загрузки мероприятий
            }
            catch(Exception ex)
            {
                LogsErrorService logsErrorService = new();
                await logsErrorService.AddLogsError(_currentUser.IdUser, ex.Message);

                LogsErrorService.ShowErrorMessage(ex.Message);
            }
        }

        //выбираем конкретное мероприятия
        private async void ChooseEventBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //получаем выбранное мероприятие и передаем его в модель
                var border = (Border)sender;
                var selectedEvent = (EventDTO)border.DataContext;
                eventDTODetail = selectedEvent;

                eventDetailsBorder.Visibility = Visibility.Visible;
                await CreateSeats(selectedEvent.IdEvent); //создаем места в схеме зала

                eventDetailsBorder.DataContext = selectedEvent; //передаем данные в контекст
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }
        
        //применить фильтры
        private async void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDataEvent(); //обновляем вывод мероприятий
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //сброс фильтров
        private async void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchTextBox.Clear();
                EventDatePicker.SelectedDate = null;
                EventTypeComboBox.SelectedIndex = -1;
                SortComboBox.SelectedIndex = -1;
                SortDirectionComboBox.SelectedIndex = -1;
                isRelevantCheckBox.IsChecked = true;

                await LoadDataEvent();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }   
        }

        //установить данные пользователя на форме
        private async Task SetEmployeeData(User user)
        {
            currentEmployee = await _employeeApiService.GetEmployeeUser(user);

            currentUserNameTextBlock.Text = currentEmployee.FirstName + " " + currentEmployee.LastName;
        }

        //открыть форму изменения данных пользователя
        private void ChangeDataButton_Click(object sender, RoutedEventArgs e)
        {
            var checkUserPasswordWindow = new CheckUserPasswordWindow(_currentUser);
            checkUserPasswordWindow.ShowDialog();
        }

        //добавление и вывод ошибки
        private async Task AddAndShowErrorAsync(Exception ex)
        {
            LogsErrorService logsErrorService = new();
            await logsErrorService.AddLogsError(_currentUser.IdUser, ex.Message); //добавляем ошибку в базу данных

            LogsErrorService.ShowErrorMessage(ex.Message); //выводим ошибку пользователю
        }

        //загрузка окна
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadDataForm(); //загружаем данные для формы
                await SetEmployeeData(_currentUser); //устанавливаем данные пользователя на форме
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }
    }
}
