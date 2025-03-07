using UnityEngine;

public class ScreenInteractionController : MonoBehaviour
{
    private ScreenInteraction currentInteraction;

    private ITouchable currentTouchable;

    private static Camera _camera;

    new public static Camera camera => _camera;

    private Vector3 touchPositionLast;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouch();
    }

    private void CheckTouch()
    {
        //if we find a touch
        if (TouchDistributor.TryGetTouch(currentInteraction != null ? currentInteraction.fingerID : -1, out Touch touch))
        {
            //if we're starting a new interaction
            if (currentInteraction == null)
            {
                //construct one of our classes
                currentInteraction = new ScreenInteraction(touch.fingerId, touch.position);
            }
            else
            {
                currentInteraction.Poll(touch.position);
                touchPositionLast = touch.position;
            }
            //try to find a touchable if we need to
            if (currentInteraction == null)
            {
                CastTouch(touch.position);
            }
            else
            {
                //else we'll update our current touchable
                ManageTouch(touch.position);
            }

            //we found a touch, so stop here
            return;
        }

        //if we don't find a touch
        NoTouch();
    }

    private void CastTouch(Vector3 touchPosition)
    {
        Ray touchRay = camera.ScreenPointToRay(touchPosition);

        //if we hit something
        if (Physics.Raycast(touchRay, out RaycastHit hit))
        {
            //and that something has a touchable script
            if (hit.transform.TryGetComponent<ITouchable>(out ITouchable currentTouchable))
            {
                //begin it's touch behaviour
                currentInteraction.TryAddTouchable(currentTouchable);
                currentTouchable.OnTouchBegin(touchPosition);
            }
        }
    }

    private void ManageTouch(Vector3 touchPosition)
    {
        currentInteraction.touchable.OnTouchStay(touchPosition);
    }

    private void NoTouch()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            //we'll use -2 as a special "this is the mouse" finger ID.


            if (currentInteraction == null)
            {
                currentInteraction = new ScreenInteraction(-2, Input.mousePosition);
            }
            else
            {
                currentInteraction.Poll(Input.mousePosition);
                touchPositionLast = Input.mousePosition;
            }

            if (currentInteraction.touchable == null)
            {
                CastTouch(Input.mousePosition);
            }
            else
            {
                ManageTouch(Input.mousePosition);
            }
            return;
        }
#endif

        if (currentInteraction != null)
        {
            // End the current interaction
            currentInteraction.End(touchPositionLast);
            //if we're managing a touchable
            if (currentInteraction?.touchable != null)
            {
                currentInteraction.touchable.OnTouchEnd(touchPositionLast);
            }
            else
            {
                //try to swipe
                if (ScreenInteraction.Swipe.Try(currentInteraction, out ScreenInteraction.Swipe swipe))
                {
                    Debug.Log($"Did a swipe from {swipe.start} to {swipe.end} covering distance of {swipe.distance}");
                }
                else if (ScreenInteraction.Tap.Try(currentInteraction, out ScreenInteraction.Tap tap))
                {
                    Debug.Log($"Did a tap at {tap.screenPosition}. In world, this is {tap.WorldPosition}");
                }
            }
        }
        currentInteraction = null;
    }
}

public interface ITouchable
{
    public void OnTouchBegin(Vector3 touchPosition);
    public void OnTouchStay(Vector3 touchPosition);
    public void OnTouchEnd(Vector3 touchPosition);
}