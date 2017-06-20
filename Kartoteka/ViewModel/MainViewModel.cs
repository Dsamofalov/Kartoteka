using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Kartoteka.Model;
using System.Windows;
using System.Windows.Input;
using System;

namespace Kartoteka.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        public ICommand AddNewAuthor { get; set; }
        public ICommand AddNewBook { get; set; }
        public ICommand StartNewSearch { get; set; }

        public MainViewModel(IDataService dataService)
        {
            AddNewAuthor = new CommandAddAuthor();
            AddNewBook = new CommandAddBook();
            StartNewSearch = new CommandToSearch();
            this.CloseWindowCommand = new RelayCommand<Window>(ClosingWindow.CloseWindow);
        }
        
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}