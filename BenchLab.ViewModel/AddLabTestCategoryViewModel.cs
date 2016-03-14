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
    public class AddLabTestCategoryViewModel : BaseViewModel<LabTestCategory>
    {
        #region Delegate
        public Action RefreshLabTestCategory { get; set; }
        #endregion

        #region Constants

        #endregion

        #region Fields
        //http://stackoverflow.com/questions/4078072/problem-with-databinding-a-combobox-in-silverlight-with-mvvm-light
        private ObservableCollection<LabTestCategory> _parentCategoryList = null;
        private LabTestCategory _selectedParentCategory;
        #endregion

        #region Properties

        public ObservableCollection<LabTestCategory> ParentCategoryList
        {
            get { return _parentCategoryList; }
            set
            {
                if (_parentCategoryList != value)
                {
                    _parentCategoryList = value;
                    RaisePropertyChanged("ParentCategoryList");
                }
            }
        }

        public LabTestCategory SelectedParentCategory
        {
            get { return _selectedParentCategory; }
            set
            {
                if (_selectedParentCategory != value)
                {
                    _selectedParentCategory = value;
                    this.Entity.ParentCategoryID = ((BenchLabEntity<LabTestCategory>)(_selectedParentCategory)).ID.HasValue ? ((BenchLabEntity<LabTestCategory>)(_selectedParentCategory)).ID.Value : 0;
                    RaisePropertyChanged("SelectedParentCategory");
                }
            }
        }

        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? Resources.TitleResources.AddLabTestCategory : Resources.TitleResources.EditLabTestCategory;
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
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSaveLabTestCategory, CanSaveLabTestCategory)); }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelLabTestCategory)); }
        }
        #endregion

       #region Constructors

        public AddLabTestCategoryViewModel(IMessenger messenger)
            : base(messenger)
        {
            GetParentCategoryList();
        }

        public AddLabTestCategoryViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
            GetParentCategoryList();
        }

        public AddLabTestCategoryViewModel(IMessenger messenger, UserLogin userLogin, LabTestCategory labTestCategory)
            : base(messenger, userLogin)
        {
            GetParentCategoryList();
            this.IsInEditMode = true;
            this.Entity = labTestCategory;
            this.SelectedParentCategory = labTestCategory.ParentLabTestCategory;
        }

        #endregion

        #region Override Methods
        private void OnSaveLabTestCategory()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode ? LabTestAction.AddLabTestCategory(this.DBConnectionString, this.Entity) : LabTestAction.UpdateLabTestCategory(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                //this.Entity.ClearValues();

                if (RefreshLabTestCategory != null)
                    this.RefreshLabTestCategory();

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
        private bool CanSaveLabTestCategory()
        {
            return this.Entity != null && this.Entity.HasValueInAllRequiredField;
            //return this.Entity != null && ( this.IsInEditMode ? this.Entity.VaidateChangePassword() && this.Entity.VaidateCurrentPassword() : ( this.Entity.VaidateNewPassword() && !string.IsNullOrEmpty(this.Entity.LoginName)));
        }

        private void OnCancelLabTestCategory()
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
        private void GetParentCategoryList()
        {
            Task.Factory.StartNew(() =>
            {
                LabTestCategory _firstCategory = new LabTestCategory();
                _firstCategory.ID = 0;
                _firstCategory.LabTestCategoryName = Resources.ComboBoxResources.Select;

                ParentCategoryList = LabTestAction.GetLabTestParentCategoryList(this.DBConnectionString).ObservableList;
                ParentCategoryList.Insert(0, _firstCategory);
            });
        }
        #endregion

    }
}