using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BenchLab.Model;

namespace BenchLab.DataAccess
{
    public class PatientDBContext : DbContext
    {
        #region Properties - DBSet

        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientTestReport> PatientTestReports { get; set; }

        #endregion

        #region Contructors
        public PatientDBContext(string connectionString)
            : base(connectionString)
        {
        }
        #endregion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<PatientDBContext>(null); // <--- This is what i needed

            modelBuilder.Configurations.Add(new PatientConfiguration());
            modelBuilder.Configurations.Add(new PatientTestReportConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
