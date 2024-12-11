using SampleProjectInterns.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IWebDbContext
{
    DbSet<Identity> Identities { get; }
    DbSet<Adres> Adresler { get; } 
    DbSet<City> Cities { get; }
    DbSet<County> Counties { get; }  
    DbSet<Odeme> Odemeler { get; }
    DbSet<Restoran> Restoranlar { get; }
    DbSet<Siparis> Siparisler { get; }
	DbSet<SiparisDetay> SiparisDetaylar { get; }
	DbSet<Urun> Urunler { get; }
	DbSet<Yorum> Yorumlar { get; }
	DbSet<KategoriRestoran> KategoriRestoranlar { get; }






	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}
