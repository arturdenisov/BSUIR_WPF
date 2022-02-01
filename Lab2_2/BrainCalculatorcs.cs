using System;


namespace Lab2_2
{
    class BrainCalculator
    {
        //две переменные бинарной операции
        double Operand1;
        public double Op1
        { set { Operand1 = value; } get { return Operand1; } }

        double Operand2;
        public double Op2
        { set { Operand2 = value; } get { return Operand2; } }

        //переменная для хранения знака операции
        char OperationMark;
        public char OpMark
        { set { OperationMark = value; } get { return OperationMark; } }

        public BrainCalculator()
        {
            Operand1 = 0;
            Operand2 = 0;
            OperationMark = ' ';
        }

        public string Fraction(string str)
        {
            double x = double.Parse(str);
            x = 1 / x;
            return Convert.ToString(x);
        }

        public string Percent(string str)
        {
            double x = double.Parse(str);
            x = x * 100;
            return Convert.ToString(x);
        }

        public string SQRT(string str)
        {
            double x = double.Parse(str);
            x = Math.Pow(x, 0.5);
            return Convert.ToString(x);
        }

        public string SignChange(string str)
        {
            double x = double.Parse(str);
            x = x * (-1);
            return Convert.ToString(x);
        }

        public string Trigonometry(double x, string Units, string func)
        {
            string result = "alarm";
            if (Units == "градусы")
            {
                switch (func)
                {
                    //перевод градусов в радианы  - мсдн
                    case "bSin": x = x * Math.PI / 180; result = string.Format("{0}", Math.Sin(x)); break;
                    case "bCos": x = x * Math.PI / 180; result = string.Format("{0}", Math.Cos(x)); break;
                    case "bTg":
                        if (((Math.IEEERemainder(x, 90)) == 0) && ((Math.IEEERemainder(x, 180)) != 0)) throw new Exception("Тангенса данного угла не существует!");
                        x = x * Math.PI / 180; result = string.Format("{0}", Math.Tan(x)); break;
                    case "bCtg":
                        if ((Math.IEEERemainder(x, 180)) == 0) throw new Exception("Котангенса данного угла не существует!");
                        x = x * Math.PI / 180; result = string.Format("{0}", 1 / Math.Tan(x)); break;
                    //перевод результата ( в радианах) в градусы 
                    case "bAsin": result = string.Format("{0}", Math.Asin(x) * (180 / Math.PI)); break;
                    case "bAcos": result = string.Format("{0}", Math.Acos(x) * (180 / Math.PI)); break;
                    case "bAtg": result = string.Format("{0}", Math.Atan(x) * (180 / Math.PI)); break;
                    case "bActg": result = string.Format("{0}", (1 / Math.Atan(x)) * (180 / Math.PI)); break;
                }
            }

            if (Units == "радианы")
            {
                switch (func)
                {
                    case "bSin": result = string.Format("{0}", Math.Sin(x)); break;
                    case "bCos": result = string.Format("{0}", Math.Cos(x)); break;
                    case "bTg":
                        if (((Math.IEEERemainder(x, 90)) == 0) && ((Math.IEEERemainder(x, 180)) != 0)) throw new Exception("Тангенса данного угла не существует!");
                        result = string.Format("{0}", Math.Tan(x)); break;
                    case "bCtg":
                        if ((Math.IEEERemainder(x, 180)) == 0) throw new Exception("Котангенса данного угла не существует!");
                        result = string.Format("{0}", 1 / Math.Tan(x)); break;
                    case "bAsin": result = string.Format("{0}", Math.Asin(x)); break;
                    case "bAcos": result = string.Format("{0}", Math.Acos(x)); break;
                    case "bAtg": result = string.Format("{0}", Math.Atan(x)); break;
                    case "bActg": result = string.Format("{0}", 1 / Math.Atan(x)); break;
                }
            }

            if (Units == "грады")
            {
                switch (func)
                {
                    // перевод градов в радианы 
                    case "bSin": x = x * (Math.PI / 200); result = string.Format("{0}", Math.Sin(x)); break;
                    case "bCos": x = x * (Math.PI / 200); result = string.Format("{0}", Math.Cos(x)); break;
                    case "bTg":
                        if (((Math.IEEERemainder(x, 90)) == 0) && ((Math.IEEERemainder(x, 180)) != 0)) throw new Exception("Тангенса данного угла не существует!");
                        x = x * (Math.PI / 200); result = string.Format("{0}", Math.Tan(x)); break;
                    case "bCtg":
                        if ((Math.IEEERemainder(x, 180)) == 0) throw new Exception("Котангенса данного угла не существует!");
                        x = x * (Math.PI / 200); result = string.Format("{0}", 1 / Math.Tan(x)); break;
                    // перевод результата (в радианах) в грады 
                    case "bAsin": result = string.Format("{0}", Math.Asin(x) * 200 / Math.PI); break;
                    case "bAcos": result = string.Format("{0}", Math.Acos(x) * 200 / Math.PI); break;
                    case "bAtg": result = string.Format("{0}", Math.Atan(x) * 200 / Math.PI); break;
                    case "bActg": result = string.Format("{0}", (1 / Math.Atan(x)) * 200 / Math.PI); break;
                }
            }
            return result;
        }

        public string GetResult()
        {
            double result = 0;
            switch (OpMark)
            {
                case '*': { result = Op1 * Op2; break; }
                case '/': { result = Op1 / Op2; break; }
                case '+': { result = Op1 + Op2; break; }
                case '-': { result = Op1 - Op2; break; }
            }
            //сохраняем результат как 1ый операнд для дальнейших расчетов
            //обнуляем знак (используется для контроля при вводе цифр)
            this.Op1 = result;
            this.OpMark = ' ';

            return Convert.ToString(result);
        }


    }
}