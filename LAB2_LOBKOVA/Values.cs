using System;
using System.ComponentModel;

namespace LAB2_LOBKOVA
{
    public class Values : IDataErrorInfo
    {
        private int n;

        public Values()
        {
            // Установка начальных значений по умолчанию
            XStart = "0";
            XStop = "0";
            Step = "1";
            N = 6;
        }

        public string XStart { get; set; }
        public string XStop { get; set; }
        public string Step { get; set; }

        public int N
        {
            get { return n; }
            set { n = value; }
        }

        public int TermsCount { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(XStart):
                        if (!double.TryParse(XStart, out _))
                        {
                            return "Пожалуйста, введите числовое значение.";
                        }
                        break;

                    case nameof(XStop):
                        if (!double.TryParse(XStop, out _))
                        {
                            return "Пожалуйста, введите числовое значение.";
                        }
                        break;

                    case nameof(Step):
                        if (!double.TryParse(Step, out _))
                        {
                            return "Пожалуйста, введите числовое значение.";
                        }
                        break;

                    case nameof(N):
                        if (N <= 5)
                        {
                            return "Значение должно быть больше 5";
                        }
                        break;
                }
                return null;
            }
        }
    }
}
