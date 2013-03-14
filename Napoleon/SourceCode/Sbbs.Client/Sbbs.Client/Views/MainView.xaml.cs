/************************************************
 * FileName: MainView.xaml.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace Sbbs.Client
{
    public partial class MainView
    {
        // 构造函数
        public MainView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ugly code. but it works..
        /// </summary>
        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModelLocator = ((ViewModelLocator)Application.Current.Resources["ViewModelLocator"]);
            if (viewModelLocator == null)
            {
                return;
            }

            switch (((Panorama)sender).SelectedIndex)
            {
                case 0:
                    {
                        viewModelLocator.HotTopicsViewModel.IsApplicationBarVisible = false;
                        viewModelLocator.BoardsViewModel.IsApplicationBarVisible = false;
                        viewModelLocator.TopTenViewModel.IsApplicationBarVisible = true;
                        break;
                    }

                case 1:
                    {
                        viewModelLocator.TopTenViewModel.IsApplicationBarVisible = false;
                        viewModelLocator.BoardsViewModel.IsApplicationBarVisible = false;
                        viewModelLocator.HotTopicsViewModel.IsApplicationBarVisible = true;
                        break;
                    }

                case 2:
                    {
                        viewModelLocator.TopTenViewModel.IsApplicationBarVisible = false;
                        viewModelLocator.HotTopicsViewModel.IsApplicationBarVisible = false;
                        viewModelLocator.BoardsViewModel.IsApplicationBarVisible = true;
                        break;
                    }
            }
        }
    }
}