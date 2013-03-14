/************************************************
 * FileName: BoardView.xaml.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-22-2013
 *************************************************/

using System.Windows.Controls;
using System.Windows.Navigation;
using Sbbs.Core;

namespace Sbbs.Client
{
    public partial class BoardView
    {
        BoardViewModel m_BoardViewModel;

        BoardViewModel BoardViewModel
        {
            get { return m_BoardViewModel ?? (m_BoardViewModel = this.DataContext as BoardViewModel); }
        }

        public BoardView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 从发帖页面返回，且需要刷新
            if (RefreshService.NeedRefresh)
            {
                RefreshService.NeedRefresh = false;
                if (this.BoardViewModel.RefreshCommand.CanExecute(null))
                {
                    this.BoardViewModel.RefreshCommand.Execute(null);
                }
            }

            if (this.BoardViewModel != null
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.BoardKey)
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.DescriptionKey))
            {
                string board = this.NavigationContext.QueryString[QueryConstants.BoardKey];

                if (board != this.BoardViewModel.EnglishName)
                {
                    this.BoardViewModel.EnglishName = board;
                    this.BoardViewModel.Description = this.NavigationContext.QueryString[QueryConstants.DescriptionKey];

                    if (this.BoardViewModel.RefreshCommand.CanExecute(null))
                    {
                        this.BoardViewModel.RefreshCommand.Execute(null);
                    }
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                TopicModel topic = e.AddedItems[0] as TopicModel;

                if (BoardViewModel.TopicSelectedCommand.CanExecute(null))
                {
                    BoardViewModel.TopicSelectedCommand.Execute(topic);
                }

                // 清除选择，否则同样的项目无法点击第二次
                (sender as ListBox).SelectedIndex = -1;
            }
        }
    }
}