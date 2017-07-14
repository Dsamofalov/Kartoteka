using GalaSoft.MvvmLight;
using Kartoteka.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewKartoteka 
{
    //Валидация должна быть реализована с помощью IDataErrorInfo ищи пример в инете
    public class AddBookViewModel:ViewModelBase, IDataErrorInfo
    {
        private readonly IKartotekaService _service;
        private string _name;
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }

        private int _year;
        public int Year { get { return _year; } set { _year = value; RaisePropertyChanged("Year"); } }

        private string _description;
        public string Description { get { return _description; } set { _description = value; RaisePropertyChanged("Description"); } }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors { get { return _authors; } set { _authors = value; RaisePropertyChanged("Authors"); } }

        private ObservableCollection<Author> _allauthors;
        public ObservableCollection<Author> AllAuthors { get { return _allauthors; } set { _allauthors = value; RaisePropertyChanged("AllAuthors"); } }

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch(columnName)
                {
                    case "Name":
                        {
                            if (String.IsNullOrEmpty(this.Name))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "Description":
                        {
                            if (String.IsNullOrEmpty(this.Description))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "Year":
                        {
                            if (Year>DateTime.Now.Year)
                                return "Такой год еще не наступил!";
                            break;
                        }
                }

                return string.Empty;
            }
        }

        public AddBookViewModel(IKartotekaService service)
        {
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            this.AllAuthors = new ObservableCollection<Author>();
        }
    }
}
