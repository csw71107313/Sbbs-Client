/************************************************
 * FileName: RoundButton.xaml.cs
 * Document-related:
 * Module: Sbbs.Controls
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-08-2013
 * Refer to:http://www.cnblogs.com/kiminozo/archive/2012/01/26/2329621.html
 *************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sbbs.Controls
{
    public partial class RoundButton : Button
    {
        protected ImageBrush OpacityImageBrush; 
        private const string OpacityImageBrushName = "OpacityImageBrush"; 

        public RoundButton()
        {
            DefaultStyleKey = typeof(RoundButton);
        } 

        public readonly DependencyProperty ImageSourceProperty = 
            DependencyProperty.Register("ImageSource", 
            typeof(ImageSource), 
            typeof(RoundButton), 
            new PropertyMetadata(OnImageSourceChanged)); 
        
        private static void OnImageSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as RoundButton; 
            if (sender == null || e.NewValue == e.OldValue)               
                return; 
            sender.SetImageBrush(e.NewValue as ImageSource);
        }
        
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); } 
            set { SetValue(ImageSourceProperty, value); }
        } 
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate(); 
            OpacityImageBrush = GetTemplateChild(OpacityImageBrushName) as ImageBrush; 

            if (ImageSource == null)
                ImageSource = new BitmapImage(new Uri("/Sbbs.Controls;component/Resources/Images/default.png", UriKind.RelativeOrAbsolute)); 
            else                
                SetImageBrush(ImageSource);
        } 
        
        private void SetImageBrush(ImageSource imageSource)
        {
            if (OpacityImageBrush == null)               
                return; 
            
            OpacityImageBrush.ImageSource = imageSource;
        }
    }
}
