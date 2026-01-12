using UnityEngine;

using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour

{

    // BİLEŞENLER

    public CharacterController controller;

    public Transform cameraTransform;

    private Animator animator;

    public WeaponPickup currentPickup; // Silah alma için geçici referans



    [Header("PAUSE SYSTEM")] // YENİ EKLENDİ

    public GameObject pauseMenuPanel; // Inspector'dan Pause Panelini buraya sürükle

    private bool isGamePaused = false;



    [Header("MOVEMENT & SPEED")]

    public float moveSpeed = 5f;

    public float sprintMultiplier = 1.8f;

    public float crouchMultiplier = 0.4f;

    public float jumpHeight = 1.2f;

    public float gravity = -9.81f;

    private float verticalVelocity;



    [Header("ROTATION SETTINGS")]

    public float rotationSmoothTime = 0.3f;

    private float rotationVelocity;



    [Header("LOOK SETTINGS")]

    public float lookSensitivity = 0.5f;

    public float verticalClamp = 80f;

    private float xRotation = 0f;      // Kamera pitch



    [Header("CROUCH SETTINGS")]

    public float crouchSpeed = 2f;

    public float crouchHeight = 0.9f;

    private float standingHeight;

    private float standingCameraY;    // Ayakta kamera yüksekliği

    private float targetCameraY;      // Hedef kamera yüksekliği (çömelme vs için)

    private bool isCrouching = false;



    [Header("THIRD PERSON CAMERA")]

    public float cameraDistance = 4f;           // Karakterin arkasındaki mesafe

    public float cameraHeight = 1.7f;           // Omuz civarı başlangıç yüksekliği

    public float cameraFollowSmoothTime = 10f;  // Kamera yüksekliği yumuşatma hızı



    [Header("BACKWARD SETTINGS")]

    public float backwardTiltAngle = 15f;       // S + A/D iken hafif yan eğilme açısı



    private float currentCameraHeight;

    private float cameraYaw;          // Kamera yatay açısı



    // Yön girişi ve mevcut hareket hızı

    private Vector2 moveInput = Vector2.zero;

    private bool isSprinting = false;



    // Toggle için durum takibi

    private bool canToggle = true;



    private float animatorSpeed = 0f;



    // Moonwalk için

    private bool wasMovingBackward = false;

    private float backwardBaseYaw = 0f;



    void Start()

    {

        // Başlangıçta mouse kilitli ve oyun akıyor

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;



        // Eğer panel açıksa kapatalım

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);



        if (PlayerPrefs.HasKey("MouseSensitivity"))

        {

            lookSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");

        }



        animator = GetComponent<Animator>();

        if (controller == null)

            controller = GetComponent<CharacterController>();



        standingHeight = controller.height;



        // Kamera için başlangıç yükseklikleri

        standingCameraY = cameraHeight;

        targetCameraY = standingCameraY;

        currentCameraHeight = standingCameraY;



        if (cameraTransform != null)

        {

            cameraYaw = transform.eulerAngles.y;

            cameraTransform.parent = null; // Kamera karakterin çocuğu olmasın

        }

    }



    void OnEnable()

    {

        if (PlayerPrefs.HasKey("MouseSensitivity"))

        {

            lookSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");

        }

    }



    void Update()

    {

        // ESC KONTROLÜ (YENİ EKLENDİ)

        if (Keyboard.current.escapeKey.wasPressedThisFrame)

        {

            TogglePause();

        }



        // Oyun duraklatıldıysa aşağıdakileri yapma

        if (isGamePaused) return;



        HandleInput();

        HandleMovement();

        UpdateAnimations();

    }



    void LateUpdate()

    {

        // Oyun duraklatıldıysa kamerayı hareket ettirme (SORUNUN ÇÖZÜMÜ)

        if (isGamePaused) return;



        HandleMouseLook();

        UpdateCameraPosition();

    }



    // YENİ PAUSE FONKSİYONU

    public void TogglePause()

    {

        isGamePaused = !isGamePaused;



        if (isGamePaused)

        {

            // OYUNU DURDUR

            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;



            if (pauseMenuPanel != null)

                pauseMenuPanel.SetActive(true);

        }

        else

        {

            // OYUNU DEVAM ETTİR



            // --- EKLENECEK KISIM (BAŞLANGIÇ) ---

            // Oyuna dönerken hafızaya bak ve yeni hassasiyeti yükle

            if (PlayerPrefs.HasKey("MouseSensitivity"))

            {

                lookSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");

            }

            // --- EKLENECEK KISIM (BİTİŞ) ---



            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;



            if (pauseMenuPanel != null)

                pauseMenuPanel.SetActive(false);

        }

    }



    private void HandleInput()

    {

        var keyboard = Keyboard.current;

        if (keyboard == null) return;



        float x = 0;

        float z = 0;



        // Yön Girişleri

        if (keyboard.wKey.isPressed) z += 1;

        if (keyboard.sKey.isPressed) z -= 1;

        if (keyboard.aKey.isPressed) x -= 1;

        if (keyboard.dKey.isPressed) x += 1;



        moveInput = new Vector2(x, z);



        // 1. SPRINT TOGGLE (Shift + W, A/D serbest)

        if (keyboard.shiftKey.isPressed)

        {

            if (canToggle)

            {

                // Sprint aç/kapat: en az W + Shift, isterse A/D de basılı olabilir

                if (!isCrouching && keyboard.wKey.isPressed)

                {

                    isSprinting = !isSprinting;

                }

                canToggle = false;

            }

        }

        else

        {

            canToggle = true;

        }



        // 2. Sprint'i otomatik iptal et

        if (isSprinting)

        {

            if (!keyboard.wKey.isPressed || keyboard.sKey.isPressed || keyboard.ctrlKey.isPressed)

            {

                isSprinting = false;

            }

        }



        // 3. ÇÖMELME (CTRL tuşu ile)

        bool shouldCrouch = keyboard.ctrlKey.isPressed;



        if (shouldCrouch != isCrouching)

        {

            SetCrouch(shouldCrouch);

        }



        // 4. Zıplama

        if (controller.isGrounded && keyboard.spaceKey.wasPressedThisFrame)

        {

            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            animator.SetTrigger("Jump");

        }

    }



    private void SetCrouch(bool crouch)

    {

        isCrouching = crouch;

        if (isCrouching)

        {

            controller.height = crouchHeight;

            controller.center = new Vector3(0, crouchHeight / 2f, 0);



            // Kamera yüksekliğini çömelmeye göre düşür

            targetCameraY = standingCameraY * crouchMultiplier;



            // Çömelmeye başladığında koşmayı iptal et

            isSprinting = false;

        }

        else

        {

            controller.height = standingHeight;

            controller.center = new Vector3(0, standingHeight / 2f, 0);

            targetCameraY = standingCameraY;

        }

    }



    private void HandleMovement()

    {

        // Diagonal hız artmasını engelle (W+A, W+D turbo olmasın)

        float inputMagnitude = Mathf.Clamp01(moveInput.magnitude);



        // Hareket ediyor muyuz?

        if (inputMagnitude >= 0.1f)

        {

            bool isMovingBackward = moveInput.y < -0.1f;

            bool hasSideInput = Mathf.Abs(moveInput.x) > 0.1f;



            // İlk defa geri harekete geçtiğimiz an, baz yaw'u kaydet

            if (isMovingBackward && !wasMovingBackward)

            {

                backwardBaseYaw = transform.eulerAngles.y;

            }



            float currentSpeed = moveSpeed;



            // Çömelme hızı

            if (isCrouching)

            {

                currentSpeed = crouchSpeed;

            }

            // Koşma sadece ileri giderken (geri değil)

            else if (isSprinting && !isMovingBackward)

            {

                currentSpeed *= sprintMultiplier;

            }



            Vector3 horizontalMove = Vector3.zero;



            if (isMovingBackward)

            {

                // HER ZAMAN GERİ YÜRÜME (moonwalk), yönü geri + hafif yan

                Vector3 backward = -transform.forward;

                Vector3 strafe = transform.right * moveInput.x; // A/D



                Vector3 moveDir = backward;



                if (hasSideInput)

                {

                    moveDir = backward + strafe;

                }

                if (moveDir.sqrMagnitude > 0.001f)

                {

                    moveDir = moveDir.normalized;

                }





                horizontalMove = moveDir * currentSpeed * inputMagnitude;



                // ✅ ROTATION: S, S+A, S+D → GERÇEK HAREKET YÖNÜNE DÖN

                float targetYaw = backwardBaseYaw;



                if (hasSideInput)

                {

                    float tiltSign = Mathf.Sign(moveInput.x);

                    targetYaw = backwardBaseYaw + backwardTiltAngle * tiltSign;

                }



                float newYaw = Mathf.SmoothDampAngle(

                    transform.eulerAngles.y,

                    targetYaw,

                    ref rotationVelocity,

                    rotationSmoothTime

                );



                transform.rotation = Quaternion.Euler(0f, newYaw, 0f);



                // Geri giderken sprint iptal

                isSprinting = false;

            }

            else

            {

                // İLERİ VE YAN HAREKETLERDE (W, W+A, W+D) ROTATE TO MOVE DIRECTION

                float angleInput = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;

                float targetAngle = angleInput + (cameraTransform != null ? cameraTransform.eulerAngles.y : transform.eulerAngles.y);



                float angle = Mathf.SmoothDampAngle(

                    transform.eulerAngles.y,

                    targetAngle,

                    ref rotationVelocity,

                    rotationSmoothTime

                );

                transform.rotation = Quaternion.Euler(0f, angle, 0f);



                horizontalMove = transform.forward * currentSpeed * inputMagnitude;

            }



            controller.Move(horizontalMove * Time.deltaTime);



            // 3. Animator Hızı

            float targetAnimatorSpeed = moveSpeed;



            if (isCrouching)

            {

                targetAnimatorSpeed = crouchSpeed;

            }

            else if (isSprinting && !isMovingBackward)

            {

                targetAnimatorSpeed = moveSpeed * sprintMultiplier;

            }



            float smoothTime = isSprinting ? 10f : 25f;



            animatorSpeed = Mathf.Lerp(

                animatorSpeed,

                targetAnimatorSpeed * inputMagnitude,

                Time.deltaTime * smoothTime

            );



            // Bu frame'de geri gidiyor muyduk, onu hatırla

            wasMovingBackward = isMovingBackward;

        }

        else

        {

            // Duruyorsa animator hızını 0'a getir

            animatorSpeed = Mathf.Lerp(animatorSpeed, 0f, Time.deltaTime * 15f);

            wasMovingBackward = false;

        }



        // 4. Yerçekimi

        if (controller.isGrounded && verticalVelocity < 0)

        {

            verticalVelocity = -2f;

        }



        verticalVelocity += gravity * Time.deltaTime;

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

    }



    private void HandleMouseLook()

    {

        if (cameraTransform == null) return;

        var mouse = Mouse.current;

        if (mouse == null) return;



        // EĞER OYUN DURAKLATILDIYSA KAMERA DÖNMESİN

        if (isGamePaused) return;

        // İKİNCİ KONTROL: Eğer mouse kilitli değilse yine dönmesin

        if (Cursor.lockState != CursorLockMode.Locked) return;



        Vector2 delta = mouse.delta.ReadValue();



        float calibrationFactor = 0.05f;



        float mouseX = delta.x * lookSensitivity * calibrationFactor;

        float mouseY = delta.y * lookSensitivity * calibrationFactor;



        cameraYaw += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

    }



    private void UpdateCameraPosition()

    {

        if (cameraTransform == null) return;



        // Kamera yüksekliğini çömelme vb için yumuşat

        currentCameraHeight = Mathf.Lerp(

            currentCameraHeight,

            targetCameraY,

            Time.deltaTime * cameraFollowSmoothTime

        );



        // Kameranın bakacağı nokta (karakterin omuz/baş bölgesi)

        Vector3 targetPosition = transform.position + Vector3.up * currentCameraHeight;



        // Kamera rotasyonu

        Quaternion camRot = Quaternion.Euler(xRotation, cameraYaw, 0f);



        // Kamera pozisyonu, karakterin arkasında

        Vector3 offset = camRot * new Vector3(0f, 0f, -cameraDistance);

        cameraTransform.position = targetPosition + offset;

        cameraTransform.rotation = camRot;

    }



    private void UpdateAnimations()

    {

        // Hız parametresi: Idle/Walk/Run için

        animator.SetFloat("Speed", animatorSpeed);



        // Çömelme

        animator.SetBool("IsCrouching", isCrouching);



        // Yere değiyor mu

        animator.SetBool("IsGrounded", controller.isGrounded);

    }

    // Pickup animasyonu tamamlandığında çağrılır
    public void PickupComplete()
    {
        if (currentPickup != null)
        {
            currentPickup.weaponInHand.SetActive(true);
            
            // Işığı kapat
            if (currentPickup.pickupLight != null)
            {
                currentPickup.pickupLight.enabled = false;
            }
            
            Destroy(currentPickup.gameObject);
            currentPickup = null;
        }
    }

}