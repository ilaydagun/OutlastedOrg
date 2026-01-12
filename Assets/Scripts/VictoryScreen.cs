using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Kazanma ekranı butonlarını yönetir (TryAgain.cs'ye benzer)
/// </summary>
public class VictoryScreen : MonoBehaviour
{
    [Header("Scene Names")]
    [Tooltip("Ana menü sahnesinin adı")]
    public string mainMenuSceneName = "MainMenu";
    
    [Tooltip("Oyun sahnesinin adı (tekrar oynamak için)")]
    public string gameSceneName = "Level1";

    /// <summary>
    /// Ana menüye dön
    /// </summary>
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ MainMenu sahne adı ayarlanmamış!");
        }
    }

    /// <summary>
    /// Oyunu tekrar başlat
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ Game sahne adı ayarlanmamış!");
        }
    }
}


