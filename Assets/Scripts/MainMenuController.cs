using UnityEngine;
using UnityEngine.SceneManagement; // Sahne değiştirmek için bu kütüphane ŞART!

public class MainMenuController : MonoBehaviour
{
    // PLAY BUTONU İÇİN FONKSİYON
    public void PlayGame()
    {
        // Büyük-küçük harf duyarlıdır! Örn: "GameScene", "Bolum1" vb.
        SceneManager.LoadScene("Level1");
    }

    // QUIT BUTONU İÇİN FONKSİYON
    public void QuitGame()
    {
        // Oyun editörde çalışırken kapandığını anlaman için konsola mesaj yazar.
        Debug.Log("Oyundan Çıkıldı (Bu mesaj sadece editörde görünür).");

        // Oyunu build alıp (exe yapıp) çalıştırdığında bu kod oyunu kapatır.
        Application.Quit();
    }

    // OPTIONS BUTONU İÇİN
    public void OpenOptions()
    {
        Debug.Log("OpenOptions() fonksiyonu çağrıldı!");
        
        // Sahne adını kontrol et
        string sceneName = "Options";
        
        // Önce sahne adıyla dene
        if (SceneExists(sceneName))
        {
            Debug.Log("Options sahnesi bulundu, yükleniyor...");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // Sahne adıyla bulunamazsa, Build Settings'teki index ile dene
            Debug.LogWarning("Options sahnesi adıyla bulunamadı, index ile deneniyor...");
            
            // Build Settings'te Options sahnesinin index'ini bul
            int sceneIndex = -1;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneNameFromPath == sceneName)
                {
                    sceneIndex = i;
                    break;
                }
            }
            
            if (sceneIndex >= 0)
            {
                Debug.Log($"Options sahnesi index {sceneIndex} ile bulundu, yükleniyor...");
                SceneManager.LoadScene(sceneIndex);
            }
            else
            {
                Debug.LogError($"Options sahnesi bulunamadı! Build Settings'te kayıtlı olduğundan emin olun.");
            }
        }
    }
    
    // Sahnenin var olup olmadığını kontrol eden yardımcı fonksiyon
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameFromPath == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}