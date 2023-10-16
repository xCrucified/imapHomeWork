using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for msgWindow.xaml
    /// </summary>
    public partial class msgWindow : Window
    {
        List<Attachment> attachments = new List<Attachment>();

        string myMailAddress;
        string accountPassword;

        public msgWindow(string login,string pass)
        {
            InitializeComponent();

            myMailAddress = login;
            accountPassword = pass;
        }

        private void PictBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            attachments.Add(new Attachment(dialog.FileName));
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MenuWindow mw = new MenuWindow(myMailAddress, accountPassword);
            mw.Show();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            MailMessage mail = new MailMessage(myMailAddress, toTxtBox.Text)
            {
                Subject = subjTxtBox.Text,
                Body = $"{writenMsgTxtBox.Text}",
                IsBodyHtml = true,
               
            };
            foreach (var attachment in attachments)
            {
                //u can add many attachments
                mail.Attachments.Add(attachment); 
            }
            if (isPriority.IsChecked == true)
            {
                mail.Priority = MailPriority.High;
            }
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(myMailAddress, accountPassword),
                EnableSsl = true
            };
            
            client.Send(mail);

            msgListBox.Items.Add($"{myMailAddress}:{mail.Body}");

            MessageBox.Show("Message was send!!!");
        }
    }
}
