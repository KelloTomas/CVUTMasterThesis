using DataLayer;
using DataLayer.Data;
using System;
using System.Linq;
using System.Windows;

namespace AdminApp
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class TerminalServicePage : System.Windows.Controls.Page
    {
        private readonly DatabaseLayer _db;
        private readonly Window _owner;
        public TerminalServicePage(Window owner, DatabaseLayer db)
        {
            _owner = owner;
            _db = db;
            InitializeComponent();
            ReloadData();
        }

        private void ReloadData()
        {
            var e = Enum.GetValues(typeof(SubApp)).Cast<SubApp>().ToList();
            functionsCombo.ItemsSource = e;
        }


        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            var selected = (SubApp)functionsCombo.SelectedItem;
            TerminalService terminalService = new TerminalService(IpAddress.Text);
            try
            {
            switch (selected)
            {
                case SubApp.Putty:
                case SubApp.VNC:
                case SubApp.WinSCP:
                case SubApp.FileZilla:
                    if (string.IsNullOrWhiteSpace(FieldData.Text))
                    {
                        FieldData.Text = "D:\\instalacky\\portable programy";
                        break;
                    }
                    terminalService.Start(selected, FieldData.Text);
                    break;
                case SubApp.NastavIP:
                    terminalService.SetIP(FieldData.Text);
                    break;
                case SubApp.NastavHostName:
                    terminalService.SetHostName(FieldData.Text);
                    break;
                case SubApp.ZistiVerziuApp:
                    MessageBox.Show(terminalService.GetVersion());
                    break;
                case SubApp.ZistiTeplotu:
                    MessageBox.Show(terminalService.GetTemperature());
                    break;
                case SubApp.StiahniLogy:
                    if (string.IsNullOrWhiteSpace(FieldData.Text))
                    {
                        FieldData.Text = "C://";
                        break;
                    }
                    terminalService.GetLogs(FieldData.Text);
                    break;
                case SubApp.AktualizujApp:
                    if (string.IsNullOrWhiteSpace(FieldData.Text))
                    {
                        FieldData.Text = "C://QTTcpServer";
                        break;
                    }
                    terminalService.SetHostName(FieldData.Text);
                    break;
            }
            }
            catch (Exception)
            {
                //pri restarte terminalu vychody chybu, preto ju mozem ignorovat
            }
        }
    }
}
