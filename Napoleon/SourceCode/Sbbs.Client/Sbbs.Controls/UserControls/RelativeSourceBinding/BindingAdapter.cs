﻿/************************************************
 * FileName: BindingAdapter.cs
 * Document-related:
 * Module: Sbbs.Controls
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-21-2013
 * Refer to:http://www.cnblogs.com/zhouhoujun/archive/2011/07/18/2109067.html
 *************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Sbbs.Controls
{
    [ContentProperty("Binding")]
    public static class BindingAdapter
    {
        #region Binding (Attached DependencyProperty)

        public static RelativeSourceBase GetBinding(DependencyObject obj)
        {
            return (RelativeSourceBase)obj.GetValue(BindingProperty);
        }

        public static void SetBinding(DependencyObject obj, RelativeSourceBase value)
        {
            obj.SetValue(BindingProperty, value);
        }

        public static readonly DependencyProperty BindingProperty = DependencyProperty.RegisterAttached("Binding", typeof(RelativeSourceBase), typeof(BindingAdapter), new PropertyMetadata(null, OnBinding));

        private static void OnBinding(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement targetElement = depObj as FrameworkElement;

            if (targetElement != null)
            {
                // attach loading event
                targetElement.Loaded += new RoutedEventHandler(targetElement_Loaded);
                targetElement.Unloaded += new RoutedEventHandler(targetElement_Unloaded);
            }
        }

        #endregion

        #region Private methods

        private static void targetElement_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement targetElement = sender as FrameworkElement;

                // release handler to prevent memory leaks
                targetElement.Loaded -= new RoutedEventHandler(targetElement_Loaded);

                RelativeSourceBase bindings = GetBinding(targetElement);


                if (bindings is RelativeSourceBinding)
                {
                    // get the binding configuration
                    RelativeSourceBinding bindingConfiguration = bindings as RelativeSourceBinding;
                    ProcessBinding(targetElement, bindingConfiguration);
                }
                else if (bindings is BindingList)
                {
                    // get the binding configuration
                    BindingList list = bindings as BindingList;

                    foreach (RelativeSourceBinding bindingConfiguration in list)
                    {
                        ProcessBinding(targetElement, bindingConfiguration);
                    }
                }

            }
            catch (Exception ex)
            {
                // ignore this exception, because the SL binding engine does not throw exceptions when a binding is wrong.
            }
        }

        static void targetElement_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement targetElement = sender as FrameworkElement;
                targetElement.Unloaded -= new RoutedEventHandler(targetElement_Unloaded);

                RelativeSourceBase bindings = GetBinding(targetElement);

                if (bindings is RelativeSourceBinding)
                {
                    // get the binding configuration
                    RelativeSourceBinding bindingConfiguration = bindings as RelativeSourceBinding;
                    if (bindingConfiguration.TargetDependencyProperty != null)
                        targetElement.ClearValue(bindingConfiguration.TargetDependencyProperty);
                }
                else if (bindings is BindingList)
                {
                    // get the binding configuration
                    BindingList list = bindings as BindingList;

                    foreach (RelativeSourceBinding bindingConfiguration in list)
                    {
                        if (bindingConfiguration.TargetDependencyProperty != null)
                            targetElement.ClearValue(bindingConfiguration.TargetDependencyProperty);
                    }
                }
            }
            catch
            {
            }
        }

        private static void ProcessBinding(FrameworkElement targetElement, RelativeSourceBinding bindingConfiguration)
        {

            if (bindingConfiguration.RelativeMode == RelativeSourceMode.FindAncestor &&
                !string.IsNullOrEmpty(bindingConfiguration.AncestorType))
            {
                // navigate up the tree to find the type
                DependencyObject currentObject = VisualTreeHelper.GetParent(targetElement);

                DependencyObject candidate = null;
                DependencyObject ancestor = null;

                while (true)
                {
                    if (currentObject == null)
                    {
                        break;
                    }

                    Type currentType = currentObject.GetType();

                    while (currentType != null && currentType.IsSubclassOf(typeof(DependencyObject)))
                    {
                        if (currentType.FullName == bindingConfiguration.AncestorType)
                        {
                            ancestor = currentObject;
                            break;
                        }

                        // for types in assemblies System.Windows, System.Windows.Controls, System.Windows.Controls.Data, etc, 
                        // its possible to define just the class name instead of the full class name including the namespace.
                        if (candidate == null && currentType.Name == bindingConfiguration.AncestorType && currentType.Assembly.FullName.StartsWith("System.Windows"))
                        {
                            // the name of the element is matching, but it is not the fullname.
                            // remeber the element in case if no element is matching to the ancestor type name
                            candidate = currentObject;
                        }

                        // next type up the hierarchy
                        currentType = currentType.BaseType;
                    }

                    // next parent                    
                    currentObject = VisualTreeHelper.GetParent(currentObject);
                }

                // concrete
                if (ancestor == null)
                {
                    ancestor = candidate;
                }

                if (ancestor != null && ancestor is FrameworkElement)
                {
                    // bind them
                    CreateBinding(targetElement, ancestor, bindingConfiguration);
                }
            }
            else if (bindingConfiguration.RelativeMode == RelativeSourceMode.ParentDataContext)
            {
                object currentDataContext = targetElement.DataContext;

                // navigate up the tree to find the parent datacontext
                DependencyObject currentObject = VisualTreeHelper.GetParent(targetElement);

                while (true)
                {
                    if (currentObject == null)
                        break;

                    FrameworkElement fe = currentObject as FrameworkElement;

                    if (fe != null)
                    {
                        if (fe.DataContext != null && fe.DataContext != currentDataContext)
                        {
                            // bind them
                            CreateBinding(targetElement, fe.DataContext, bindingConfiguration);
                            break;
                        }
                    }

                    // next parent                    
                    currentObject = VisualTreeHelper.GetParent(currentObject);
                }

            }
        }

        private static List<string> GetClassNames(Type type)
        {
            List<string> result = new List<string>();

            // check
            if (type == null && type.IsSubclassOf(typeof(DependencyObject)))
                return result;

            // process
            do
            {
                result.Add(type.FullName);
                type = type.BaseType;
            } while (type != null && type.IsSubclassOf(typeof(DependencyObject)));

            // return
            return result;
        }

        private static void CreateBinding(FrameworkElement targetElement, object sourceElement, RelativeSourceBinding bindingConfiguration)
        {
            // input check
            if (targetElement == null)
                return;
            if (sourceElement == null)
                return;
            if (bindingConfiguration == null)
                return;

            // check binding configuration
            // ...target property must be set
            //if (bindingConfiguration.TargetProperty.IsNullOrWhiteSpace())
            if (String.IsNullOrEmpty(bindingConfiguration.TargetProperty) || bindingConfiguration.TargetProperty.Trim().Length == 0)
                return;
            // ...path property must be set
            //if (bindingConfiguration.Path.IsNullOrWhiteSpace())
            if (String.IsNullOrEmpty(bindingConfiguration.Path) || bindingConfiguration.Path.Trim().Length == 0)
                return;



            // support of attached property binding syntax: TargetProperty='(Grid.Row)'
            string targetPropertyName = (bindingConfiguration.TargetProperty + "").Trim().TrimStart('(').TrimEnd(')') + "Property";

            // find the target dependency property
            DependencyProperty targetDependencyProperty = null;
            if (targetPropertyName.Contains("."))
            {
                // it is an attached dependency property
                string[] parts = targetPropertyName.Split('.');

                //if (parts.Length == 2 && !parts[0].IsNullOrWhiteSpace() && !parts[1].IsNullOrWhiteSpace())
                if (parts.Length == 2 && !(String.IsNullOrEmpty(parts[0]) || parts[0].Trim().Length == 0) && !(String.IsNullOrEmpty(parts[1]) || parts[1].Trim().Length == 0))
                {
                    Type attachedType = TypeLoader.GetType(parts[0], bindingConfiguration.TargetNamespace);

                    if (attachedType != null)
                    {
                        FieldInfo[] targetFields = attachedType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        FieldInfo targetDependencyPropertyField = targetFields.FirstOrDefault(i => i.Name == parts[1]);
                        if (targetDependencyPropertyField != null)
                            targetDependencyProperty = targetDependencyPropertyField.GetValue(null) as DependencyProperty;
                    }
                }
            }
            else
            {
                // it is a standard dependency property
                FieldInfo[] targetFields = targetElement.GetType().GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                FieldInfo targetDependencyPropertyField = targetFields.FirstOrDefault(i => i.Name == targetPropertyName);

                if (targetDependencyPropertyField != null)
                    targetDependencyProperty = targetDependencyPropertyField.GetValue(null) as DependencyProperty;
            }


            // set binding
            if (targetDependencyProperty != null)
            {

                if (bindingConfiguration.TargetDependencyProperty != null
                    && bindingConfiguration.TargetDependencyProperty != targetDependencyProperty)
                    targetElement.ClearValue(bindingConfiguration.TargetDependencyProperty);

                bindingConfiguration.TargetDependencyProperty = targetDependencyProperty;


                Binding binding = new Binding();
                binding.Source = sourceElement;
                binding.Path = new PropertyPath(bindingConfiguration.Path);
                binding.Mode = bindingConfiguration.BindingMode;
                binding.Converter = bindingConfiguration.Converter;
                binding.ConverterParameter = bindingConfiguration.ConverterParameter;
                binding.ConverterCulture = bindingConfiguration.ConverterCulture;
                binding.NotifyOnValidationError = bindingConfiguration.NotifyOnValidationError;
#if !WINDOWS_PHONE
                binding.ValidatesOnDataErrors = bindingConfiguration.ValidatesOnDataErrors;
#endif
                binding.ValidatesOnExceptions = bindingConfiguration.ValidatesOnExceptions;
#if !WINDOWS_PHONE
                binding.ValidatesOnNotifyDataErrors = bindingConfiguration.ValidatesOnNotifyDataErrors;
#endif
                // set the binding on our target element
                targetElement.SetBinding(targetDependencyProperty, binding);
            }
        }

        #endregion
    }
}
