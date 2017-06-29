using System.Windows;
using Kartoteka.ViewModel;
using MahApps.Metro.Controls;

namespace Kartoteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.ShowCloseButton = false;
        }
    }
}