/************************************************
 * FileName: HotTopicsView.xaml.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using Microsoft.Phone.Controls;
using Sbbs.Core;

namespace Sbbs.Client
{
    public partial class HotTopicsView
    {
        HotTopicsViewModel m_HotTopicsViewModel;

        HotTopicsViewModel HotTopicsViewModel
        {
            get { return m_HotTopicsViewModel ?? (m_HotTopicsViewModel = this.DataContext as HotTopicsViewModel); }
        }

        public HotTopicsView()
        {
            InitializeComponent();
        }

        private void LongListSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                TopicModel topic = (e.AddedItems[0] as Microsoft.Phone.Controls.LongListSelector.LongListSelectorItem).Item as TopicModel;

                if (HotTopicsViewModel.TopicSelectedCommand.CanExecute(null))
                {
                    HotTopicsViewModel.TopicSelectedCommand.Execute(topic);
                }

                // 清除选择，否则同样的项目无法点击第二次
                (sender as LongListSelector).SelectedItem = null;
            }
        }
    }
}