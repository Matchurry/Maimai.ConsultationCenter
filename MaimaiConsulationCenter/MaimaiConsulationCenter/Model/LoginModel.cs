using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaimaiConsulationCenter.Common;

namespace MaimaiConsulationCenter.Model
{
    public class LoginModel : NotifyBase
    {
        private string _UserName;
        public LoginModel(string userName, string password, string validationCode)
        {
            UserName = userName;
            Password = password;
            ValidationCode = validationCode;
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; this.DoNotify(); }
        }

        private string _password;
        public string Password
        {
            get { return this._password; }
            set { _password = value; this.DoNotify(); }

        }

        private string _validationCode;
        public string ValidationCode
        {
            get { return this._validationCode; }
            set { _validationCode = value; this.DoNotify(); }
        }
    }
}
