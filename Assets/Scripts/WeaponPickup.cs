using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponInHand;
    public Light pickupLight; // Inspector'dan light'ı sürükleyin
    public float glowSpeed = 2f; // Yanıp sönme hızı
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.5f;

    private void Update()
    {
        if (pickupLight != null)
        {
            // Sinüs dalgası ile yanıp sönme
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, 
                (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f);
            pickupLight.intensity = intensity;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E BASILDI → SİLAH ELDE");

            // Işığı hemen kapat
            if (pickupLight != null)
            {
                pickupLight.enabled = false;
            }

            // Animasyon tetikleme
            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Pickup");
            }

            // PlayerController'a referans ver
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.currentPickup = this;
            }
        }
    }
}