using UnityEngine;

public class RealisticCompanionGenerator : MonoBehaviour
{
    [Header("Character Colors")]
    public Color skinColor = new Color(0.98f, 0.85f, 0.75f); // Canlı cilt tonu
    public Color hairColor = new Color(0.25f, 0.15f, 0.08f); // Kahverengi saç
    public Color eyeColor = new Color(0.3f, 0.5f, 0.7f); // Mavi göz
    public Color dressColor = new Color(0.9f, 0.4f, 0.5f); // Pembe elbise
    public Color dressAccentColor = new Color(0.95f, 0.95f, 0.95f); // Beyaz detaylar
    public Color shoeColor = new Color(0.8f, 0.2f, 0.2f); // Kırmızı ayakkabı
    public Color sockColor = new Color(1f, 1f, 1f); // Beyaz çorap

    [Header("Settings")]
    public bool generateOnStart = false;

    private GameObject characterRoot;

    void Start()
    {
        if (generateOnStart && characterRoot == null)
        {
            GenerateCharacter();
        }
    }

    [ContextMenu("Generate Realistic Character")]
    public void GenerateCharacter()
    {
        // Eski karakteri temizle
        if (characterRoot != null)
        {
            DestroyImmediate(characterRoot);
        }

        characterRoot = new GameObject("RealisticCompanionGirl");
        characterRoot.transform.position = transform.position;
        characterRoot.transform.SetParent(transform);

        // === GÖVDE VE KAFA ===
        CreateBody();
        CreateHead();

        // === YÜZ DETAYLARı ===
        CreateFace();

        // === SAÇ ===
        CreateHair();

        // === KOLLAR ===
        CreateArms();

        // === BACAKLAR ===
        CreateLegs();

        // === AYAKKABI VE ÇORAPLAR ===
        CreateShoes();

        // === ELBİSE DETAYLARı ===
        CreateDressDetails();

        // === COLLIDER VE RİGİDBODY ===
        SetupPhysics();

        Debug.Log("✨ Realistic companion character generated!");
    }

    void CreateBody()
    {
        // Ana gövde (elbise)
        GameObject torso = CreatePrimitive("Torso", PrimitiveType.Capsule, characterRoot.transform);
        torso.transform.localPosition = new Vector3(0, 0.7f, 0);
        torso.transform.localScale = new Vector3(0.45f, 0.55f, 0.45f);
        SetColor(torso, dressColor);

        // Elbise alt kısmı (daha geniş)
        GameObject dressBottom = CreatePrimitive("DressBottom", PrimitiveType.Sphere, characterRoot.transform);
        dressBottom.transform.localPosition = new Vector3(0, 0.35f, 0);
        dressBottom.transform.localScale = new Vector3(0.55f, 0.35f, 0.55f);
        SetColor(dressBottom, dressColor);

        // Göğüs/üst gövde
        GameObject chest = CreatePrimitive("Chest", PrimitiveType.Sphere, characterRoot.transform);
        chest.transform.localPosition = new Vector3(0, 0.95f, 0);
        chest.transform.localScale = new Vector3(0.38f, 0.25f, 0.3f);
        SetColor(chest, dressColor);
    }

    void CreateHead()
    {
        // Ana kafa
        GameObject head = CreatePrimitive("Head", PrimitiveType.Sphere, characterRoot.transform);
        head.transform.localPosition = new Vector3(0, 1.4f, 0);
        head.transform.localScale = new Vector3(0.38f, 0.42f, 0.38f);
        SetColor(head, skinColor);

        // Boyun
        GameObject neck = CreatePrimitive("Neck", PrimitiveType.Cylinder, characterRoot.transform);
        neck.transform.localPosition = new Vector3(0, 1.15f, 0);
        neck.transform.localScale = new Vector3(0.12f, 0.1f, 0.12f);
        SetColor(neck, skinColor);
    }

    void CreateFace()
    {
        // Sol göz (daha büyük ve detaylı)
        GameObject leftEyeWhite = CreatePrimitive("LeftEyeWhite", PrimitiveType.Sphere, characterRoot.transform);
        leftEyeWhite.transform.localPosition = new Vector3(-0.1f, 1.45f, 0.16f);
        leftEyeWhite.transform.localScale = new Vector3(0.08f, 0.08f, 0.04f);
        SetColor(leftEyeWhite, Color.white);

        GameObject leftIris = CreatePrimitive("LeftIris", PrimitiveType.Sphere, characterRoot.transform);
        leftIris.transform.localPosition = new Vector3(-0.1f, 1.45f, 0.18f);
        leftIris.transform.localScale = new Vector3(0.05f, 0.05f, 0.02f);
        SetColor(leftIris, eyeColor);

        GameObject leftPupil = CreatePrimitive("LeftPupil", PrimitiveType.Sphere, characterRoot.transform);
        leftPupil.transform.localPosition = new Vector3(-0.1f, 1.45f, 0.19f);
        leftPupil.transform.localScale = new Vector3(0.03f, 0.03f, 0.01f);
        SetColor(leftPupil, Color.black);

        // Sağ göz
        GameObject rightEyeWhite = CreatePrimitive("RightEyeWhite", PrimitiveType.Sphere, characterRoot.transform);
        rightEyeWhite.transform.localPosition = new Vector3(0.1f, 1.45f, 0.16f);
        rightEyeWhite.transform.localScale = new Vector3(0.08f, 0.08f, 0.04f);
        SetColor(rightEyeWhite, Color.white);

        GameObject rightIris = CreatePrimitive("RightIris", PrimitiveType.Sphere, characterRoot.transform);
        rightIris.transform.localPosition = new Vector3(0.1f, 1.45f, 0.18f);
        rightIris.transform.localScale = new Vector3(0.05f, 0.05f, 0.02f);
        SetColor(rightIris, eyeColor);

        GameObject rightPupil = CreatePrimitive("RightPupil", PrimitiveType.Sphere, characterRoot.transform);
        rightPupil.transform.localPosition = new Vector3(0.1f, 1.45f, 0.19f);
        rightPupil.transform.localScale = new Vector3(0.03f, 0.03f, 0.01f);
        SetColor(rightPupil, Color.black);

        // Burun
        GameObject nose = CreatePrimitive("Nose", PrimitiveType.Sphere, characterRoot.transform);
        nose.transform.localPosition = new Vector3(0, 1.38f, 0.18f);
        nose.transform.localScale = new Vector3(0.06f, 0.08f, 0.08f);
        SetColor(nose, skinColor);

        // Ağız (gülümseme)
        GameObject mouth = CreatePrimitive("Mouth", PrimitiveType.Sphere, characterRoot.transform);
        mouth.transform.localPosition = new Vector3(0, 1.3f, 0.17f);
        mouth.transform.localScale = new Vector3(0.12f, 0.04f, 0.04f);
        SetColor(mouth, new Color(0.9f, 0.4f, 0.4f)); // Pembe dudak

        // Yanaklar (allık)
        GameObject leftCheek = CreatePrimitive("LeftCheek", PrimitiveType.Sphere, characterRoot.transform);
        leftCheek.transform.localPosition = new Vector3(-0.13f, 1.35f, 0.15f);
        leftCheek.transform.localScale = new Vector3(0.08f, 0.08f, 0.04f);
        SetColor(leftCheek, new Color(1f, 0.7f, 0.7f, 0.5f)); // Hafif pembe

        GameObject rightCheek = CreatePrimitive("RightCheek", PrimitiveType.Sphere, characterRoot.transform);
        rightCheek.transform.localPosition = new Vector3(0.13f, 1.35f, 0.15f);
        rightCheek.transform.localScale = new Vector3(0.08f, 0.08f, 0.04f);
        SetColor(rightCheek, new Color(1f, 0.7f, 0.7f, 0.5f));
    }

    void CreateHair()
    {
        // Ana saç kütlesi
        GameObject hairMain = CreatePrimitive("HairMain", PrimitiveType.Sphere, characterRoot.transform);
        hairMain.transform.localPosition = new Vector3(0, 1.52f, 0);
        hairMain.transform.localScale = new Vector3(0.42f, 0.3f, 0.42f);
        SetColor(hairMain, hairColor);

        // Ön saç (patlama)
        GameObject bangs = CreatePrimitive("Bangs", PrimitiveType.Sphere, characterRoot.transform);
        bangs.transform.localPosition = new Vector3(0, 1.48f, 0.15f);
        bangs.transform.localScale = new Vector3(0.35f, 0.15f, 0.12f);
        SetColor(bangs, hairColor);

        // At kuyruğu (daha uzun ve detaylı)
        GameObject ponytailBase = CreatePrimitive("PonytailBase", PrimitiveType.Sphere, characterRoot.transform);
        ponytailBase.transform.localPosition = new Vector3(0, 1.45f, -0.22f);
        ponytailBase.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        SetColor(ponytailBase, hairColor);

        GameObject ponytail1 = CreatePrimitive("Ponytail1", PrimitiveType.Capsule, characterRoot.transform);
        ponytail1.transform.localPosition = new Vector3(0, 1.35f, -0.28f);
        ponytail1.transform.localRotation = Quaternion.Euler(20, 0, 0);
        ponytail1.transform.localScale = new Vector3(0.12f, 0.15f, 0.12f);
        SetColor(ponytail1, hairColor);

        GameObject ponytail2 = CreatePrimitive("Ponytail2", PrimitiveType.Capsule, characterRoot.transform);
        ponytail2.transform.localPosition = new Vector3(0, 1.18f, -0.32f);
        ponytail2.transform.localRotation = Quaternion.Euler(30, 0, 0);
        ponytail2.transform.localScale = new Vector3(0.1f, 0.12f, 0.1f);
        SetColor(ponytail2, hairColor);

        // Saç tokası
        GameObject hairTie = CreatePrimitive("HairTie", PrimitiveType.Sphere, characterRoot.transform);
        hairTie.transform.localPosition = new Vector3(0, 1.45f, -0.22f);
        hairTie.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        SetColor(hairTie, new Color(1f, 0.8f, 0.9f)); // Pembe toka
    }

    void CreateArms()
    {
        // Sol kol - üst
        GameObject leftUpperArm = CreatePrimitive("LeftUpperArm", PrimitiveType.Capsule, characterRoot.transform);
        leftUpperArm.transform.localPosition = new Vector3(-0.32f, 0.92f, 0);
        leftUpperArm.transform.localRotation = Quaternion.Euler(0, 0, 10);
        leftUpperArm.transform.localScale = new Vector3(0.11f, 0.22f, 0.11f);
        SetColor(leftUpperArm, dressColor); // Kol elbise içinde

        // Sol ön kol
        GameObject leftForearm = CreatePrimitive("LeftForearm", PrimitiveType.Capsule, characterRoot.transform);
        leftForearm.transform.localPosition = new Vector3(-0.36f, 0.58f, 0);
        leftForearm.transform.localRotation = Quaternion.Euler(0, 0, 5);
        leftForearm.transform.localScale = new Vector3(0.09f, 0.18f, 0.09f);
        SetColor(leftForearm, skinColor);

        // Sol el
        GameObject leftHand = CreatePrimitive("LeftHand", PrimitiveType.Sphere, characterRoot.transform);
        leftHand.transform.localPosition = new Vector3(-0.38f, 0.35f, 0);
        leftHand.transform.localScale = new Vector3(0.11f, 0.11f, 0.08f);
        SetColor(leftHand, skinColor);

        // Sağ kol - üst
        GameObject rightUpperArm = CreatePrimitive("RightUpperArm", PrimitiveType.Capsule, characterRoot.transform);
        rightUpperArm.transform.localPosition = new Vector3(0.32f, 0.92f, 0);
        rightUpperArm.transform.localRotation = Quaternion.Euler(0, 0, -10);
        rightUpperArm.transform.localScale = new Vector3(0.11f, 0.22f, 0.11f);
        SetColor(rightUpperArm, dressColor);

        // Sağ ön kol
        GameObject rightForearm = CreatePrimitive("RightForearm", PrimitiveType.Capsule, characterRoot.transform);
        rightForearm.transform.localPosition = new Vector3(0.36f, 0.58f, 0);
        rightForearm.transform.localRotation = Quaternion.Euler(0, 0, -5);
        rightForearm.transform.localScale = new Vector3(0.09f, 0.18f, 0.09f);
        SetColor(rightForearm, skinColor);

        // Sağ el
        GameObject rightHand = CreatePrimitive("RightHand", PrimitiveType.Sphere, characterRoot.transform);
        rightHand.transform.localPosition = new Vector3(0.38f, 0.35f, 0);
        rightHand.transform.localScale = new Vector3(0.11f, 0.11f, 0.08f);
        SetColor(rightHand, skinColor);
    }

    void CreateLegs()
    {
        // Sol bacak - üst
        GameObject leftThigh = CreatePrimitive("LeftThigh", PrimitiveType.Capsule, characterRoot.transform);
        leftThigh.transform.localPosition = new Vector3(-0.1f, 0.28f, 0);
        leftThigh.transform.localScale = new Vector3(0.13f, 0.22f, 0.13f);
        SetColor(leftThigh, skinColor);

        // Sol bacak - alt
        GameObject leftShin = CreatePrimitive("LeftShin", PrimitiveType.Capsule, characterRoot.transform);
        leftShin.transform.localPosition = new Vector3(-0.1f, 0.08f, 0);
        leftShin.transform.localScale = new Vector3(0.11f, 0.08f, 0.11f);
        SetColor(leftShin, skinColor);

        // Sağ bacak - üst
        GameObject rightThigh = CreatePrimitive("RightThigh", PrimitiveType.Capsule, characterRoot.transform);
        rightThigh.transform.localPosition = new Vector3(0.1f, 0.28f, 0);
        rightThigh.transform.localScale = new Vector3(0.13f, 0.22f, 0.13f);
        SetColor(rightThigh, skinColor);

        // Sağ bacak - alt
        GameObject rightShin = CreatePrimitive("RightShin", PrimitiveType.Capsule, characterRoot.transform);
        rightShin.transform.localPosition = new Vector3(0.1f, 0.08f, 0);
        rightShin.transform.localScale = new Vector3(0.11f, 0.08f, 0.11f);
        SetColor(rightShin, skinColor);
    }

    void CreateShoes()
    {
        // Sol çorap
        GameObject leftSock = CreatePrimitive("LeftSock", PrimitiveType.Cylinder, characterRoot.transform);
        leftSock.transform.localPosition = new Vector3(-0.1f, 0.04f, 0);
        leftSock.transform.localScale = new Vector3(0.11f, 0.03f, 0.11f);
        SetColor(leftSock, sockColor);

        // Sol ayakkabı
        GameObject leftShoe = CreatePrimitive("LeftShoe", PrimitiveType.Cube, characterRoot.transform);
        leftShoe.transform.localPosition = new Vector3(-0.1f, 0.02f, 0.06f);
        leftShoe.transform.localScale = new Vector3(0.14f, 0.06f, 0.22f);
        SetColor(leftShoe, shoeColor);

        // Sol ayakkabı burun
        GameObject leftShoeTip = CreatePrimitive("LeftShoeTip", PrimitiveType.Sphere, characterRoot.transform);
        leftShoeTip.transform.localPosition = new Vector3(-0.1f, 0.02f, 0.16f);
        leftShoeTip.transform.localScale = new Vector3(0.13f, 0.05f, 0.08f);
        SetColor(leftShoeTip, shoeColor);

        // Sağ çorap
        GameObject rightSock = CreatePrimitive("RightSock", PrimitiveType.Cylinder, characterRoot.transform);
        rightSock.transform.localPosition = new Vector3(0.1f, 0.04f, 0);
        rightSock.transform.localScale = new Vector3(0.11f, 0.03f, 0.11f);
        SetColor(rightSock, sockColor);

        // Sağ ayakkabı
        GameObject rightShoe = CreatePrimitive("RightShoe", PrimitiveType.Cube, characterRoot.transform);
        rightShoe.transform.localPosition = new Vector3(0.1f, 0.02f, 0.06f);
        rightShoe.transform.localScale = new Vector3(0.14f, 0.06f, 0.22f);
        SetColor(rightShoe, shoeColor);

        // Sağ ayakkabı burun
        GameObject rightShoeTip = CreatePrimitive("RightShoeTip", PrimitiveType.Sphere, characterRoot.transform);
        rightShoeTip.transform.localPosition = new Vector3(0.1f, 0.02f, 0.16f);
        rightShoeTip.transform.localScale = new Vector3(0.13f, 0.05f, 0.08f);
        SetColor(rightShoeTip, shoeColor);
    }

    void CreateDressDetails()
    {
        // Yaka
        GameObject collar = CreatePrimitive("Collar", PrimitiveType.Sphere, characterRoot.transform);
        collar.transform.localPosition = new Vector3(0, 1.15f, 0.12f);
        collar.transform.localScale = new Vector3(0.22f, 0.08f, 0.08f);
        SetColor(collar, dressAccentColor);

        // Kemer
        GameObject belt = CreatePrimitive("Belt", PrimitiveType.Cylinder, characterRoot.transform);
        belt.transform.localPosition = new Vector3(0, 0.65f, 0);
        belt.transform.localRotation = Quaternion.Euler(90, 0, 0);
        belt.transform.localScale = new Vector3(0.48f, 0.02f, 0.48f);
        SetColor(belt, dressAccentColor);

        // Kemer tokası
        GameObject beltBuckle = CreatePrimitive("BeltBuckle", PrimitiveType.Cube, characterRoot.transform);
        beltBuckle.transform.localPosition = new Vector3(0, 0.65f, 0.22f);
        beltBuckle.transform.localScale = new Vector3(0.08f, 0.06f, 0.02f);
        SetColor(beltBuckle, new Color(1f, 0.84f, 0f)); // Altın rengi

        // Elbise cepleri
        GameObject leftPocket = CreatePrimitive("LeftPocket", PrimitiveType.Cube, characterRoot.transform);
        leftPocket.transform.localPosition = new Vector3(-0.15f, 0.5f, 0.2f);
        leftPocket.transform.localScale = new Vector3(0.1f, 0.12f, 0.02f);
        SetColor(leftPocket, dressAccentColor);

        GameObject rightPocket = CreatePrimitive("RightPocket", PrimitiveType.Cube, characterRoot.transform);
        rightPocket.transform.localPosition = new Vector3(0.15f, 0.5f, 0.2f);
        rightPocket.transform.localScale = new Vector3(0.1f, 0.12f, 0.02f);
        SetColor(rightPocket, dressAccentColor);

        // Düğmeler
        for (int i = 0; i < 3; i++)
        {
            GameObject button = CreatePrimitive($"Button{i}", PrimitiveType.Sphere, characterRoot.transform);
            button.transform.localPosition = new Vector3(0, 1.0f - (i * 0.12f), 0.22f);
            button.transform.localScale = new Vector3(0.04f, 0.04f, 0.02f);
            SetColor(button, dressAccentColor);
        }
    }

    void SetupPhysics()
    {
        // Ana collider
        CapsuleCollider mainCollider = characterRoot.AddComponent<CapsuleCollider>();
        mainCollider.center = new Vector3(0, 0.75f, 0);
        mainCollider.height = 1.6f;
        mainCollider.radius = 0.35f;

        // Rigidbody
        Rigidbody rb = characterRoot.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 30f; // Çocuk ağırlığı
    }

    GameObject CreatePrimitive(string name, PrimitiveType type, Transform parent)
    {
        GameObject obj = GameObject.CreatePrimitive(type);
        obj.name = name;
        obj.transform.SetParent(parent);

        // Primitive collider'ı kaldır
        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            Destroy(col);
        }

        return obj;
    }

    void SetColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;

            // Hafif parlak yap
            if (color.a < 1f)
            {
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }

            renderer.material = mat;
        }
    }

    [ContextMenu("Delete Character")]
    public void DeleteCharacter()
    {
        if (characterRoot != null)
        {
            DestroyImmediate(characterRoot);
        }
    }
}