using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3_2
{
    public class Person
    {
        public int PersID { get; set; }

        private string secName;
        public string SecName
        {
            get { return secName; }
            set
            {
                if (value.All(c => Char.IsLetter(c) || c == ' ' || c == '-')) secName = value;
                else throw new ArgumentException("В имени могут быть только буквы и знак (-)");
            }
        }

        private string workStatus;
        public string WorkStatus
        {
            get { return workStatus; }
            set
            {
               if(value.All(c => Char.IsLetter(c) || c == ' ' || c == '-')) workStatus = value;
                else throw new ArgumentException("Статус может содержать только буквы и знак (-)");
            }
        }

        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if (value.All(c => Char.IsLetter(c) || c == ' ' || c == '-')) city = value;
                else throw new ArgumentException("В названии города могут быть только буквы и знак (-)");
            }
        }

        private string street;
        public string Street
        {
            get { return street; }
            set
            {
                if (value.All(c => Char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c=='.' || c==',')) street = value;
                else throw new ArgumentException("В названии города могут быть только буквы, цифры и знаки (-, .)");
            }
        }

        private string houseNumber;
        public string HouseNumber
        {
            get { return houseNumber; }
            set
            {
                if (value.All(c => Char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '.' || c == ',')) houseNumber = value;
                else throw new ArgumentException("Номер дома может содержать только буквы, цифры и знаки (-, .)");
            }
        }

        private double salary = 1;
        public double Salary
        {
            get { return salary; }
            set
            {
                if (value > 0) salary = value;
                else throw new ArgumentException("Зряплата не может быть меньше нуля.");
            }
        }

        public Person() { }

        public string Passport()
        {
            string passport = String.Format(
                "***Personal ID № {0}***" +
                "\n Name: {1}" +
                "\n Salary: {2}" +
                "\n Status: {3}" +
                "\n City: {4}" +
                "\n Street: {5}" +
                "\n House: {6}" +
                "\n",
                PersID, SecName, Salary, WorkStatus, City, Street, HouseNumber);
            return passport;
        }
    }
}
