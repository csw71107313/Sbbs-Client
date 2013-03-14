/************************************************
 * FileName: BoardViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-22-2013
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
    public class BoardViewModel : NotificationObject
    {
        #region [Constants]

        /// <summary>
        /// 每页显示帖子数
        /// </summary>
        private const int PAGESIZE = 10;

        private const string NEXTPAGE = "更多";
        private const string LOADING = "载入中...";

        #endregion

        #region [Private Fields]

        private string m_Name;
        private string m_Description;
        private ObservableCollection<TopicModel> m_Topics;
        private ICommand m_PostCommand;
        private ICommand m_RefreshCommand;
        private ICommand m_BoardSettingsCommand;
        private ICommand m_TopicSelectedCommand;
        private ICommand m_LoadMoreCommand;
        private string m_LoadMore;
        private bool m_CanLoadMore = true;
        private bool m_IsFullyLoaded = true;

        // 当前页数
        private int m_CurrentPageIndex = 0;

        private readonly IModeService m_ModeService;
        private readonly INavigationServiceFacade m_NavigationServiceFacade;
        private bool m_IsLoading = false;

        // 是否已经登陆
        private bool IsLoggedIn
        {
            get { return !String.IsNullOrEmpty(CoreService.Instance.Token); }
        }

        #endregion

        #region [Constructors]

        public BoardViewModel(IModeService modeService, INavigationServiceFacade navigationServiceFacade)
        {
            if (modeService == null) throw new ArgumentNullException("modeService");
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            m_ModeService = modeService;
            m_NavigationServiceFacade = navigationServiceFacade;

            m_ModeService.ModeSettings.CurrentModeChanged += ModeSettings_CurrentModeChanged;
        }

        #endregion

        #region [Properties]

        public string EnglishName
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (m_Name != value)
                {
                    m_Name = value;
                    RaisePropertyChanged("EnglishName");
                }
            }
        }

        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                if (m_Description != value)
                {
                    m_Description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        public ObservableCollection<TopicModel> Topics
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

        public ICommand PostCommand
        {
            get { return m_PostCommand ?? (m_PostCommand = new DelegateCommand(PostExecute)); }
        }

        public ICommand RefreshCommand
        {
            get { return m_RefreshCommand ?? (m_RefreshCommand = new DelegateCommand<ScrollViewer>(RefreshExecute)); }
        }
        
        public ICommand BoardSettingsCommand
        {
            get { return m_BoardSettingsCommand ?? (m_BoardSettingsCommand = new DelegateCommand(BoardSettingsExecute)); }
        }

        public ICommand TopicSelectedCommand
        {
            get { return m_TopicSelectedCommand ?? (m_TopicSelectedCommand = new DelegateCommand<TopicModel>(TopicSelectedCommandExecute)); }
        }

        public ICommand LoadMoreCommand
        {
            get { return m_LoadMoreCommand ?? (m_LoadMoreCommand = new DelegateCommand(LoadMoreExecute)); }
        }

        #endregion

        #region [Private Methods]

        private void PostExecute()
        {
            //已登录的话转至发表话题页面
            if (IsLoggedIn)
            {
                m_NavigationServiceFacade.Navigate(
                    new Uri("/Views/PostView.xaml?" + QueryConstants.BoardKey + "=" + EnglishName, UriKind.Relative));
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
            LoadTopics();
        }

        private void BoardSettingsExecute()
        {
            m_NavigationServiceFacade.Navigate(
                new Uri("/Views/BoardSettingsView.xaml", UriKind.Relative));
        }

        private void TopicSelectedCommandExecute(TopicModel topic)
        {
            m_NavigationServiceFacade.Navigate(
                new Uri("/Views/TopicView.xaml?" + QueryConstants.BoardKey + "=" + topic.Board
                    + "&" + QueryConstants.IdKey + "=" + topic.Id
                    + "&" + QueryConstants.TitleKey + "=" + HttpUtility.UrlEncode(topic.Title), UriKind.Relative));
        }

        private void LoadMoreExecute()
        {
            LoadMore = LOADING;
            CanLoadMore = false;
            LoadTopics(true);
        }

        private void LoadTopics(bool append = false)
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

            CoreService.Instance.GetBoard(EnglishName, page * PAGESIZE, PAGESIZE,
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
                                        foreach (var topic in topics)
                                        {
                                            Topics.Add(topic);
                                        }
                                    }
                                    else
                                    {
                                        Topics = topics;
                                    }
                                });
        }

        #endregion

        #region [Handlers]

        private void ModeSettings_CurrentModeChanged(object sender, CurrentModeChangedEventArgs e)
        {
            if (RefreshCommand.CanExecute(null))
            {
                RefreshCommand.Execute(null);
            }
        }

        #endregion
    }
}
