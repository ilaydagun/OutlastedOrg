using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Sahne geçişleri için gerekli kütüphane
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Referansları")]
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public Toggle vsyncToggle;

    [Header("Sayı Yazıları")]
    public TMP_Text sensitivityValueText;
    public TMP_Text volumeValueText;

    // Ana Menü sahnesinin adı. Unity'deki sahne isminle BİREBİR aynı olmalı.
    [Header("Sahne Ayarları")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // --- 1. VOLUME AYARI (0-100 Arası) ---
        // Kayıtta 0.0 - 1.0 arası tutuyoruz ama Slider'da 0-100 gösteriyoruz.
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        // Kaydedilen ondalık değeri (örn 0.5) 100 ile çarpıp slider'a veriyoruz (50)
        volumeSlider.value = savedVolume * 100f;

        // Gerçek sesi ayarla
        AudioListener.volume = savedVolume;
        UpdateVolumeText(volumeSlider.value);


        // --- 2. SENSITIVITY AYARI ---
        float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        sensitivitySlider.value = savedSens;
        UpdateSensitivityText(savedSens);


        // --- 3. VSYNC AYARI ---
        int vsyncVal = QualitySettings.vSyncCount;
        vsyncToggle.isOn = vsyncVal > 0;


        // --- LISTENER'LARI EKLE ---
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        vsyncToggle.onValueChanged.AddListener(SetVSync);
    }

    public void SetVolume(float value)
    {
        // Slider'dan gelen değer 0 ile 100 arasında.
        // Unity sesi 0.0 ile 1.0 arasında kabul eder. O yüzden 100'e bölüyoruz.
        float normalizedVolume = value / 100f;

        AudioListener.volume = normalizedVolume; // Oyunun sesini ayarla
        PlayerPrefs.SetFloat("MasterVolume", normalizedVolume); // Kaydederken 0-1 olarak kaydet

        UpdateVolumeText(value); // Ekrana 0-100 arası yaz
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        UpdateSensitivityText(sensitivity);
    }

    public void SetVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
    }

    // --- YAZI GÜNCELLEME ---
    void UpdateSensitivityText(float value)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = value.ToString("F1"); // Örn: 1.2
    }

    void UpdateVolumeText(float value)
    {
        if (volumeValueText != null)
        {
            // Tam sayı olarak göster (Örn: 85)
            volumeValueText.text = value.ToString("0");
        }
    }

    // --- BACK (GERİ DÖN) FONKSİYONU ---
    public void BackToMainMenu()
    {
        // Zamanı normal akışa döndür (Eğer oyun duraklatılmışsa takılı kalmasın)
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}