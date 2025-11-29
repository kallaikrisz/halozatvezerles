using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace halozatvezerles
{
    public partial class MainWindow : Window
    {
        public string RoomName { get; }
        public int Rows { get; }
        public int Cols { get; }
        public string AdminUser { get; }
        public string AdminPass { get; }

        private List<RadioButton> allRadioButtons = new List<RadioButton>();

        public MainWindow(string roomName, int rows, int cols, string adminUser, string adminPass)
        {
            InitializeComponent();

            RoomName = roomName;
            Rows = rows;
            Cols = cols;
            AdminUser = adminUser;
            AdminPass = adminPass;

            BuildGridUI();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BuildGridUI()
        {
            MachinesGrid.Rows = Rows;
            MachinesGrid.Columns = Cols;

            for (int i = 1; i <= Rows; i++)
            {
                for (int j = 1; j <= Cols; j++)
                {
                    string gepNev = $"{RoomName}-{i}-{j}";

                    var border = new Border
                    {
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(5),
                        Padding = new Thickness(10)
                    };

                    var cellPanel = new StackPanel();

                    cellPanel.Children.Add(new TextBlock
                    {
                        Text = gepNev,
                        FontWeight = FontWeights.Bold,
                        FontSize = 18,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 10, 0, 10)
                    });

                    var radioPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 10, 0, 10)
                    };

                    foreach (var state in new[] { "Nincs változás", "Internet ki", "Internet be" })
                    {
                        var rb = new RadioButton
                        {
                            Content = state,
                            GroupName = gepNev,
                            Margin = new Thickness(5, 0, 5, 0),
                            FontSize = 16
                        };
                        radioPanel.Children.Add(rb);
                        allRadioButtons.Add(rb);
                    }

                    cellPanel.Children.Add(radioPanel);
                    border.Child = cellPanel;
                    MachinesGrid.Children.Add(border);
                }
            }
        }

        private void AllOffButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var rb in allRadioButtons)
            {
                if (rb.Content.ToString() == "Internet ki")
                {
                    rb.IsChecked = true;
                }
            }
        }

        private void AllOnButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var rb in allRadioButtons)
            {
                if (rb.Content.ToString() == "Internet be")
                {
                    rb.IsChecked = true;
                }
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            var beallitasok = new List<(string GepNev, string Mod)>();

            foreach (var rb in allRadioButtons)
            {
                if (rb.IsChecked == true)
                {
                    beallitasok.Add((rb.GroupName, rb.Content.ToString()));
                }
            }

            string json = System.Text.Json.JsonSerializer.Serialize(beallitasok);

            string path = "beallitasok.json";

            System.IO.File.WriteAllText(path, json, System.Text.Encoding.UTF8);

            string scriptPath = "halozat.ps1";
            string arguments = $"-File \"{scriptPath}\" -User \"{AdminUser}\" -Pass \"{AdminPass}\" -Config \"{path}\"";

            System.Diagnostics.Process.Start("powershell.exe", arguments);
        }
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            string scriptPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.ps1");
            string arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"";

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = arguments,
                UseShellExecute = true // <-- fontos, így külön ablakban fut
            };

            System.Diagnostics.Process.Start(psi);
        }






    }
}
