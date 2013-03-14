/************************************************
 * FileName: TopTenViewModel.cs
 * Document-related:
 * Module: Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Sbbs.Controls;
using Sbbs.Core;
using System.Collections.ObjectModel;

namespace Sbbs.Client
{
    public class TopTenViewModel : NotificationObject, IApplicationBarService
    {
        #region [Fields]

        private ObservableCollection<TopicModel> m_TopTenTopics;

        private ICommand m_RefreshCommand;

        private ICommand m_SettingsCommand;

        private ICommand m_TopicSelectedCommand;

        private ICommand m_BoardCommand;

        private readonly INavigationServiceFacade m_NavigationServiceFacade;

        private readonly IIsolatedStorageFacade m_IsolatedStorageFacade;

        private readonly IEventAggregator m_EventAggregator;

        private bool m_IsApplicationBarVisible = true;

        private bool m_IsLoading = false;

        #endregion

        #region [Constructors]

        public TopTenViewModel(INavigationServiceFacade navigationServiceFacade, 
                               IIsolatedStorageFacade isolatedStorageFacade,
                               IEventAggregator eventAggregator)
        {
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            if (isolatedStorageFacade == null) throw new ArgumentNullException("isolatedStorageFacade");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
            m_NavigationServiceFacade = navigationServiceFacade;
            m_IsolatedStorageFacade = isolatedStorageFacade;
            m_EventAggregator = eventAggregator;

            LoadFromIsolatedStorage();

            // 延迟一秒后开始刷新
            DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += delegate(object s, EventArgs arg)
            {
                Load();
                timer.Stop();
            };
            timer.Start();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        #endregion

        #region [Properties]

        public ObservableCollection<TopicModel> TopTenTopics
        {
            get
            {
                return m_TopTenTopics;
            }
            set
            {
                if (value != m_TopTenTopics)
                {
                    m_TopTenTopics = value;
                    RaisePropertyChanged("TopTenTopics");
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
                    OnIsLoadingPropertyChanged();
                }
            }
        }

        #endregion

        #region [Commands]

        public ICommand RefreshCommand
        {
            get { return m_RefreshCommand ?? (m_RefreshCommand = new DelegateCommand(RefreshExecute)); }
        }
        
        public ICommand SettingsCommand
        {
            get { return m_SettingsCommand ?? (m_SettingsCommand = new DelegateCommand(SettingsExecute)); }
        }

        public ICommand TopicSelectedCommand
        {
            get { return m_TopicSelectedCommand ?? (m_TopicSelectedCommand = new DelegateCommand<TopicModel>(TopicSelectedCommandExecute)); }
        }

        public ICommand BoardCommand
        {
            get { return m_BoardCommand ?? (m_BoardCommand = new DelegateCommand<TopicModel>(BoardCommandExecute)); }
        }

        #endregion

        #region [Handlers]

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            PairBoardDescriptionAndName();
        }

        #endregion

        #region [Private Methods]

        private void RefreshExecute()
        {
            Load();
        }

        private void SettingsExecute()
        {
            m_NavigationServiceFacade.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.Relative));
        }

        private void TopicSelectedCommandExecute(TopicModel topic)
        {
            m_NavigationServiceFacade.Navigate(
                new Uri("/Views/TopicView.xaml?" + QueryConstants.BoardKey + "=" + topic.Board
                    + "&" + QueryConstants.IdKey + "=" + topic.Id
                    + "&" + QueryConstants.TitleKey + "=" + HttpUtility.UrlEncode(topic.Title), UriKind.Relative));
        }

        private void BoardCommandExecute(TopicModel topic)
        {
            var board = topic.Board;
            var description = string.Empty;
            try
            {
                description = m_IsolatedStorageFacade.GetBoardDescriptionByName(board);
            }
            catch (Exception)
            {
                
            }

            m_NavigationServiceFacade.Navigate(
                new Uri("/Views/BoardView.xaml?" + QueryConstants.BoardKey + "=" + topic.Board
                    + "&" + QueryConstants.DescriptionKey + "=" + description, UriKind.Relative));
        }

        private void Load()
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;

            CoreService.Instance.GetTopTen(
                delegate(ObservableCollection<TopicModel> topics, bool success, string error)
                    {
                        IsLoading = false;

                        if (error != null)
                            return;

                        if (topics == null)
                            return;

                        TopTenTopics = topics;
                        try
                        {
                            //将十大保存到本地存储中
                            m_IsolatedStorageFacade.SaveTopTen(TopTenTopics);
                        }
                        catch (Exception)
                        {

                        }
                    });
        }

        /// <summary>
        /// 从本地存储中获取十大
        /// </summary>
        private void LoadFromIsolatedStorage()
        {
            try
            {
                var topTenTopics = m_IsolatedStorageFacade.GetTopTen();
                TopTenTopics = topTenTopics;
            }
            catch (Exception)
            {
                
            }
        }

        /// <summary>
        /// 将所有Board的Description和Name存贮到本地存储中
        /// </summary>
        private void PairBoardDescriptionAndName()
        {
            CoreService.Instance.GetAllSections(delegate(ObservableCollection<BoardModel> sections, bool success, string error)
            {
                if (error != null)
                    return;

                if (sections == null)
                    return;

                foreach (BoardModel section in sections)
                {
                    RecurseBoards(section);
                }
            });
        }

        private void RecurseBoards(BoardModel board)
        {
            if (board.Boards == null)
            {
                return;
            }

            foreach (var b in board.Boards)
            {
                if (!b.Leaf)
                {
                    RecurseBoards(b);
                }

                else
                {
                    try
                    {
                        m_IsolatedStorageFacade.SaveBoardDescription(b.EnglishName, b.Description);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void OnIsLoadingPropertyChanged()
        {
            var loadingEvent = m_EventAggregator.GetEvent<LoadingEvent>();
            loadingEvent.Publish(new LoadingEventArgs(IsLoading));
        }

        #endregion

        #region [Interface IApplicationBarService Implementation]

        public bool IsApplicationBarVisible
        {
            get
            {
                return m_IsApplicationBarVisible;
            }
            set
            {
                if (value != m_IsApplicationBarVisible)
                {
                    m_IsApplicationBarVisible = value;
                    RaisePropertyChanged("IsApplicationBarVisible");
                }
            }
        }

        #endregion
    }
}
