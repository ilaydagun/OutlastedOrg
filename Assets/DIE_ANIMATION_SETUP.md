# Die Animasyonu Kurulum Kılavuzu

## 1. Death Effect Prefab Nedir? (OPSİYONEL)

Death Effect Prefab, oyuncu öldüğünde gösterilecek özel bir particle system efektidir. 

**Eğer özel bir efekt istemiyorsanız, hiçbir şey yapmanıza gerek yok!** Kod otomatik olarak basit bir kırmızı parçacık efekti oluşturur.

**Eğer özel efekt istiyorsanız:**

1. Unity Editor'da Hierarchy'de sağ tıklayın
2. **Effects → Particle System** seçin
3. Particle System'i istediğiniz gibi ayarlayın (renk, boyut, hız vb.)
4. Bu GameObject'i **Prefabs** klasörüne sürükleyip prefab yapın
5. MainCharacter GameObject'ine gidin
6. **HealthSystem** component'inde **Death Effect Prefab** alanına bu prefab'ı sürükleyin

---

## 2. Animator Controller'da Die Animasyonu Kurulumu

### Adım 1: Die Trigger Parametresi Ekleme

1. Unity Editor'da **PlayerAnimator.controller** dosyasını açın (double-click)
2. **Parameters** sekmesine gidin (sol üstte)
3. **+** butonuna tıklayın
4. **Trigger** seçin
5. İsmini **"Die"** yapın (büyük-küçük harf duyarlı!)

### Adım 2: Die State Oluşturma

1. Animator penceresinde boş bir alana sağ tıklayın
2. **Create State → Empty** seçin
3. Yeni state'e tıklayın, Inspector'da ismini **"Die"** yapın
4. Inspector'da **Motion** alanına **die.fbx** animasyonunu sürükleyin
5. **Write Defaults** işaretli olsun

### Adım 3: Transition (Geçiş) Oluşturma

**AnyState'ten Die State'ine Transition:**

1. Animator penceresinde **Any State** (üstteki turuncu kutu) üzerine sağ tıklayın
2. **Make Transition** seçin
3. Oluşan ok'u **Die** state'ine sürükleyin
4. Bu transition'a tıklayın (ok üzerinde)
5. Inspector'da:
   - **Conditions** bölümünde **+** butonuna tıklayın
   - **Die** trigger'ını seçin
   - **Has Exit Time** işaretini KALDIRIN (çok önemli!)
   - **Transition Duration** 0.1 yapın (hızlı geçiş için)

**Die State'ten Exit'e Transition (Opsiyonel - animasyon bitince çıkmak için):**

1. **Die** state'ine sağ tıklayın
2. **Make Transition** seçin
3. **Exit** (sağ alttaki kırmızı kutu) üzerine sürükleyin
4. Bu transition'a tıklayın
5. Inspector'da:
   - **Has Exit Time** işaretli olsun
   - **Exit Time** 0.95 yapın (animasyonun %95'i bitince çık)
   - **Transition Duration** 0.1 yapın

### Adım 4: Die State Ayarları

1. **Die** state'ine tıklayın
2. Inspector'da:
   - **Speed**: 1.0 (normal hız)
   - **Write Defaults**: İşaretli
   - **Motion**: die.fbx animasyonu atanmış olmalı

---

## 3. Test Etme

1. Play moduna geçin
2. MainCharacter'a hasar verin (canı 0'a düşürün)
3. Die animasyonu oynamalı
4. Ölüm efekti görünmeli
5. Birkaç saniye sonra MainMenu sahnesine geçilmeli

---

## Önemli Notlar

- **Die trigger parametresi** mutlaka **"Die"** olmalı (büyük D, küçük ie)
- **Has Exit Time** AnyState'ten Die'ye transition'da **KAPALI** olmalı (hemen geçiş için)
- Die animasyonu **loop olmamalı** (tek seferlik oynatılmalı)
- Eğer animasyon oynamıyorsa, Animator Controller'ın MainCharacter'a atandığından emin olun

---

## Sorun Giderme

**Animasyon oynamıyor:**
- Die trigger parametresi eklendi mi?
- Transition doğru yapıldı mı?
- die.fbx animasyonu Die state'ine atandı mı?
- Animator Controller MainCharacter'a atandı mı?

**Menü açılmıyor:**
- MainMenu sahnesi Build Settings'te ekli mi?
- SceneManager.LoadScene("MainMenu") doğru sahne ismini kullanıyor mu?

