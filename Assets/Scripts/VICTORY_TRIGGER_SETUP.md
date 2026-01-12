# 🎉 Kazanma Trigger Sistemi Kurulum Rehberi

## 📋 Genel Bakış

Bu sistem, oyuncu belirli bir noktayı (kırmızı çizgi) geçtiğinde otomatik olarak kazanma ekranını gösterir.

## 🔧 Kurulum Adımları

### 1️⃣ VictoryTrigger Zone Oluştur

1. **Hierarchy**'de boş bir GameObject oluştur (örneğin: "VictoryTrigger")
2. Bu GameObject'e **VictoryTrigger** script'ini ekle (Add Component → VictoryTrigger)
3. Bu GameObject'e bir **Collider** ekle:
   - **Box Collider** önerilir (en kolay)
   - Collider'ın **Is Trigger** checkbox'ını işaretle (✓)
4. Collider'ı istediğin boyutlara ayarla (örneğin: genişlik 20, yükseklik 5, derinlik 1)
5. VictoryTrigger'ı oyunun sonunda, oyuncunun geçmesi gereken yere yerleştir

### 2️⃣ Kırmızı Çizgi Görseli Oluştur (Opsiyonel ama Önerilir)

**Seçenek A: RedLineVisual Script'i ile**

1. VictoryTrigger GameObject'inin yanına boş bir GameObject oluştur (örneğin: "RedLine")
2. Bu GameObject'e **RedLineVisual** script'ini ekle
3. Inspector'da ayarları yap:
   - **Line Height**: Çizginin yüksekliği (örneğin: 3)
   - **Line Width**: Çizginin kalınlığı (örneğin: 0.1)
   - **Line Length**: Çizginin uzunluğu (örneğin: 20)
   - **Line Color**: Kırmızı (varsayılan)
   - **Use Line Renderer**: ✓ (işaretli)
4. RedLine GameObject'ini VictoryTrigger'ın konumuna yerleştir

**Seçenek B: Manuel 3D Cube ile**

1. Hierarchy'de **3D Object → Cube** oluştur
2. Scale'i ayarla (örneğin: X=20, Y=3, Z=0.1)
3. Material oluştur ve rengini kırmızı yap
4. Cube'u VictoryTrigger'ın konumuna yerleştir
5. VictoryTrigger'ın **Red Line Visual** alanına bu Cube'u sürükle

### 3️⃣ VictoryTrigger Ayarları

VictoryTrigger script'inde:
- **Victory Scene Name**: Kazanma ekranı sahnenizin adı (örneğin: "VictoryScreen")
- **Victory UI Panel**: Eğer sahne yerine in-game UI kullanmak istersen, buraya UI panel GameObject'ini sürükle
- **Delay Before Victory**: Kazanma ekranından önce bekleme süresi (varsayılan: 0.5 saniye)
- **Target Tag**: "Player" (oyuncunun tag'i)

### 4️⃣ Player Tag Kontrolü

Oyuncunun GameObject'inin tag'inin **"Player"** olduğundan emin ol:
1. Hierarchy'de Player GameObject'ini seç
2. Inspector'da üst kısımdaki **Tag** dropdown'ından "Player" seç
3. Eğer "Player" tag'i yoksa: **Add Tag...** → **+** → "Player" ekle

### 5️⃣ VictoryScreen Sahnesi Oluştur

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

### 6️⃣ Sahneyi Build Settings'e Ekle

1. **File → Build Settings** aç
2. VictoryScreen sahnesini **Scenes In Build** listesine ekle
3. Scene'leri sürükle-bırak ile ekleyebilirsin

## 🎮 Test Et

1. Oyunu çalıştır
2. Oyuncuyu VictoryTrigger zone'una doğru hareket ettir
3. Zone'a girdiğinde kazanma ekranı görünmeli

## ⚙️ Nasıl Çalışıyor?

1. VictoryTrigger bir **Trigger Collider** zone'u oluşturur
2. Oyuncu (Player tag'ine sahip) bu zone'a girdiğinde `OnTriggerEnter` tetiklenir
3. Belirlenen süre sonra kazanma ekranı gösterilir (sahne veya UI panel)

## 🐛 Sorun Giderme

**Kazanma ekranı görünmüyor:**
- VictoryTrigger GameObject'i sahneye ekli mi?
- Collider'ın **Is Trigger** işaretli mi?
- Player'ın tag'i "Player" mı?
- Victory Scene Name doğru mu? (sahne adı tam olarak eşleşmeli)
- Sahne Build Settings'e ekli mi?

**Kırmızı çizgi görünmüyor:**
- RedLineVisual script'i ekli mi?
- Line Renderer veya Cube görseli oluşturuldu mu?
- GameObject'ler doğru konumda mı?

**Trigger çalışmıyor:**
- Collider'ın **Is Trigger** işaretli mi?
- Player'ın tag'i doğru mu?
- Collider'ın boyutu yeterince büyük mü?
- Console'da hata mesajı var mı?

## 📝 Notlar

- VictoryTrigger zone'unu oyunun sonunda, oyuncunun geçmesi gereken yere yerleştir
- Zone'un boyutunu oyuncunun kolayca geçebileceği şekilde ayarla
- Kırmızı çizgi görseli opsiyoneldir ama oyuncuya nereye gitmesi gerektiğini gösterir
- Scene view'da Gizmos'u açarak trigger zone'unu görebilirsin (kırmızı, yarı saydam kutu)


