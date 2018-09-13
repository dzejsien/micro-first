using Microsoft.EntityFrameworkCore;
using Policy.Domain;

namespace Policy.DataAccess.SqlServer
{
    using PolicyEntity = Domain.Policy;

    public partial class PolicyContext : DbContext
    {
        public PolicyContext()
        {
        }

        public PolicyContext(DbContextOptions<PolicyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Insured> Insured { get; set; }
        public virtual DbSet<PolicyEntity> Policy { get; set; }
        public virtual DbSet<PolicyProduct> PolicyProduct { get; set; }
        public virtual DbSet<Product> Product { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=MicroDemo;Database=MicroDemo_Policy;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Insured>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<PolicyEntity>(entity =>
            {
                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.HasOne(d => d.Insured)
                    .WithMany(p => p.Policy)
                    .HasForeignKey(d => d.InsuredId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Policy_Insured");
            });

            modelBuilder.Entity<PolicyProduct>(entity =>
            {
                entity.HasKey(e => new { e.PolicyId, e.ProductId });

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.PolicyProduct)
                    .HasForeignKey(d => d.PolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PolicyProduct_Policy");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PolicyProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PolicyProduct_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
