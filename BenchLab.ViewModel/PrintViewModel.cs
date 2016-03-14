using BenchLab.Model;
using BenchLab.Resources;
using BenchLab.SimpleUI.Entities;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Reflection;



namespace BenchLab.ViewModel
{
    public class PrintViewModel : BaseViewModel
    {
        private IBenchLabEntity _entity;

        #region Properties
        public IBenchLabEntity Entity
        {
            get { return _entity; }
            set
            {
                _entity = value;
                this.RaisePropertyChanged(() => this.Entity);
            }
        }
        private IEnumerable<LabTestReport> _labTestReports;

        public IEnumerable<LabTestReport> LabTestReports
        {
            get { return _labTestReports; }
            set
            {
                _labTestReports = value;
                RaisePropertyChanged(() => LabTestReports);
            }
        }

        #region Window Properties
        public override string Title
        {
            get { return TitleResources.Print; }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 750;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 950;
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
       

        public PrintViewModel(IMessenger messenger, UserLogin userLogin, IBenchLabEntity entity, IEnumerable<LabTestReport> labTestReports)
            : base(messenger, userLogin)
        {
            this.Entity = entity;
            this.LabTestReports = labTestReports;
            //Assembly app = Assembly.GetExecutingAssembly();
            //app.GetCustomAttribute(typeof(AssemblyCompanyAttribute
        }
    }
}