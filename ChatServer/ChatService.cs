using ChatInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]

    public class ChatService : IChatService
    {
        public ConcurrentDictionary<string, ConnectedClient> connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string userName)
        {
            foreach(var client in connectedClients)
            {
                if(client.Key.ToLower() == userName.ToLower())
                {
                    return 1;
                }
            }
            var establisedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            ConnectedClient newClient = new ConnectedClient();
            newClient.connection = establisedUserConnection;
            newClient.UserName = userName;
            connectedClients.TryAdd(userName, newClient);
            UpdateHelper(0, userName);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Client login: {0} at {1}", newClient.UserName, System.DateTime.Now);
            Console.ResetColor();
            return 0;
        }
        public void SendMessageToAll(string message, string userName)
        {
            foreach(var client in connectedClients)
            {
                if(client.Key.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetMessage(message, userName);
                }
            }
        }
        public void Logout()
        {
            ConnectedClient client = GetMyClient();
            if (client != null)
            {
                ConnectedClient removedClient;
                connectedClients.TryRemove(client.UserName, out removedClient);
                UpdateHelper(1, removedClient.UserName);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Client logoff: {0} at {1}", removedClient.UserName, System.DateTime.Now);
                Console.ResetColor();
            }
        }
        public ConnectedClient GetMyClient()
        {
            var establisedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            foreach (var client in connectedClients)
            {
                if(client.Value.connection == establisedUserConnection)
                {
                    return client.Value;
                }
            }
            return null;
        }
        private void UpdateHelper(int value, string userName)
        {
            foreach (var client in connectedClients)
            {
                if (client.Value.UserName.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetUpdate(value, userName);
                }
            }
        }
        public List<string> GetCurrentUsers()
        {
            List<string> listOfUsers = new List<string>();
            foreach(var client in connectedClients)
            {
                listOfUsers.Add(client.Value.UserName);
            }
            return listOfUsers;
        }

    }
}
