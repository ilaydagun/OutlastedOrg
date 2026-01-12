using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçişleri için kütüphane

public class SceneController : MonoBehaviour
{
    // "static" demek, bu değişken sahneler değişse bile hafızada kalır demek.
    public static string oncekiSahneIsmi;

    // Settings butonuna basınca çalışacak
    public void AyarlaraGit()
    {
        // 1. Gitmeden önce şu an hangi sahnedeysen (MainMenu veya Level1) ismini kaydet
        oncekiSahneIsmi = SceneManager.GetActiveScene().name;

        // 2. Options sahnesini yükle (İsmi klasördekiyle aynı olmalı)
        SceneManager.LoadScene("Options");
    }

    // Back butonuna basınca çalışacak
    public void GeriDon()
    {
        // Hafızada bir sahne ismi varsa oraya dön
        if (!string.IsNullOrEmpty(oncekiSahneIsmi))
        {
            SceneManager.LoadScene(oncekiSahneIsmi);
        }
        else
        {
            // Eğer hafıza boşsa (direkt options'tan başlattıysan) MainMenu'ye at
            Debug.Log("Önceki sahne bulunamadı, Menüye dönülüyor.");
            SceneManager.LoadScene("MainMenu");
        }
    }
}