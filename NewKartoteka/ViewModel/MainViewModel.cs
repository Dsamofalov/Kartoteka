using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Kartoteka.DAL;
using Kartoteka.Domain;
using NewKartoteka.Model;
using System;
using System.Windows;

namespace NewKartoteka.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKartotekaService _service;
        public MainViewModel(IKartotekaService service)
        {
            if (service == null) throw new ArgumentNullException("service", "service is null");
            else _service = service;
        }
    }
}