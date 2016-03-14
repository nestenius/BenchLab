using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.DataAccess;
using BenchLab.ErrorLog;
using BenchLab.Model;
using BenchLab.Model.Utilities;
using BenchLab.Resources;
using BenchLab.SimpleUI.Entities;

namespace BenchLab.ViewModel
{
    public class AddPatientViewModel : BaseViewModel<Patient>
    {
        #region Delegate
        public Action RefreshPatients { get; set; }
        //public Func<Customer, bool> SaveCustomer { get; set; }
        #endregion

        #region Fields
        //private WeakReference _weakRefCityCollection;
        //private WeakReference _weakRefStateCollection;
        //private WeakReference _weakRefLocationCollection;
        #endregion

        #region Properties
        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? Resources.TitleResources.AddNewPatient : Resources.TitleResources.EditPatient;
            }
        }

        public override double DialogStartupSizePercentage
        {
            get
            {
                return 85;
            }
        }

        public override double DialogStartupCustomHeight
        {
            get
            {
                return 540;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 450;
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

        //public List<String> CountryCollection
        //{
        //    get { return  new List<string>(){"India"};}
        //}

        //public List<String> CityCollection
        //{
        //    get
        //    {
        //        if (_weakRefCityCollection == null || _weakRefCityCollection.Target as List<String> == null)
        //            GetLocationCollection();
        //        if (_weakRefCityCollection != null && _weakRefCityCollection.IsAlive)
        //            return _weakRefCityCollection.Target as List<String>;
        //        return null;
        //    }
        //}

        //public List<String> StateCollection
        //{
        //    get
        //    {
        //        if (_weakRefStateCollection == null || _weakRefStateCollection.Target as List<String> == null)
        //            GetLocationCollection();
        //        if (_weakRefStateCollection != null && _weakRefStateCollection.IsAlive)
        //            return _weakRefStateCollection.Target as List<String>;
        //        return null;
        //    }
        //}
        #endregion

        #region Commands
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSavePatient, CanSavePatient)); }
        }

        private ICommand _cancelCommand;
        //private IDictionary<string, string> _locationCollection;
        //private ICommand _addPhoneCommand;
        //private ICommand _deletePhoneCommand;
        //private ICommand _deleteAddressCommand;
        //private ICommand _addAddressCommand;

        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelPatient)); }
        }

        //public ICommand AddPhoneCommand
        //{
        //    get { return this._addPhoneCommand ?? (this._addPhoneCommand = new RelayCommand(OnAddPhone)); }
        //}

        //public ICommand DeletePhoneCommand
        //{
        //    get { return this._deletePhoneCommand ?? (this._deletePhoneCommand = new RelayCommand(OnDeletePhone, CanDeletePhone)); }
        //}

        //public ICommand AddAddressCommand
        //{
        //    get { return this._addAddressCommand ?? (this._addAddressCommand = new RelayCommand(OnAddAddress)); }
        //}

        //public ICommand DeleteAddressCommand
        //{
        //    get { return this._deleteAddressCommand ?? (this._deleteAddressCommand = new RelayCommand(OnDeleteAddress, CanDeleteAddress)); }
        //}

        #endregion

        #region Constructors

        public AddPatientViewModel(IMessenger messenger)
            : base(messenger)
        {
            //this.Entity.PatientAddressCollection = new Collection<PatientAddress> {new PatientAddress()};
            //this.Entity.PatientPhoneCollection = new Collection<PatientPhone> { new PatientPhone() };
        }

        public AddPatientViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
            //this.Entity.PatientAddressCollection = new Collection<PatientAddress> { new PatientAddress(){} };
            //this.Entity.PatientPhoneCollection = new Collection<PatientPhone> { new PatientPhone() };

            //if (this.Entity != null)
            //    this.Entity.PatientAddressCollection.ForEach(x =>
            //    {
            //        x.PropertyChanged += (sender, args) =>
            //        {
            //            if (args.PropertyName == "City")
            //            {
            //                var item = sender as PatientAddress;
                            
            //                if (item != null)
            //                {
            //                    item.IsOtherCity = (item.City == "Other");

            //                    if (!_weakRefLocationCollection.IsAlive)
            //                        GetLocationCollection();
            //                    if (_weakRefLocationCollection.IsAlive)
            //                    {
            //                        var collection = _weakRefLocationCollection.Target as Dictionary<string, string>;
            //                        if (collection != null)
            //                            item.State = collection.FirstOrDefault(y => y.Key == item.City).Value;
            //                        item.Country = CountryCollection.FirstOrDefault();
            //                    }
            //                }
            //            }
            //        };
            //    });
        }

        public AddPatientViewModel(IMessenger messenger, UserLogin userLogin, Patient patient)
            : base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = patient;
        }
        #endregion

        #region Override Methods

        public override void Initialize()
        {
            base.Initialize();
           

        }

        protected override void OnAction(ActionResult<Patient> result)
        {
           
        }

        #endregion

        #region Command Methods
        private bool CanSavePatient()
        {
            return this.Entity != null; //&& this.Entity.HasValueInAllRequiredField;
        }
        private void OnSavePatient()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode ? PatientAction.AddPatient(this.DBConnectionString, this.Entity) : PatientAction.UpdatePatient(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                if (RefreshPatients != null)
                    this.RefreshPatients();

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


        private void OnCancelPatient()
        {
            var messageDailog = new MessageDailog((result) =>
                {
                    if (result == DialogResult.Ok)
                    {
                        if (this.ParentViewModel != null)
                            this.ParentViewModel.ChildViewModel = null;
                        this.Unload();

                        if (this.CloseWindow != null)
                            this.CloseWindow();
                    }
                }) { Caption = Resources.MessageResources.CancelWindowMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);

          
        }

        //private void OnAddPhone()
        //{
        //    if(this.Entity.CustomerPhoneCollection == null)
        //        this.Entity.CustomerPhoneCollection = new Collection<CustomerPhone>();
        //    var tempCollection = this.Entity.CustomerPhoneCollection;
        //    tempCollection.Add(new CustomerPhone());
        //    this.Entity.CustomerPhoneCollection = null;
        //    this.Entity.CustomerPhoneCollection = tempCollection;
        //    this.Entity.NotifyPropertyChanged("CustomerPhoneCollection");
        //}
        //private bool CanDeletePhone()
        //{
        //    return this.Entity != null && this.Entity.CustomerPhoneCollection != null &&
        //           this.Entity.CustomerPhoneCollection.Any(x => x.IsSelected);
        //}

        //private void OnDeletePhone()
        //{
        //    if (this.Entity.CustomerPhoneCollection != null &&
        //         this.Entity.CustomerPhoneCollection.Any(x => x.IsSelected))
        //    {
        //        var qureyItems = this.Entity.CustomerPhoneCollection.Where(x => x.IsSelected && (x.ID.HasValue && x.ID.Value > 0) && !x.IsDeleted).ToList();
        //        if (qureyItems.Any())
        //        {
        //            CustomerAction.DeleteCustomerPhones(this.DBConnectionString, qureyItems);
        //            qureyItems.ForEach(x => x.IsDeleted = true);
        //        }

        //        var items = this.Entity.CustomerPhoneCollection.Where(x => x.IsSelected && (!x.ID.HasValue || x.ID.Value <= 0)).ToList();

        //        if (items.Any())
        //        {
        //            foreach (var customerPhone in items)
        //            {
        //                this.Entity.CustomerPhoneCollection.Remove(customerPhone);
        //            }
        //            var tempCollection = new CustomerPhoneCollection(this.Entity.CustomerPhoneCollection.ToList());
        //            this.Entity.CustomerPhoneCollection = null;
        //            this.Entity.CustomerPhoneCollection = tempCollection.InternalList;
        //            this.Entity.NotifyPropertyChanged("CustomerPhoneCollection");
        //        }
        //    }
        //}

        //private void OnAddAddress()
        //{
        //    if (this.Entity.CustomerAddresseCollection == null)
        //        this.Entity.CustomerAddresseCollection = new Collection<CustomerAddress>();
        //    var tempCollection = new CustomerAddressCollection(this.Entity.CustomerAddresseCollection.ToList());
        //    tempCollection.InternalList.Add(new CustomerAddress());
        //    this.Entity.CustomerAddresseCollection = null;
        //    this.Entity.CustomerAddresseCollection = tempCollection.InternalList;
        //    this.Entity.NotifyPropertyChanged("CustomerAddresseCollection");
        //}
        //private bool CanDeleteAddress()
        //{
        //    return this.Entity != null && this.Entity.CustomerAddresseCollection != null &&
        //           this.Entity.CustomerAddresseCollection.Any(x => x.IsSelected);
        //}

        //private void OnDeleteAddress()
        //{
        //    if (this.Entity.CustomerAddresseCollection != null &&
        //         this.Entity.CustomerAddresseCollection.Any(x => x.IsSelected))
        //    {

        //        var qureyItems = this.Entity.CustomerAddresseCollection.Where(x => x.IsSelected && (x.ID.HasValue && x.ID.Value > 0) && !x.IsDeleted).ToList();
        //        if (qureyItems.Any())
        //        {
        //            CustomerAction.DeleteCustomerAddresses(this.DBConnectionString, qureyItems);
        //            qureyItems.ForEach(x => x.IsDeleted = true);
        //        }

        //        var items = this.Entity.CustomerAddresseCollection.Where(x => x.IsSelected && (!x.ID.HasValue || x.ID.Value <= 0)).ToList();

        //        if (items.Any())
        //        {
        //            foreach (var customerAddress in items)
        //            {
        //                this.Entity.CustomerAddresseCollection.Remove(customerAddress);
        //            }
        //            var tempCollection = new CustomerAddressCollection(this.Entity.CustomerAddresseCollection.ToList());
        //            this.Entity.CustomerAddresseCollection = null;
        //            this.Entity.CustomerAddresseCollection = tempCollection.InternalList;
        //            this.Entity.NotifyPropertyChanged("CustomerAddresseCollection");
        //        }
        //    }
        //}

        #endregion

        #region Private Methods
        //private void GetLocationCollection()
        //{
        //    try
        //    {
        //            using (Stream stream =
        //                Assembly.GetExecutingAssembly()
        //                .GetManifestResourceStream("VH.ViewModel.SupportingFiles.LocationDataBase.txt"))
        //            {
        //                if (stream != null)
        //                    using (var streamReader = new StreamReader(stream))
        //                    {
        //                        var locationCollection = new Dictionary<string, string>();
        //                        string line;
        //                        while ((line = streamReader.ReadLine()) != null)
        //                        {
        //                            var location = new Location().GetLocation(line);
        //                            if (location != null)
        //                            {
        //                                if (!locationCollection.ContainsKey(location.CityName))
        //                                {
        //                                    locationCollection.Add(location.CityName,location.State);
                                           
        //                                }
        //                            }
        //                         _weakRefLocationCollection = new WeakReference(locationCollection);}

        //                        _weakRefCityCollection =
        //                            new WeakReference(locationCollection.OrderBy(x => x.Key).Select(x => x.Key).Distinct().ToList());
        //                        _weakRefStateCollection =
        //                            new WeakReference(locationCollection.OrderBy(x => x.Value).Select(x => x.Value).Distinct().ToList());
        //                    }
        //            }
        //    }
        //    catch (Exception exception)
        //    {
        //        NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
        //                            ExceptionResources.ExceptionOccuredLogDetail);
        //    }

        //}
        #endregion
    }
}