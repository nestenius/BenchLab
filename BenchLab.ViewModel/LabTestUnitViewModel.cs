﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.DataAccess;
using BenchLab.ErrorLog;
using BenchLab.Model;
using BenchLab.Resources;
using BenchLab.SimpleUI.Entities;

namespace BenchLab.ViewModel
{
    public class LabTestUnitViewModel : BaseViewModel<LabTestUnitCollection>
    {
        #region Fields


        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LabTestUnitViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
            if (userLogin != null && userLogin.IsAdmin)
            {
                this.GetRefreshLabTestUnitCollection();
            }
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
            AddCommand = new RelayCommand(OnAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
        }
        #endregion

        #region Command Methods
        public override void OnAddItem()
        {
            try
            {
                var childVM = new AddLabTestUnitViewModel(this.Messenger, this.UserLogin) { ParentViewModel = this };
                childVM.RefreshLabTestUnit += this.GetRefreshLabTestUnitCollection;
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        public override void OnDeleteItem()
        {
            this.ParentViewModel.ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteLabTestUnit) { Caption = MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanDeleteItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }

        public override void OnEditItem()
        {
            var childVM = new AddLabTestUnitViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
            childVM.RefreshLabTestUnit += this.GetRefreshLabTestUnitCollection;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanEditItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        public override void OnRefreshItem()
        {
            this.GetRefreshLabTestUnitCollection();
        }

        #endregion

        #region Public Methods


        #endregion

        #region Private Methods
        private void GetRefreshLabTestUnitCollection()
        {

            Task.Factory.StartNew(() =>
            {
                this.Entity = LabTestAction.GetLabTestUnitList(this.DBConnectionString);
            });
        }

        private void DeleteLabTestUnit(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    LabTestAction.DeleteLabTestUnits(this.DBConnectionString, this.Entity.InternalList.Where(x => x.IsSelected));
                    GetRefreshLabTestUnitCollection();
                    this.ParentViewModel.ShowProgressBar = false;
                });
            }
            else
            {
                this.ParentViewModel.ShowProgressBar = false;
            }
        }
        #endregion

    }
}