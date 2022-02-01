using System;
using System.Linq;
using System.Xml.Linq;

namespace DenisovArt_Kurs
{
    public partial class CatalogOfService
    {
        public int MyServiceID
        {
            get { return ServiceID; }
            set
            {
                if (value < 0) throw new ArgumentException("ID должен быть больше 0 и состоять только из цифр!");
                else
                {
                    ServiceID = value;
                }
            }
        }

        public decimal MyPrice
        {
            get { return Price; }
            set
            {
                if (value < 0) throw new ArgumentException("Цена не может быть равно (или меньше) 0!");
                else
                {
                    Price = value;
                }
            }
        }

        public string _NeedTicket
        {
            get
            {
                if (NeedTicket == true) return "Необходим талон";
                return "Без талона";
            }
        }

        public string DocSpeciality
        {
            get
            {
                foreach (DocSpeciality spec in DocSpecialities)
                {
                    return spec.SpecialityTitle;
                }
                return "0";
            }
        }
        public Int32 DocSpecialityID
        {
            get
            {
                foreach (DocSpeciality spec in DocSpecialities)
                {
                    return spec.SpecialityID;
                }
                return 0;
            }
        }

        public XElement ServicePassportXml()
        {
            XElement xServicePassport =
                    new XElement("Service",
                        new XElement("ServiceID", ServiceID.ToString()),
                        new XElement("ServiceTitle", ServiceTitle.ToString()),
                        new XElement("Price", Price.ToString()),
                        new XElement("NeedTicket", NeedTicket.ToString()),
                        new XElement("DocSpecialityID", DocSpecialityID.ToString())
                            );
            return xServicePassport;
        }

    }
}
