
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField] private int _subtractPoints = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {   
            GameEvents.OnPointsChangeEvent?.Invoke(_subtractPoints * -1);
        }
    }
}
