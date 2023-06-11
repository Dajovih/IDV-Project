using Unity.VisualScripting;
using UnityEngine;

public class PlayerScreenWrap : MonoBehaviour {
    [SerializeField] private GameObject _clone; 
    private bool _inCamera = true;  //Condición necesaria para cuando sale del portal y de la cámara

    private void Awake() {
        _clone.SetActive(false); //Apagar el clon
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Vector3 nPosition = new Vector3(transform.position.x, transform.position.y, 0);
        if (collider.gameObject.tag == "MainCamera") {  //Si se encuentra en la vista de la cámara
            //Debug.Log("En cámara");
            _inCamera = true;
        }
        if (collider.gameObject.tag == "LeftPortal") {  //Si se entra en el portal izquierdo, se activa el clon al otro lado
            //Debug.Log(collider.gameObject.name);
            //Debug.Log("Left");
            _clone.SetActive(true);
            nPosition.x = nPosition.x + 20;
            _clone.transform.position = nPosition;  //Posición del clon 20 bloques a la derecha        
        } 
        if (collider.gameObject.tag == "RightPortal") { //Si se entra en el portal derecho, se activa el clon al otro lado
            //Debug.Log(collider.gameObject.name);
            //Debug.Log("Right");
            _clone.SetActive(true);
            nPosition.x = nPosition.x - 20; //Posicion del clon 20 bloques a la izquieda
            _clone.transform.position = nPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "MainCamera") {  //Si salen de la camara es necesario guardar un booleano
            _inCamera = false;
            //Debug.Log("Sale de la cámara");
        }
        if (collider.gameObject.tag == "LeftPortal" || collider.gameObject.tag == "RightPortal") {
            if (_inCamera) {    //Si permanecen en camara y salen de los portales, es porque el jugador principal no paso al otro lado
                //Debug.Log("Sale del portal izquierdo pero está en cámara");
                _clone.SetActive(false);
            } else {    //Si salen de la camara y del portal, el jugador principal debe tomar la posición del clon. 
                //Debug.Log("Sale del portal izquierdo y de la cámara");
                transform.position = _clone.transform.position;
                _clone.SetActive(false);

            }
        }
    }
}