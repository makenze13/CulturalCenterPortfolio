using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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

namespace CulturalCenterWpf.Views.Manager
{
    /// <summary>
    /// Логика взаимодействия для AddNewEventManagerWindow.xaml
    /// </summary>
    public partial class AddNewEventManagerWindow : Window
    {        
        private readonly EventApiService _eventApiService = new();
        private readonly TypeEventApiService _typeEventApiService = new();
        private readonly OrganizerApiService _organizerApiService = new();
        private int idEvent;
        private readonly int _idUser;
        private readonly MainManagerWindow _mainManagerWindow;

        //показываем необходимые кнопки, получаем id пользователя системы, заполняем поля данными в случае если был выбран определенный сотрудник
        public AddNewEventManagerWindow(int idUser, MainManagerWindow mainManagerWindow, Visibility delete = Visibility.Collapsed, Visibility add = Visibility.Collapsed, Visibility save = Visibility.Collapsed, EventDTO @event = null)
        {
            InitializeComponent();

            DeleteButton.Visibility = delete;
            AddButton.Visibility = add;
            SaveButton.Visibility = save;

            _idUser = idUser;
            _mainManagerWindow = mainManagerWindow;

            LoadDataComboBox(@event);
        }

        //закрываем форму
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //добавление нового мероприятия
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Event @event = ReturnEvent();

            try
            {
                if (@event == null)
                    return;

                await _eventApiService.PostEventAsync(@event);

                await AddActionAsync("Добавление нового мероприятия", "Таблица мероприятия");

                //очищаем поля на форме
                SetData(new EventDTO());

                //обновляем таблицу в окне менеджера
                await _mainManagerWindow.SetEventsDataAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //сохранить изменения мероприятия
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Event @event = ReturnEvent();

            try
            {
                if (@event == null) 
                    return;

                await _eventApiService.PutEventAsync(idEvent, @event);

                await AddActionAsync($"Изменены данные мероприятия id:{idEvent}", "Таблица мероприятия");

                await CloseAndUpdateAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //удаление мероприятия
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _eventApiService.DeleteEventAsync(idEvent);

                await AddActionAsync($"Удаление мероприятия id:{idEvent}", "Таблица мероприятия");

                await CloseAndUpdateAsync();
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //вернуть объект мероприятия с формы
        private Event ReturnEvent()
        {
            var typeEvent = TypeComboBox.SelectedItem as TypesEvent;
            var selectedOrganizer = OrganizerComboBox.SelectedItem as Organizer;
            var date = EventDatePicker.SelectedDate.Value.Date.Add(TimeSpan.Parse(EventTimeComboBox.Text));
            var name = NameEventTextBox.Text.Trim();
            var description = DescriptionTextBox.Text.Trim();
            var time = EventTimeComboBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(time) ||
                EventDatePicker.SelectedDate == null || typeEvent == null || selectedOrganizer == null)
            {
                LogsErrorService.ShowWarningMessage("Заполните пустые поля!", "Пустые поля");
                return null;
            }

            var @event = new Event
            {
                EventName = NameEventTextBox.Text.Trim(),
                IdTypeEvent = typeEvent.IdTypeEvent,
                IdOrganizer = selectedOrganizer.IdOrganizer,
                EventDate = date,
                AvgTicketPrice = Decimal.Parse(PriceTextBox.Text.Trim()),
                EventDescription = DescriptionTextBox.Text.Trim()
            };

            return @event;
        }

        //устанавливаем в комбобоксы на форме типы мероприятий и организаторов
        private async void LoadDataComboBox(EventDTO @event)
        {
            TypeComboBox.ItemsSource = await _typeEventApiService.GetTypesEventsAsync();
            OrganizerComboBox.ItemsSource = await _organizerApiService.GetOrganizerAsync();

            if (@event != null)
            {
                SetData(@event);
                idEvent = @event.IdEvent;
            }
                
        }

        //устанавливаем данные на форме
        private void SetData(EventDTO? eventDto)
        {
            NameEventTextBox.Text = eventDto.EventName;
            TypeComboBox.Text = eventDto.TypeEvent;
            OrganizerComboBox.Text = eventDto.OrganizerName;
            EventDatePicker.SelectedDate = eventDto.EventDate.Date;
            EventTimeComboBox.Text = eventDto.EventDate.ToString("HH:mm") ?? null;
            PriceTextBox.Text = eventDto.AvgTicketPrice.ToString() ?? null;
            DescriptionTextBox.Text = eventDto.EventDescription;
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
            await _mainManagerWindow.SetEventsDataAsync();
        }

        //показать добавление нового типа мероприятия
        private void ShowAddTypeButton_Click(object sender, RoutedEventArgs e)
        {
            AddTypePanel.Visibility = Visibility.Visible;
        }

        //скрыть добавление нового типа мероприятия
        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            AddTypePanel.Visibility = Visibility.Collapsed;
        }

        //удаление типа мероприятия
        private async void DeleteTypeButton_Click(object sender, RoutedEventArgs e)
        {
            var typeEvent = TypeComboBox.SelectedItem as TypesEvent;

            if (typeEvent == null)
            {
                LogsErrorService.ShowWarningMessage("Выберите тип мероприятия для удаления!", "Пустые поля");
                return;
            }

            try
            {
                await _typeEventApiService.DeleteTypeEventAsync(typeEvent.IdTypeEvent);

                await AddActionAsync($"Удаление типа мероприятия id:{typeEvent.IdTypeEvent}", "Таблица типы мероприятий");

                TypeComboBox.SelectedItem = null;
                LoadDataComboBox(null);
            }
            catch(Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }

        //добавление нового типа мероприятия
        private async void AddTypeButton_Click(object sender, RoutedEventArgs e)
        {
            var nameType = NewTypeTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(nameType))
            {
                LogsErrorService.ShowWarningMessage("Заполните тип мероприятия!", "Пустые поля");
                return;
            }

            TypesEvent typesEvent = new TypesEvent { TypeEvent = nameType };

            try
            {
                await _typeEventApiService.PostTypeEventAsync(typesEvent);

                await AddActionAsync("Добавление нового типа мероприятия", "Таблица типы мероприятий");

                LoadDataComboBox(null);
                NewTypeTextBox.Text = null;
            }
            catch (Exception ex)
            {
                await AddAndShowErrorAsync(ex);
            }
        }
    }
}
