using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kartoteka
{
    class CommandAddAuthor : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AddAuthor NewAuthor = new AddAuthor();
            NewAuthor.Show();
        }
    }
}
