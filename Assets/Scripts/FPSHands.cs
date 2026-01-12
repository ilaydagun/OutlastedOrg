using UnityEngine;

public class FPSHands : MonoBehaviour
{
    [Header("Sway Settings (Looking)")]
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmooth = 4f;

    [Header("Bobbing Settings (Walking)")]
    public float bobbingSpeed = 10f; // Increase for faster bob (sprinting)
    public float bobbingAmount = 0.05f; // Increase for heavier bob
    public PlayerController playerController; // Reference to your player script to check if moving

    private Vector3 initialPosition;
    private float timer = 0;

    void Start()
    {
        // Remember where the hands started relative to the camera
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        HandleSway();
        HandleBobbing();
    }

    private void HandleSway()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        // Calculate target rotation based on mouse movement (Inverse direction)
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // Rotate towards the target smoothly
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmooth * Time.deltaTime);
    }

    private void HandleBobbing()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Check if player is moving
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed * Time.deltaTime;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        Vector3 currentPosition = initialPosition;

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);

            translateChange = totalAxes * translateChange;

            // Apply the bobbing to the Y (up/down) and X (side/side) slightly
            currentPosition.y = initialPosition.y + translateChange;
            currentPosition.x = initialPosition.x + (translateChange * 0.5f); // Less side movement
        }

        // Apply position smoothly
        transform.localPosition = Vector3.Lerp(transform.localPosition, currentPosition, Time.deltaTime * 6f);
    }
}