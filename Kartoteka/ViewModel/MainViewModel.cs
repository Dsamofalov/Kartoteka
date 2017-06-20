using GalaSoft.MvvmLight;
using Kartoteka.Model;
using System.Windows.Input;

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
       
        public ICommand AddNewAuthor { get; set; }
        public ICommand AddNewBook { get; set; }
        public ICommand StartNewSearch { get; set; }
        public ICommand Exit { get; set; }
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        public MainViewModel(IDataService dataService)
        {
            AddNewAuthor = new CommandAddAuthor();
            AddNewBook = new CommandAddBook();
            StartNewSearch = new CommandToSearch();
            Exit = new CommandExit();

      
        }
        
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}