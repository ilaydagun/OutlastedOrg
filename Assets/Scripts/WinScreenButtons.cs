using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenButtons : MonoBehaviour
{
    // "MAIN MENU" butonuna bağlanacak
    public void ReturnToMenu()
    {
        // Ana menü sahnenin adı neyse (MainMenu, Menu vs.) tam olarak onu yaz
        SceneManager.LoadScene("MainMenu");
    }

    // "QUIT" butonuna bağlanacak
    public void QuitGame()
    {
        Debug.Log("Oyundan çıkılıyor...");
        Application.Quit(); // Build'de çalışır
        
        // Editörde test ederken çalışması için:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}