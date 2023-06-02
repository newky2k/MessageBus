using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int updateCount = 0;

        private MainViewModel _viewModel;

        public MainViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = _viewModel; }
        }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();

            ViewModel.OnComplete += OnComplete;
        }

        private void OnComplete(object sender, bool e)
        {
            Dispatcher.Invoke(()=>
            {
                txtLog.ScrollToEnd();
            });
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (updateCount > 50)
                {
                    updateCount = 0;

                    ((TextBox)sender).ScrollToEnd();
                }
                
            }
        }
    }
}
