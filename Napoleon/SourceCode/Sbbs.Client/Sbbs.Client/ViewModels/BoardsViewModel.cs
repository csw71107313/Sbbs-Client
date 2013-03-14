/************************************************
 * FileName: BoardsViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-06-2013
 *************************************************/

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Sbbs.Controls;
using Sbbs.Core;

namespace Sbbs.Client
{
    public class BoardsViewModel : NotificationObject, IApplicationBarService
    {
        #region [Fields]

        private ObservableCollection<BoardModel> m_FavoriteItems;
        
        private ObservableCollection<BoardsGroupModel> m_SectionItems;
        
        private readonly INavigationServiceFacade m_NavigationServiceFacade;

        private readonly IIsolatedStorageFacade m_IsolatedStorageFacade;

        private readonly IEventAggregator m_EventAggregator;

        private ICommand m_SettingsCommand;

        private ICommand m_FavoriteCommand;

        private ICommand m_AllBoardsCommand;

        private ICommand m_BoardSelectedCommand;

        private bool m_IsApplicationBarVisible = false;

        private bool m_IsFavoriteVisible = true;

        private bool m_IsFavoriteEmpty = true;

        private bool m_IsAllBoardsVisible = false;

        // 是否已经登陆
        private bool IsLoggedIn
        {
            get { return !String.IsNullOrEmpty(CoreService.Instance.Token); }
        }

        #endregion

        #region [Constructors]

        public BoardsViewModel(INavigationServiceFacade navigationServiceFacade, IIsolatedStorageFacade isolatedStorageFacade, IEventAggregator eventAggregator)
        {
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            if (isolatedStorageFacade == null) throw new ArgumentNullException("isolatedStorageFacade");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
            m_NavigationServiceFacade = navigationServiceFacade;
            m_IsolatedStorageFacade = isolatedStorageFacade;
            m_EventAggregator = eventAggregator;

            var loggedInEvent = m_EventAggregator.GetEvent<LoggedInEvent>();
            loggedInEvent.Subscribe(LoggedInEventHandler);

            LoadFavoritesFromIsolatedStorage();

            LoadAllBoardsFromIsolatedStorage();

            // 延迟两秒后开始刷新
            DispatcherTimer timer = new DispatcherTimer() {Interval = TimeSpan.FromSeconds(2)};
            timer.Tick += delegate(object s, EventArgs arg)
                              {
                                  LoadFavorites();
                                  LoadAllBoards();
                                  timer.Stop();
                              };
            timer.Start();
        }

        #endregion

        #region [Properties]

        /// <summary>
        /// 收藏夹
        /// </summary>
        public ObservableCollection<BoardModel> FavoriteItems
        {
            get
            {
                return m_FavoriteItems;
            }
            set
            {
                if (value != m_FavoriteItems)
                {
                    m_FavoriteItems = value;
                    RaisePropertyChanged("FavoriteItems");
                }
            }
        }

        /// <summary>
        /// 全部分区
        /// </summary>
        public ObservableCollection<BoardsGroupModel> SectionItems
        {
            get
            {
                return m_SectionItems;
            }
            set
            {
                if (value != m_SectionItems)
                {
                    m_SectionItems = value;
                    RaisePropertyChanged("SectionItems");
                }
            }
        }

        public bool IsFavoriteVisible
        {
            get
            {
                return m_IsFavoriteVisible;
            }
            set
            {
                if (value != m_IsFavoriteVisible)
                {
                    m_IsFavoriteVisible = value;
                    RaisePropertyChanged("IsFavoriteVisible");
                }
            }
        }

        public bool IsFavoriteEmpty
        {
            get
            {
                return m_IsFavoriteEmpty;
            }
            set
            {
                if (value != m_IsFavoriteEmpty)
                {
                    m_IsFavoriteEmpty = value;
                    RaisePropertyChanged("IsFavoriteEmpty");
                }
            }
        }

        public bool IsAllBoardsVisible
        {
            get
            {
                return m_IsAllBoardsVisible;
            }
            set
            {
                if (value != m_IsAllBoardsVisible)
                {
                    m_IsAllBoardsVisible = value;
                    RaisePropertyChanged("IsAllBoardsVisible");
                }
            }
        }

        #endregion

        #region [Commands]

        public ICommand SettingsCommand
        {
            get { return m_SettingsCommand ?? (m_SettingsCommand = new DelegateCommand(SettingsExecute)); }
        }

        public ICommand FavoriteCommand
        {
            get { return m_FavoriteCommand ?? (m_FavoriteCommand = new DelegateCommand(FavoriteExecute)); }
        }

        public ICommand AllBoardsCommand
        {
            get { return m_AllBoardsCommand ?? (m_AllBoardsCommand = new DelegateCommand(AllBoardsExecute)); }
        }

        public ICommand BoardSelectedCommand
        {
            get { return m_BoardSelectedCommand ?? (m_BoardSelectedCommand = new DelegateCommand<BoardModel>(BoardSelectedExecute)); }
        }

        #endregion

        #region [Private Methods]

        private void SettingsExecute()
        {
            m_NavigationServiceFacade.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.Relative));
        }

        private void FavoriteExecute()
        {
            LoadFavorites();
        }

        private void AllBoardsExecute()
        {
            IsFavoriteVisible = false;
            IsFavoriteEmpty = false;
            IsAllBoardsVisible = true;
        }

        private void BoardSelectedExecute(BoardModel board)
        {
            m_NavigationServiceFacade.Navigate(
                new Uri("/Views/BoardView.xaml?" + QueryConstants.BoardKey + "=" + board.EnglishName
                + "&" + QueryConstants.DescriptionKey + "=" + board.Description, UriKind.Relative));
        }

        private void LoadFavorites()
        {
            // 登录时载入收藏夹，未登陆时清空
            if (IsLoggedIn)
            {
                CoreService.Instance.GetFavorites(delegate(ObservableCollection<BoardModel> boards, bool success, string error)
                {
                    if (error != null)
                        return;

                    if (boards == null)
                        return;
                    
                    FavoriteItems = boards;

                    try
                    {
                        //将收藏夹保存到本地存储中
                        m_IsolatedStorageFacade.SaveFavorites(FavoriteItems);
                    }
                    catch (Exception)
                    {
                        
                    }

                    if (FavoriteItems == null || FavoriteItems.Count == 0)
                    {
                        IsFavoriteEmpty = true;
                        return;
                    }

                    IsFavoriteVisible = true;
                    IsFavoriteEmpty = false;
                });
            }
            else
            {
                FavoriteItems = null;
                IsFavoriteVisible = false;
                IsFavoriteEmpty = true;
            }

            IsAllBoardsVisible = false;
        }

        private void LoadFavoritesFromIsolatedStorage()
        {
            // 登录时载入收藏夹，未登陆时清空
            if (IsLoggedIn)
            {
                try
                {
                    var favorites = m_IsolatedStorageFacade.GetFavorites();
                    FavoriteItems = favorites;
                }
                catch (Exception)
                {

                }

                if (FavoriteItems == null || FavoriteItems.Count == 0)
                {
                    IsFavoriteEmpty = true;
                    return;
                }

                IsFavoriteVisible = true;
                IsFavoriteEmpty = false;
            }
        }

        private void LoadAllBoards()
        {
            CoreService.Instance.GetAllSections(delegate(ObservableCollection<BoardModel> sections, bool success, string error)
            {
                if (error != null)
                    return;

                if (sections == null)
                    return;

                try
                {
                    m_IsolatedStorageFacade.SaveAllSections(sections);
                }
                catch (Exception)
                {

                }

                ObservableCollection<BoardsGroupModel> newGroup = new ObservableCollection<BoardsGroupModel>();

                foreach (BoardModel section in sections)
                {
                    BoardsGroupModel newItem = new BoardsGroupModel(section.EnglishName);

                    RecurseBoards(section, newItem);

                    //fix typo
                    //去除[上层目录]这一项
                    newItem.RemoveAt(0);
                     
                    newGroup.Add(newItem);
                }

                SectionItems = newGroup;
            });
        }

        private void RecurseBoards(BoardModel board, BoardsGroupModel newItem)
        {
            if (board.Boards == null)
            {
                return;
            }

            foreach (var b in board.Boards)
            {
                if (!b.Leaf)
                {
                    RecurseBoards(b, newItem);
                }

                else
                {
                    newItem.Add(b);
                }
            }
        }

        private void LoadAllBoardsFromIsolatedStorage()
        {
            try
            {
                var sections = m_IsolatedStorageFacade.GetAllSections();

                ObservableCollection<BoardsGroupModel> newGroup = new ObservableCollection<BoardsGroupModel>();

                foreach (BoardModel section in sections)
                {
                    BoardsGroupModel newItem = new BoardsGroupModel(section.EnglishName);
                    foreach (BoardModel board in section.Boards)
                        newItem.Add(board);

                    //fix typo
                    //去除[上层目录]这一项
                    newItem.RemoveAt(0);

                    newGroup.Add(newItem);
                }

                SectionItems = newGroup;
            }
            catch (Exception)
            {

            }
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

        #region [Handlers]

        /// <summary>
        /// Not knowing why, but somehow this method must be declared as public
        /// if is private, a MethodAccessException will be thrown. Weird..
        /// </summary>
        /// <param name="e"></param>
        public void LoggedInEventHandler(EventArgs e)
        {
            LoadFavorites();
            ////Todo
            ////需要判断收藏夹是否是当前选中项
        }

        #endregion
    }
}
