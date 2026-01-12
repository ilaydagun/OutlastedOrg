# 🗡️ Silah Hasar Sistemi Kurulum Rehberi

## 📍 Scriptlerin Yerleştirilmesi

### 1️⃣ **Player GameObject'ine Eklenecekler:**

```
Player (GameObject)
├── PlayerController (✓ Zaten var)
├── HealthSystem (✓ Zaten var)
├── WeaponHolder (✓ Zaten var - Child olarak)
└── WeaponAttack (🆕 YENİ EKLE!)
```

**Adımlar:**
1. Hierarchy'de **Player** GameObject'ini seç
2. Inspector'da **Add Component** → **WeaponAttack** ekle
3. Ayarları yap:
   - **Current Weapon**: Boş bırak (otomatik bulur)
   - **Animator**: Player'ın Animator component'ini sürükle
   - **Attack Trigger Name**: "Attack" (animasyon trigger adın)

---

### 2️⃣ **Silah Prefab'ına Eklenecekler:**

```
WeaponPrefab (GameObject)
├── Mesh Renderer
├── Collider (Is Trigger = ✓)
└── MeleeWeapon (🆕 YENİ EKLE!)
```

**Adımlar:**
1. Silah prefab'ını aç (Project'te bul)
2. **Add Component** → **MeleeWeapon** ekle
3. Ayarları yap:
   - **Damage**: 25 (veya istediğin değer)
   - **Can Damage**: false (başlangıçta)
   - **Target Tag**: "Enemy"
   - **Prevent Multiple Hits**: ✓ (işaretli)
4. Silahın **Collider**'ını kontrol et:
   - **Is Trigger**: ✓ işaretli olmalı

---

### 3️⃣ **Zombi GameObject'lerine Kontroller:**

```
Zombie (GameObject)
├── Tag: "Enemy" veya "Zombie" (✓ Kontrol et!)
├── HealthSystem (✓ Zaten var olmalı)
└── Collider (✓ Var olmalı)
```

**Kontrol:**
- Zombilerin tag'i **"Enemy"** veya **"Zombie"** olmalı
- Her zombide **HealthSystem** component'i olmalı

---

## 🎮 Kullanım

1. **Saldırı**: Sol mouse tıkla (veya belirlediğin tuş)
2. **Cooldown**: Saldırılar arasında bekleme süresi var
3. **Hasar**: Silah zombiye değdiğinde otomatik hasar verir

---

## ⚙️ Ayarlar (Inspector'da)

### WeaponAttack Ayarları:
- **Attack Duration**: Saldırı süresi (0.5 saniye)
- **Attack Cooldown**: Saldırılar arası bekleme (1 saniye)
- **Attack Key**: Saldırı tuşu (Mouse0 = sol tık)

### MeleeWeapon Ayarları:
- **Damage**: Verilecek hasar miktarı
- **Target Tag**: Hedef tag ("Enemy")

---

## 🔧 Sorun Giderme

**Silah hasar vermiyorsa:**
1. Silahın Collider'ı **Is Trigger** olmalı
2. Zombi tag'i **"Enemy"** veya **"Zombie"** olmalı
3. Zombide **HealthSystem** component'i olmalı
4. Player'da **WeaponAttack** component'i olmalı

**Saldırı çalışmıyorsa:**
1. Player'da **WeaponAttack** component'i var mı?
2. Silah **WeaponHolder**'da mı?
3. Silah prefab'ında **MeleeWeapon** component'i var mı?

