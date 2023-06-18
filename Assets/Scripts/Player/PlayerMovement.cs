using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
       
    //Parámetros de salto y velocidad. Ajustarlo en interfaz
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _body;
    private bool _inGround; //Para verificar que solo puede saltar si se encuentra en el suelo



    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        float direction = Input.GetAxis("Horizontal"); //Obtener la direccion hacia la cual se está movimendo : (1,-1)
        _body.velocity = new Vector2(_speed * direction, _body.velocity.y); //Aplicar desplazamiento en la coordenada x teniendo en cuenta la velocidad y dirección

        if (direction > 0f) { //Voltear al jugador según la dirección hacia la cual se está moviendo
            transform.localScale = Vector3.one;
        } else if (direction < 0f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.Space) && _inGround) {  //Salto
            Jump();
        }
    }

    private void Jump() {
        _body.velocity = new Vector2(_body.velocity.x, _jumpForce); //Cambia la coordenada en y según jumpForce
        _inGround = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) { 
        if (collision.gameObject.tag == "Ground") {
            _inGround = true;
        }

    }   

}
