using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VectorGraphicViewer.UI.Helper
{
  public  class HandlerCommand: ICommand
    {
        #region Fields

        private Action _action;
        private Func<bool> _canExecute;

        #endregion

        #region Constructor

        public HandlerCommand(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        #endregion

        #region Methods

        /// <summary>
        /// CanExecuteChanged notifies any command sources (like a Button (EnterCommand -Ok Button ))
        /// that are bound to that ICommand that the value returned by CanExecute has changed.
        /// Command sources care about this because they generally need to update their status
        /// accordingly (eg. a Button will disable itself if CanExecute() returns false).
        ///The CommandManager.RequerySuggested event is raised whenever the CommandManager thinks that
        /// something has changed that will affect the ability of commands to execute. This might be
        /// a change of focus, for example. 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }

        #endregion
    }
}
