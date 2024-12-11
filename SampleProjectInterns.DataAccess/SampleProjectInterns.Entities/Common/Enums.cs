namespace SampleProjectInterns.Entities.Common;

public class Enums
{
    //SABİT DEĞERLER İÇİN ENUMLAR KULLANILIR
    public enum AdminAuthorization : byte
    {
        none = 0,
        admin = 1,
        moderator = 2, 
        user = 3//en basit yetkili 
    }

    public enum Status : byte
    {
        none = 0,
        approved = 1,
        unapproved = 2,
        suspended = 3,
        deleted = 4
    }
    public enum AccountVerification : byte
    {
        sms = 1,
        eposta
    }
    public enum Gender : byte {
        none = 0,
        male,
        female
    }
	public enum SiparisDurumu:byte
	{
		Hazirlaniyor,
		Yolda,
		TeslimEdildi
	}
	public enum OdemeTipi:byte
	{
		KrediKarti,
		Nakit,
		Cuzdan
	}
}
