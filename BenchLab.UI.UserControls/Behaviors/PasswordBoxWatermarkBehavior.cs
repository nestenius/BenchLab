﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace BenchLab.UI.UserControls
{
    public sealed class PasswordBoxWatermarkBehavior
        : System.Windows.Interactivity.Behavior<PasswordBox>
    {
        #region · Attached Properties ·

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.RegisterAttached("Label", typeof(string), typeof(PasswordBoxWatermarkBehavior));

        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.RegisterAttached("LabelStyle", typeof(Style), typeof(PasswordBoxWatermarkBehavior));

        #endregion

        #region · Fields ·

        private TextBlockAdorner adorner;
        private WeakPropertyChangeNotifier notifier;

        #endregion

        #region · Properties ·

        public string Label
        {
            get { return (string)base.GetValue(LabelProperty); }
            set { base.SetValue(LabelProperty, value); }
        }

        public Style LabelStyle
        {
            get { return (Style)base.GetValue(LabelStyleProperty); }
            set { base.SetValue(LabelStyleProperty, value); }
        }

        #endregion

        #region · Overriden Methods ·

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Loaded += this.AssociatedObjectLoaded;
            this.AssociatedObject.PasswordChanged += this.AssociatedObjectPasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= this.AssociatedObjectLoaded;
            this.AssociatedObject.PasswordChanged -= this.AssociatedObjectPasswordChanged;

            this.notifier = null;
        }

        #endregion

        #region · Private Methods ·

        private void AssociatedObjectPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            this.UpdateAdorner();
        }

        private void AssociatedObjectLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.adorner = new TextBlockAdorner(this.AssociatedObject, this.Label, this.LabelStyle);

            this.UpdateAdorner();

            //AddValueChanged for IsFocused in a weak manner
            this.notifier = new WeakPropertyChangeNotifier(this.AssociatedObject, UIElement.IsFocusedProperty);
            this.notifier.ValueChanged += new EventHandler(this.UpdateAdorner);
        }

        private void UpdateAdorner(object sender, EventArgs e)
        {
            this.UpdateAdorner();
        }

        private void UpdateAdorner()
        {
            if (!String.IsNullOrEmpty(this.AssociatedObject.Password) || this.AssociatedObject.IsFocused)
            {
                // Hide the Watermark Label if the adorner layer is visible
                this.AssociatedObject.TryRemoveAdorners<TextBlockAdorner>();
            }
            else
            {
                // Show the Watermark Label if the adorner layer is visible
                this.AssociatedObject.TryAddAdorner<TextBlockAdorner>(adorner);
            }
        }

        #endregion
    }
}
