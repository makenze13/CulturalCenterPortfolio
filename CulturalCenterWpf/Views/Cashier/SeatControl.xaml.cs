using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CulturalCenterWpf.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для SeatControl.xaml
    /// </summary>
    public partial class SeatControl : UserControl
    {
        private MainCashierWindow? _window;

        public SeatControl()
        {
            InitializeComponent();
        }

        //нажатие на кнопку места в схеме зала
        private async void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_window == null)
                return;

            try
            {
                //проверяем что контекст является классом Seat
                if (DataContext is not Seat seat)
                    return;

                //получаем текущее окно в котором была нажата кнопка


                //если билет не продан открываем окно продажи билета
                if (seat.Status == SeatStatus.Free)
                {
                    var saleSeatCashierWindow = new SaleSeatCashierWindow(seat, seat.SeatEventDTO, _window);
                    saleSeatCashierWindow.Show();
                }
                //если билет продан открываем окно информации о проданном билете
                else
                {
                    var soldTicketCashierWindow = new SoldTicketCashierWindow(seat, _window._currentUser);
                    soldTicketCashierWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                await AddAndShowError(ex);
            }
        }

        //проверка мест и изменение цвета кнопки в соответствии со статусом места
        private async Task CheckSeats(Seat seat)
        {
            try
            {
                if (seat != null && seat.Status == SeatStatus.Free)
                    SeatButton.Background = new SolidColorBrush(Color.FromRgb(232, 162, 58));
                else if (seat != null && seat.Status == SeatStatus.Sold)
                    SeatButton.Background = new SolidColorBrush(Color.FromRgb(101, 55, 18));
            }
            catch (Exception ex)
            {
                await AddAndShowError(ex);
            }
        }

        //вызываем метод для проверки мест при изменени контекста
        private async void SeatButton_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            await CheckSeats(DataContext as Seat);
        }

        //добавление и вывод ошибки
        private async Task AddAndShowError(Exception ex)
        {
            //проверяем что окно было получено
            if (_window == null)
                return;

            LogsErrorService logsErrorService = new();
            await logsErrorService.AddLogsError(_window._currentUser.IdUser, ex.Message); //добавляем ошибку в базу данных

            LogsErrorService.ShowErrorMessage(ex.Message); //выводим ошибку пользователю
        }

        //получаем окно в локальную переменную
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _window = Window.GetWindow(this) as MainCashierWindow;
        }
    }
}
