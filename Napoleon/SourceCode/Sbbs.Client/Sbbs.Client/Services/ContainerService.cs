/************************************************
 * FileName: ContainerService.cs
 * Document-related:
 * Module: Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System;
using System.Windows;
using Funq;
using Microsoft.Practices.Prism.Events;

namespace Sbbs.Client
{
    /// <summary>
    /// Represents a container service that wraps the Funq container
    /// </summary>
    public class ContainerService : IDisposable
    {
        private bool m_Disposed;

        /// <summary>
        /// Gets the container.
        /// </summary>
        public Container Container { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sbbs.Client.ContainerService"/> class.
        /// </summary>
        public ContainerService()
        {
            this.Container = new Container();
            this.ConfigureContainer();
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
                this.Container.Dispose();
            }

            m_Disposed = true;
        }

        private void ConfigureContainer()
        {
            this.Container.Register<INavigationServiceFacade>(c => new NavigationServiceFacade(((App)Application.Current).RootFrame));
            this.Container.Register<IIsolatedStorageFacade>(c => new IsolatedStorageFacade());
            this.Container.Register<IEventAggregator>(c => new EventAggregator());

            this.Container.Register<IModeService>(
                c => new ModeService(c.Resolve<IIsolatedStorageFacade>()));

            this.Container.Register<LoadingViewModel>(
                c => new LoadingViewModel(
                    c.Resolve<IIsolatedStorageFacade>(),
                    c.Resolve<IEventAggregator>()));

            this.Container.Register<TopTenViewModel>(
                c => new TopTenViewModel(
                    c.Resolve<INavigationServiceFacade>(),
                    c.Resolve<IIsolatedStorageFacade>(),
                    c.Resolve<IEventAggregator>()));

            this.Container.Register<HotTopicsViewModel>(
                c => new HotTopicsViewModel(
                    c.Resolve<INavigationServiceFacade>(),
                    c.Resolve<IIsolatedStorageFacade>(),
                    c.Resolve<IEventAggregator>()));

            this.Container.Register<BoardsViewModel>(
                c => new BoardsViewModel(
                    c.Resolve<INavigationServiceFacade>(),
                    c.Resolve<IIsolatedStorageFacade>(),
                    c.Resolve<IEventAggregator>()));

            this.Container.Register<LoginViewModel>(
                c => new LoginViewModel(
                    c.Resolve<INavigationServiceFacade>(), 
                    c.Resolve<IIsolatedStorageFacade>(),
                    c.Resolve<IEventAggregator>()));

            this.Container.Register<AboutViewModel>(
                c => new AboutViewModel());

            this.Container.Register<SettingsViewModel>(
                c => new SettingsViewModel(
                    c.Resolve<INavigationServiceFacade>(), 
                    c.Resolve<LoginViewModel>(),
                    c.Resolve<AboutViewModel>()));

            this.Container.Register<TopicViewModel>(
                c => new TopicViewModel(
                    c.Resolve<INavigationServiceFacade>()));

            this.Container.Register<PostViewModel>(
                c => new PostViewModel(
                    c.Resolve<INavigationServiceFacade>()));

            this.Container.Register<BoardViewModel>(
                c => new BoardViewModel(
                    c.Resolve<IModeService>(),
                    c.Resolve<INavigationServiceFacade>()));

            this.Container.Register<BoardSettingsViewModel>(
                c => new BoardSettingsViewModel(
                    c.Resolve<IModeService>(),
                    c.Resolve<INavigationServiceFacade>(),
                    c.Resolve<IIsolatedStorageFacade>()));
        }
    }
}
