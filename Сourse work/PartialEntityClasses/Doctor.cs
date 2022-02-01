using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace DenisovArt_Kurs
{

    
    public partial class Doctor
    {
        public int MyDocID
        {
            get { return DocID; }
            set
            {
                if (value < 0) throw new ArgumentException("ID должен быть больше 0 и состоять только из цифр!");
                else
                {
                    DocID = value;
                }
            }
        }

        public string MySecondName
        {
            get { return SecondName; }
            set
            {
                if ((value as String).Any(char.IsDigit) || (value.Any(char.IsPunctuation))) throw new ArgumentException("В фамилии не могут использоваться цифры и знаки пунктуации!");
                else
                {
                    SecondName = value;
                }
            }
        }

        public string MyFirstName
        {
            get { return FirstName; }
            set
            {
                if ((value as String).Any(char.IsDigit) || (value.Any(char.IsPunctuation))) throw new ArgumentException("В фамилии не могут использоваться цифры и знаки пунктуации!");
                else
                {
                    FirstName = value;
                }
            }
        }

        public string MyThirdName
        {
            get { return ThirdName; }
            set
            {
                if ((value as String).Any(char.IsDigit) || (value.Any(char.IsPunctuation))) throw new ArgumentException("В фамилии не могут использоваться цифры и знаки пунктуации!");
                else
                {
                    ThirdName = value;
                }
            }
        }

        public string SpecialityTitle
        {
            get
            {
                return this.DocSpeciality.SpecialityTitle;
            }
        }

        public int MyRoom
        {
            get
            {
                foreach (DocTimeTable doc in DocTimeTables)
                {

                    return doc.Room;
                }
                return 0;
            }
            set
            {
                if (value < 0) throw new ArgumentException("Номер кабинета должен быть больше 0 и состоять только из цифр!");
                else
                {

                    if (this.DocTimeTables.Count == 0 ) this.DocTimeTables.Add(new DocTimeTable());
                    foreach (DocTimeTable doc in DocTimeTables)
                    {

                        doc.Room = value;
                        return;
                    }
                }
            }
        }

        public bool WorkShiftFirst
        {
            get
            {
                foreach (DocTimeTable doc in DocTimeTables)
                {

                    return doc.WorkingShiftFirst;
                }
                return true;
            }
        }

        public Int32 DocTimeTableID
        {
            get
            {
                foreach (DocTimeTable doc in DocTimeTables)
                {

                    return doc.DocTimeTableID;
                }
                return 0;
            }

        }

        public XElement DocPassportXml()
        {
            if (String.IsNullOrEmpty(FirstName.ToString()) == true) FirstName = string.Empty;
            if (String.IsNullOrEmpty(ThirdName.ToString()) == true) ThirdName = string.Empty;

            List<Ticket> cTick = new List<Ticket>();
            foreach (Ticket t in (new ProjectEntitiesContext()).Tickets)
            {
                cTick.Add(t);
            }

            XElement xClientPassport =
                        new XElement("Doctor",
                            new XElement("DocID", DocID.ToString()),
                            new XElement("FirstName", FirstName),
                            new XElement("SecondName", SecondName),
                            new XElement("ThirdName", ThirdName),
                            new XElement("SpecialityID", SpecialityID.ToString()));
            return xClientPassport;
        }

    }
}
