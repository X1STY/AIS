using System.Data.Entity;

namespace Lab6AIS
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=Model")
        {
        }
        public virtual DbSet<Product> SmartWatch { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("SmartWatch");
            base.OnModelCreating(modelBuilder);
        }
    }
}
