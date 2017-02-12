using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatInterfaces
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IChatService
    {
        [OperationContract]
        int Login(string userName);
        [OperationContract]
        void Logout();
        [OperationContract]
        void SendMessageToAll(string message, string userName);
        [OperationContract]
        List<string> GetCurrentUsers();
    }
}
