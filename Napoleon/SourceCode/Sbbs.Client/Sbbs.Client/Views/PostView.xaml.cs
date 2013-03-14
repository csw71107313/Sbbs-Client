/************************************************
 * FileName: PostView.xaml.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-18-2013
 *************************************************/

using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Sbbs.Client
{
    public partial class PostView : PhoneApplicationPage
    {
        PostViewModel m_PostViewModel;

        PostViewModel PostViewModel
        {
            get { return m_PostViewModel ?? (m_PostViewModel = this.DataContext as PostViewModel); }
        }


        public PostView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.PostViewModel != null
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.TitleKey)
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.BoardKey)
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.ReIdKey))
            {
                this.PostViewModel.Title = this.NavigationContext.QueryString[QueryConstants.TitleKey];
                this.PostViewModel.Board = this.NavigationContext.QueryString[QueryConstants.BoardKey];
                this.PostViewModel.ReId = int.Parse(this.NavigationContext.QueryString[QueryConstants.ReIdKey]);
            }
            else if (this.PostViewModel != null 
                && this.NavigationContext.QueryString.ContainsKey(QueryConstants.BoardKey))
            {
                this.PostViewModel.Title = string.Empty;
                this.PostViewModel.Board = this.NavigationContext.QueryString[QueryConstants.BoardKey];
                this.PostViewModel.ReId = 0;
            }
        }
    }
}