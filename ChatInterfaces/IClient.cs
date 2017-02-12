using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatInterfaces
{
    public interface IClient
    {
        [OperationContract]
        void GetMessage(string message, string userName);
        [OperationContract]
        void GetUpdate(int value, string userName);
    }
}
