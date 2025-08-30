using UnityEngine;

/// Top fırlatma yönü için görsel önizleme çizgisini yönetir
public class LaunchPreview : MonoBehaviour
{
    [Header("Çizgi Renderer Ayarları")]
    [SerializeField] private int linePoints = 2;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        InitializeLineRenderer();
    }

    /// Çizgi renderer bileşenini başlatır
    private void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.LogError("LaunchPreview üzerinde LineRenderer bulunamadı!");
            return;
        }

        SetupLineRenderer();
    }

    /// Çizgi renderer'ı başlangıç ayarları ile kurar
    private void SetupLineRenderer()
    {
        lineRenderer.positionCount = linePoints;
        lineRenderer.enabled = false;
    }

    /// Önizleme çizgisinin başlangıç noktasını ayarlar
    public void SetStartPoint(Vector3 startPoint)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.enabled = true;
        }
    }


    /// Önizleme çizgisinin bitiş noktasını ayarlar
    public void SetEndPoint(Vector3 endPoint)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(1, endPoint);
        }
    }


    /// Önizleme çizgisini gizler
    public void Hide()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }
}