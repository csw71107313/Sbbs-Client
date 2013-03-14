/************************************************
 * FileName: BoardSettingsViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-08-2013
 *************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Sbbs.Client
{
    public class BoardSettingsViewModel : NotificationObject
    {
        #region [Private Fields]

        private bool m_IsNewInstance;
        private Mode m_SelectedMode;
        private ICommand m_ModeSelectedCommand;

        private readonly IModeService m_ModeService;
        private readonly INavigationServiceFacade m_NavigationServiceFacade;
        private readonly IIsolatedStorageFacade m_IsolatedStorageFacade;

        #endregion

        #region [Constructors]

        public BoardSettingsViewModel(IModeService modeService, 
                                      INavigationServiceFacade navigationServiceFacade, 
                                      IIsolatedStorageFacade isolatedStorageFacade)
        {
            if (modeService == null) throw new ArgumentNullException("modeService");
            if (navigationServiceFacade == null) throw new ArgumentNullException("navigationServiceFacade");
            if (isolatedStorageFacade == null) throw new ArgumentNullException("isolatedStorageFacade");
            m_ModeService = modeService;
            m_NavigationServiceFacade = navigationServiceFacade;
            m_IsolatedStorageFacade = isolatedStorageFacade;

            m_IsNewInstance = true;
        }

        #endregion

        #region [Properties]

        public IList<Mode> Modes
        {
            get { return m_ModeService.ModeSettings.Modes; }
        }

        public Mode SelectedMode
        {
            get
            {
                return m_SelectedMode;
            }
            set
            {
                if (value != m_SelectedMode)
                {
                    m_SelectedMode = value;
                    RaisePropertyChanged("SelectedMode");
                }
            }
        }

        #endregion

        #region [Commands]

        public ICommand ModeSelectedCommand
        {
            get { return m_ModeSelectedCommand ?? (m_ModeSelectedCommand = new DelegateCommand<Mode>(ModeSelectedExecute)); }
        }

        #endregion

        #region [Private Methods]

        private void ModeSelectedExecute(Mode mode)
        {
            if (mode != null)
            {
                SelectedMode = mode;
                m_ModeService.ModeSettings.SetCurrentMode(mode.Id);
                m_IsolatedStorageFacade.SaveBoardMode(mode.Id);
                m_NavigationServiceFacade.GoBack();
            }
        }

        private void SetSelectedMode()
        {
            SelectedMode = m_ModeService.ModeSettings.CurrentMode;
        }

        #endregion

        #region [Public Methods]

        public void NavigatedTo()
        {
            if (m_IsNewInstance)
            {
                SetSelectedMode();
            }
            m_IsNewInstance = false;
        }

        #endregion
    }
}
