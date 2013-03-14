/************************************************
 * FileName: PostViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-18-2013
 *************************************************/

using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Sbbs.Core;

namespace Sbbs.Client
{
    public class PostViewModel : NotificationObject
    {
        #region [Private Fields]

        private string m_Board;
        private string m_Title;
        private int m_ReId;
        private string m_Content;

        private ICommand m_PostCommand;
        private ICommand m_AttachCommand;

        private readonly INavigationServiceFacade m_NavigationServiceFacade;

        #endregion

        #region [Constructors]

        public PostViewModel(INavigationServiceFacade navigationServiceFacade)
        {
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            m_NavigationServiceFacade = navigationServiceFacade;
        }

        #endregion

        #region [Properties]

        /// <summary>
        /// 版面
        /// </summary>
        public string Board
        {
            get { return m_Board; }
            set
            {
                if (value != m_Board)
                {
                    m_Board = value;
                    RaisePropertyChanged("Board");
                }
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set
            {
                if (value != m_Title)
                {
                    m_Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Id
        /// </summary>
        public int ReId
        {
            get { return m_ReId; }
            set
            {
                if (value != m_ReId)
                {
                    m_ReId = value;
                    RaisePropertyChanged("ReId");
                }
            }
        }

        /// <summary>
        /// 正文
        /// </summary>
        public string Content
        {
            get { return m_Content; }
            set
            {
                if (value != m_Content)
                {
                    m_Content = value;
                    RaisePropertyChanged("Content");
                }
            }
        }

        #endregion

        #region [Commands]

        public ICommand PostCommand
        {
            get { return m_PostCommand ?? (m_PostCommand = new DelegateCommand(PostExecute)); }
        }

        public ICommand AttachCommand
        {
            get { return m_AttachCommand ?? (m_AttachCommand = new DelegateCommand(AttachExecute)); }
        }

        #endregion

        #region [Private Methods]

        private void PostExecute()
        {
            CoreService.Instance.PostTopic(Board, ReId, Title, Content, 
                delegate(TopicModel topic, bool success, string error)
                    {
                        if (!success)
                            MessageBox.Show("网络错误");
                        else if (error != null)
                            MessageBox.Show(error);
                        else
                        {
                            m_NavigationServiceFacade.GoBack();
                            RefreshService.NeedRefresh = true;
                        }
                    });
            //Reset Content
            Content = string.Empty;
        }

        private void AttachExecute()
        {
            ////Todo
        }

        #endregion
    }
}
