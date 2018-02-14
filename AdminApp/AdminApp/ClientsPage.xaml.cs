using DataLayer;
using DataLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminApp
{
	/// <summary>
	/// Interaction logic for UsersPage.xaml
	/// </summary>
	public partial class ClientsPage : Page
	{
		private readonly DatabaseLayer db;
		private List<Client> clients;
		public ClientsPage(DatabaseLayer db)
		{
			InitializeComponent();
			this.db = db;
			LoadData();
		}

		private void LoadData()
		{
			clients = db.GetClients().ToList();
			clientsDataGrid.ItemsSource = clients;
		}

		private void CancelBtnClick(object sender, RoutedEventArgs e)
		{
			LoadData();
		}

		private void SaveBtnClick(object sender, RoutedEventArgs e)
		{
			foreach(var client in clients)
			{
				db.UpdateClient(client);
			}
		}
	}
}
