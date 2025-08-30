using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// Top fırlatma mekaniklerini ve top yönetimini ele alır
public class BallLauncher : MonoBehaviour
{
    [Header("Top Ayarları")]
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float ballLaunchDelay = 0.1f;

    [Header("Referanslar")]
    private LaunchPreview launchPreview;
    private BlockSpawner blockSpawner;

    private List<Ball> activeBalls = new List<Ball>();
    private int ballsReturned = 0;

    private bool canMove = true;
    private bool canDrag = false;

    private Vector3 dragStartPosition;
    private Vector3 dragEndPosition;

    private void Awake()
    {
        InitializeComponents();
        CreateInitialBall();
    }

    private void Update()
    {
        if (!canMove) return;

        HandleInput();
    }


    /// Gerekli bileşenleri başlatır
    private void InitializeComponents()
    {
        launchPreview = GetComponent<LaunchPreview>();
        blockSpawner = FindObjectOfType<BlockSpawner>();
    }

    /// Fırlatıcı için başlangıç topunu oluşturur
    private void CreateInitialBall()
    {
        CreateNewBall();
    }

    /// Top fırlatma için tüm girişleri yönetir
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && canDrag)
        {
            StartDrag();
        }
        else if (Input.GetMouseButton(0))
        {
            UpdateDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    /// Sürükleme işlemini başlatır
    private void StartDrag()
    {
        canDrag = false;
        dragStartPosition = transform.position;
        launchPreview.SetStartPoint(dragStartPosition);
    }

    /// Sürükleme işlemini günceller
    private void UpdateDrag()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        dragEndPosition = mouseWorldPosition;
        launchPreview.SetEndPoint(dragEndPosition);
    }

    /// Sürükleme işlemini sonlandırır ve topları fırlatır
    private void EndDrag()
    {
        StartCoroutine(LaunchAllBalls());
    }

    /// Fare ekran konumunu dünya konumuna dönüştürür
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        return mousePosition;
    }

    /// Yeni bir top oluşturur ve aktif toplar listesine ekler
    private void CreateNewBall()
    {
        Ball newBall = Instantiate(ballPrefab);
        newBall.gameObject.SetActive(false);
        activeBalls.Add(newBall);
        ballsReturned++;
        canDrag = true;
    }

    /// Bir top fırlatıcıya geri döndüğünde çağrılır
    public void ReturnBall()
    {
        ballsReturned++;

        if (AllBallsReturned())
        {
            HandleAllBallsReturned();
        }
    }


    /// Tüm topların geri dönüp dönmediğini kontrol eder
    private bool AllBallsReturned()
    {
        return ballsReturned == activeBalls.Count;
    }

    /// Tüm toplar geri döndüğünde senaryoyu yönetir
    private void HandleAllBallsReturned()
    {
        blockSpawner.SpawnBlocks();
        CreateNewBall();
        canMove = true;
    }

    /// Tüm topları her fırlatma arasında gecikme ile fırlatır
    private IEnumerator LaunchAllBalls()
    {
        canMove = false;
        Vector3 launchDirection = CalculateLaunchDirection();

        foreach (Ball ball in activeBalls)
        {
            LaunchSingleBall(ball, launchDirection);
            ballsReturned--;
            yield return new WaitForSeconds(ballLaunchDelay);
        }
    }

    /// Sürükleme başlangıç ve bitiş konumlarına göre fırlatma yönünü hesaplar
    private Vector3 CalculateLaunchDirection()
    {
        return (dragEndPosition - dragStartPosition).normalized;
    }

    /// Tek bir topu fırlatır
    private void LaunchSingleBall(Ball ball, Vector3 direction)
    {
        ball.transform.position = transform.position;
        ball.gameObject.SetActive(true);

        Rigidbody2D ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballRigidbody.velocity = Vector2.zero;
        ballRigidbody.AddForce(direction * launchForce, ForceMode2D.Impulse);
    }
}
