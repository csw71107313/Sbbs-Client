/************************************************
 * FileName: BoardSettingsView.xaml.cs
 * Document-related:
 * Module: Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-08-2013
 *************************************************/

using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Sbbs.Client
{
    public partial class BoardSettingsView : PhoneApplicationPage
    {
        BoardSettingsViewModel m_BoardSettingsViewModel;

        BoardSettingsViewModel BoardSettingsViewModel
        {
            get { return m_BoardSettingsViewModel ?? (m_BoardSettingsViewModel = this.DataContext as BoardSettingsViewModel); }
        }

        public BoardSettingsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) 
        {
            base.OnNavigatedTo(e);

            if (this.BoardSettingsViewModel != null) 
            {
                this.BoardSettingsViewModel.NavigatedTo();
            }
        }
    }
}