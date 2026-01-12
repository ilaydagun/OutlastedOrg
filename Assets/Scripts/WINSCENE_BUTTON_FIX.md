# 🔧 WinScene Buton Sorun Giderme Rehberi

## ❌ Butonlar Çalışmıyor - Kontrol Listesi

### 1️⃣ EventSystem Kontrolü

WinScene'de **EventSystem** olmalı:
- Hierarchy'de **EventSystem** GameObject'i var mı?
- Yoksa: Hierarchy'de sağ tıklayın → **UI → Event System**

### 2️⃣ WinScreenButtons Script'i Kontrolü

WinScene'de **WinScreenButtons** script'i bir GameObject'e ekli olmalı:
- Hierarchy'de WinScreenButtons script'ine sahip bir GameObject var mı?
- Genellikle boş bir GameObject'e eklenir (örneğin: "WinScreenButtons" veya "ButtonsController")
- Script aktif mi? (Inspector'da checkbox işaretli mi?)

### 3️⃣ Buton OnClick Event'leri Kontrolü

Her butonun **OnClick** event'i bağlı olmalı:

**Main Menu / MainPage Butonu:**
1. Hierarchy'de butonu seçin
2. Inspector'da **Button** component'ini bulun
3. **OnClick ()** bölümüne bakın
4. **+** butonuna tıklayın (liste boşsa)
5. **None (Object)** alanına WinScreenButtons script'inin olduğu GameObject'i sürükleyin
6. Dropdown'dan **WinScreenButtons → ReturnToMenu()** seçin

**Quit Butonu:**
1. Hierarchy'de butonu seçin
2. Inspector'da **Button** component'ini bulun
3. **OnClick ()** bölümüne bakın
4. **+** butonuna tıklayın (liste boşsa)
5. **None (Object)** alanına WinScreenButtons script'inin olduğu GameObject'i sürükleyin
6. Dropdown'dan **WinScreenButtons → QuitGame()** seçin

### 4️⃣ Cursor Kontrolü

ZombieManager WinScene'e geçerken cursor'ı serbest bırakıyor, bu doğru. Ama emin olmak için:
- Oyun çalışırken cursor görünüyor mu?
- Cursor butonların üzerine gelince değişiyor mu? (hover efekti)

### 5️⃣ Buton Interactable Kontrolü

Her butonun **Interactable** checkbox'ı işaretli olmalı:
- Butonu seçin
- Inspector'da **Button** component'inde **Interactable** işaretli mi?

## 🎯 Hızlı Çözüm Adımları

1. **WinScene'i açın**
2. **Hierarchy'de EventSystem var mı kontrol edin** (yoksa ekleyin)
3. **WinScreenButtons script'ini bulun veya ekleyin:**
   - Boş bir GameObject oluşturun (örneğin: "ButtonController")
   - WinScreenButtons script'ini ekleyin
4. **Her butonu kontrol edin:**
   - Butonu seçin
   - Inspector'da OnClick event'ini kontrol edin
   - Gerekirse yeniden bağlayın

## 📝 Notlar

- **ReturnToMenu()** → MainMenu sahnesine gider
- **QuitGame()** → Oyunu kapatır (build'de çalışır, editörde test ederken çalışmaz)


