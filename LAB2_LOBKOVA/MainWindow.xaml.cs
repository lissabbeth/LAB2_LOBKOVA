using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace LAB2_LOBKOVA
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> results;
        private Values values;

        public MainWindow()
        {
            InitializeComponent();
            values = new Values();
            grid.DataContext = values;

            results = new ObservableCollection<string>();
            lResult.ItemsSource = results;  // Привязка коллекции результатов к ListBox
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double xStart = double.Parse(values.XStart);
                double xStop = double.Parse(values.XStop);
                double step = double.Parse(values.Step);
                int termsCount = values.N;

                // Выполняем вычисления для каждого x в диапазоне от xStart до xStop с шагом step
                for (double x = xStart; x <= xStop; x += step)
                {
                    double sResult = CalculateS(x, termsCount); // Вычисляем S(x)
                    double yResult = CalculateY(x);              // Вычисляем Y(x)

                    // Добавляем результат в коллекцию results
                    results.Add($"x = {x:F2}, S(x) = {sResult:F4}, Y(x) = {yResult:F4}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в расчете: {ex.Message}");
            }
        }

        // Метод для вычисления S(x)
        private double CalculateS(double x, int n)
        {
            double sum = 0;
            for (int k = 0; k <= n; k++)
            {
                double term = Math.Cos(k * Math.PI / 4) / Factorial(k) * Math.Pow(x, k);
                sum += term;
            }
            return sum;
        }

        // Метод для вычисления Y(x)
        private double CalculateY(double x)
        {
            return Math.Exp(x * Math.Cos(Math.PI / 4)) * Math.Cos(x * Math.Sin(Math.PI / 4));
        }

        // Метод для вычисления факториала
        private double Factorial(int n)
        {
            double result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
