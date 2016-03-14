using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BenchLab.Model;

namespace BenchLab.DataAccess
{
    public class LabTestConfiguration : EntityTypeConfiguration<LabTest>
    {
        internal LabTestConfiguration()
            : base()
        {
            this.HasKey(p => p.ID);

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.TestName).
              HasColumnName("TESTNAME").
              IsRequired();

            Property(p => p.Status).
               HasColumnName("STATUS").
               IsRequired();

            HasRequired(x => x.LabTestCategory).WithMany(x => x.LabTestCollection);

            ToTable("TBL_LABTEST");
        }
    }

    public class LabTestCategoryConfiguration : EntityTypeConfiguration<LabTestCategory>
    {
        internal LabTestCategoryConfiguration()
            : base()
        {
            this.HasKey(p => p.ID);

            //this.HasOptional(c => c.ParentLabTestCategory)
            //    .WithMany()
            //    .Map(m => m.MapKey(fk => fk.ID, "ParentCategoryID"));

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.LabTestCategoryName).
               HasColumnName("LABTESTCATEGORYNAME").
               IsRequired();

            Property(p => p.Status).
               HasColumnName("STATUS").
               IsRequired();

            ToTable("TBL_LABTEST_CATEGORY");
        }
    }

    public class LabTestUnitConfiguration : EntityTypeConfiguration<LabTestUnit>
    {
        internal LabTestUnitConfiguration()
            : base()
        {
            this.HasKey(p => p.ID);

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.UnitName).
               HasColumnName("UNITNAME").
               IsRequired();

            Property(p => p.Status).
               HasColumnName("STATUS").
               IsRequired();

            ToTable("TBL_LABTEST_UNIT");
        }
    }

    public class LabTestReportConfiguration : EntityTypeConfiguration<LabTestReport>
    {
        internal LabTestReportConfiguration()
            : base()
        {
            this.HasKey(p => p.ID);

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.ReportDescription).
              HasColumnName("REPORTDESCRIPTION").
              IsRequired();

            Property(p => p.Status).
               HasColumnName("STATUS").
               IsRequired();

            HasRequired(x => x.LabTest).WithMany(x => x.LabTestReportCollection);

            HasRequired(x => x.PatientTestReport).WithMany(x => x.LabTestReportCollection);

            ToTable("TBL_LABTEST_REPORT");
        }
    }
    
}
