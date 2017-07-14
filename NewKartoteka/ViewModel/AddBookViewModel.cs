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
    //Валидация должна быть реализована с помощью IDataErrorInfo ищи пример в инете
    public class AddBookViewModel:ViewModelBase
    {
        private readonly IKartotekaService _service;
        private Book _newbook;
        private ObservableCollection<Author> _authors;
        
        //Нельзя просто так отдавать класс бизнес логики для редактирования, его свойства могут быть только на чтение 
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
        //Правильная конструкция и так двех его свойств
        private string _name;

        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }


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
