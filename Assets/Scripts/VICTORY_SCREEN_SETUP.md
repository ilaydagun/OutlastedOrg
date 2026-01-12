# 🎉 Kazanma Ekranı Kurulum Rehberi

## 📋 Genel Bakış

Bu sistem, oyundaki tüm zombiler öldüğünde otomatik olarak bir kazanma ekranı gösterir.

## 🔧 Kurulum Adımları

### 1️⃣ ZombieManager'ı Sahneye Ekle

1. **Hierarchy**'de boş bir GameObject oluştur (örneğin: "ZombieManager")
2. Bu GameObject'e **ZombieManager** script'ini ekle (Add Component → ZombieManager)
3. Inspector'da ayarları yap:
   - **Delay Before Victory Screen**: Kazanma ekranından önce bekleme süresi (varsayılan: 1.5 saniye)
   - **Victory Scene Name**: Kazanma ekranı sahnesinin adı (örneğin: "VictoryScreen")
   - **Victory UI Panel**: Eğer sahne yerine in-game UI kullanmak istersen, buraya UI panel GameObject'ini sürükle

### 2️⃣ Kazanma Ekranı Sahnesi Oluştur (Önerilen)

**Seçenek A: Yeni Sahne Oluştur**

1. Unity'de **File → New Scene** ile yeni bir sahne oluştur
2. Sahneyi **VictoryScreen** olarak kaydet (Assets/Scenes klasörüne)
3. DeadScreen sahnesini referans alarak benzer bir UI oluştur:
   - Canvas ekle
   - "Kazandınız!" yazısı ekle (Text veya TextMeshPro)
   - "Ana Menü" butonu ekle
   - "Tekrar Oyna" butonu ekle
4. Sahneye boş bir GameObject ekle ve **VictoryScreen** script'ini ekle
5. Butonlara VictoryScreen script'indeki fonksiyonları bağla:
   - "Ana Menü" butonu → `ReturnToMainMenu()`
   - "Tekrar Oyna" butonu → `RestartGame()`

**Seçenek B: In-Game UI Panel (Alternatif)**

1. Oyun sahnesinde bir Canvas oluştur
2. Canvas içinde bir Panel oluştur (örneğin: "VictoryPanel")
3. Panel'i başlangıçta **inactive** yap (Inspector'da checkbox'ı kapat)
4. Panel içine "Kazandınız!" yazısı ve butonlar ekle
5. ZombieManager'ın **Victory UI Panel** alanına bu panel'i sürükle
6. VictoryScreen script'ini bu panel'e veya başka bir GameObject'e ekle

### 3️⃣ Sahne Adlarını Ayarla

**VictoryScreen.cs** script'inde:
- **Main Menu Scene Name**: Ana menü sahnenizin adı (varsayılan: "MainMenu")
- **Game Scene Name**: Oyun sahnenizin adı (varsayılan: "Level1")

Bu değerleri kendi sahne adlarınıza göre değiştirin!

### 4️⃣ Test Et

1. Oyunu çalıştır
2. Tüm zombileri öldür
3. 1.5 saniye sonra kazanma ekranı görünmeli
4. Butonlar çalışmalı

## ⚙️ Nasıl Çalışıyor?

1. **ZombieManager** sahne başladığında tüm zombileri bulur ve listeler
2. Bir zombi öldüğünde, **HealthSystem** ZombieManager'a bildirir
3. ZombieManager canlı zombi sayısını kontrol eder
4. Tüm zombiler öldüğünde, belirlenen süre sonra kazanma ekranını gösterir

## 🐛 Sorun Giderme

**Kazanma ekranı görünmüyor:**
- ZombieManager sahneye eklendi mi kontrol et
- Console'da hata mesajı var mı bak
- Victory Scene Name doğru mu kontrol et (sahne adı tam olarak eşleşmeli)

**Zombiler sayılmıyor:**
- Zombilerin HealthSystem component'i var mı kontrol et
- Zombilerin `isPlayerObject` false olmalı
- Console'da "ZombieManager başlatıldı" mesajını görüyor musun?

**Butonlar çalışmıyor:**
- VictoryScreen script'i sahneye ekli mi?
- Butonların OnClick event'lerine fonksiyonlar bağlı mı?
- Sahne adları doğru mu?

## 📝 Notlar

- ZombieManager singleton pattern kullanır (sahne başına sadece bir tane olmalı)
- Runtime'da yeni zombi eklendiğinde `ZombieManager.Instance.RegisterZombie()` ile ekleyebilirsin
- Debug için `ZombieManager.Instance.GetAliveZombieCount()` ile canlı zombi sayısını öğrenebilirsin


