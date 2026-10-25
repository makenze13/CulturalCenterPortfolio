using CulturalCenterWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stimulsoft.Report;

namespace CulturalCenterWpf.Reports
{
    internal class CreateTicket
    {
        public void ShowCheck(Ticket ticket, Seat seat)
        {
            var report = new Stimulsoft.Report.StiReport();

            //загружаем отчет из папки
            report.Load("Reports/TicketReport.mrt");

            //добавляем данные в словарь 
            report.RegBusinessObject("Seat", seat); //данные о месте
            report.RegBusinessObject("Ticket", ticket); //данные о билете
            report.RegBusinessObject("Event", seat.SeatEventDTO); //данные о мероприятии

            report.Dictionary.SynchronizeBusinessObjects(); //синхронизируем бизнес объекты с данными в словаре

            report.ShowWithWpf(); //вывод отчета
        }
    }
}
