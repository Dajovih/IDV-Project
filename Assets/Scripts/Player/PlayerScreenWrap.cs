using Unity.VisualScripting;
using UnityEngine;

public class PlayerScreenWrap : MonoBehaviour {
    [SerializeField] private GameObject _clone; //Es necesario crear un clon para el screen wrap
    
    private bool _inCamera = true;  //Condición necesaria para cuando sale del portal y de la cámara

    private void Awake() {
        _clone.SetActive(false); //Apagar el clon por si se encuentra prendido
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Vector3 nPosition = new Vector3(transform.position.x, transform.position.y, 0);
        if (collider.gameObject.tag == "MainCamera") {  //Si se encuentra dentro de la cámara
            _inCamera = true;
        }
        if (collider.gameObject.tag == "LeftPortal") {  //Si se entra en el portal izquierdo, se activa el clon al otro lado
            _clone.SetActive(true);
            nPosition.x = nPosition.x + 20;
            _clone.transform.position = nPosition;  //Posición del clon 20 bloques a la derecha, esto es por la cantidad de cuadrdos         
        } 
        if (collider.gameObject.tag == "RightPortal") { //Si se entra en el portal derecho, se activa el clon al otro lado
            _clone.SetActive(true);
            nPosition.x = nPosition.x - 20; //Posicion del clon 20 bloques a la izquieda
            _clone.transform.position = nPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "MainCamera") {  //Si salen de la camara es necesario guardar un booleano
            _inCamera = false;
        }
        if (collider.gameObject.tag == "LeftPortal" || collider.gameObject.tag == "RightPortal") {
            if (_inCamera) {    //Si permanecen en camara y salen de los portales, es porque el jugador principal no paso al otro lado
                _clone.SetActive(false);
            } else {    //Si salen de la camara y del portal, el jugador principal debe tomar la posición del clon puesto que sale de la cámara 
                transform.position = _clone.transform.position;
                _clone.SetActive(false);

            }
        }
    }
}