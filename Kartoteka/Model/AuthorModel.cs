using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka
{
    public class AuthorModel : ObservableObject
    {
        private int id;
        private string name;
        private string surname;

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                Set<int>(() => this.ID, ref id, value);
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                Set<string>(() => this.Name, ref name, value);
            }
        }

        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                Set<string>(() => this.Surname, ref surname, value);
            }
        }
    }
}
