using System;
using UnityEngine;
using UnityEngine.Events;

// This namespace helps us sort through and compare different types
using System.Linq;

public class RoundManager : MonoBehaviour
{
    private bool _isRoundActive = true;
    public bool isRoundActive => _isRoundActive;

    public UnityEvent onRoundEnd;

    private static RoundManager _singleton;

    public static RoundManager Singleton
    {
        // access the private variable via the Get property
        get
        {
            return _singleton;
        }
        // private set means only this script can set via the property
        private set
        {
            // "value" is whatever we tried to set the property to.
            // i.e. player.Health = 6; value is 6
            _singleton = value;
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public void NewGame()
    {
        _isRoundActive = true;
        foreach (IRestart restart in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                                                        .OfType<IRestart>())
        {
            restart.Restart();
        }
    }

    public void EndGame()
    {
        _isRoundActive = false;
        // this triggers the event and any attached behaviours
        onRoundEnd.Invoke();

        foreach (IStop stop in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IStop>())
        {
            stop.Stop();
        }
    }
}



/*
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
*/