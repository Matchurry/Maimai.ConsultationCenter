using System;
using System.Windows.Input;

namespace MaimaiConsulationCenter.Common
{
    public class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DoCanExecute?.Invoke(parameter) == true;
        }

        public void Execute(object parameter)
        {
            DoExecute?.Invoke(parameter);
        }

        public Action<Object> DoExecute { get; set; }

        public Func<object, bool> DoCanExecute { get; set; }
    }
}
