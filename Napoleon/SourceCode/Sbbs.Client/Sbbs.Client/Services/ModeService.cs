/************************************************
 * FileName: ModeService.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-08-2013
 *************************************************/

using System;

namespace Sbbs.Client
{
    public class ModeService : IModeService
    {
        private readonly IIsolatedStorageFacade m_IsolatedStorageFacade;

        private ModeSettings m_ModeSetting;

        public ModeSettings ModeSettings
        {
            get
            {
                if (m_ModeSetting != null)
                {
                    return m_ModeSetting;
                }
                m_ModeSetting = new ModeSettings();

                try
                {
                    var currentModeId = m_IsolatedStorageFacade.GetBoardMode();
                    m_ModeSetting.SetCurrentMode(currentModeId);
                }
                catch (Exception)
                {
                    m_ModeSetting.SetCurrentMode(CoreService.Instance.BoardMode);
                }

                return m_ModeSetting;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeService"/> class.
        /// </summary>
        /// <param name="isolatedStorageFacade">The isolated storage facade.</param>
        public ModeService(IIsolatedStorageFacade isolatedStorageFacade)
        {
            if (isolatedStorageFacade == null) throw new ArgumentNullException("isolatedStorageFacade");
            m_IsolatedStorageFacade = isolatedStorageFacade;
        }
    }
}
