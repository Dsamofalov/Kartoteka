using Kartoteka.DAL;
using Kartoteka.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kartoteka
{
    class CommandAddBook : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AddBook newbook = new AddBook();
            newbook.Show();
        }
    }
}
