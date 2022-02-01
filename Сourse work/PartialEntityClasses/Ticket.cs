using System;
using System.Xml.Linq;

namespace DenisovArt_Kurs
{
    public partial class Ticket
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Ticket()
        //{
        //    this.OrderedServices = new HashSet<OrderedService>();
        //}

        //public int TicketID { get; set; }
        //public int DocTimetableID { get; set; }
        //public System.DateTime VisitDateTime { get; set; }
        //public Nullable<bool> Reserved { get; set; }

        //public virtual DocTimeTable DocTimeTable { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<OrderedService> OrderedServices { get; set; }

        public string DoctorSpeciality
        {
            get
            {
                return DocTimeTable.Doctor.DocSpeciality.SpecialityTitle;
            }
        }
        public string DoctorName
        {
            get
            {
                return DocTimeTable.Doctor.SecondName;
            }
        }
        public DateTime VisitDateTime_normalized
        {
            get
            {
                //string result = String.Format("Дата: {0:00}.{1:00}.{2:0000} Время: {3:00}ч.{4:00}м.",
                //    VisitDateTime.Day, VisitDateTime.Month, VisitDateTime.Year,
                //    VisitDateTime.Hour, VisitDateTime.Minute);
                return VisitDateTime;
            }
        }


        public XElement TickPassportXml()
        {
            XElement xClientPassport =
                    new XElement("Ticket",
                        new XElement("TicketID", TicketID.ToString()),
                        new XElement("DocTimetableID", DocTimetableID.ToString()),
                        new XElement("VisitDateTime", VisitDateTime.ToString()),
                        new XElement("Reserved", Reserved.ToString())
                            );
            return xClientPassport;
        }
    }
}
