/************************************************
 * FileName: ViewModelLocator.cs
 * Document-related:
 * Module: Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System;

namespace Sbbs.Client
{
    /// <summary>
    /// Represents a view model locator
    /// </summary>
    public class ViewModelLocator : IDisposable
    {
        private readonly ContainerService m_ContainerService;
        private bool m_Disposed;

        /// <summary>
        /// Gets the Loading view model.
        /// </summary>
        public LoadingViewModel LoadingViewModel
        {
            get { return m_ContainerService.Container.Resolve<LoadingViewModel>(); }
        }

        /// <summary>
        /// Gets the Top Ten view model.
        /// </summary>
        public TopTenViewModel TopTenViewModel
        {
            get { return m_ContainerService.Container.Resolve<TopTenViewModel>(); }
        }

        /// <summary>
        /// Gets the Hot Topics view model.
        /// </summary>
        public HotTopicsViewModel HotTopicsViewModel
        {
            get { return m_ContainerService.Container.Resolve<HotTopicsViewModel>(); }
        }

        /// <summary>
        /// Gets the Boards view model.
        /// </summary>
        public BoardsViewModel BoardsViewModel
        {
            get { return m_ContainerService.Container.Resolve<BoardsViewModel>(); }
        }

        /// <summary>
        /// Gets the Settings view model.
        /// </summary>
        public SettingsViewModel SettingsViewModel
        {
            get { return m_ContainerService.Container.Resolve<SettingsViewModel>(); }
        }

        /// <summary>
        /// Gets the Topic view model.
        /// </summary>
        public TopicViewModel TopicViewModel
        {
            get { return m_ContainerService.Container.Resolve<TopicViewModel>(); }
        }

        /// <summary>
        /// Gets the Post view model.
        /// </summary>
        public PostViewModel PostViewModel
        {
            get { return m_ContainerService.Container.Resolve<PostViewModel>(); }
        }

        /// <summary>
        /// Gets the Board view model.
        /// </summary>
        public BoardViewModel BoardViewModel
        {
            get { return m_ContainerService.Container.Resolve<BoardViewModel>(); }
        }

        /// <summary>
        /// Gets the Board settings view model.
        /// </summary>
        public BoardSettingsViewModel BoardSettingsViewModel
        {
            get { return m_ContainerService.Container.Resolve<BoardSettingsViewModel>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sbbs.Client.ViewModelLocator"/> class.
        /// </summary>
        public ViewModelLocator()
        {
            m_ContainerService = new ContainerService();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                m_ContainerService.Dispose();
            }

            m_Disposed = true;
        }
    }
}
