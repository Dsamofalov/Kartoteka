using GalaSoft.MvvmLight;
using Kartoteka.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewKartoteka
{
   public class AddBookViewModel:ViewModelBase
    {
        private readonly IKartotekaService _service;
        private Book _newbook;
        private ObservableCollection<Author> _authors;

        public Book NewBook
        {
            get
            {
                return _newbook;
            }

            set
            {
                _newbook = value;
                RaisePropertyChanged("NewBook");
            }
        }

        public ObservableCollection<Author> Authors
        {
            get
            {
                return _authors;
            }

            set
            {
                _authors = value;
                RaisePropertyChanged("Authors");
            }
        }

        public AddBookViewModel(IKartotekaService service)
        {
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            this.NewBook = new Book();
            this.Authors = new ObservableCollection<Author>();
        }
    }
}
