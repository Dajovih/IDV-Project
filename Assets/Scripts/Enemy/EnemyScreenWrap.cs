using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScreenWrap : MonoBehaviour
{
    [SerializeField] private Transform _lastLeftPortal;
    [SerializeField] private GameObject _clone; //Es necesario crear un clon para el screen wrap
 
    private Transform _parent;
    private bool _inDelimiter = true;  //Condici�n necesaria para cuando sale del portal y de la c�mara


    private void Awake()
    {
        _clone.SetActive(false); //Apagar el clon por si se encuentra prendido
        _parent = transform.parent;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Vector3 nPosition = new Vector3(_parent.position.x, _parent.position.y, 0);

        if (collider.gameObject.tag == "LevelDelimiter")
        {  //Si se encuentra dentro de la camara
            _inDelimiter = true;
            Debug.Log($"Enemy {_parent.name} has enter the delimiter");
        }

        if (collider.gameObject.tag == "LeftPortal")
        {   
            Debug.Log($"Enemy {_parent.name} has enter a LeftPortal");
            if (collider.gameObject.name == _lastLeftPortal.name)
            {
                _clone.SetActive(true);
                nPosition.x = nPosition.x + 20;
                nPosition.y = nPosition.y - 5*2;
                _clone.transform.position = nPosition;  //Posici�n del clon 20 bloques a la derecha, esto es por la cantidad de cuadrdos
            }
            else
            {
                _clone.SetActive(true);
                nPosition.x = nPosition.x + 20;
                nPosition.y = nPosition.y + 2;
                _clone.transform.position = nPosition;  //Posici�n del clon 20 bloques a la derecha, esto es por la cantidad de cuadrdos
            }
        }       
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "LevelDelimiter")
        {  //Si salen de la camara es necesario guardar un booleano
            _inDelimiter = false;
            Debug.Log($"Enemy {_parent.name} has exit the delimiter");
        }

        if (collider.gameObject.tag == "LeftPortal")
        {   
            Debug.Log($"Enemy {_parent.name} has exit a LeftPortal");
            if (!_inDelimiter)
            {      
                //Si salen de la camara y del portal, el enemigo debe tomar la posici�n del clon puesto que sale de la c�mara 
                _parent.position = _clone.transform.position;
                _clone.SetActive(false);
            }
        }
    }
}
