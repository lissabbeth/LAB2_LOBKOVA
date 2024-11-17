using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace LAB2_LOBKOVA_2
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Employee> employees;

        public MainWindow()
        {
            InitializeComponent();
            employees = new ObservableCollection<Employee>();
            EmployeeListBox.ItemsSource = employees;
        }

        private void ValidateTextInput(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string input = textBox.Text;
                Regex textRegex = new Regex("^[a-zA-Zа-яА-Я\\s]+$");

                if (string.IsNullOrWhiteSpace(input) || !textRegex.IsMatch(input))
                {
                    textBox.Background = Brushes.LightCoral;
                    textBox.ToolTip = "Поле должно содержать только буквы.";
                }
                else
                {
                    textBox.Background = Brushes.White;
                    textBox.ToolTip = null;
                }
            }
        }

        private void ValidateNumericInput(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string input = textBox.Text;
                if (!double.TryParse(input, out _) || (textBox.Name == "HouseTextBox" && !int.TryParse(input, out _)))
                {
                    textBox.Background = Brushes.LightCoral;
                    textBox.ToolTip = "Поле должно содержать только числовое значение.";
                }
                else
                {
                    textBox.Background = Brushes.White;
                    textBox.ToolTip = null;
                }
            }
        }

        private void ValidateComboBoxTextInput(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string input = comboBox.Text;
                Regex textRegex = new Regex("^[a-zA-Zа-яА-Я\\s]+$");

                if (string.IsNullOrWhiteSpace(input) || !textRegex.IsMatch(input))
                {
                    comboBox.Background = Brushes.LightCoral;
                    comboBox.ToolTip = "Поле должно содержать только буквы.";
                }
                else
                {
                    comboBox.Background = Brushes.White;
                    comboBox.ToolTip = null;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsFieldInvalid(SurnameTextBox) || IsFieldInvalid(SalaryTextBox) ||
                IsFieldInvalid(PositionComboBox) || IsFieldInvalid(CityComboBox) ||
                IsFieldInvalid(StreetComboBox) || IsFieldInvalid(HouseTextBox))
            {
                StatusTextBlock.Text = "Ошибка в одном или нескольких полях.";
                return;
            }

            try
            {
                string surname = SurnameTextBox.Text.Trim();
                if (!double.TryParse(SalaryTextBox.Text.Trim(), out double salary) || salary < 0)
                {
                    StatusTextBlock.Text = "Ошибка: некорректная зарплата.";
                    return;
                }
                string position = PositionComboBox.Text.Trim();
                string city = CityComboBox.Text.Trim();
                string street = StreetComboBox.Text.Trim();
                string house = HouseTextBox.Text.Trim();

                Employee employee = new Employee(surname, salary, position, city, street, house);
                employees.Add(employee);

                using (StreamWriter writer = new StreamWriter("employees.txt", true))
                {
                    writer.WriteLine(employee.ToFileFormat());
                }

                StatusTextBlock.Text = "Данные сохранены.";
                ClearFields();
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Ошибка при сохранении: {ex.Message}";
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Выберите файл для загрузки данных"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                if (File.Exists(filePath))
                {
                    employees.Clear();
                    try
                    {
                        foreach (string line in File.ReadAllLines(filePath))
                        {
                            var employee = Employee.FromFileFormat(line);
                            if (employee != null)
                            {
                                employees.Add(employee);
                            }
                        }
                        StatusTextBlock.Text = "Данные загружены.";
                    }
                    catch (Exception ex)
                    {
                        StatusTextBlock.Text = $"Ошибка при загрузке: {ex.Message}";
                    }
                }
                else
                {
                    StatusTextBlock.Text = "Файл не найден.";
                }
            }
        }

        private void ClearFields()
        {
            SurnameTextBox.Clear();
            SalaryTextBox.Clear();
            PositionComboBox.Text = string.Empty;
            CityComboBox.Text = string.Empty;
            StreetComboBox.Text = string.Empty;
            HouseTextBox.Clear();
        }

        private bool IsFieldInvalid(Control control)
        {
            return control.Background == Brushes.LightCoral;
        }
    }

    public class Employee
    {
        public string Surname { get; set; }
        public double Salary { get; set; }
        public string Position { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }

        public Employee(string surname, double salary, string position, string city, string street, string house)
        {
            Surname = surname;
            Salary = salary;
            Position = position;
            City = city;
            Street = street;
            House = house;
        }

        public string ToFileFormat()
        {
            return $"{Surname}|{Salary}|{Position}|{City}|{Street}|{House}";
        }

        public static Employee? FromFileFormat(string line)
        {
            try
            {
                var data = line.Split('|');
                if (data.Length != 6)
                {
                    return null;
                }

                var surname = data[0];
                var salary = double.Parse(data[1]);
                var position = data[2];
                var city = data[3];
                var street = data[4];
                var house = data[5];

                return new Employee(surname, salary, position, city, street, house);
            }
            catch
            {
                return null;
            }
        }

        public override string ToString()
        {
            return $"{Surname}, {Position}, {Salary} руб., {City}, {Street}, {House}";
        }
    }
}
