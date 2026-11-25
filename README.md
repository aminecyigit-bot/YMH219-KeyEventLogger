\# YMH219 - Key Event Logger (Eğitsel Klavye Olayı Kaydedici)



\## Proje Adı ve Kısa Açıklama

\*\*YMH219-KeyEventLogger\*\*, Nesne Tabanlı Programlama (YMH 219) dersi kapsamında C# ve .NET Windows Forms kullanılarak geliştirilmiş eğitsel bir klavye olayı izleme uygulamasıdır. Uygulama yalnızca kendi penceresi aktif (odakta) iken basılan tuşları milisaniye hassasiyetinde zaman damgasıyla kaydeder, istatistik üretir ve yerel `.txt` dosyasına aktarır.



\## Kullanılan Teknoloji / NuGet Paketleri

\* \*\*Programlama Dili:\*\* C#

\* \*\*Platform / Framework:\*\* .NET (Windows Forms)

\* \*\*Sistem Kütüphaneleri:\*\* `System.Windows.Forms`, `System.Drawing`, `System.IO`, `System.Linq`, `System.Text` (Harici NuGet paketi gerektirmez, doğrudan derlenip çalıştırılabilir.)



\## Kurulum ve Çalıştırma Adımları

1\. Depoyu bilgisayarınıza klonlayın: `git clone \[https://github.com/aminecyigit-bot/YMH219-KeyEventLogger.git](https://github.com/aminecyigit-bot/YMH219-KeyEventLogger.git)`

2\. Klasör içerisindeki `YMH219-KeyEventLogger.slnx` çözüm dosyasını \*\*Visual Studio\*\* ile açın.

3\. Üst menüden \*\*Start (F5)\*\* butonuna basarak uygulamayı derleyin ve çalıştırın.



\## Uygulamanın Temel Özellikleri

\* \*\*Nesne Tabanlı Mimari:\*\* Tek bir tuş kaydını temsil eden `KeyLogEntry` modeli ile kayıt listesi, istatistik ve dosya işlemlerini yöneten `KeyLoggerManager` sınıfı ayrı sorumluluklarla tasarlanmıştır.

\* \*\*Kayıt Kontrolü ve Durum Göstergesi:\*\* \*\*Başlat\*\*, \*\*Durdur\*\*, \*\*Temizle\*\* butonları ve anlık kayıt durumunu bildiren renkli durum göstergesi bulunur.

\* \*\*Canlı İstatistikler:\*\* Toplam basılan tuş sayısı ve en sık kullanılan ilk 3 tuş anlık olarak hesaplanır.

\* \*\*Tarih/Saat Filtreleme ve Dışa Aktarma:\*\* Kayıtlar belirli zaman aralığına göre filtrelenebilir ve rapor formatında `.txt` dosyasına kaydedilebilir.



\## Ekran Görüntüleri

\### Aktif Kayıt ve İstatistik Ekranı

!\[Kayıt Aktif](screenshots/kayit-aktif.png)



\### Tarih/Saat Filtresi ve Kayıt Görünümü

!\[Filtre ve Kayıt](screenshots/filtre-ve-kayit.png)



\## Bilinen Sınırlamalar / Güvenlik Sınırı

\* Bu uygulama tamamen eğitsel amaçlıdır; \*\*global keyboard hook\*\* kullanmaz, arka planda veya gizli modda çalışmaz ve yalnızca kendi formu aktifken yapılan tuş girişlerini yakalar.



\## Öğrenci Bilgileri

\* \*\*Adı-Soyadı:\*\* Amine Ceren Yiğit

\* \*\*Öğrenci Numarası:\*\* 250541048

