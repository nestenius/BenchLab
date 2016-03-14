using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.Bases;
using BenchLab.Model;
using BenchLab.Resources;

namespace BenchLab.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Fields
        private string _selectedMenuItem;
        private IBaseViewModel _detailSectionViewModel;
        #endregion

        #region Properties
        public List<string> MenuList
        {
            get
            {
                return new List<string>
                    {
                        MenuResources.LabTestCategory
                        , MenuResources.LabTestUnit
                        , MenuResources.LabTest
                    };
            }
        }

        public string SelectedMenuItem
        {
            get
            {
                //if (_selectedMenuItem == null)
                //    SetSummaryView();
                return _selectedMenuItem;
            }
            set
            {
                _selectedMenuItem = value;
                this.RaisePropertyChanged(() => this.SelectedMenuItem);
                OnSelectedMenuItem();
            }
        }

        public LabTestCategoryViewModel LabTestCategoryViewModel { get; set; }

        public LabTestUnitViewModel LabTestUnitViewModel { get; set; }

        public LabTestViewModel LabTestViewModel { get; set; }

        public IBaseViewModel DetailSectionViewModel
        {
            get { return _detailSectionViewModel; }
            set
            {
                _detailSectionViewModel = value;
                this.RaisePropertyChanged(() => this.DetailSectionViewModel);
            }
        }
        #endregion

        #region Constructors

        public SettingsViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }

        #endregion

        #region Override Methods
        public override void HandleViewModeChanges(dynamic data)
        {
            //base.HandleViewModeChanges(data);
            var model = this.ParentViewModel as MainWindowViewModel;
            if (model != null)
            {
                model.IsSettingTabSelected = true;
            }
        }
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void OnSelectedMenuItem()
        {
            if (SelectedMenuItem == MenuResources.LabTestCategory)
            {
                if (LabTestCategoryViewModel == null)
                    LabTestCategoryViewModel = new LabTestCategoryViewModel(this.Messenger, this.UserLogin)
                        {
                            ParentViewModel = this
                        };

                DetailSectionViewModel = LabTestCategoryViewModel;
                return;
            }

            if (SelectedMenuItem == MenuResources.LabTestUnit)
            {
                if (LabTestUnitViewModel == null)
                    LabTestUnitViewModel = new LabTestUnitViewModel(this.Messenger, this.UserLogin)
                    {
                        ParentViewModel = this
                    };

                DetailSectionViewModel = LabTestUnitViewModel;
                return;
            }
            
            if (SelectedMenuItem == MenuResources.LabTest)
            {
                if (LabTestViewModel == null)
                    LabTestViewModel = new LabTestViewModel(this.Messenger, this.UserLogin)
                    {
                        ParentViewModel = this
                    };

                DetailSectionViewModel = LabTestViewModel;
                return;
            }
        }

        
        #endregion

    }
}
