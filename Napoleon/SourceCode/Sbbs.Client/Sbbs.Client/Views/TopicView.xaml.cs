/************************************************
 * FileName: TopicView.xaml.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-14-2013
 *************************************************/

using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Sbbs.Client
{
    public partial class TopicView : PhoneApplicationPage
    {
        TopicViewModel m_TopicViewModel;

        TopicViewModel TopicViewModel
        {
            get { return m_TopicViewModel ?? (m_TopicViewModel = this.DataContext as TopicViewModel); }
        }

        public TopicView()
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
                if (this.TopicViewModel.RefreshCommand.CanExecute(null))
                {
                    this.TopicViewModel.RefreshCommand.Execute(null);
                }
            }

            if (this.TopicViewModel != null
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.BoardKey)
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.IdKey)
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.TitleKey))
            {
                string board = this.NavigationContext.QueryString[QueryConstants.BoardKey];
                int id = int.Parse(this.NavigationContext.QueryString[QueryConstants.IdKey]);

                if (board != this.TopicViewModel.Board || id != this.TopicViewModel.Id)
                {
                    this.TopicViewModel.Board = board;
                    this.TopicViewModel.Id = id;
                    this.TopicViewModel.Title = this.NavigationContext.QueryString[QueryConstants.TitleKey];

                    if (this.TopicViewModel.RefreshCommand.CanExecute(null))
                    {
                        this.TopicViewModel.RefreshCommand.Execute(null);
                    }
                }
            }
        }
    }
}