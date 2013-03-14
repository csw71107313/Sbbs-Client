/************************************************
 * FileName: HotTopicsViewModel.cs
 * Document-related:
 * Module: Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Sbbs.Controls;
using Sbbs.Core;

namespace Sbbs.Client
{
    /// <summary>
    /// 分区热点
    /// </summary>
    public class HotTopicsViewModel : NotificationObject, IApplicationBarService
    {
        #region [Fields]

        private ObservableCollection<TopicsGroupModel> m_TopicsGroupItems;

        private ICommand m_RefreshCommand;

        private ICommand m_SettingsCommand;

        private ICommand m_TopicSelectedCommand;

        private ICommand m_BoardCommand;

        private readonly INavigationServiceFacade m_NavigationServiceFacade;

        private readonly IIsolatedStorageFacade m_IsolatedStorageFacade;

        private readonly IEventAggregator m_EventAggregator;

        private bool m_IsApplicationBarVisible = false;

        private bool m_IsLoading = false;

        #endregion

        #region [Constructors]

        public HotTopicsViewModel(INavigationServiceFacade navigationServiceFacade, 
                                  IIsolatedStorageFacade isolatedStorageFacade,
                                  IEventAggregator eventAggregator)
        {
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            if (isolatedStorageFacade == null) throw new ArgumentNullException("isolatedStorageFacade");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
            m_NavigationServiceFacade = navigationServiceFacade;
            m_IsolatedStorageFacade = isolatedStorageFacade;
            m_EventAggregator = eventAggregator;

            Load();
        }

        #endregion

        #region [Properties]

        public ObservableCollection<TopicsGroupModel> TopicsGroupItems
        {
            get
            {
                return m_TopicsGroupItems;
            }
            set
            {
                if (value != m_TopicsGroupItems)
                {
                    m_TopicsGroupItems = value;
                    RaisePropertyChanged("TopicsGroupItems");
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

            CoreService.Instance.GetHotTopics(
                delegate(ObservableCollection<HotTopicsModel> topics, bool success, string error)
                    {
                        IsLoading = false;

                        if (error != null)
                            return;

                        if (topics == null)
                            return;

                        ObservableCollection<TopicsGroupModel> newGroup =
                            new ObservableCollection<TopicsGroupModel>();

                        foreach (HotTopicsModel hot in topics)
                        {
                            TopicsGroupModel newItem = new TopicsGroupModel(hot.Description);
                            foreach (TopicModel topic in hot.Topics)
                                newItem.Add(topic);
                            newGroup.Add(newItem);
                        }

                        TopicsGroupItems = newGroup;
                    });
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
