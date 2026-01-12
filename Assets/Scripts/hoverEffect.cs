using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Button))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Ayarlar")]
    [Tooltip("Mouse üzerine gelince buton ne kadar büyüsün? (Örn: 1.1 = %10 büyüme)")]
    public float hoverScaleAmount = 1.1f;

    [Tooltip("Büyüme/Küçülme hýzý ne olsun? (Daha düþük = Daha yavaþ)")]
    public float transitionSpeed = 5f; // YENÝ: Hýz ayarý

    [Tooltip("Mouse üzerine gelince yazý hangi renge dönsün?")]
    public Color hoverTextColor = Color.yellow;

    // Deðiþkenler
    private Vector3 originalScale;
    private Vector3 targetScale; // YENÝ: Hedeflediðimiz boyut
    private Color originalTextColor;
    private TextMeshProUGUI buttonText;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale; // Baþlangýçta hedefimiz kendi boyutumuz.

        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            originalTextColor = buttonText.color;
        }
    }

    // --- HER KAREDE ÇALIÞAN KISIM (ANÝMASYON BURADA) ---
    // --- HER KAREDE ÇALIŞAN KISIM (ANİMASYON BURADA) ---
    void Update()
    {
        // Time.deltaTime yerine Time.unscaledDeltaTime kullanıyoruz.
        // Böylece Time.timeScale = 0 olsa (oyun dursa) bile animasyon çalışır.
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hedefi deðiþtiriyoruz (Direkt boyutu deðil)
        targetScale = originalScale * hoverScaleAmount;

        // Renk deðiþimi genelde anlýk olmasý daha iyidir ama istersen onu da yumuþatabiliriz.
        if (buttonText != null) buttonText.color = hoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hedefi tekrar eski haline çekiyoruz
        targetScale = originalScale;

        if (buttonText != null) buttonText.color = originalTextColor;
    }

    public void OnDisable()
    {
        // Obje kapanýrsa boyutu sýfýrla ki sonraki açýlýþta dev gibi kalmasýn
        transform.localScale = originalScale;
        targetScale = originalScale;
        if (buttonText != null) buttonText.color = originalTextColor;
    }
}