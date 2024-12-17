using SampleProjectInterns.Entities;
using SampleProjectInterns.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace SampleProjectInterns.Persistence;
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Identity> Identities => Set<Identity>();
	public DbSet<Restoran> Restoranlar => Set<Restoran>();
	public DbSet<Siparis> Siparisler => Set<Siparis>();
	public DbSet<SiparisDetay> SiparisDetaylar => Set<SiparisDetay>();
	public DbSet<Odeme> Odemeler => Set<Odeme>();
	public DbSet<Adres> Adresler => Set<Adres>();
	public DbSet<Yorum> Yorumlar => Set<Yorum>();
	public DbSet<Urun> Urunler => Set<Urun>();
	public DbSet<City> Cities => Set<City>();
	public DbSet<County> Counties => Set<County>();
	public DbSet<KategoriRestoran> KategoriRestoranlar => Set<KategoriRestoran>();



	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Identity>(ConfigureIdentity);
		modelBuilder.Entity<Restoran>(ConfigureRestoran);
		modelBuilder.Entity<Urun>(ConfigureUrun); // Urun yapılandırması
		modelBuilder.Entity<Siparis>(ConfigureSiparis);
		modelBuilder.Entity<SiparisDetay>(ConfigureSiparisDetay);
		modelBuilder.Entity<Odeme>(ConfigureOdeme);
		modelBuilder.Entity<Adres>(ConfigureAdres);
		modelBuilder.Entity<Yorum>(ConfigureYorum);
		modelBuilder.Entity<City>(ConfigureCities);
		modelBuilder.Entity<County>(ConfigureCounties);
		modelBuilder.Entity<KategoriRestoran>(ConfigureRestoranKategori);


	}


	private void ConfigureIdentity(EntityTypeBuilder<Identity> builder)
    {
        builder.ToTable("Identities", Schemas.Identity);


        builder.HasIndex(i => i.Email).IsUnique();

        builder.Property(i => i.Email).HasMaxLength(512);

        builder.Property(i => i.Password).HasMaxLength(1024);
        builder.Property(i => i.Salt).HasMaxLength(2048);

        builder.Property(i => i.Name).HasMaxLength(512);
        builder.Property(i => i.LastName).HasMaxLength(512);
		builder.HasMany(k => k.Siparisler)
				   .WithOne()
				   .HasForeignKey(s => s.IdentityId);
		builder.HasMany(k => k.Adresler)
				   .WithOne()
				   .HasForeignKey(s => s.IdentityId);
		builder.HasMany(k => k.Yorumlar)
				   .WithOne()
				   .HasForeignKey(s => s.IdentityId);
	}
	private void ConfigureRestoran(EntityTypeBuilder<Restoran> builder)
	{
		builder.ToTable("Restoranlar");
		builder.HasKey(r => r.Id);
		builder.Property(r => r.Ad).IsRequired().HasMaxLength(255);

		// Restoran ile Urun ilişkisi
		builder.HasMany(r => r.Urunler)
			   .WithOne()
			   .HasForeignKey(u => u.RestoranId);
		builder.HasMany(r => r.Siparisler)
			   .WithOne()
			   .HasForeignKey(u => u.RestoranId);
		builder.HasMany(r => r.Identities)
			   .WithOne()
			   .HasForeignKey(u => u.RestoranId);
		
	}
	private void ConfigureUrun(EntityTypeBuilder<Urun> builder)
	{
		builder.ToTable("Urunler");
		builder.HasKey(u => u.Id);
		builder.Property(u => u.Ad).IsRequired().HasMaxLength(255);
		builder.Property(u => u.Aciklama).HasMaxLength(1000);
		builder.Property(u => u.Fiyat).HasColumnType("decimal(18,2)");

		// Ürün ile SiparisDetay ilişkisi
		builder.HasMany(u => u.SiparisDetaylari)
			   .WithOne()
			   .HasForeignKey(sd => sd.UrunId);
	}
	private void ConfigureSiparis(EntityTypeBuilder<Siparis> builder)
	{
		builder.ToTable("Siparisler");
		builder.HasKey(s => s.Id);
		builder.Property(s => s.ToplamTutar).HasColumnType("decimal(18,2)");
		builder.Property(s => s.Durum).IsRequired();

		 builder.HasMany(u => u.SiparisDetaylari)
			   .WithOne()
			   .HasForeignKey(sd => sd.SiparisId);

		builder.HasMany(u => u.Yorumlar)
			  .WithOne()
			  .HasForeignKey(sd => sd.SiparisId);
		builder.HasOne(s => s.Restoran)
	   .WithMany(r => r.Siparisler)
	   .HasForeignKey(s => s.RestoranId)
	   .OnDelete(DeleteBehavior.Restrict);

	}
	private void ConfigureSiparisDetay(EntityTypeBuilder<SiparisDetay> builder)
	{
		builder.ToTable("SiparisDetaylar");
		builder.HasKey(sd => sd.Id);
		builder.Property(sd => sd.Adet).IsRequired();
		builder.Property(sd => sd.Fiyat).HasColumnType("decimal(18,2)");
		builder.HasOne(s => s.Urun)
   .WithMany(r => r.SiparisDetaylari)
   .HasForeignKey(s => s.UrunId)
   .OnDelete(DeleteBehavior.Restrict);


	}
	private void ConfigureOdeme(EntityTypeBuilder<Odeme> builder)
	{
		builder.ToTable("Odemeler");
		builder.HasKey(o => o.Id);
		
		

		
	}
	private void ConfigureAdres(EntityTypeBuilder<Adres> builder)
	{
		builder.ToTable("Adresler");
		builder.HasKey(a => a.Id);
		builder.Property(a => a.AdresBilgisi).IsRequired().HasMaxLength(1000);
		builder.HasMany(u => u.Siparisler)
			  .WithOne()
			  .HasForeignKey(sd => sd.AdresId);


		// Adres ile Sipariş ilişkisi

	}
	private void ConfigureYorum(EntityTypeBuilder<Yorum> builder)
	{
		builder.ToTable("Yorumlar");
		builder.HasKey(y => y.Id);
		builder.HasOne(s => s.Identity)
.WithMany(r => r.Yorumlar)
.HasForeignKey(s => s.IdentityId)
.OnDelete(DeleteBehavior.Restrict);


		// Yorum ile Restoran ilişkisi

	}
	private void ConfigureCities(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities", Schemas.Definition);
        builder.HasIndex(i => i.Id).IsUnique();
    }
    private void ConfigureCounties(EntityTypeBuilder<County> builder)
    {
        builder.ToTable("Counties", Schemas.Definition);
        builder.HasIndex(i => i.Id).IsUnique();
        
    }
	private void ConfigureRestoranKategori(EntityTypeBuilder<KategoriRestoran> builder)
	{
		builder.ToTable("RestoranKategori");
		builder.HasKey(u => u.Id);
		builder.Property(u => u.Ad).IsRequired().HasMaxLength(255);

		// Ürün ile SiparisDetay ilişkisi
		builder.HasMany(u => u.Restoranlar)
			   .WithOne()
			   .HasForeignKey(sd => sd.KategoriRestoranId);
	}



	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetCreatedAtAndUpdatedAt();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetCreatedAtAndUpdatedAt()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => (x.Entity is ICreatedAt || x.Entity is IUpdatedAt)
                && (x.State == EntityState.Added || x.State == EntityState.Modified))
            .ToList();

        if (entities.Count == 0) return;

        var currentDate = DateTime.UtcNow;

        foreach (var entity in entities)
        {
            switch (entity.State)
            {
                case EntityState.Modified:
                    if (entity.Entity is IUpdatedAt updatedAtEntity)
                    {
                        updatedAtEntity.UpdatedAt = currentDate;
                    }

                    break;

                case EntityState.Added:
                    if (entity.Entity is ICreatedAt createdAtEntity)
                    {
                        createdAtEntity.CreatedAt = currentDate;
                    }
                    break;
            }
        }
    }
}

