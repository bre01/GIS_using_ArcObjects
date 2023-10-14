using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1
{
    class DemoInterface : IMailSender
    {
        public string MailType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void SendMail()
        {
            throw new NotImplementedException();
        }
    }


    public interface IMailSender
    {
        void SendMail();
        string MailType {get;set;}
    }
}
