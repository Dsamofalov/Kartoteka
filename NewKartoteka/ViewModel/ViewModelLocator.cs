/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:NewKartoteka.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Kartoteka.DAL;
using Kartoteka.Domain;
using Microsoft.Practices.ServiceLocation;
using NewKartoteka.Model;
using NewKartoteka.Model.NLogRealization;
using NLog;
using System;

namespace NewKartoteka.ViewModel
{
    public class ViewModelLocator
    {
        static bool IsInit = false;
        public static Messenger _editBookMessenger;
        public static Messenger _editAuthorMessenger;
        public static Messenger _addBookMessenger;
        public static Messenger _addAuthorMessenger;
        static ViewModelLocator()
        {
            if(!IsInit)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Book, BookModel>().MaxDepth(3);
                    cfg.CreateMap<BookModel, Book>().MaxDepth(3);
                    cfg.CreateMap<Author, AuthorModel>().MaxDepth(3);
                    cfg.CreateMap<AuthorModel, Author>().MaxDepth(3);
                });
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            _editBookMessenger = new Messenger();
            _editAuthorMessenger = new Messenger();
            _addBookMessenger = new Messenger();
            _addAuthorMessenger = new Messenger();
            SimpleIoc.Default.Register(() => _editBookMessenger, KartotekaConstants.EditBookMessengerKey);
            SimpleIoc.Default.Register(() => _editAuthorMessenger, KartotekaConstants.EditAuthorMessengerKey);
            SimpleIoc.Default.Register(() => _addBookMessenger, KartotekaConstants.AddBookMessengerKey);
            SimpleIoc.Default.Register(() => _addAuthorMessenger, KartotekaConstants.AddAuthorMessengerKey);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddBookViewModel>();
            SimpleIoc.Default.Register<AddAuthorViewModel>();
            SimpleIoc.Default.Register<EditBookViewModel>();
            SimpleIoc.Default.Register<EditAuthorViewModel>();
            SimpleIoc.Default.Register<IAuthorsRepository, EFAuthorsRepository>();
            SimpleIoc.Default.Register<IBooksRepository, EFBooksRepository>();
            SimpleIoc.Default.Register<ILoggerService, NLogLoggingService>();
            SimpleIoc.Default.Register<IKartotekaService, DefaultKartotekaService>();
                IsInit = true;
            }
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public AddBookViewModel AddBook
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddBookViewModel>();
            }
        }
        public EditBookViewModel EditBook
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditBookViewModel>();
            }
        }
        public AddAuthorViewModel AddAuthor
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddAuthorViewModel>();
            }
        }
        public EditAuthorViewModel EditAuthor
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditAuthorViewModel>();
            }
        }
        public static void Cleanup()
        {
        }
    }
}