# 🎉 WinScene Kurulum Rehberi

## 📋 Genel Bakış

Bu sistem, tüm zombiler öldüğünde otomatik olarak **WinScene** sahnesini gösterir.

## 🔧 Unity'de Yapılacaklar

### 1️⃣ ZombieManager'ı Sahneye Ekle

1. **Hierarchy**'de boş bir GameObject oluştur (örneğin: "ZombieManager")
2. Bu GameObject'e **ZombieManager** script'ini ekle (Add Component → ZombieManager)
3. Inspector'da **Victory Scene Name** alanının **"WinScene"** olduğundan emin ol (zaten varsayılan olarak ayarlı)

### 2️⃣ Zombilerin HealthSystem Component'i Olduğundan Emin Ol

- Her zombide **HealthSystem** component'i olmalı
- HealthSystem'de **Is Player Object** checkbox'ı **işaretli OLMAMALI** (sadece Player için işaretli olmalı)

### 3️⃣ WinScene Sahnesini Build Settings'e Ekle

1. **File → Build Settings** aç
2. WinScene sahnesini **Scenes In Build** listesine ekle (sürükle-bırak ile)
3. WinScene'in listede olduğundan emin ol

### 4️⃣ Test Et

1. Oyunu çalıştır (Play)
2. Console'da şu mesajı görmelisin: `🧟 ZombieManager başlatıldı. Toplam X zombi bulundu.`
3. Tüm zombileri öldür
4. Her zombi öldüğünde Console'da: `💀 Zombi öldü: [isim]`
5. Son zombi öldüğünde: `✅ Tüm zombiler öldü! Kazanma ekranı tetikleniyor...`
6. 1.5 saniye sonra WinScene sahnesi yüklenmeli

## ⚠️ Sorun Giderme

**WinScene gelmiyor:**
- ZombieManager sahneye ekli mi?
- Console'da `🧟 ZombieManager başlatıldı` mesajı görünüyor mu?
- Tüm zombilerin HealthSystem component'i var mı?
- WinScene sahnesi Build Settings'e ekli mi?
- WinScene sahne adı tam olarak "WinScene" mi? (büyük/küçük harf önemli)

**Zombiler sayılmıyor:**
- Console'da `🧟 ZombieManager başlatıldı. Toplam 0 zombi bulundu.` görüyorsan:
  - Zombilerin HealthSystem component'i var mı?
  - HealthSystem'de **Is Player Object** işaretli mi? (işaretli OLMAMALI)
  - Zombiler sahne başladığında aktif mi?

**Console'da hata var:**
- Hata mesajını oku ve kontrol et
- ZombieManager sadece bir tane olmalı (birden fazla olmamalı)

## 📝 Notlar

- ZombieManager otomatik olarak sahne başladığında tüm zombileri bulur
- Her zombi öldüğünde ZombieManager'a bildirilir
- Tüm zombiler öldüğünde otomatik olarak WinScene yüklenir
- Varsayılan bekleme süresi 1.5 saniye (Inspector'da değiştirebilirsin)


