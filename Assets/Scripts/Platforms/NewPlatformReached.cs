using System;
using UnityEngine;

public class NewPlatformReached : MonoBehaviour
{
    private bool _reached = false;
    private PlatformHole _platformHole;
    public static event Action onNewPlatform;

    void Start()
    {
        _platformHole = GameObject.FindObjectOfType<PlatformHole>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (!_reached)
            {
                _reached = true;
                onNewPlatform?.Invoke();
            }
        }
    }
}
