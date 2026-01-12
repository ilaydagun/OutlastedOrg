using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class FontApplier : EditorWindow
{
    private Font targetFont;
    private TMP_FontAsset tmpFontAsset;

    [MenuItem("Tools/Apply BloodyTerror Font")]
    public static void ShowWindow()
    {
        GetWindow<FontApplier>("Font Applier");
    }

    void OnGUI()
    {
        GUILayout.Label("Font Ayarları", EditorStyles.boldLabel);
        
        targetFont = (Font)EditorGUILayout.ObjectField("BloodyTerror Font", targetFont, typeof(Font), false);
        
        if (targetFont == null)
        {
            // Otomatik olarak font'u bul
            string[] guids = AssetDatabase.FindAssets("BloodyTerror t:Font");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                targetFont = AssetDatabase.LoadAssetAtPath<Font>(path);
            }
        }

        GUILayout.Space(10);
        
        if (GUILayout.Button("Tüm Text Componentlerine Uygula"))
        {
            ApplyFontToAllTexts();
        }
        
        if (GUILayout.Button("Sadece Seçili GameObject'lere Uygula"))
        {
            ApplyFontToSelected();
        }
    }

    void ApplyFontToAllTexts()
    {
        if (targetFont == null)
        {
            EditorUtility.DisplayDialog("Hata", "Font bulunamadı! Lütfen font'u seçin.", "Tamam");
            return;
        }

        int count = 0;
        
        // Tüm Text componentlerini bul
        Text[] allTexts = FindObjectsOfType<Text>(true);
        foreach (Text text in allTexts)
        {
            Undo.RecordObject(text, "Apply Font");
            text.font = targetFont;
            count++;
        }

        // Tüm TextMeshPro componentlerini bul ve uyarı ver
        TextMeshProUGUI[] allTMPTexts = FindObjectsOfType<TextMeshProUGUI>(true);
        if (allTMPTexts.Length > 0)
        {
            EditorUtility.DisplayDialog("Bilgi", 
                $"{count} Text componenti güncellendi.\n\n{allTMPTexts.Length} TextMeshPro componenti bulundu. " +
                "TextMeshPro için TMP_FontAsset oluşturmanız gerekir.", "Tamam");
        }
        else
        {
            EditorUtility.DisplayDialog("Başarılı", $"{count} Text componenti güncellendi.", "Tamam");
        }

        Debug.Log($"Font uygulandı: {count} Text componenti güncellendi.");
    }

    void ApplyFontToSelected()
    {
        if (targetFont == null)
        {
            EditorUtility.DisplayDialog("Hata", "Font bulunamadı! Lütfen font'u seçin.", "Tamam");
            return;
        }

        if (Selection.activeGameObject == null)
        {
            EditorUtility.DisplayDialog("Hata", "Lütfen bir GameObject seçin.", "Tamam");
            return;
        }

        int count = 0;
        Text[] texts = Selection.activeGameObject.GetComponentsInChildren<Text>(true);
        
        foreach (Text text in texts)
        {
            Undo.RecordObject(text, "Apply Font");
            text.font = targetFont;
            count++;
        }

        EditorUtility.DisplayDialog("Başarılı", $"{count} Text componenti güncellendi.", "Tamam");
        Debug.Log($"Font uygulandı: {count} Text componenti güncellendi.");
    }
}

