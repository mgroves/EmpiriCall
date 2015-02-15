using System.Data.Entity.ModelConfiguration;
using EmpiriCall.Data.Data;

namespace EmpiriCall.Data.SQLServer.Entities
{
    internal class MetaDataMap : EntityTypeConfiguration<MetaData>
    {
        public MetaDataMap()
        {
            this.ToTable("EmpiriCall_MetaData");
            this.HasKey(x => x.Id);
            this.Property(x => x.LastUpdated);
            this.HasMany(x => x.ActionInfo);
        }
    }
}