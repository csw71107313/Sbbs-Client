/************************************************
 * FileName: LoadMoreListBox.xmal.cs
 * Document-related:
 * Module: Sbbs.Controls
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-06-2013
 *************************************************/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sbbs.Controls
{
    public partial class LoadMoreListBox : ListBox
    {
        public LoadMoreListBox()
        {
            DefaultStyleKey = typeof(LoadMoreListBox);
        }

        private ScrollViewer m_ScrollViewer;

        public ScrollViewer ScrollViewer
        {
            get { return m_ScrollViewer; }
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_ScrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");
        }

        #region [Properties]

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public Visibility IsFullyLoaded
        {
            get { return (Visibility)GetValue(IsFullyLoadedProperty); }
            set { SetValue(IsFullyLoadedProperty, value); }
        }

        public bool CanLoadMore
        {
            get { return (bool)GetValue(CanLoadMoreProperty); }
            set { SetValue(CanLoadMoreProperty, value); }
        }

        public double LineWidth
        {
            get { return (double)GetValue(LineWidthProperty); }
            set { SetValue(LineWidthProperty, value); }
        }

        public double MarginLeft
        {
            get { return (double)GetValue(MarginLeftProperty); }
            set { SetValue(MarginLeftProperty, value); }
        }

        #endregion

        #region [Dependency Properties Implementation]

        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty CommandProperty;
        public static readonly DependencyProperty IsFullyLoadedProperty;
        public static readonly DependencyProperty CanLoadMoreProperty;
        public static readonly DependencyProperty LineWidthProperty;
        public static readonly DependencyProperty MarginLeftProperty;

        static LoadMoreListBox()
        {
            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LoadMoreListBox), null);
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(LoadMoreListBox), null);
            IsFullyLoadedProperty = DependencyProperty.Register("IsFullyLoaded", typeof(Visibility), typeof(LoadMoreListBox), new PropertyMetadata(Visibility.Collapsed));
            CanLoadMoreProperty = DependencyProperty.Register("CanLoadMore", typeof(bool), typeof(LoadMoreListBox), new PropertyMetadata(true));
            LineWidthProperty = DependencyProperty.Register("LineWidth", typeof(double), typeof(LoadMoreListBox), new PropertyMetadata(0.0));
            MarginLeftProperty = DependencyProperty.Register("MarginLeft", typeof(double), typeof(LoadMoreListBox), new PropertyMetadata(0.0));
        }

        #endregion
    }
}
