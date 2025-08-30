using UnityEngine;

/// Toplar dönüş bölgesi ile çarpıştığında top dönüş mantığını yönetir
public class BallReturn : MonoBehaviour
{
    [Header("Top Ayarları")]
    [SerializeField] private string ballTag = "Ball";

    private BallLauncher ballLauncher;

    private void Awake()
    {
        FindBallLauncher();
    }

    /// BallLauncher referansını bulur ve saklar
    private void FindBallLauncher()
    {
        ballLauncher = FindObjectOfType<BallLauncher>();

        if (ballLauncher == null)
        {
            Debug.LogError("BallLauncher sahnede bulunamadı!");
        }
    }

    /// Bu nesne ile çarpışma meydana geldiğinde çağrılır
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsBallCollision(collision))
        {
            HandleBallReturn(collision);
        }
    }

    /// Çarpışmanın bir top ile olup olmadığını kontrol eder
    private bool IsBallCollision(Collision2D collision)
    {
        return collision.collider.CompareTag(ballTag);
    }

    /// Top dönüş sürecini yönetir
    private void HandleBallReturn(Collision2D collision)
    {
        ballLauncher.ReturnBall();
        DeactivateBall(collision);
    }

    /// Dönen topu devre dışı bırakır
    private void DeactivateBall(Collision2D collision)
    {
        collision.collider.gameObject.SetActive(false);
    }
}
