using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BenchLab.Model;

namespace BenchLab.DataAccess
{
    public class LabTestDBContext : DbContext
    {
        #region Properties - DBSet

        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabTestCategory> LabTestCategories { get; set; }
        public DbSet<LabTestUnit> LabTestUnits { get; set; }
        public DbSet<LabTestReport> LabTestReports { get; set; }

        #endregion

        #region Contructors
        public LabTestDBContext(string connectionString)
            : base(connectionString)
        {
        }
        #endregion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<LabTestDBContext>(null); // <--- This is what i needed

            modelBuilder.Configurations.Add(new LabTestConfiguration());
            modelBuilder.Configurations.Add(new LabTestCategoryConfiguration());
            modelBuilder.Configurations.Add(new LabTestUnitConfiguration());
            modelBuilder.Configurations.Add(new LabTestReportConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
