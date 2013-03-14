/************************************************
 * FileName: LoadingViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-06-2013
 *************************************************/

using System;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace Sbbs.Client
{
    public class LoadingViewModel : NotificationObject
    {
        #region [Private Fields]

        private bool m_IsLoading = false;
        private readonly IEventAggregator m_EventAggregator;
        private readonly IIsolatedStorageFacade m_IsolatedStorageFacade;

        #endregion

        #region [Constructors]

        public LoadingViewModel(IIsolatedStorageFacade isolatedStorageFacade, IEventAggregator eventAggregator)
        {
            if (isolatedStorageFacade == null) throw new ArgumentNullException("isolatedStorageFacade");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
            m_IsolatedStorageFacade = isolatedStorageFacade;
            m_EventAggregator = eventAggregator;

            try
            {
                CoreService.Instance.Token = m_IsolatedStorageFacade.GetToken();
            }
            catch(Exception)
            {
                
            }

            var loadingEvent = m_EventAggregator.GetEvent<LoadingEvent>();
            loadingEvent.Subscribe(LoadingEventHandler);
        }

        #endregion

        #region [Properties]

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

        #endregion

        #region [Handlers]

        public void LoadingEventHandler(LoadingEventArgs e)
        {
            IsLoading = e.IsLoading;
        }

        #endregion
    }
}
