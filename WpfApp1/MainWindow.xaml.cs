using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.Models;

namespace WpfApp1
{
    
    public partial class MainWindow : Window
    {
        private ObservableCollection<Contact> _contacts;
        private string _filePath= $@"C:\Users\jacob\Documents\Nackademin\ProgrammeringC#\ovningar\Uppgift2_TEST\addressbook.json";


        public MainWindow()
        {
            InitializeComponent();
            
            _contacts = new ObservableCollection<Contact>();
            RefreshList();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            var _contact = _contacts.Where(x => x.EmailAddress == tb_Email.Text).ToList();

            if (_contact.Count == 0)
            {
                if(tb_Email.Text == "")
                {
                    MessageBox.Show("Vänligen fyll i en mailadress");
                }
                else
                {
                    _contacts.Add(new Contact
                    {
                        FirstName = tb_FirstName.Text,
                        LastName = tb_LastName.Text,
                        EmailAddress = tb_Email.Text,
                        PhoneNumber = tb_PhoneNumber.Text,
                        StreetAddress = tb_StreetAddress.Text,
                        PostalCode = tb_PostalCode.Text,
                        City = tb_City.Text,
                    });
                }
            }
            else
            {
                MessageBox.Show("Vänligen fyll i kontakuppgifter");
            }

            RefreshList();
            ClearFields();
        }

        private void ClearFields()
        {
            tb_FirstName.Text = "";
            tb_LastName.Text = "";
            tb_Email.Text = "";
            tb_PhoneNumber.Text = "";
            tb_StreetAddress.Text = "";
            tb_PostalCode.Text = "";
            tb_City.Text = "";
        }

        private void RefreshList()
        {
            lv_Contacts.ItemsSource = _contacts.OrderBy(x => x.FirstName);
        }
    }
}
