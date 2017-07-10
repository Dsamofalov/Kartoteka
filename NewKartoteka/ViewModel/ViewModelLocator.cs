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
using Kartoteka.DAL;
using Kartoteka.Domain;
using Microsoft.Practices.ServiceLocation;
using NewKartoteka.Model;

namespace NewKartoteka.ViewModel
{
    public class ViewModelLocator
    {
        static bool IsInit = false;
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
                IsInit = true;
            }
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IAuthorsRepository, EFAuthorsRepository>();
            SimpleIoc.Default.Register<IBooksRepository, EFBooksRepository>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public static void Cleanup()
        {
        }
    }
}