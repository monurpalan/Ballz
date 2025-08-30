using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// Oyunda vuruş noktaları ve renk değişimleri olan kırılabilir bloğu temsil eder
public class Block : MonoBehaviour
{
    [Header("Blok Ayarları")]
    [SerializeField] private List<Color> blockColors;
    [SerializeField] private int maxHitsPerColor = 10;

    private int currentHitPoints;
    private SpriteRenderer spriteRenderer;
    private TMP_Text hitPointsText;

    private void Awake()
    {
        InitializeComponents();
    }

    /// Gerekli bileşenleri başlatır
    private void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitPointsText = GetComponentInChildren<TMP_Text>();

        if (spriteRenderer == null)
        {
            Debug.LogError("Blok üzerinde SpriteRenderer bulunamadı!");
        }

        if (hitPointsText == null)
        {
            Debug.LogError("Blok üzerinde TMP_Text bulunamadı!");
        }
    }

    /// Blok için başlangıç vuruş noktalarını ayarlar
    public void SetHitPoints(int hitPoints)
    {
        currentHitPoints = hitPoints;
        UpdateVisualState();
    }

    /// Mevcut vuruş noktalarına göre görsel görünümü günceller
    private void UpdateVisualState()
    {
        UpdateHitPointsText();
        UpdateBlockColor();
    }

    /// Vuruş noktaları görüntü metnini günceller
    private void UpdateHitPointsText()
    {
        if (hitPointsText != null)
        {
            hitPointsText.SetText(currentHitPoints.ToString());
        }
    }

    /// Kalan vuruş noktalarına göre blok rengini günceller
    private void UpdateBlockColor()
    {
        if (blockColors == null || blockColors.Count == 0)
        {
            Debug.LogWarning("Blok için renk tanımlanmamış!");
            return;
        }

        Color newColor = CalculateBlockColor();
        spriteRenderer.color = newColor;
    }

    /// Vuruş noktalarına göre uygun rengi hesaplar
    private Color CalculateBlockColor()
    {
        int colorIndex = currentHitPoints / maxHitsPerColor;
        float colorBlend = (float)(currentHitPoints % maxHitsPerColor) / maxHitsPerColor;

        Color primaryColor = blockColors[colorIndex % blockColors.Count];
        Color secondaryColor = blockColors[(colorIndex + 1) % blockColors.Count];

        return Color.Lerp(primaryColor, secondaryColor, colorBlend);
    }

    /// Bu blok ile çarpışma meydana geldiğinde çağrılır
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleBlockHit();
    }

    /// Blok vurulduğunda ne olacağını yönetir
    private void HandleBlockHit()
    {
        currentHitPoints--;

        if (currentHitPoints > 0)
        {
            UpdateVisualState();
        }
        else
        {
            DestroyBlock();
        }
    }

    /// Vuruş noktaları sıfıra ulaştığında bloğu yok eder
    private void DestroyBlock()
    {
        Destroy(gameObject);
    }
}