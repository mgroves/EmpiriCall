using System.Data.Entity.ModelConfiguration;
using EmpiriCall.Data.Data;

namespace EmpiriCall.Data.RabbitMQ.Entities
{
    internal class ParameterBasicInfoMap : EntityTypeConfiguration<ParameterBasicInfo>
    {
        public ParameterBasicInfoMap()
        {
            this.ToTable("EmpiriCall_MetaData_ActionInfo_ParameterBasicInfo");
            this.HasKey(x => x.Id);
            this.Property(x => x.ParameterTypeFullName);
            this.Property(x => x.ParameterName);
        }
    }
}