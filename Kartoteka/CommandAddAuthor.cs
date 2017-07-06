using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Kartoteka.Domain;
using Kartoteka.DAL;

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
            AddAuthor newauthor = new AddAuthor();
            newauthor.Show();
        }
    }
}
