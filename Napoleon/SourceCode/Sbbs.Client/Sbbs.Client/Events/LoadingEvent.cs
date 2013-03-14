/************************************************
 * FileName: LoadingEvent.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-06-2013
 *************************************************/

using System;
using Microsoft.Practices.Prism.Events;

namespace Sbbs.Client
{
    /// <summary>
    /// The event is raised when TopTen or HotTopics start or stop loading.
    /// </summary>
    public class LoadingEvent : CompositePresentationEvent<LoadingEventArgs> { }

    public class LoadingEventArgs : EventArgs
    {
        public bool IsLoading
        {
            get; private set;
        }

        public LoadingEventArgs(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }
}
