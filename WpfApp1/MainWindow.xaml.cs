using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        private ObservableCollection<Contact> _contacts; //Använder Observable Collection för att ListView ska fungera.
        private readonly IFileManager _fileManager;
        private string _filePath ="";
        private bool _isDeleted;
        /* Dessa fält finns här för att dessa variabler måste gå att nå igenom hela programmet i alla funktioner*/

        enum MenuState
        {
            startup,
            add,
            edit
            /* Jag har i min applikation tre olika visningslägen. Startup - när applikationen startar som låter användare skriva in en sökväg dit filen
             ska sparas. Add - Som är att lägga till en kontakt och innehåller kontaktformulär samt visar adressboken.
            Edit - Som skiftar Add-knappen till tre stycken andra knappar med olika funktioner. Jag har därför valt att skapa denna enum för att 
            lätt kunna hoppa emellan de olika visningslägena.*/
        }

        public MainWindow(IFileManager fileManager) 
        {
            InitializeComponent(); //Startar upp applikationen 
            _fileManager = fileManager; //DI av min Services - Filemanager
            MenuPresenter(MenuState.startup); //Visar min Startup-meny för att lägga till sökväg
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            var _contact = _contacts.Where(x => x.EmailAddress == tb_Email.Text).ToList();
            /* När användaren fyller i formuläret för att lägga till en kontakt så vill jag med .Where.ToList() kontrollera om kontakten redan finns.
             Jag har valt att matcha detta mot mailadress. Om .Where hittar en match så sparar .ToList() in det i variabeln _contact.*/

            if (_contact.Count == 0) //Om _contact är tom betyder det att .Where() inte har hittat en matchande email och ingen dubblett kommer att skapas.
            {
                if(tb_Email.Text == "") //Jag vill inte att användaren ska kunna lägga in en tom kontakt och har valt email för att styra detta.
                {
                    MessageBox.Show("Vänligen fyll i en Mailadress.");
                }
                else //Om kontakten inte är en dubblett, mailfältet inte är tomt, så läggs kontakten till i adressboken.
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
                MessageBox.Show("Finns redan en kontakt med denna Mailadress.");
            }

            RefreshList();
            ClearFields();
            _fileManager.Save(_filePath, JsonConvert.SerializeObject(_contacts)); //Sparar adressboken i en jsonformatterad sträng.
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
            /* Tömmer alla TextBoxes på information så att fälten är redo för att lägga in en ny kontakt.*/
        }

        private void RefreshList()
        {
            lv_Contacts.ItemsSource = _contacts.OrderBy(x => x.FirstName);
            /* Min ListView är namgiven: lv_Contacts och ItemsSource är vart den ska få datan ifrån som ska visas. Jag har valt att använda
             OrderBy() för att sortera kontakterna i adressboken i bokstavsordning på förnamn.*/
        }

        private void lv_Contacts_SelectionChanged(object sender, SelectionChangedEventArgs e) //Ett event när man klicker på ett objekt i ListView (en kontakt)
        {
            if (_isDeleted)
            {
                MenuPresenter(MenuState.add);
                /* lv_Contacts.SelectedItems[0] som jag använder för att hämta den information som använder har markerat behöver ett indexvärde för
                 att veta vilken av objekten som den ska hämta. Annars hade SelectedItems hämtat alla kontakter i ListView, därav [0]. 
                Jag har valt att göra så att min Delete-knapp först blir synlig när en kontakt är markerad och det betyder att den styrs ifrån lv_Contacts_SelectionChanged.
                Informationen för att ta bort en kontakt kommer alltså ifrån det här eventet. Om användaren tar bort en kontakt kommer då
                lv_Contacts.SelectedItems[0] att räkna ner indexet till -1 och Exception Handler sparkar bakut. Jag har därför valt att skapa en bool
                _isDeleted som jag sätter till true om en kontakt är borttagen och denna if-sats kollar detta och visar min Add-meny
                istället. På det viset hamnar aldrig programmet i lv_Contacts.SelectedItems[-1]. När en kontakt är borttagen så återvänder programmet hit.*/ 
            }
            else
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
                /* Fyller TextBoxarna med den markerade kontaktens information.*/
            }
            _isDeleted=false; //Återställer _isDeleted så att det går att ta fram en ny kontakts information.
        }
           

        private void MenuPresenter(MenuState state)
        {
            switch (state)
            {
                case MenuState.startup:
                    Main.Visibility = Visibility.Collapsed;
                    StartUp.Visibility = Visibility.Visible;
                    break;

                case MenuState.add:
                    bt_Add.Visibility = Visibility.Visible;
                    bt_Edit.Visibility = Visibility.Collapsed;
                    bt_Delete.Visibility = Visibility.Collapsed;
                    bt_Return.Visibility = Visibility.Collapsed;
                    Main.Visibility = Visibility.Visible;
                    StartUp.Visibility = Visibility.Collapsed;
                    break;

                case MenuState.edit:
                    bt_Add.Visibility = Visibility.Collapsed;
                    bt_Edit.Visibility = Visibility.Visible;
                    bt_Delete.Visibility = Visibility.Visible;
                    bt_Return.Visibility = Visibility.Visible;
                    break;
            }
            /* Switch-satsen styr via ett enum vilket visningsläge applikationen ska ha och vilka knappar som ska vara synliga.*/
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            var contact = (Contact)lv_Contacts.SelectedItems[0]!;
            /* Hämtar datan om den markerade kontakten och castar om den till objektet Contact och stoppar in det i en ny Contact här kallad contact.*/

            var index = _contacts.IndexOf(contact);
            /* Finner indexvärdet av denna kontakt för att jag vill att det ska hamna på rätt Guid Id.*/

            _contacts[index].FirstName = tb_FirstName.Text;
            _contacts[index].LastName = tb_LastName.Text;
            _contacts[index].EmailAddress = tb_Email.Text;
            _contacts[index].PhoneNumber = tb_PhoneNumber.Text;
            _contacts[index].StreetAddress = tb_StreetAddress.Text;
            _contacts[index].PostalCode = tb_PostalCode.Text;
            _contacts[index].City = tb_City.Text;
            /* Skriver in den uppdaterade informationen om kontakten i rätt kontakt i adressboken.*/
            RefreshList();
            MessageBox.Show("Kontakt uppdaterad.");
            _fileManager.Save(_filePath, JsonConvert.SerializeObject(_contacts));
            /* Sparar över adressboken i filen.*/
        }
        
        private void bt_Delete_Click(object sender, RoutedEventArgs e)
        {
            var contact = (Contact)lv_Contacts.SelectedItems[0]!;

            _contacts.Remove(contact); //Tar bort den valda kontakten ifrån adressboken.
            _isDeleted = true; //För att lv_Contacts_SelectionChanged inte ska stöta på ovan nämna problem
            _fileManager.Save(_filePath, JsonConvert.SerializeObject(_contacts));
            RefreshList();
            ClearFields();
            MessageBox.Show("Kontakt raderad.");
        }

        private void bt_Return_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            MenuPresenter(MenuState.add);
            /* Om en kontakt är markerad men användaren vill återvända till Add-menyn*/
        }

        private void bt_AddFilePath_Click(object sender, RoutedEventArgs e)
        {
            _filePath = $@"{tb_filePath.Text}\addressbook.json"; //Tar emot användarens inmatning och lägger till ett namn på filen och rätt format.
            try
            {
                _contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(_fileManager.Read(_filePath));
                /* Provar att läsa ifrån denna sökväg och casta om den som en ObservableCollection*/
            }
            catch
            {
                using (File.Create(_filePath))
                _contacts = new ObservableCollection<Contact>();
                /* Om det inte existerar en fil på denna sökväg så skapa en ny fil och en ny ObservableCollection*/
            }
            RefreshList();
            MenuPresenter(MenuState.add);
        }

       
    }
}
