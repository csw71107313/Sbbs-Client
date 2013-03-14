/************************************************
 * FileName: SettingsView.xaml.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System.Windows.Controls;
using Sbbs.Core;

namespace Sbbs.Client
{
    public partial class TopTenView
    {
        TopTenViewModel m_TopTenViewModel;

        TopTenViewModel TopTenViewModel
        {
            get { return m_TopTenViewModel ?? (m_TopTenViewModel = this.DataContext as TopTenViewModel); }
        }

        public TopTenView()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                TopicModel topic = e.AddedItems[0] as TopicModel;

                if (TopTenViewModel.TopicSelectedCommand.CanExecute(null))
                {
                    TopTenViewModel.TopicSelectedCommand.Execute(topic);
                }

                // 清除选择，否则同样的项目无法点击第二次
                (sender as ListBox).SelectedIndex = -1;
            }
        }
    }
}