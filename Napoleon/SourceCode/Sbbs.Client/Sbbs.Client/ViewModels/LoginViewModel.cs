/************************************************
 * FileName: LoginViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-09-2013
 *************************************************/

using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Sbbs.Controls;

namespace Sbbs.Client
{
    public class LoginViewModel : NotificationObject, IApplicationBarService
    {
        #region [Fields]

        private string m_Username = string.Empty;

        private string m_Password = string.Empty;

        private ICommand m_LoginCommand;

        private ICommand m_CancelCommand;

        private readonly INavigationServiceFacade m_NavigationServiceFacade;

        private readonly IIsolatedStorageFacade m_IsolatedStorageFacade;

        private readonly IEventAggregator m_EventAggregator;

        private bool m_IsApplicationBarVisible = true;

        // 是否已经登陆
        private bool IsLoggedIn
        {
            get { return !String.IsNullOrEmpty(CoreService.Instance.Token); }
        }

        private bool m_UserLoggedIn = false;

        private string m_LoginText;

        private const string LOGIN = "登陆";

        private const string LOGOUT = "注销";

        #endregion

        #region [Constructors]

        public LoginViewModel(INavigationServiceFacade navigationServiceFacade, 
                              IIsolatedStorageFacade isolatedStorageFacade,
                              IEventAggregator eventAggregator)
        {
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            if (isolatedStorageFacade == null) throw new ArgumentNullException("isolatedStorageFacade");
            if (eventAggregator == null) throw new ArgumentException("eventAggregator");
            m_NavigationServiceFacade = navigationServiceFacade;
            m_IsolatedStorageFacade = isolatedStorageFacade;
            m_EventAggregator = eventAggregator;

            Initialize();
        }

        #endregion

        #region [Properties]

        public string Username
        {
            get { return m_Username; }
            set
            {
                if (value != m_Username)
                {
                    m_Username = value;
                    RaisePropertyChanged("Username");
                }
            }
        }

        public string Password
        {
            get { return m_Password; }
            set
            {
                if (value != m_Password)
                {
                    m_Password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }
        
        public bool UserLoggedIn
        {
            get { return m_UserLoggedIn; }
            set
            {
                if (value != m_UserLoggedIn)
                {
                    m_UserLoggedIn = value;
                    RaisePropertyChanged("UserLoggedIn");
                }
            }
        }

        public string LoginText
        {
            get { return m_LoginText; }
            set
            {
                if (value != m_LoginText)
                {
                    m_LoginText = value;
                    RaisePropertyChanged("LoginText");
                }
            }
        }

        #endregion

        #region [Commands]

        public ICommand LoginCommand
        {
            get { return m_LoginCommand ?? (m_LoginCommand = new DelegateCommand(LoginExecute)); }
        }

        public ICommand CancelCommand
        {
            get { return m_CancelCommand ?? (m_CancelCommand = new DelegateCommand(CancelExecute)); }
        }

        #endregion

        #region [Private Methods]

        private void LoginExecute()
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                else
                {
                    Logout();
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void CancelExecute()
        {
            m_NavigationServiceFacade.GoBack();
        }

        private void Login()
        {
            CoreService.Instance.Login(Username, Password, delegate(string token, bool success, string error)
            {
                if (error == null)
                {
                    m_NavigationServiceFacade.GoBack();

                    // 保存获得的Token
                    CoreService.Instance.Token = token;
                    UserLoggedIn = true;
                    LoginText = LOGOUT;
                    
                    var loggedInEvent = m_EventAggregator.GetEvent<LoggedInEvent>();
                    loggedInEvent.Publish(new EventArgs());

                    m_IsolatedStorageFacade.SaveToken(token);
                    m_IsolatedStorageFacade.SaveUsername(Username);
                }
                else if (!success)
                    MessageBox.Show("网络错误");
                else
                    MessageBox.Show("用户名密码错误");
            });
        }

        private void Logout()
        {
            m_NavigationServiceFacade.GoBack();

            CoreService.Instance.Token = null;
            UserLoggedIn = false;
            LoginText = LOGIN;

            Username = string.Empty;
            Password = string.Empty;

            var loggedInEvent = m_EventAggregator.GetEvent<LoggedInEvent>();
            loggedInEvent.Publish(new EventArgs());

            m_IsolatedStorageFacade.SaveToken(null);
            m_IsolatedStorageFacade.SaveUsername(null);
        }

        private void Initialize()
        {
            try
            {
                if (!IsLoggedIn)
                {
                    UserLoggedIn = false;
                    LoginText = LOGIN;
                }
                else
                {
                    UserLoggedIn = true;
                    LoginText = LOGOUT;

                    var username = m_IsolatedStorageFacade.GetUsername();
                    Username = username;
                }
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
    }
}
