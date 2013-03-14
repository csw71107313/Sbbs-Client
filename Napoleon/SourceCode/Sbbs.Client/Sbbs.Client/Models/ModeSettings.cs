/************************************************
 * FileName: ModeSettings.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-08-2013
 *************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sbbs.Client
{
    public class ModeSettings
    {
        /// <summary>
        /// Occurs when the current mode is changed.
        /// </summary>
        public event EventHandler<CurrentModeChangedEventArgs> CurrentModeChanged;

        /// <summary>
        /// Gets the current mode.
        /// </summary>
        /// <value>The current mode.</value>
        public Mode CurrentMode { get; private set; }

        /// <summary>
        /// Gets or sets the modes.
        /// </summary>
        /// <value>The modes being tracked by this program.</value>
        public IList<Mode> Modes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeSettings"/> class.
        /// </summary>
        public ModeSettings()
        {
            InitializeModes();
        }

        /// <summary>
        /// Sets the current mode.
        /// </summary>
        /// <param name="modeId">The current mode id.</param>
        public void SetCurrentMode(int modeId)
        {
            this.CurrentMode = this.Modes.Where(m => m.Id == modeId).Single();
            CoreService.Instance.BoardMode = CurrentMode.Id;

            OnCurrentModeChanged(this.CurrentMode);
        }

        protected void OnCurrentModeChanged(Mode currentMode)
        {
            var handler = this.CurrentModeChanged;
            if (handler != null)
            {
                handler(this, new CurrentModeChangedEventArgs(currentMode));
            }
        }

        /// <summary>
        /// Initialize all modes and set the default mode.
        /// </summary>
        private void InitializeModes()
        {
            this.Modes = new List<Mode>();

            var regularMode = new Mode(0, "普通模式");
            var topicMode = new Mode(1, "主题模式");
            var forumMode = new Mode(2, "论坛模式");

            this.Modes.Add(regularMode);
            this.Modes.Add(topicMode);
            this.Modes.Add(forumMode);

            //foreach (var mode in Modes)
            //{
            //    if (mode.Id == CoreService.Instance.BoardMode)
            //    {
            //        CurrentMode = mode;
            //    }
            //}
        }
    }
}
