using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private bool _isRoundActive = true;
    public bool isRoundActive => _isRoundActive;

    private static RoundManager _singleton;

    public static RoundManager singleton
    {
        // When something accesses RoundManager.singleton, it will evaluate and return whatever is in _singleton
        get => _singleton; //?? (_singleton = new RoundManager());

        // we can say "private" before "set" to make this read-only to other scripts
        private set
        {
            // Value is whatever the user is trying to apply
            // e.g. RoundManager.singleton = objectA, value stands in for objectA
            if (_singleton != null)
            {
                Debug.LogWarning("There should only be one RoundManager in the scene! The other manager has been destroyed.");
            }
            _singleton = value;
        }
    }

    private void Awake()
    {
        _singleton = this;
    }

    public void NewGame()
    {
        _isRoundActive = true;
    }

    public void EndGame()
    {
        _isRoundActive = false;
    }
}
