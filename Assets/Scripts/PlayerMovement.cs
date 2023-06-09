using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;

    void Update()
    {
        float horizontalIn = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(horizontalIn*_speed*Time.deltaTime,0f,0f); 
    }
}
