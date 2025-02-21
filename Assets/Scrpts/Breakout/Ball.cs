using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour, IStop, IRestart
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Restart();
    }

    void Update()
    {
        // If the ball is moving faster or slower than it should...
        if (rb.linearVelocity.magnitude != speed)
        {
            // Keep our direction, but fix the speed
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Killzone"))
        {
            RoundManager.Singleton.EndGame();
        }
    }

    public void Stop()
    {
        rb.simulated = false;
    }

    public void Restart()
    {
        rb.simulated = true;
        transform.position = Vector3.zero;
        
        // Get a random point in a circle, normalise it, and then apply our speed
        rb.linearVelocity = Random.insideUnitCircle.normalized * speed;
    }
}
