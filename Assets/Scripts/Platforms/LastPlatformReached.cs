using System;
using UnityEngine;

public class LastPlatformReached : MonoBehaviour
{
    private bool _reached = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (!_reached)
            {
                _reached = true;

                GameEvents.onWin?.Invoke();
                GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag("Player");

                foreach (GameObject obj in objectsToDisable) { 
                    obj.SetActive(false);  
                }

                GameManager.Instance.NextLevel();
                AudioManager.Instance.PlaySound2D("GoalSFX");
            }
        }
    }
}
