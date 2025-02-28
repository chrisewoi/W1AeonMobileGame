using UnityEngine;

public class Paddle : MonoBehaviour, IRestart
{
    [SerializeField] private float speed;
    
    

    void Update()
    {
        if (!RoundManager.Singleton.isRoundActive) return;

        //Vector2 point = new Vector2();
        if (!TryGetInputPosition(out Vector3 point))
            return;
        
        // convert screenspace point to world point
        point = Camera.main.ScreenToWorldPoint(point);
        
        Vector3 target = new Vector3(point.x, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private bool TryGetInputPosition(out Vector3 position)
    {
        // this must be included to initialise the "out" variable
        position = new Vector3();
        
        // if we"re getting touches, do this stuff
        if (Input.touchCount > 0)
        {
            position = Input.GetTouch(0).position;
            return true;
        }
        // if we make it here, no touch is happening
#if UNITY_EDITOR
        // if we're in editor, check for mouse controls
        if (!Input.GetMouseButton(0))
            return false;

        position = Input.mousePosition;
        return true;
#endif
        
        // if we get here, we're not in editor, AND we have no touch, so return
        return false;
    }

    public void Restart()
    {
        // Recenter the paddle
        transform.position = new(0, transform.position.y, transform.position.z);
    }

    private bool GetInputPosition(out Vector3 position)
    {
        position = new Vector3();

        if (Input.touchCount > 0)
        {
            position = Input.GetTouch(0).position;
            return true;
        }
#if UNITY_EDITOR
        if (!Input.GetMouseButton(0))
            return false;
        position = Input.mousePosition;
        return true;
#endif
        // if we get here, we're not in the editor, AND we have no touch, so return false
        return false;
    }
}
