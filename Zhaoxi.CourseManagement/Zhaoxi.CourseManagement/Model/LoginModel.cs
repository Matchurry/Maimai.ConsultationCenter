using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zhaoxi.CourseManagement.Common;

namespace Zhaoxi.CourseManagement.Model
{
    public class LoginModel : NotifyBase
    {
        private string _UserName;

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
