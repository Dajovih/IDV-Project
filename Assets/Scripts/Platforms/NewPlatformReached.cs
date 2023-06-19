using System;
using UnityEngine;

public class NewPlatformReached : MonoBehaviour
{   
    [SerializeField] private int _points = 20;

    private bool _reached = false;
    private PlatformHole _platformHole;

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
                GameEvents.onNewPlatformEvent?.Invoke();
                GameEvents.OnPointsChangeEvent?.Invoke(_points);
            }
        }
    }
}
