using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EmpiriCall.Data.Data;

namespace EmpiriCall.Data.SQLServer.Entities
{
    public class CustomValueMap : EntityTypeConfiguration<CustomValue>
    {
        public CustomValueMap()
        {
            this.ToTable("EmpiriCall_DetailRecord_CustomValue");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(x => x.Id);
            this.Property(x => x.Key);
            this.Property(x => x.Value);
        }
    }
}