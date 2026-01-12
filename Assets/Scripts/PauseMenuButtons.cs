using UnityEngine;
using UnityEngine.SceneManagement; // Menüye dönme ihtimaline karşı

public class PauseMenuButtons : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject pausePanel;     // Pause ekranı (Resume butonunun olduğu yer)
    public GameObject settingsPanel;  // Ayarlar ekranı

    [Header("Oyuncu Bağlantısı")]
    public PlayerController playerScript; // Resume yapmak için Player'a ihtiyacımız var

    // 1. RESUME (DEVAM ET)
    public void ResumeGame()
    {
        // PlayerController'daki TogglePause fonksiyonunu tetikler
        if (playerScript != null)
        {
            playerScript.TogglePause();
        }
    }

    // 2. SETTINGS AÇ (AYARLAR)
    public void OpenSettings()
    {
        pausePanel.SetActive(false);    // Ana pause menüsünü gizle
        settingsPanel.SetActive(true);  // Ayarları aç
    }

    // 3. SETTINGS KAPAT (GERİ DÖN)
    public void CloseSettings()
    {
        settingsPanel.SetActive(false); // Ayarları gizle
        pausePanel.SetActive(true);     // Ana pause menüsünü geri getir
    }

    // 4. QUIT (ÇIKIŞ)
    public void QuitGame()
    {
        Debug.Log("Oyundan çıkılıyor...");
        Application.Quit(); // Build alınca çalışır

        // Editörde test ederken çalışması için:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // 5. MAIN MENU (ANA MENÜYE DÖN - Opsiyonel)
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Zamanı düzeltmeyi unutma
        SceneManager.LoadScene("MainMenu"); // "Menu" sahnenin adı neyse onu yaz
    }
}