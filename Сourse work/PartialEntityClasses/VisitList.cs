using System;
using System.Xml.Linq;

namespace DenisovArt_Kurs
{
    public partial class VisitList
    {
        public int MyVisitListID
        {
            get { return VisitListID; }
            set
            {
                if (value < 0) throw new ArgumentException("ID должен быть больше 0 и состоять только из цифр! ");
                else
                {
                    VisitListID = value;
                }
            }
        }

        public int MyClientID
        {
            get { return ClientID; }
            set
            {
                if (value < 0) throw new ArgumentException("ID должен быть больше 0 и состоять только из цифр!");
                else
                {
                    ClientID = value;
                }
            }
        }

        public string ClintName { get { return Client.SecondName; } }

        public XElement VisitListPassportXml()
        {
            XElement xVisitListPassport =
                    new XElement("VisitList",
                        new XElement("VisitListID", VisitListID.ToString()),
                        new XElement("ClientID", ClientID.ToString()),
                        new XElement("VisitDate", VisitDate.ToString()),
                        new XElement("PriceTotal", PriceTotal.ToString())
                            );
            return xVisitListPassport;
        }
    }
}
