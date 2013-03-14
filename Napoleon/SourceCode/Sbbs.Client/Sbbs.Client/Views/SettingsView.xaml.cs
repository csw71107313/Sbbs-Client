/************************************************
 * FileName: SettingsView.xaml.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System.Windows;
using Microsoft.Phone.Controls;

namespace Sbbs.Client.Views
{
    public partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ugly code. but it works..
        /// </summary>
        private void Pivot_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var viewModelLocator = ((ViewModelLocator)Application.Current.Resources["ViewModelLocator"]);
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    {
                        viewModelLocator.SettingsViewModel.LoginViewModel.IsApplicationBarVisible = true;
                        break;
                    }

                case 1:
                    {
                        viewModelLocator.SettingsViewModel.LoginViewModel.IsApplicationBarVisible = false;
                        break;
                    }
            }
        }
    }
}