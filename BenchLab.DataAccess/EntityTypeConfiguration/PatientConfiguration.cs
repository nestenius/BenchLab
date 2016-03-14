using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BenchLab.Model;

namespace BenchLab.DataAccess
{
    public class PatientConfiguration : EntityTypeConfiguration<Patient>
    {
        internal PatientConfiguration()
            : base()
        {
            this.HasKey(p => p.ID);

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.FirstName).
              HasColumnName("FIRSTNAME").
              IsRequired();

            Property(p => p.LastName).
              HasColumnName("LASTNAME").
              IsRequired();

            Property(p => p.Gender).
              HasColumnName("GENDER").
              IsRequired();

            Property(p => p.Status).
               HasColumnName("STATUS").
               IsRequired();

            ToTable("TBL_PATIENT");
        }
    }

    public class PatientTestReportConfiguration : EntityTypeConfiguration<PatientTestReport>
    {
        internal PatientTestReportConfiguration()
            : base()
        {
            this.HasKey(p => p.ID);

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.Sn).
               HasColumnName("SN").
               IsRequired();

            Property(p => p.ReportDateNp).
              HasColumnName("REPORTDATENP").
              IsRequired();

            Property(p => p.Status).
               HasColumnName("STATUS").
               IsRequired();

            HasRequired(x => x.Patient).WithMany(x => x.PatientTestReportCollection);

            ToTable("TBL_PATIENT_TESTREPORT");
        }
    }

    
}
