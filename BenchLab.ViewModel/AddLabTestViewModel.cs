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
    public class AddLabTestViewModel : BaseViewModel<LabTest>
    {
        #region Delegate
        public Action RefreshLabTest { get; set; }
        #endregion

        #region Constants

        #endregion

        #region Fields
        private ObservableCollection<LabTestCategory> _labTestCategoryList = null;
        private LabTestCategory _selectedCategory;
        private ObservableCollection<LabTestUnit> _labTestUnitList = null;
        private LabTestUnit _selectedUnit;
        #endregion

        #region Properties

        public ObservableCollection<LabTestCategory> LabTestCategoryList
        {
            get { return _labTestCategoryList; }
            set
            {
                if (_labTestCategoryList != value)
                {
                    _labTestCategoryList = value;
                    RaisePropertyChanged("LabTestCategoryList");
                }
            }
        }

        public LabTestCategory SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    this.Entity.LabTestCategoryRefID = ((BenchLabEntity<LabTestCategory>)(_selectedCategory)).ID.HasValue 
                        ? ((BenchLabEntity<LabTestCategory>)(_selectedCategory)).ID.Value 
                        : 0;
                    RaisePropertyChanged("SelectedCategory");
                }
            }
        }

        public ObservableCollection<LabTestUnit> LabTestUnitList
        {
            get { return _labTestUnitList; }
            set
            {
                if (_labTestUnitList != value)
                {
                    _labTestUnitList = value;
                    RaisePropertyChanged("LabTestUnitList");
                }
            }
        }

        public LabTestUnit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                if (_selectedUnit != value)
                {
                    _selectedUnit = value;
                    this.Entity.LabTestUnitRefID = ((BenchLabEntity<LabTestUnit>)(_selectedUnit)).ID.HasValue
                        ? ((BenchLabEntity<LabTestUnit>)(_selectedUnit)).ID.Value
                        : 0;
                    RaisePropertyChanged("SelectedUnit");
                }
            }
        }

        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? Resources.TitleResources.AddLabTest : Resources.TitleResources.EditLabTest;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 555;
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
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSaveLabTest, CanSaveLabTest)); }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelLabTest)); }
        }
        #endregion

       #region Constructors

        public AddLabTestViewModel(IMessenger messenger)
            : base(messenger)
        {
            GetLabTestCategoryList();
            GetLabTestUnitList();
        }

        public AddLabTestViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
            GetLabTestCategoryList();
            GetLabTestUnitList();
        }

        public AddLabTestViewModel(IMessenger messenger, UserLogin userLogin, LabTest labTest)
            : base(messenger, userLogin)
        {
            GetLabTestCategoryList();
            GetLabTestUnitList();

            this.IsInEditMode = true;
            this.Entity = labTest;
            this.SelectedCategory = labTest.LabTestCategory;
            this.SelectedUnit = labTest.LabTestUnit;
        }

        #endregion

        #region Override Methods
        private void OnSaveLabTest()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode ? LabTestAction.AddLabTest(this.DBConnectionString, this.Entity) : LabTestAction.UpdateLabTest(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                //this.Entity.ClearValues();

                if (RefreshLabTest != null)
                    this.RefreshLabTest();

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
        private bool CanSaveLabTest()
        {
            return this.Entity != null && this.Entity.HasValueInAllRequiredField;
            //return this.Entity != null && ( this.IsInEditMode ? this.Entity.VaidateChangePassword() && this.Entity.VaidateCurrentPassword() : ( this.Entity.VaidateNewPassword() && !string.IsNullOrEmpty(this.Entity.LoginName)));
        }

        private void OnCancelLabTest()
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
        private void GetLabTestCategoryList()
        {
            Task.Factory.StartNew(() =>
            {
                LabTestCategory _firstTestCategory = new LabTestCategory();
                _firstTestCategory.ID = 0;
                _firstTestCategory.LabTestCategoryName = Resources.ComboBoxResources.Select;

                LabTestCategoryList = LabTestAction.GetLabTestCategoryList(this.DBConnectionString).ObservableList;
                LabTestCategoryList.Insert(0, _firstTestCategory);
            });
        }

        private void GetLabTestUnitList()
        {
            Task.Factory.StartNew(() =>
            {
                LabTestUnit _firstTestUnit = new LabTestUnit();
                _firstTestUnit.ID = 0;
                _firstTestUnit.UnitName = Resources.ComboBoxResources.Select;

                LabTestUnitList = LabTestAction.GetLabTestUnitList(this.DBConnectionString).ObservableList;
                LabTestUnitList.Insert(0, _firstTestUnit);
            });
        }
        #endregion

    }
}