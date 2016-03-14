using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.DataAccess;
using BenchLab.Model;
using BenchLab.SimpleUI.Entities;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BenchLab.ViewModel
{
    public class AddLabTestUnitViewModel : BaseViewModel<LabTestUnit>
    {
        #region Delegate
        public Action RefreshLabTestUnit { get; set; }
        #endregion

        #region Constants

        #endregion

        #region Fields

        #endregion

        #region Properties

        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? Resources.TitleResources.AddLabTestUnit : Resources.TitleResources.EditLabTestUnit;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 280;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 490;
            }
        }

        public override DialogType DialogType
        {
            get
            {
                return DialogType.BySizeInPixel;
            }
        }
        #endregion
        #endregion

        #region Commands
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSaveLabTestUnit, CanSaveLabTestUnit)); }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelLabTestUnit)); }
        }
        #endregion

       #region Constructors

        public AddLabTestUnitViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public AddLabTestUnitViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }

        public AddLabTestUnitViewModel(IMessenger messenger, UserLogin userLogin, LabTestUnit labTestUnit)
            : base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = labTestUnit;
        }

        #endregion

        #region Override Methods
        private void OnSaveLabTestUnit()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode ? LabTestAction.AddLabTestUnit(this.DBConnectionString, this.Entity) : LabTestAction.UpdateLabTestUnit(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                //this.Entity.ClearValues();

                if (RefreshLabTestUnit != null)
                    this.RefreshLabTestUnit();

                var messageDailog = new MessageDailog()
                {
                    Caption = Resources.MessageResources.DataSavedSuccessfully,
                    DialogButton = DialogButton.Ok,
                    Title = Resources.TitleResources.Information
                };

                MessengerInstance.Send(messageDailog);

                if (this.CloseWindow != null)
                    this.CloseWindow();

                
            }
            else
            {
                var messageDailog = new MessageDailog() { Caption = Resources.MessageResources.DataSavedFailed, DialogButton = DialogButton.Ok, Title = Resources.TitleResources.Error };
                MessengerInstance.Send(messageDailog);
            }
        }
        private bool CanSaveLabTestUnit()
        {
            return this.Entity != null && this.Entity.HasValueInAllRequiredField;
            //return this.Entity != null && ( this.IsInEditMode ? this.Entity.VaidateChangePassword() && this.Entity.VaidateCurrentPassword() : ( this.Entity.VaidateNewPassword() && !string.IsNullOrEmpty(this.Entity.LoginName)));
        }

        private void OnCancelLabTestUnit()
        {
            var messageDailog = new MessageDailog((result) =>
            {
                if (result == DialogResult.Ok)
                {
                    //this.Entity.ClearValues();

                    if (this.ParentViewModel != null)
                        this.ParentViewModel.ChildViewModel = null;
                    this.Unload();

                    if (this.CloseWindow != null)
                        this.CloseWindow();

                  
                }
            }) { Caption = Resources.MessageResources.CancelWindowMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);


        }
        #endregion

        #region Private Methods

        #endregion

    }
}