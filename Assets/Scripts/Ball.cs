using UnityEngine;

/// Topun fizik ve hareket davranışını kontrol eder
public class Ball : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minVelocityThreshold = 0.01f;

    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleBallMovement();
    }

    /// Top hareketini yönetir ve sabit hızı sağlar
    private void HandleBallMovement()
    {
        if (IsBallStopped())
        {
            ApplyRandomUpwardForce();
        }
        else
        {
            MaintainConstantSpeed();
        }
    }

    /// Topun durup durmadığını kontrol eder
    private bool IsBallStopped()
    {
        return rigidbody2D.velocity.sqrMagnitude < minVelocityThreshold;
    }

    /// Top durduğunda rastgele yukarı doğru kuvvet uygular
    private void ApplyRandomUpwardForce()
    {
        Vector2 randomDirection = new Vector2(
            Random.Range(-1f, 1f),
            1f
        ).normalized;

        rigidbody2D.velocity = randomDirection * moveSpeed;
    }

    /// Topun sabit hızını korurken yönünü korur
    private void MaintainConstantSpeed()
    {
        rigidbody2D.velocity = rigidbody2D.velocity.normalized * moveSpeed;
    }
}