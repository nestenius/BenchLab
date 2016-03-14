﻿using System.Windows;
using System.Windows.Interactivity;
using BenchLab.SimpleUI.Entities;
using GalaSoft.MvvmLight.Messaging;

namespace BenchLab.UI.UserControls
{
    public class VMModalDialogBehavior : Behavior<FrameworkElement>
    {
        readonly IMessenger _messenger = Messenger.Default;

        protected override void OnAttached()
        {
            base.OnAttached();

            _messenger.Register<VMMessageDailog>(this, Identifier, ShowDialog);
        }

        public string Identifier { get; set; }

        private void ShowDialog(VMMessageDailog dm)
        {
            var messageWindow = new ModalDailogWindow(dm);
            messageWindow.ShowDialog();
            //dm.Callback(messageWindow.DialogResult);
        }
    }
}