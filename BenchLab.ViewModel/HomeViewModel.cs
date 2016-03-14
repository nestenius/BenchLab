using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.DataAccess;
using BenchLab.ErrorLog;
using BenchLab.Model;
using BenchLab.Resources;

namespace BenchLab.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public HomeViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public HomeViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }

        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void HandleViewModeChanges(dynamic data)
        {
            //base.HandleViewModeChanges(data);
            var model = this.ParentViewModel as MainWindowViewModel;
            if (model != null)
            {
                model.IsHomeTabSelected = true;
            }
        }
        #endregion

        #region Private Methods
        
        #endregion
    }
}