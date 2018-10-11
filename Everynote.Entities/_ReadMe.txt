Entity sınıflarında dataAnotation kullanmak için EF paketini kurmak gerekmiyor, 
sadece .Net'in 'System.ComponentModel.DataAnnotations.Schema' kütüphanesini referanslara eklemek ve kullanmak yeterli.

*Ayrı yeten
Veritabanı modeli olmayan fakat ihtiyaçlar sonucunda oluşturulması gereken diğer modeller 'DTO', 'ValueObjects' vb. isimler altında klasörlenip Entiy katmanı
altında saklanır. Bu şekilde model ihtiyaçları karşılanır.