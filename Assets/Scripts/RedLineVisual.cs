using UnityEngine;

/// <summary>
/// Kırmızı çizgi görseli oluşturur (kazanma çizgisi için)
/// </summary>
public class RedLineVisual : MonoBehaviour
{
    [Header("Line Settings")]
    [Tooltip("Çizginin yüksekliği")]
    public float lineHeight = 3f;
    
    [Tooltip("Çizginin genişliği")]
    public float lineWidth = 0.1f;
    
    [Tooltip("Çizginin uzunluğu (X ekseni boyunca)")]
    public float lineLength = 20f;
    
    [Tooltip("Çizginin rengi")]
    public Color lineColor = Color.red;
    
    [Header("Visual Options")]
    [Tooltip("Çizgiyi görselleştirmek için Line Renderer kullan")]
    public bool useLineRenderer = true;
    
    [Tooltip("Çizgiyi görselleştirmek için 3D Cube kullan (alternatif)")]
    public bool useCube = false;

    private LineRenderer lineRenderer;
    private GameObject cubeVisual;

    void Start()
    {
        if (useLineRenderer)
        {
            CreateLineRenderer();
        }
        
        if (useCube)
        {
            CreateCubeVisual();
        }
    }

    void CreateLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        
        // Çizgiyi dikey olarak çiz (Y ekseni boyunca)
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.up * lineHeight;
        
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        
        // Çizgiyi X ekseni boyunca tekrarla (birden fazla çizgi)
        if (lineLength > 0)
        {
            GameObject lineParent = new GameObject("RedLineParent");
            lineParent.transform.SetParent(transform);
            lineParent.transform.localPosition = Vector3.zero;
            
            int lineCount = Mathf.CeilToInt(lineLength / 0.5f); // Her 0.5 birimde bir çizgi
            
            for (int i = 0; i < lineCount; i++)
            {
                GameObject lineObj = new GameObject("RedLine_" + i);
                lineObj.transform.SetParent(lineParent.transform);
                lineObj.transform.localPosition = new Vector3(i * 0.5f - lineLength / 2f, 0, 0);
                
                LineRenderer lr = lineObj.AddComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = lineColor;
                lr.endColor = lineColor;
                lr.startWidth = lineWidth;
                lr.endWidth = lineWidth;
                lr.positionCount = 2;
                lr.useWorldSpace = false;
                
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, Vector3.up * lineHeight);
            }
        }
    }

    void CreateCubeVisual()
    {
        cubeVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeVisual.name = "RedLineCube";
        cubeVisual.transform.SetParent(transform);
        cubeVisual.transform.localPosition = Vector3.zero;
        cubeVisual.transform.localScale = new Vector3(lineLength, lineHeight, lineWidth);
        
        // Kırmızı materyal ekle
        Renderer renderer = cubeVisual.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = lineColor;
        mat.SetFloat("_Metallic", 0f);
        mat.SetFloat("_Glossiness", 0.5f);
        renderer.material = mat;
        
        // Collider'ı kaldır (sadece görsel)
        Collider col = cubeVisual.GetComponent<Collider>();
        if (col != null)
        {
            Destroy(col);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        Vector3 center = transform.position + Vector3.up * (lineHeight / 2f);
        Vector3 size = new Vector3(lineLength, lineHeight, lineWidth);
        Gizmos.DrawWireCube(center, size);
    }
}

