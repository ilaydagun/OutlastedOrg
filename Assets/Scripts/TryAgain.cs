using UnityEngine;
using UnityEngine.SceneManagement; // Sahne iþlemleri için bu KÜTÜPHANE ÞART!

public class TryAgain : MonoBehaviour
{
    // Bu fonksiyonu "Try Again" butonuna baðlayacaksýn
    public void RestartGame()
    {
        // 1. Zamaný normale döndür (Eðer durdurduysan)
        Time.timeScale = 1f;

        // 2. Oyun sahnesini yeniden yükle
        // "Level1" yerine kendi oyun sahnenin tam adýný yazmalýsýn!
        SceneManager.LoadScene("Level1");
    }

    // Bu fonksiyonu "Main Menu" butonuna baðlayacaksýn
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        // Ana menü sahnenin adý "MainMenu" ise böyle kalabilir
        SceneManager.LoadScene("MainMenu");
    }
}