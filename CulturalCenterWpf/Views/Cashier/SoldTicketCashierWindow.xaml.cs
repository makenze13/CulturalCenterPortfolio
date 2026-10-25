using CulturalCenterWpf.Models;
using CulturalCenterWpf.Reports;
using CulturalCenterWpf.Services;
using System.Windows;

namespace CulturalCenterWpf.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для SoldTicketCashierWindow.xaml
    /// </summary>
    public partial class SoldTicketCashierWindow : Window
    {
        private readonly TicketApiService _ticketApiService = new();
        private readonly Seat _seat;
        private readonly User _user;

        public SoldTicketCashierWindow(Seat seat, User user)
        {
            InitializeComponent();

            //устанавливаем контекст и передаем данные в локальные переменные
            DataContext = seat;
            _seat = seat;
            _user = user;
        }

        //печать чека
        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var idEvent = _seat.SeatEventDTO.IdEvent; //получаем id мероприятия
                var ticket = await _ticketApiService.GetSeatTicketAsync(idEvent, _seat); //получаем билет в соответствии с местом и id мероприятия

                CreateTicket createTicket = new();

                createTicket.ShowCheck(ticket, _seat); //печатаем чек

                LogsActionService logsActionService = new();
                await logsActionService.AddLogsAction(_user.IdUser, "Печать чека", $"Напечатан чек по билету id:{ticket.IdTicket}"); //добавляем действие пользователя в базу данных
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
