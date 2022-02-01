using System;
using System.Linq;
using System.Xml.Linq;

namespace DenisovArt_Kurs
{
    public partial class Client 
    {
        public int MyClientID
        {
            get { return ClientID; }
            set
            {
                if (value < 0) { throw new ArgumentException("ID должен быть больше 0 и состоять только из цифр!"); }
                else
                {
                    ClientID = value;
                }           
            }
        }
        
        public string MYSecondName
        { 
            get { return SecondName; }
            set
                {
                if (
                    (value).Any(char.IsDigit)|| 
                    (value.Any(char.IsPunctuation)))
                    throw new ArgumentException("В фамилии не могут использоваться цифры и знаки пунктуации!");
                else SecondName = value;
            }
        }

        public string MyFitstName
        {
            get { return FitstName; }
            set
            {
                if (
                    (value as String).Any(char.IsDigit) || (value.Any(char.IsPunctuation))
                    ) throw new ArgumentException("В фамилии не могут использоваться цифры и знаки пунктуации!");
                else FitstName = value;
            }
        }

        public string MyThirdName
        {
            get { return ThirdName; }
            set
            {
                if (
                    (value as String).Any(char.IsDigit) || (value.Any(char.IsPunctuation))
                    )
                    throw new ArgumentException("В фамилии не могут использоваться цифры и знаки пунктуации!");
                else ThirdName = value;
            }
        }

        public string MyPassportNumber
        {
            get { return PassportNumber; }
            set
            {
                if ((value.Length > 9) || ((value as String).Any(char.IsSymbol))
                    || (value.Any(char.IsPunctuation)) || (value.Any(char.IsWhiteSpace))) throw new ArgumentException("Не более 9 знаков (букв и цифр)!");
                else PassportNumber = value;
                  
            }
        }

        public string BirthdayDate_woutTime
        {
            get
            {
                return String.Format("{0:00}.{1:00}.{2:00}", BirthdayDate.Value.Year, BirthdayDate.Value.Month, BirthdayDate.Value.Day);
            }
            set {; }
        }

        public XElement ClientPassportXml()
        {
            if (String.IsNullOrEmpty(FitstName.ToString()) == true) FitstName = string.Empty;
            if (String.IsNullOrEmpty(ThirdName.ToString()) == true) ThirdName = string.Empty;
            if (String.IsNullOrEmpty(PassportNumber.ToString()) == true) PassportNumber = string.Empty;
            if (String.IsNullOrEmpty(ContactInformation.ToString()) == true) ContactInformation = string.Empty;

            XElement xClientPassport =
                    new XElement("Client",
                        new XElement("ClientID", ClientID.ToString()),
                        new XElement("RegisterDate", RegisterDate.ToString()),
                        new XElement("FirstName", FitstName.ToString()),
                        new XElement("SecondName", SecondName.ToString()),
                        new XElement("ThirdName", ThirdName.ToString()),
                        new XElement("PassportNumber", PassportNumber.ToString()),
                        new XElement("ContactInformation", ContactInformation.ToString()),
                        new XElement("BirthdayDate", BirthdayDate.ToString())
                            );
            return xClientPassport;
        }
    }
}