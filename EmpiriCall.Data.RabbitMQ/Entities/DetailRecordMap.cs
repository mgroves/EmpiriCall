using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EmpiriCall.Data.Data;

namespace EmpiriCall.Data.RabbitMQ.Entities
{
    internal class DetailRecordMap : EntityTypeConfiguration<DetailRecord>
    {
        public DetailRecordMap()
        {
            this.ToTable("EmpiriCall_DetailRecord");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(x => x.Id);
            this.Property(x => x.TimeStamp);
            this.Property(x => x.UserName);

            this.HasMany(x => x.CustomValues);
        }
    }
}