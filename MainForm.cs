using Microsoft.Data.SqlClient;

namespace KaktuszGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Load += OnMainFormLoad;
            keresesTextBox.TextChanged += OnKeresesTextBoxTextChanged;
            mainDataGridView.CellClick += OnMainDataGridViewCellClick;
        }

        private void DataGrodViewFeltoltes(string nevresz = null)
        {
            mainDataGridView.Rows.Clear();
            using SqlConnection conn = new("SERVER=(localdb)\\MSSQLLocalDB;DATABASE=novenytermesztes;");
            conn.Open();
            var reader = new SqlCommand(
                "SELECT * FROM kaktuszok " +
                $"WHERE nev LIKE '%{nevresz}%';",
                conn).ExecuteReader();
            while (reader.Read())
            {
                string nehezseg = reader.GetInt32(3) switch
                {
                    1 => "Könnyû",
                    2 => "Mérsékelt",
                    3 => "Nehéz",
                    _ => throw new Exception("error!"),
                };
                mainDataGridView.Rows.Add(
                    reader[0],
                    reader[1],
                    reader[2],
                    nehezseg,
                    $"{reader[4]} ml/h",
                    $"{reader[5]} lm");
            }
        }

        private void OnKeresesTextBoxTextChanged(object? sender, EventArgs e)
            => DataGrodViewFeltoltes(keresesTextBox.Text);

        private void OnMainDataGridViewCellClick(object? sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(
                caption: "TUDOMÁNYOS NÉV:",
                text: $"{mainDataGridView[2, e.RowIndex].Value}");
        }

        private void OnMainFormLoad(object? sender, EventArgs e)
            => DataGrodViewFeltoltes();
    }
}