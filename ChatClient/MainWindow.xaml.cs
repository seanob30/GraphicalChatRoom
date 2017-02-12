using ChatInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace ChatClient
{
    public partial class MainWindow : Window
    {
        public static IChatService Server;
        private static DuplexChannelFactory<IChatService> channelFactory;
        public MainWindow()
        {
            InitializeComponent();
            channelFactory = new DuplexChannelFactory<IChatService>(new ClientCallback(), "ChatRoomEndPoint");
            Server = channelFactory.CreateChannel();
        }
        public void TakeMessage(string message, string userName)
        {
            textDisplayTextBox.Text += userName + " ## " + DateTime.Now + ": " + message + "\n";
            textDisplayTextBox.ScrollToEnd();
        }
        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (messageTextBox.Text.Length == 0)
            {
                return;
            }
            Server.SendMessageToAll(messageTextBox.Text, usernameTextBox.Text);
            TakeMessage(messageTextBox.Text, "You");
            messageTextBox.Text = "";
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            int returnValue = Server.Login(usernameTextBox.Text);
            if(returnValue == 1)
            {
                MessageBox.Show("That login is already taken..Try again");
            }
            else if (returnValue == 0)
            {
                welcomeLabel.Content = "Welcome " + usernameTextBox.Text + "!";
                usernameTextBox.IsEnabled = false;
                loginButton.IsEnabled = false;
                LoadUserList(Server.GetCurrentUsers());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Server.Logout();
        }
        public void AddUserToList(string userName)
        {
            if(loggedInListBox.Items.Contains(userName))
            {
                return;
            }
            loggedInListBox.Items.Add(userName);
        }
        public void RemoveUserFromList(string userName)
        {
            if (loggedInListBox.Items.Contains(userName))
            {
                loggedInListBox.Items.Remove(userName);
            }
        }
        private void LoadUserList(List<string> users)
        {
            foreach (var user in users)
            {
                AddUserToList(user);
            }
        }
    }
}
