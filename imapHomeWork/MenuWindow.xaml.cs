using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace imapHomeWork
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        string myEmailAddress;
        string accountPassword;
        private ImapClient client;

        public MenuWindow(string login, string pass)
        {
            InitializeComponent();

            myEmailAddress = login;
            accountPassword = pass;
            client = new ImapClient();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListFolders();
        }

        private async void ListFolders()
        {
            try
            {
                await client.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(myEmailAddress, accountPassword);

                var folder = client.GetFolder(SpecialFolder.Sent);
                folder.Open(FolderAccess.ReadWrite);
                IList<UniqueId> uids = folder.Search(SearchQuery.All);
                List<MimeMessage> messages = new List<MimeMessage>();

                foreach (var i in uids)
                {
                    MimeMessage message = folder.GetMessage(i);
                    messages.Add(message);
                }

                foreach (var fl in await client.GetFoldersAsync(client.PersonalNamespaces[0]))
                {
                    foldListBox.Items.Add(fl.Name);
                }

                var sortedMessages = messages.OrderBy(message => message.From.ToString());
                //messListBox.Items.Clear();

                foreach (var message in sortedMessages)
                {
                    messListBox.Items.Add($"{message.Date}: {message.Subject} - {new string(message.TextBody)}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error!: {ex.Message}");
            }
            finally
            {
                if (client.IsConnected)
                {
                    client.Disconnect(true);
                }
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (client != null && client.IsConnected)
            {
                client.Disconnect(true);
            }
            Application.Current.Shutdown();
        }

        private void OnSelected(object sender, RoutedEventArgs e)
        {
            //ListBoxItem messListBox = e.Source as ListBoxItem;

            //if (messListBox != null)
            //{
            //    messListBox.Content = messListBox.Content.ToString() + " is selected.";
            //}
        }
    }
}
