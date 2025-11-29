using System.Windows;

namespace halozatvezerles
{
    public partial class InputWindow : Window
    {
        // Ezekben tároljuk az adatokat
        public string RoomName { get; private set; }
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public string AdminUser { get; private set; }
        public string AdminPass { get; private set; }

        public InputWindow()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Ellenőrzés: minden mező ki van-e töltve
            if (string.IsNullOrWhiteSpace(RoomBox.Text) ||
                string.IsNullOrWhiteSpace(RowsBox.Text) ||
                string.IsNullOrWhiteSpace(ColsBox.Text) ||
                string.IsNullOrWhiteSpace(AdminUserBox.Text) ||
                string.IsNullOrWhiteSpace(AdminPassBox.Password))
            {
                MessageBox.Show("Kérlek tölts ki minden mezőt!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Adatok mentése változókba
            RoomName = RoomBox.Text;
            AdminUser = AdminUserBox.Text;
            AdminPass = AdminPassBox.Password;

            // Sorok/oszlopok számának átalakítása
            if (!int.TryParse(RowsBox.Text, out int rows) || rows <= 0)
            {
                MessageBox.Show("A sorok száma érvénytelen!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Rows = rows;

            if (!int.TryParse(ColsBox.Text, out int cols) || cols <= 0)
            {
                MessageBox.Show("Az oszlopok száma érvénytelen!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Cols = cols;

            // Ha minden rendben, nyissuk meg a MainWindow-t
            MainWindow main = new MainWindow(RoomName, Rows, Cols, AdminUser, AdminPass);
            main.Show();

            // Bezárjuk az InputWindow-t
            this.Close();
        }
    }
}
