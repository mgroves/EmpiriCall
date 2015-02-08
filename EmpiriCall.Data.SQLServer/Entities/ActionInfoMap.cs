using System.Data.Entity.ModelConfiguration;
using EmpiriCall.Data.Data;

namespace EmpiriCall.Data.SQLServer.Entities
{
    public class ActionInfoMap : EntityTypeConfiguration<ActionInfo>
    {
        public ActionInfoMap()
        {
            this.ToTable("EmpiriCall_MetaData_ActionInfo");
            this.HasKey(x => x.Id);
            this.Property(x => x.ControllerName);
            this.Property(x => x.ActionName);
            this.Property(x => x.Feature);
            this.HasMany(x => x.ParameterInfo);
        }
    }
}