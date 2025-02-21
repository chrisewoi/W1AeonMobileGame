using UnityEngine;

public class PipePair : MonoBehaviour, IStop, IRestart
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.left * speed;
    }

    void Update()
    {
        
    }

    public void Stop()
    {
        rb.simulated = false;
    }

    public void Restart()
    {
        Destroy(gameObject);
    }
}
