using UnityEngine;

public class ScreenWrap : MonoBehaviour
{   
    [SerializeField]
    private BoxCollider2D _left;
    [SerializeField]
    private BoxCollider2D _right;

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider>()==_left){
            Debug.Log("Izquierda");
        }
        else if (collision.GetComponent<Collider>()==_right){
            Debug.Log("Derecha");
        }
    }
}
