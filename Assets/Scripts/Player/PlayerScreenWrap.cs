using System;
using System.Diagnostics;
using UnityEngine;

public class PlayerScreenWrap : MonoBehaviour {
    [SerializeField] private GameObject _clone; //Es necesario crear un clon para el screen wrap

    private bool _inDelimiter = true;  //Condici�n necesaria para cuando sale del portal y de la c�mara
    private Transform _parent;

    private void Awake() {
        _clone.SetActive(false); //Apagar el clon por si se encuentra prendido
        _parent = transform.parent;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Vector3 nPosition = new Vector3(_parent.position.x, _parent.position.y, 0);

        if (collider.gameObject.tag == "LevelDelimiter") {  //Si se encuentra dentro de la c�mara
            _inDelimiter = true;
        }

        if (collider.gameObject.tag == "LeftPortal") {  //Si se entra en el portal izquierdo, se activa el clon al otro lado
            _clone.SetActive(true);
            nPosition.x = nPosition.x + 20;
            _clone.transform.position = nPosition;  //Posici�n del clon 20 bloques a la derecha, esto es por la cantidad de cuadrdos 
        } 

        if (collider.gameObject.tag == "RightPortal") { //Si se entra en el portal derecho, se activa el clon al otro lado
            _clone.SetActive(true);
            nPosition.x = nPosition.x - 20; //Posicion del clon 20 bloques a la izquieda
            _clone.transform.position = nPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "LevelDelimiter") {  //Si salen de la camara es necesario guardar un booleano
            _inDelimiter = false;
            AudioManager.Instance.PlaySound2D("ScreenWrapSFX");
        }
        
        if (collider.gameObject.tag == "LeftPortal" || collider.gameObject.tag == "RightPortal") {
            if (_inDelimiter) {    //Si permanecen en camara y salen de los portales, es porque el jugador principal no paso al otro lado
                _clone.SetActive(false);
            } else {    //Si salen de la camara y del portal, el jugador principal debe tomar la posici�n del clon puesto que sale de la c�mara 
                _parent.position = _clone.transform.position;
                _clone.SetActive(false);
            }
        }
    }
}