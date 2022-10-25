using Bogus;
using Shmail_Messenger.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
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
using Person = Shmail_Messenger.Models.Person;

namespace Shmail_Messenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Message> Messages { get; set; }
        Person FakePerson = new Faker<Person>().RuleFor(p => p.Name, faker => faker.Person.FullName).RuleFor(p => p.ImageUrl, faker => faker.Person.Avatar).RuleFor(p => p.Phone, faker => faker.Person.Phone);

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            Messages = new();

            Photo.Source = new BitmapImage(new Uri(FakePerson.ImageUrl!));
            PersonName.Content = FakePerson.Name;
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
                return;


            listViewMessages.ItemsSource = null;
            Messages.Add(new Message("You", txtMessage.Text));
            listViewMessages.ItemsSource = Messages;
            txtMessage.Text = String.Empty;



            Status.Content = "Typing...";
            await Task.Run(() =>
            {
                Thread.Sleep(Random.Shared.Next(1000, 3000));

                Messages.Add(new Message(FakePerson.Name, new Faker().Lorem.Text()));
            });
            Status.Content = "Online";



            listViewMessages.ItemsSource = null;
            listViewMessages.ItemsSource = Messages;
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSend_Click(sender, e);
        }


    }
}
