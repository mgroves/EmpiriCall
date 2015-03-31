using System.Data.Common;
using System.Data.Entity;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.RabbitMQ.Entities;

namespace EmpiriCall.Data.RabbitMQ
{
    public class EmpiriCallDbContext : DbContext
    {
        public DbSet<MetaData> MetaData { get; set; }
        public DbSet<DetailRecord> DetailRecord { get; set; }

        public EmpiriCallDbContext(DbConnection connection): base(connection, false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MetaDataMap());
            modelBuilder.Configurations.Add(new ActionInfoMap());
            modelBuilder.Configurations.Add(new ParameterBasicInfoMap());
            modelBuilder.Configurations.Add(new DetailRecordMap());
            modelBuilder.Configurations.Add(new CustomValueMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}