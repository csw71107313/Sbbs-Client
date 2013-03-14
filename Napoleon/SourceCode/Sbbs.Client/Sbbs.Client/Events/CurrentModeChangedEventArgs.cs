/************************************************
 * FileName: CurrentModeChangedEventArgs.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-08-2013
 *************************************************/

using System;

namespace Sbbs.Client
{
    public class CurrentModeChangedEventArgs : EventArgs
    {
        private readonly Mode m_CurrentMode;

        /// <summary>
        /// Gets the current mode.
        /// </summary>
        public Mode CurrentMode
        {
            get { return m_CurrentMode; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentModeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="currentMode">The current mode.</param>
        public CurrentModeChangedEventArgs(Mode currentMode)
        {
            if (currentMode == null) throw new ArgumentNullException("currentMode");
            this.m_CurrentMode = currentMode;
        }
    }
}
