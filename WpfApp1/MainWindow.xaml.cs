using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        private ObservableCollection<Contact> _contacts;
        private string _filePath= $@"C:\Users\jacob\Documents\Nackademin\ProgrammeringC#\ovningar\Uppgift2_TEST\addressbook.json";

        enum MenuState
        {
            startup,
            add,
            edit
        }

        public MainWindow()
        {
            InitializeComponent();
            
            MenuPresenter(MenuState.add);
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

        private void lv_Contacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MenuPresenter(MenuState.edit);
            var contact = (Contact)lv_Contacts.SelectedItems[0]!;

            tb_FirstName.Text = contact.FirstName;
            tb_LastName.Text = contact.LastName;
            tb_Email.Text = contact.EmailAddress;
            tb_PhoneNumber.Text = contact.PhoneNumber;
            tb_StreetAddress.Text = contact.StreetAddress;
            tb_PostalCode.Text = contact.PostalCode;
            tb_City.Text = contact.City;
        }

        private void MenuPresenter(MenuState state)
        {
            switch (state)
            {
                case MenuState.startup:
                    break;

                case MenuState.add:
                    bt_Add.Visibility = Visibility.Visible;
                    bt_Edit.Visibility = Visibility.Hidden;
                    break;

                case MenuState.edit:
                    bt_Add.Visibility = Visibility.Hidden;
                    bt_Edit.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var contact = (Contact)button!.DataContext;

            var index = _contacts.IndexOf(contact);


        }
    }
}
