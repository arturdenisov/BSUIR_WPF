using System;
using System.Xml.Linq;

namespace DenisovArt_Kurs
{
    public partial class OrderedService
    {

        public string ServiceTitle
        {
            get
            {
                return CatalogOfService.ServiceTitle;
            }
        }
        public Decimal Price
        {
            get
            {
                return CatalogOfService.Price;
            }
        }
        public bool NeedTicket
        {
            get
            {
                return CatalogOfService.NeedTicket;
            }
        }
        public string Speciality
        {
            get
            {
                return CatalogOfService.DocSpeciality;
            }
        }
        public int tickRoom
        {
            get
            {
                return Ticket.DocTimeTable.Room;
            }
        }
        public string tickDoctorName
        {
            get
            {
                return Ticket.DocTimeTable.Doctor.SecondName;
            }
        }
        public DateTime tickDateTime
        {
                get
                {
                    return Ticket.VisitDateTime_normalized;
                }
        }
        public string tickSaved
        {
            get
            {
                if(NeedTicket == false ) return "Не нужен";
                if (Ticket == null) return "Не заказан";
                else return "Забронирован";
            }
        }

        public XElement OrderedServPassportXml()
        {
            XElement xOrderedServPassport =
                    new XElement("OrderedService",
                        new XElement("ServiceID", ServiceID.ToString()),
                        new XElement("VisitListID", VisitListID.ToString()),
                        new XElement("TicketID", TicketID.ToString())
                            );
            return xOrderedServPassport;
        }
    }
}
