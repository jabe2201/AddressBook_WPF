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
            lv_Contacts.ItemsSource = _contacts;
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            var contact = _contacts.Where(x => x.EmailAddress == tb_)
        }
    }
}
