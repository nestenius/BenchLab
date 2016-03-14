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
    public class LabTestAction
    {
        private static readonly Object thisLock = new Object();
        private static readonly Object thisCategoryLock = new Object();
        private static readonly Object thisTestReportLock = new Object();
        private static readonly Object thisTestUnitLock = new Object();
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
        #region Lab Test Category

        #region ADD

        public static bool AddLabTestCategory(string connectionString, LabTestCategory labTestCategory)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    context.LabTestCategories.Add(labTestCategory);
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

        public static bool DeleteLabTestCategory(string connectionString, LabTestCategory labTestCategory)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    context.LabTestCategories.Attach(new LabTestCategory() { ID = labTestCategory.ID });
                    context.LabTestCategories.Remove(labTestCategory);
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
        
        public static bool DeleteLabTestCategories(string connectionString, IEnumerable<LabTestCategory> labTestCategories)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    foreach (var labTestCategory in labTestCategories.Select(labTestCategory => new LabTestCategory() { ID = labTestCategory.ID }))
                    {
                        context.LabTestCategories.Attach(labTestCategory);
                        context.LabTestCategories.Remove(labTestCategory);
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

        public Dictionary<string, string> GetLabTestCategorySearchDictionary(string connectionString, string strSearchText)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                    return null;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static LabTestCategoryCollection GetLabTestCategoryList(string connectionString)
        {
            try
            {
                lock (thisCategoryLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.LabTestCategories.Select(x => x).ToList();
                    return new LabTestCategoryCollection(items);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static LabTestCategory GetLabTestCategoryByID(string connectionString, string labTestCategoryId)
        {
            try
            {
                // var context = new CustomerDBContext(connectionString);
                _connectionString = connectionString;
                var id = Convert.ToInt32(labTestCategoryId);
                return DBContextInstance.LabTestCategories.FirstOrDefault(x => x.ID == id);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static LabTestCategoryCollection GetLabTestParentCategoryList(string connectionString)
        {
            try
            {
                lock (thisCategoryLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.LabTestCategories.Where(x => x.ParentCategoryID == 0).ToList();
                    return new LabTestCategoryCollection(items);
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

        #region UPDATE

        public static bool UpdateLabTestCategory(string connectionString, LabTestCategory labTestCategory)
        {
            try
            {
                _connectionString = connectionString;
                var entry = DBContextInstance.Entry(labTestCategory);
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

        public static bool UpdateLabTestCategories(string connectionString, IEnumerable<LabTestCategory> labTestCategories)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var labTestCategory in labTestCategories)
                {
                    DBContextInstance.LabTestCategories.Add(labTestCategory);
                    var entry = DBContextInstance.Entry(labTestCategory);
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

        #endregion

        #region Lab Test Unit

        #region ADD

        public static bool AddLabTestUnit(string connectionString, LabTestUnit labTestUnit)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    context.LabTestUnits.Add(labTestUnit);
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

        public static bool DeleteLabTestUnit(string connectionString, LabTestUnit labTestUnit)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    context.LabTestUnits.Attach(new LabTestUnit() { ID = labTestUnit.ID });
                    context.LabTestUnits.Remove(labTestUnit);
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

        public static bool DeleteLabTestUnits(string connectionString, IEnumerable<LabTestUnit> labTestUnits)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    foreach (var labTestUnit in labTestUnits.Select(labTestUnit => new LabTestUnit() { ID = labTestUnit.ID }))
                    {
                        context.LabTestUnits.Attach(labTestUnit);
                        context.LabTestUnits.Remove(labTestUnit);
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

        public Dictionary<string, string> GetLabTestUnitSearchDictionary(string connectionString, string strSearchText)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                    return null;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static LabTestUnitCollection GetLabTestUnitList(string connectionString)
        {
            try
            {
                lock (thisTestUnitLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.LabTestUnits.Select(x => x).ToList();
                    return new LabTestUnitCollection(items);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static LabTestUnit GetLabTestUnitByID(string connectionString, string labTestUnitId)
        {
            try
            {
                // var context = new CustomerDBContext(connectionString);
                _connectionString = connectionString;
                var id = Convert.ToInt32(labTestUnitId);
                return DBContextInstance.LabTestUnits.FirstOrDefault(x => x.ID == id);
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

        public static bool UpdateLabTestUnit(string connectionString, LabTestUnit labTestUnit)
        {
            try
            {
                _connectionString = connectionString;
                var entry = DBContextInstance.Entry(labTestUnit);
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

        public static bool UpdateLabTestUnits(string connectionString, IEnumerable<LabTestUnit> labTestUnits)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var labTestUnit in labTestUnits)
                {
                    DBContextInstance.LabTestUnits.Add(labTestUnit);
                    var entry = DBContextInstance.Entry(labTestUnit);
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

        #endregion

        #region Lab Test

        #region ADD

        public static bool AddLabTest(string connectionString, LabTest labTest)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    context.LabTests.Add(labTest);
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

        public static bool DeleteLabTest(string connectionString, LabTest labTest)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    context.LabTests.Attach(new LabTest() { ID = labTest.ID });
                    context.LabTests.Remove(labTest);
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

        public static bool DeleteLabTests(string connectionString, IEnumerable<LabTest> labTests)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    foreach (var labTest in labTests.Select(labTest => new LabTest() { ID = labTest.ID }))
                    {
                        context.LabTests.Attach(labTest);
                        context.LabTests.Remove(labTest);
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

        public Dictionary<string, string> GetLabTestSearchDictionary(string connectionString, string strSearchText)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                    return null;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static LabTestCollection GetLabTestList(string connectionString)
        {
            try
            {
                lock (thisLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.LabTests.Select(x => x).ToList();
                    return new LabTestCollection(items);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static LabTest GetLabTestByID(string connectionString, string labTestId)
        {
            try
            {
                // var context = new CustomerDBContext(connectionString);
                _connectionString = connectionString;
                var id = Convert.ToInt32(labTestId);
                return DBContextInstance.LabTests.FirstOrDefault(x => x.ID == id);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static LabTestCollection GetLabTestList(string connectionString, LabTestCategory labTestCategory)
        {
            try
            {
                lock (thisLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.LabTests
                        .Where(x => x.LabTestCategory.ID == labTestCategory.ID).OrderByDescending(y => y.LastChangeDateTime).ToList();
                    return new LabTestCollection(items);
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

        #region UPDATE

        public static bool UpdateLabTest(string connectionString, LabTest labTest)
        {
            try
            {
                _connectionString = connectionString;
                var entry = DBContextInstance.Entry(labTest);
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

        public static bool UpdateLabTests(string connectionString, IEnumerable<LabTest> labTests)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var labTest in labTests)
                {
                    DBContextInstance.LabTests.Add(labTest);
                    var entry = DBContextInstance.Entry(labTest);
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

        #endregion

        #region Lab Test Report

        #region ADD
        public static bool AddLabTestReport(string connectionString, LabTestReport labTestReport)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    var cloneObject = labTestReport.CreateAClone();
                    cloneObject.ApplyCurrentDateTime();
                    context.LabTestReports.Add(cloneObject);

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
            //return false;
        }
        public static bool AddLabTestReports(string connectionString, LabTestReportCollection labTestReports)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    foreach (var labTestReport in labTestReports)
                    {
                        var cloneObject = labTestReport.CreateAClone();
                        cloneObject.ApplyCurrentDateTime();
                        context.LabTestReports.Add(cloneObject);
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
            //return false;
        }
        #endregion

        #region DELETE
        public static bool DeleteLabTestReport(string connectionString, LabTestReport labTestReport)
        {
            try
            {
                //using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    DBContextInstance.LabTestReports.Attach(new LabTestReport() { ID = labTestReport.ID });
                    DBContextInstance.LabTestReports.Remove(labTestReport);
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

        public static bool DeleteLabTestReports(string connectionString, IEnumerable<LabTestReport> labTestReports)
        {
            try
            {
                using (var context = new BenchLabDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    foreach (var labTestReport in labTestReports.Select(labTestReport => new LabTestReport() { ID = labTestReport.ID }))
                    {
                        context.LabTestReports.Attach(labTestReport);
                        context.LabTestReports.Remove(labTestReport);
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
        public static bool UpdateLabTestReport(string connectionString, LabTestReport labTestReport)
        {
            try
            {
                _connectionString = connectionString;

                var entry = DBContextInstance.Entry(labTestReport);
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

        public static bool UpdateLabTestReports(string connectionString, LabTestReportCollection labTestReports)
        {
            try
            {
                _connectionString = connectionString;
                foreach (var labTestReport in labTestReports)
                {
                    if (labTestReport.ID > 0)
                    {
                        //We query local context first to see if it's there.
                        var attachedEntity = DBContextInstance.LabTestReports.Find(labTestReport.ID);
                        //We have it in the context, need to update.
                        if (attachedEntity != null)
                        {
                            var attachedEntry = DBContextInstance.Entry(attachedEntity);
                            attachedEntry.CurrentValues.SetValues(labTestReport);
                            var attachedLabTestEntry = DBContextInstance.Entry(attachedEntity.LabTest);
                            attachedLabTestEntry.CurrentValues.SetValues(labTestReport.LabTest);
                        }
                        else
                        {
                            //If it's not found locally, we can attach it by setting state to modified.
                            //This would result in a SQL update statement for all fields
                            //when SaveChanges is called. 
                            var entry = DBContextInstance.Entry(labTestReport);
                            entry.State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        DBContextInstance.LabTestReports.Add(labTestReport);
                    }
                    //var entry = DBContextInstance.Entry(labTestReport);
                    //if (entry.State == EntityState.Detached)
                    //{
                    //    DBContextInstance.Set<LabTestReport>().Attach(labTestReport);
                    //    entry.State = EntityState.Modified;
                    //}
                        
                }
                
                var result = DBContextInstance.SaveChanges();
                return true;

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


        public Dictionary<string, string> GetLabTestReportSearchDictionary(string connectionString, string strSearchText)
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

        public static LabTestReportCollection LabTestReportList(string connectionString)
        {
            try
            {
                _connectionString = connectionString;

                var items = DBContextInstance.LabTestReports.Select(x => x).ToList();
                return new LabTestReportCollection(items);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static LabTestReportCollection GetLabTestReportList(string connectionString, PatientTestReport patientTestReport)
        {
            try
            {
                lock (thisTestReportLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.LabTestReports.Where(x => x.PatientTestReportRefID == patientTestReport.ID).OrderByDescending(y => y.LastChangeDateTime).ToList();
                    return new LabTestReportCollection(items);
                }

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }
        

        public static LabTestReportCollection GetLabTestReportListByPatientTestReport(string connectionString, PatientTestReport patientTestReport)
        {
            try
            {
                lock (thisTestReportLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.LabTestReports
                        .Where(x => x.PatientTestReport.ID == patientTestReport.ID)
                        .ToList();

                    var items1 = from lt in DBContextInstance.LabTests
                                join ltc in DBContextInstance.LabTestCategories on lt.LabTestCategoryRefID equals ltc.ID
                                join ltu in DBContextInstance.LabTestUnits on lt.LabTestUnitRefID equals ltu.ID
                                join ltr in DBContextInstance.LabTestReports on lt.ID equals ltr.LabTestRefID into lt_ltr
                                from x in lt_ltr.Where(y => y.PatientTestReportRefID == patientTestReport.ID.Value).DefaultIfEmpty()
                                select new CustomLabTestElement()
                                {
                                    ID = x.ID.HasValue ? x.ID.Value : 0
                                    ,
                                    LabTestRefID =  lt.ID == null ? 0 : lt.ID
                                    ,
                                    TestName = lt.TestName
                                    ,
                                    ReportDescription = string.IsNullOrEmpty(x.ReportDescription) ? string.Empty : x.ReportDescription
                                    ,
                                    UnitName = ltu.UnitName
                                    ,
                                    LabTestCategoryName = ltc.LabTestCategoryName
                                    ,
                                    LastChangeDateTimeUTC = x.LastChangeDateTimeUTC == null ? DateTime.Now : x.LastChangeDateTimeUTC
                                    ,
                                    LastChangeUser = x.LastChangeUser
                                    ,
                                    CreationDateTimeUTC = x.CreationDateTimeUTC == null ? DateTime.Now : x.CreationDateTimeUTC
                                    ,
                                    CreationUser = x.CreationUser
                                    ,
                                    Status = x.Status == null ? Status.InActive : x.Status
                                };
                    List<LabTestReport> labTestReports = new List<LabTestReport>();
                    foreach (var item in items1)
                    {
                        LabTestReport labTestReport = new LabTestReport();
                        labTestReport.ID = item.ID;
                        labTestReport.LastChangeDateTimeUTC = item.LastChangeDateTimeUTC;
                        labTestReport.LastChangeUser = item.LastChangeUser;
                        labTestReport.CreationDateTimeUTC = item.CreationDateTimeUTC;
                        labTestReport.CreationUser = item.CreationUser;
                        labTestReport.Status = item.Status;
                        labTestReport.ReportDescription = item.ReportDescription;
                        labTestReport.PatientTestReportRefID = patientTestReport == null ? 0 : (patientTestReport.ID.HasValue ? patientTestReport.ID.Value : 0);
                        labTestReport.PatientTestReport = DBContextInstance.PatientTestReports.Where(x => x.ID == patientTestReport.ID).FirstOrDefault();
                        labTestReport.LabTestRefID = item.LabTestRefID.HasValue ? item.LabTestRefID.Value : 0;
                        labTestReport.LabTest = DBContextInstance.LabTests.Where(x => x.ID == item.LabTestRefID).FirstOrDefault();
                        labTestReport.LabTestRange = GetLabTestRange(labTestReport.PatientTestReport.Patient, labTestReport.LabTest, labTestReport.ReportDescription);
                        labTestReports.Add(labTestReport);
                    }
                    return /*labTestReports;*/new LabTestReportCollection(labTestReports);
                }

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }
        /// <summary>
        /// Method that evaluates the labtest range
        /// </summary>
        /// <param name="patient">Particular patient whose test is to be evaluated</param>
        /// <param name="labTest">Standard lab test element</param>
        /// <param name="reportDescription">Entered lab test value</param>
        /// <returns></returns>
        public static string GetLabTestRange(Patient patient, LabTest labTest, string reportDescription)
        {
            string labTestRange = string.Empty;
            if (labTest != null)
            {
                var minStdGeneral = labTest.MinStdGeneral.HasValue ? labTest.MinStdGeneral.Value : 0;
                var maxStdGeneral = labTest.MaxStdGeneral.HasValue ? labTest.MaxStdGeneral.Value : 0;
                var minStdFemale = labTest.MinStdFemale.HasValue ? labTest.MinStdFemale.Value : 0;
                var maxStdFemale = labTest.MaxStdFemale.HasValue ? labTest.MaxStdFemale.Value : 0;
                var minStdMale = labTest.MinStdMale.HasValue ? labTest.MinStdMale.Value : 0;
                var maxStdMale = labTest.MaxStdMale.HasValue ? labTest.MaxStdMale.Value : 0;

                if (patient != null)
                {
                    var gender = patient.Gender;
                    if (gender == Gender.Male)
                    {
                        if (minStdMale > 0 && maxStdMale > 0)
                        {
                            labTestRange = "[ " + minStdMale.ToString() + " - " + maxStdMale.ToString() + " ]";
                        }
                        else if (minStdGeneral > 0 && maxStdGeneral > 0)
                        {
                            labTestRange = "[ " + minStdGeneral.ToString() + " - " + maxStdGeneral.ToString() + " ]";
                        }
                    }
                    if (gender == Gender.Female)
                    {
                        if (minStdFemale > 0 && maxStdFemale > 0)
                        {
                            labTestRange = "[ " + minStdFemale.ToString() + " - " + maxStdFemale.ToString() + " ]";
                        }
                        else if (minStdGeneral > 0 && maxStdGeneral > 0)
                        {
                            labTestRange = "[ " + minStdGeneral.ToString() + " - " + maxStdGeneral.ToString() + " ]";
                        }
                    }
                }
            }
            return labTestRange;
        }
        #endregion

        #endregion

    }
}
