using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using BenchLab.ErrorLog;
using BenchLab.Model;
using BenchLab.Resources;

namespace BenchLab.DataAccess
{
    public class PatientAction
    {
        private static readonly Object thisLock = new Object();
        private static readonly Object thisTestReportLock = new Object();
        private static BenchLabDBContext _benchLabDBContext;
        private static string _connectionString;

        public static BenchLabDBContext DBContextInstance
        {
            get
            {
                return _benchLabDBContext ?? (_benchLabDBContext = new BenchLabDBContext(_connectionString));

                //if (_customerDBContext == null)
                //{
                //    _customerDBContext = new CustomerDBContext(_connectionString);
                //    if(_customerDBContext.Database.Connection.State == ConnectionState.Closed)
                //        _customerDBContext.Database.Connection.Open();

                //    return _customerDBContext;
                //}

                //if (_customerDBContext.Database.Connection.State == ConnectionState.Closed)
                //    _customerDBContext.Database.Connection.Open();

                //return _customerDBContext;

            }
        }

        #region ADD

        public static bool AddPatient(string connectionString, Patient patient)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    context.Patients.Add(patient);
                    var result = context.SaveChanges();
                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        #endregion

        #region DELETE

        public static bool DeletePatient(string connectionString, Patient patient)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    context.Patients.Attach(new Patient() {ID = patient.ID});
                    context.Patients.Remove(patient);
                    var result = context.SaveChanges();
                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        public static bool DeletePatients(string connectionString, IEnumerable<Patient> patients)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    foreach (var patient in patients.Select(patient => new Patient() {ID = patient.ID}))
                    {
                        context.Patients.Attach(patient);
                        context.Patients.Remove(patient);
                    }

                    var result = context.SaveChanges();
                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        #endregion

        #region GET
        public IList<String> GetPatientSearchList(string connectionString, string strSearchText)
        {
            try
            {
               // using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    return DBContextInstance.Patients.Where(x => x.FirstName.StartsWith(strSearchText) || x.MiddleName.StartsWith(strSearchText) || x.LastName.StartsWith(strSearchText)).ToList().Select(x => x.FormattedName).ToList();
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public Dictionary<string, string> GetPatientSearchDictionary(string connectionString, string strSearchText)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                lock (thisLock)
                {
                    _connectionString = connectionString;
                    return
                        context.Patients.Where(
                            x => x.FirstName.StartsWith(strSearchText) || x.MiddleName.StartsWith(strSearchText) || x.LastName.StartsWith(strSearchText))
                               .ToList()
                               .Select(x => new {PatientID = x.ID, Name = x.FormattedName})
                               .ToDictionary(y => y.PatientID.Value.ToString(CultureInfo.InvariantCulture), y => y.Name);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static PatientCollection GetPatientList(string connectionString)
        {
            try
            {
                lock (thisLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.Patients.Select(x => x).ToList();
                    return new PatientCollection(items);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static Patient GetPatientByID(string connectionString, string patientId)
        {
            try
            {
               // var context = new CustomerDBContext(connectionString);
                _connectionString = connectionString;
                var id = Convert.ToInt32(patientId);
                return DBContextInstance.Patients.FirstOrDefault(x => x.ID == id);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        #endregion

        #region UPDATE

        public static bool UpdatePatient(string connectionString, Patient patient)
        {
            try
            {
                _connectionString = connectionString;
                var entry = DBContextInstance.Entry(patient);
                entry.State = EntityState.Modified;
                var result = DBContextInstance.SaveChanges();
                return result > 0;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured, ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        public static bool UpdatePatients(string connectionString, IEnumerable<Patient> patients)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var patient in patients)
                {
                    DBContextInstance.Patients.Add(patient);
                    var entry = DBContextInstance.Entry(patient);
                    entry.State = EntityState.Modified;
                }

                var result = DBContextInstance.SaveChanges();
                return result > 0;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        #endregion

        #region Patient Test Report

        #region ADD
        public static bool AddPatientTestReport(string connectionString, PatientTestReport patientTestReport)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    //var cloneObject = patientTestReport.CreateAClone();
                    //cloneObject.ApplyCurrentDateTime();
                    //context.PatientTestReports.Add(cloneObject);

                    //var result = context.SaveChanges();
                    //return result > 0;
                    _connectionString = connectionString;
                    /*foreach (var testReport in patientTestReport.LabTestReportCollection)
                    {
                        testReport.PatientTestReportRefID = patientTestReport.ID.HasValue ? patientTestReport.ID.Value : 0;
                        context.LabTestReports.Attach(testReport);
                    }*/
                    /*ICollection<LabTestReport> tempLabTestReports = new List<LabTestReport>();
                    tempLabTestReports = patientTestReport.LabTestReportCollection;
                    patientTestReport.LabTestReportCollection = null;*/
                    context.PatientTestReports.Add(patientTestReport);
                    var result = context.SaveChanges();
                    /*foreach (var testReport in tempLabTestReports)
                    {
                        testReport.PatientTestReportRefID = patientTestReport.ID.HasValue ? patientTestReport.ID.Value : 0;
                        context.LabTestReports.Attach(testReport);
                    }
                    result = context.SaveChanges();
                    */return result > 0;

                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
            //return false;
        }
        #endregion

        #region DELETE
        public static bool DeletePatientTestReport(string connectionString, PatientTestReport patientTestReport)
        {
            try
            {
                //using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    DBContextInstance.PatientTestReports.Attach(new PatientTestReport() { ID = patientTestReport.ID });
                    DBContextInstance.PatientTestReports.Remove(patientTestReport);
                    var result = DBContextInstance.SaveChanges();
                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        public static bool DeletePatientTestReports(string connectionString, IEnumerable<PatientTestReport> patientTestReports)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    foreach (var patientTestReport in patientTestReports.Select(patientTestReport => new PatientTestReport() { ID = patientTestReport.ID }))
                    {
                        context.PatientTestReports.Attach(patientTestReport);
                        context.PatientTestReports.Remove(patientTestReport);
                    }

                    var result = context.SaveChanges();
                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }
        #endregion

        #region UPDATE
        public static bool UpdatePatientTestReport(string connectionString, PatientTestReport patientTestReport)
        {
            try
            {
                _connectionString = connectionString;
                
                var entry = DBContextInstance.Entry(patientTestReport);
                entry.State = EntityState.Modified;
                var result = DBContextInstance.SaveChanges();
                return result > 0;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured, ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        public static bool UpdatePatientTestReports(string connectionString, IEnumerable<PatientTestReport> patientTestReports)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var patientTestReport in patientTestReports)
                {
                    DBContextInstance.PatientTestReports.Add(patientTestReport);
                    var entry = DBContextInstance.Entry(patientTestReport);
                    entry.State = EntityState.Modified;
                }

                var result = DBContextInstance.SaveChanges();
                return result > 0;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }
        #endregion

        #region GET


        public Dictionary<string, string> GetPatientTestReportSearchDictionary(string connectionString, string strSearchText)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    return null;

                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static PatientTestReportCollection PatientTestReportList(string connectionString)
        {
            try
            {
                _connectionString = connectionString;

                var items = DBContextInstance.PatientTestReports.Select(x => x).ToList();
                return new PatientTestReportCollection(items);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }
        public static PatientTestReportCollection PatientTestReportListWithSearch(string connectionString, string firstName, string middleName, string lastName, string sn, string labNr, string technician)
        {
            try
            {
                _connectionString = connectionString;
                var filteredItems = DBContextInstance.PatientTestReports.AsQueryable();
                if ( !string.IsNullOrEmpty(firstName))
                    filteredItems = filteredItems.Where(p => p.Patient.FirstName.Contains(firstName.Trim()));
                if (!string.IsNullOrEmpty(middleName))
                    filteredItems = filteredItems.Where(p => p.Patient.MiddleName.Contains(middleName.Trim()));
                if (!string.IsNullOrEmpty(lastName))
                    filteredItems = filteredItems.Where(p => p.Patient.LastName.Contains(lastName.Trim()));
                if (!string.IsNullOrEmpty(sn))
                {
                    int paramSn;
                    if(int.TryParse(sn, out paramSn))
                        filteredItems = filteredItems.Where(r => r.Sn == paramSn);
                }
                if (!string.IsNullOrEmpty(labNr))
                    filteredItems = filteredItems.Where(r => r.LabNr.Contains(labNr.Trim()));
                if (!string.IsNullOrEmpty(technician))
                    filteredItems = filteredItems.Where(r => r.TechnicianName.Contains(technician.Trim()));

                return new PatientTestReportCollection(filteredItems.ToList());
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }
        public static PatientTestReportCollection GetPatientTestReportList(string connectionString, Patient patient)
        {
            try
            {
                lock (thisTestReportLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.PatientTestReports.Where(x => x.Patient.ID == patient.ID)/*.OrderByDescending(y => y.LastChangeDateTime)*/.ToList();
                    return new PatientTestReportCollection(items);
                }
               
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }
        

        #endregion

        #endregion

    }
}