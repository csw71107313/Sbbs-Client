/************************************************
 * FileName: TopicViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-14-2013
 *************************************************/

using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Sbbs.Core;

namespace Sbbs.Client
{
    public class TopicViewModel : NotificationObject
    {
        #region [Constants]

        /// <summary>
        /// 每页显示回复数
        /// </summary>
        private const int PAGESIZE = 10;

        private const string NEXTPAGE = "更多";
        private const string LOADING = "载入中...";

        #endregion

        #region [Private Fields]

        private int m_Id;
        private string m_Board;
        private string m_Title;
        private ObservableCollection<IndexedTopicModel> m_Topics;
        private ICommand m_ReplyCommand;
        private ICommand m_RefreshCommand;
        private ICommand m_LoadMoreCommand;
        private string m_LoadMore;
        private bool m_CanLoadMore = true;
        private bool m_IsFullyLoaded = true;

        // 当前页数
        private int m_CurrentPageIndex = 0;

        private readonly INavigationServiceFacade m_NavigationServiceFacade;
        private bool m_IsLoading = false;

        // 是否已经登陆
        private bool IsLoggedIn
        {
            get { return !String.IsNullOrEmpty(CoreService.Instance.Token); }
        }

        #endregion

        #region [Constructors]

        public TopicViewModel(INavigationServiceFacade navigationServiceFacade)
        {
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            m_NavigationServiceFacade = navigationServiceFacade;

            LoadMore = NEXTPAGE;
        }

        #endregion

        #region [Properties]

        private int m_ItemIndex;
        public int ItemIndex
        {
            get
            {
                return m_ItemIndex;
            }
            set
            {
                if (value != m_ItemIndex)
                {
                    m_ItemIndex = value;
                    RaisePropertyChanged("ItemIndex");
                }
            }
        }

        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                if (m_Id != value)
                {
                    m_Id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        public string Board
        {
            get
            {
                return m_Board;
            }
            set
            {
                if (m_Board != value)
                {
                    m_Board = value;
                    RaisePropertyChanged("Board");
                }
            }
        }

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                if (m_Title != value)
                {
                    m_Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        public ObservableCollection<IndexedTopicModel> Topics
        {
            get
            {
                return m_Topics;
            }
            set
            {
                if (m_Topics != value)
                {
                    m_Topics = value;
                    RaisePropertyChanged("Topics");
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return m_IsLoading;
            }
            set
            {
                if (m_IsLoading != value)
                {
                    m_IsLoading = value;
                    RaisePropertyChanged("IsLoading");
                }
            }
        }

        public string LoadMore
        {
            get
            {
                return m_LoadMore;
            }
            set
            {
                if (m_LoadMore != value)
                {
                    m_LoadMore = value;
                    RaisePropertyChanged("LoadMore");
                }
            }
        }

        public bool CanLoadMore
        {
            get
            {
                return m_CanLoadMore;
            }
            set
            {
                if (m_CanLoadMore != value)
                {
                    m_CanLoadMore = value;
                    RaisePropertyChanged("CanLoadMore");
                }
            }
        }

        public bool IsFullyLoaded 
        {
            get
            {
                return m_IsFullyLoaded;
            }
            set
            {
                if (m_IsFullyLoaded != value)
                {
                    m_IsFullyLoaded = value;
                    RaisePropertyChanged("IsFullyLoaded");
                }
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                return m_CurrentPageIndex;
            }
            set
            {
                if (m_CurrentPageIndex != value)
                {
                    m_CurrentPageIndex = value;
                }
            }
        }

        #endregion

        #region [Commands]

        public ICommand ReplyCommand
        {
            get { return m_ReplyCommand ?? (m_ReplyCommand = new DelegateCommand<TopicModel>(ReplyExecute)); }
        }

        public ICommand RefreshCommand
        {
            get { return m_RefreshCommand ?? (m_RefreshCommand = new DelegateCommand<ScrollViewer>(RefreshExecute)); }
        }

        public ICommand LoadMoreCommand
        {
            get { return m_LoadMoreCommand ?? (m_LoadMoreCommand = new DelegateCommand(LoadMoreExecute)); }
        }

        #endregion

        #region [Private Methods]

        private void ReplyExecute(TopicModel topic)
        {
            //已登录的话转至发表回复页面
            if (IsLoggedIn)
            {
                if (topic != null)
                {
                    var title = topic.Title;
                    if (title.Length > 3 && title.Substring(0, 3) == "Re:")
                        title = title;
                    else
                        title = "Re: " + title;
                    m_NavigationServiceFacade.Navigate(
                        new Uri("/Views/PostView.xaml?" + QueryConstants.TitleKey + "=" + HttpUtility.UrlEncode(title)
                                + "&" + QueryConstants.BoardKey + "=" + topic.Board
                                + "&" + QueryConstants.ReIdKey + "=" + topic.Id, UriKind.Relative));
                }
                else
                {
                    var title = "Re: " + Title;
                    m_NavigationServiceFacade.Navigate(
                        new Uri("/Views/PostView.xaml?" + QueryConstants.TitleKey + "=" + HttpUtility.UrlEncode(title)
                                + "&" + QueryConstants.BoardKey + "=" + Board
                                + "&" + QueryConstants.ReIdKey + "=" + Id, UriKind.Relative));
                }
            }
            //未登录提示是否前往登录
            else
            {
                MessageBoxResult result = MessageBox.Show("尚未登录，前往登录？", "未登录", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    m_NavigationServiceFacade.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.Relative));
                }
            }
        }

        private void RefreshExecute(ScrollViewer scrollViewer)
        {
            //回到顶部
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset(0.0);
            }

            IsFullyLoaded = true;
            CurrentPageIndex = 0;
            LoadReplies();
        }

        private void LoadMoreExecute()
        {
            LoadMore = LOADING;
            CanLoadMore = false;
            LoadReplies(true);
        }

        #endregion

        #region [Public Methods]

        public void LoadReplies(bool append = false)
        {
            // 清空已有内容
            if (!append && Topics != null)
                Topics.Clear();

            if (IsLoading)
            {
                return;
            }

            IsLoading = true;

            // 重新加载
            int page = append ? CurrentPageIndex + 1 : CurrentPageIndex;

            CoreService.Instance.GetTopic(Board, Id, page * PAGESIZE, PAGESIZE,
                                          delegate(ObservableCollection<TopicModel> topics, bool success, string error)
                                              {
                                                  IsLoading = false;

                                                  if (error != null)
                                                      return;

                                                  if (topics == null)
                                                      return;

                                                  IsFullyLoaded = topics.Count < PAGESIZE;

                                                  LoadMore = NEXTPAGE;
                                                  CanLoadMore = true;

                                                  // 重置还是添加
                                                  if (append)
                                                  {
                                                      CurrentPageIndex++;
                                                      int index = CurrentPageIndex * PAGESIZE;

                                                      foreach (var topic in topics)
                                                      {
                                                          IndexedTopicModel indexedTopic = new IndexedTopicModel(topic, index);
                                                          Topics.Add(indexedTopic);
                                                          index++;
                                                      }
                                                  }
                                                  else
                                                  {
                                                      Topics = new ObservableCollection<IndexedTopicModel>();
                                                      int index = 0;

                                                      foreach (var topic in topics)
                                                      {
                                                          IndexedTopicModel indexedTopic = new IndexedTopicModel(topic, index);
                                                          Topics.Add(indexedTopic);
                                                          index++;
                                                      }
                                                  }
                                              });
        }

        #endregion
    }

    public class IndexedTopicModel : TopicModel
    {
        public int Index { get; set; }

        public IndexedTopicModel(TopicModel topic, int index)
        {
            foreach (var basePropertyInfo in topic.GetType().GetProperties())
            {
                var derivedPropertyInfo = topic.GetType().GetProperty(basePropertyInfo.Name);
                derivedPropertyInfo.SetValue(this, basePropertyInfo.GetValue(topic, null), null);
            }

            Index = index;
        }
    }
}
