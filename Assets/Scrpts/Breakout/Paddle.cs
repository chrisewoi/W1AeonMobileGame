using UnityEngine;

public class Paddle : MonoBehaviour, IRestart
{
    [SerializeField] private float speed;
    
    

    void Update()
    {
        if (!RoundManager.Singleton.isRoundActive) return;

        Vector2 point = new Vector2();

        if (!Input.GetMouseButton(0)) return;

        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 target = new Vector3(point.x, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void Restart()
    {
        // Recenter the paddle
        transform.position = new(0, transform.position.y, transform.position.z);
    }
}
