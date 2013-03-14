/************************************************
 * FileName: SettingsViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System;
using Microsoft.Practices.Prism.ViewModel;

namespace Sbbs.Client
{
    public class SettingsViewModel : NotificationObject
    {
        #region [Fields]

        private readonly INavigationServiceFacade m_NavigationServiceFacade;

        private readonly LoginViewModel m_LoginViewModel;

        private readonly AboutViewModel m_AboutViewModel;

        #endregion

        #region [Constructors]

        public SettingsViewModel(INavigationServiceFacade navigationServiceFacade, LoginViewModel loginViewModel, AboutViewModel aboutViewModel)
        {
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            if (loginViewModel == null) throw new ArgumentNullException("loginViewModel");
            if (aboutViewModel == null) throw new ArgumentNullException("aboutViewModel");
            m_NavigationServiceFacade = navigationServiceFacade;
            m_LoginViewModel = loginViewModel;
            m_AboutViewModel = aboutViewModel;
        }

        #endregion    

        #region [Properties]

        /// <summary>
        /// Gets the Login view model.
        /// </summary>
        public LoginViewModel LoginViewModel
        {
            get { return m_LoginViewModel; }
        }


        /// <summary>
        /// Gets the About view model.
        /// </summary>
        public AboutViewModel AboutViewModel
        {
            get { return m_AboutViewModel; }
        }

        #endregion
    }
}
