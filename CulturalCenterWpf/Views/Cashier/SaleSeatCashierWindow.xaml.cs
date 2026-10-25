using CulturalCenterWpf.Models;
using CulturalCenterWpf.Reports;
using CulturalCenterWpf.Services;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows;

namespace CulturalCenterWpf.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для SaleSeatCashierWindow.xaml
    /// </summary>
    public partial class SaleSeatCashierWindow : Window
    {
        private readonly Seat _seat;
        private readonly EventDTO _eventDTO;
        private readonly TicketApiService _ticketApiService = new TicketApiService();

        private readonly MainCashierWindow _mainCashierWindow;

        public SaleSeatCashierWindow(Seat seat, EventDTO seatEventDTO, MainCashierWindow mainCashierWindow)
        {

            InitializeComponent();

            //устанавливаем в локальные переменные переданные на форму данные
            DataContext = seat; 
            _seat = seat; 
            _eventDTO = seatEventDTO; 
            _mainCashierWindow = mainCashierWindow;
        }

        private async void PayButton_Click(object sender, RoutedEventArgs e)
        {
            //получаем подтверждение пользователем
            var result = MessageBox.Show("Подтвердить оплату?", "Подтверждение", MessageBoxButton.YesNo);
        
            if(result == MessageBoxResult.Yes)
            {
                try
                {
                    //создаем билет заполняя информацию
                    Ticket ticket = new Ticket
                    {
                        UnicTicketNumber = Guid.NewGuid().ToString("N")[..10].ToUpper(),
                        IdEvent = _eventDTO.IdEvent,
                        IdUser = null,
                        Seat = _seat.Sector + "-" + _seat.Row + "-" + _seat.Number,
                        TicketPrice = _seat.Price,
                        SellDateTime = DateTime.Now
                    };

                    //создаем билет
                    var createdTicket = await _ticketApiService.PostTicketAsync(ticket);

                    //проверяем что билет был создан
                    if (createdTicket == null)
                    {
                        MessageBox.Show("Не удалось создать билет!");
                        return;
                    }


                    CreateTicket createTicket = new CreateTicket();

                    //выводим чек пользователю
                    createTicket.ShowCheck(createdTicket, _seat);

                    LogsActionService logsActionService = new();
                    await logsActionService.AddLogsAction(_mainCashierWindow._currentUser.IdUser, "Продажа билета", $"Продан билет id:{ticket.IdTicket}"); //добавляем действие пользователя в базу данных

                    Close();

                    //обновляем схему зала
                    await _mainCashierWindow.CreateSeats(_eventDTO.IdEvent);
                }
                catch (Exception ex)
                {
                    LogsErrorService logsErrorService = new();
                    await logsErrorService.AddLogsError(_mainCashierWindow._currentUser.IdUser, ex.Message);

                    LogsErrorService.ShowErrorMessage(ex.Message);
                }
            }
            else
            {
                return; //возвращаемся если пользователь не подтвердил оплату
            }
        }
    }
}
