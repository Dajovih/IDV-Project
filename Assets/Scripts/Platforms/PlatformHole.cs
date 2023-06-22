using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class PlatformHole : MonoBehaviour {
    [SerializeField] private float _movementTime; //Cada cuanto se van a mover las plataformas. Cada movementTime se va a ejecutar la función de movimiento

    private List<List<GameObject>> _allLevels;  //Este contiene todas las plataformas en listas por niveles
    private List<List<int>> _holes; //Todos los huecos que hay
    private bool _firstHole = false;
    private float _counter = 0f;
    private float _firstAuxTime = 0f;

    private float _firstHoleStartTime = 2f; //Cuando empieza el primer hueco. Allí empieza el nivel



    private void Awake()
    {
        _holes = new List<List<int>>();
        _allLevels = new List<List<GameObject>>();
        foreach (Transform level in transform) //Por cada hijo level en el Empty Object Platforms
        {
            List<GameObject> levelPlatforms = new List<GameObject>(); //Se crea la lista que almacena las plataformas por cada piso
            foreach (Transform platform in level) //Por cada plataforma en el piso
            {
                levelPlatforms.Add(platform.gameObject);
            }
            _allLevels.Add(levelPlatforms); //Agregar la lista del piso con las plataformas
        }
    }

    private void Start()
    {
        GameEvents.onNewPlatformEvent += HoleCreation;
        GameEvents.onWin += HolesDestruction;
        GameEvents.onAttack += StopMoving;
        Invoke("HoleCreation", _firstHoleStartTime);    //Se invoca al segundo _firstsHoleStartTime el metodo hole creation, para crear el primer hueco
    }

    private void Update()
    {
        _firstAuxTime += Time.deltaTime;
        if (_firstAuxTime >= _firstHoleStartTime)   //Hay que asegurarse de que ya haya pasado el tiempo de creación del primer hueco
        {
            _counter += Time.deltaTime;
            if (_counter >= _movementTime)  //Cada que se llegue al tiempo de movimiento se invoca. Es decir que la función se va a llamar cada movementTime segundos
            {
                HoleMovement(); 
                _counter = 0;
            }
        }

       
    }

    private void OnDestroy()
    {
        GameEvents.onNewPlatformEvent -= HoleCreation;
        GameEvents.onWin -= HolesDestruction;
        GameEvents.onAttack -= StopMoving;
    }

    private void HoleUpdate(List<int> hole) //Es necesario que el collider se vuelva trigger y que la imagen desaparezca para que parezca hueco
    {
        _allLevels[hole[0]][hole[1]].GetComponent<Collider2D>().isTrigger = true;
        _allLevels[hole[0]][hole[1]].GetComponent<SpriteRenderer>().enabled = false;
        _allLevels[hole[2]][hole[3]].GetComponent<Collider2D>().isTrigger = true;
        _allLevels[hole[2]][hole[3]].GetComponent<SpriteRenderer>().enabled = false;
    }

    private void HoleDestroy(List<int> hole) //Si activa el sprite y no es trigger
    {
        _allLevels[hole[0]][hole[1]].GetComponent<Collider2D>().isTrigger = false;
        _allLevels[hole[0]][hole[1]].GetComponent<SpriteRenderer>().enabled = true;
        _allLevels[hole[2]][hole[3]].GetComponent<Collider2D>().isTrigger = false;
        _allLevels[hole[2]][hole[3]].GetComponent<SpriteRenderer>().enabled = true;
    }

    private void HoleCreation() 
    {   
        List<int> hole = new List<int>();   //Almacena informacion del hueco
        int nLevel, nPlatform, orientation; //Es necesario saber el piso donde se encuentra el hueco, la plataforma donde se va a generar y la orientacion de movimiento (izquierda -1, derecha 1)
        while (true)
        {
            if (!_firstHole) { nLevel = 0; } else { nLevel = UnityEngine.Random.Range(0, _allLevels.Count); } //Si se trata del primer hueco, este se va a generar en el piso 1, en caso contrario escoge aleatoriamente un piso
            nPlatform = UnityEngine.Random.Range(1, _allLevels[nLevel].Count - 1);  //Se escoge aleatoriamente una plataforma
            if (nPlatform == 1) { orientation = 1; } else if (nPlatform == 18) { orientation = -1;} else { orientation = (UnityEngine.Random.Range(0, 2) * 2) - 1; }    //Se escoge aleatoriamente una direccion. Si se trata de los extremos, se escogen direcciones hacia el centro
            if (!_allLevels[nLevel][nPlatform].GetComponent<Collider2D>().isTrigger && !_allLevels[nLevel][nPlatform + orientation].GetComponent<Collider2D>().isTrigger) 
            {
                break;  //Si las plataformas no son huecos se sale del ciclo, en caso contrario se buscan nuevas plataformas
            }
        }
        if (!_firstHole) { _firstHole = true; }
        /*Por cada hueco se gurda el piso de la primera plataforma, la posicion de la primera plataforma, el piso de la segunda plataforma, la posicion de la segunda plataforma,
         y la dirección hacia donde se mueve*/
        hole.Add(nLevel);
        hole.Add(nPlatform);
        hole.Add(nLevel);
        hole.Add(nPlatform + orientation);
        hole.Add(orientation);
        _holes.Add(hole);
        HoleUpdate(hole);
    }


    private void HoleMovement() {
        //Por cada uno de los huecos se debe de hacer el movimiento
        foreach(List<int> hole in _holes) {
            //El hueco mas hacia el centro debe de volverse plataforma y los huecos se generan en los extremos
            _allLevels[hole[0]][hole[1]].GetComponent<Collider2D>().isTrigger = false;
            _allLevels[hole[0]][hole[1]].GetComponent<SpriteRenderer>().enabled = true;
            hole[0] = hole[2];
            hole[1] = hole[3];
            hole[3] = hole[1] + hole[4];
            //En los casos en que se obtenga 20 o -1 es porque hay un cambio de nivel. Si se obtiene -1 o 6 en las plataformas es porque se vuelve a empezar desde los niveles extremos
            
            if (hole[3] == -1) {
                hole[3] = 19;
                hole[2] = hole[2] - hole[4];
                if (hole[2] == 7) {
                    hole[2] = 0;
                }
            } else if (hole[3] == 20) {
                hole[3] = 0;
                hole[2] = hole[2] - hole[4];
                if (hole[2] == -1) {
                    hole[2] = 6;
                }
            }
            HoleUpdate(hole);
        }
    }

    private void HolesDestruction()
    {
        List<List<int>> copy = _holes.ToList();
        _holes.Clear();
        foreach (List<int> hole in copy)
        {
            HoleDestroy(hole);
        }
    }

    private void StopMoving(float seconds)
    {
        StartCoroutine(StopCoroutine(seconds));
    }

    private System.Collections.IEnumerator StopCoroutine(float seconds)
    {
        float aux = _movementTime;
        _movementTime = 999;
        yield return new WaitForSeconds(seconds);
        _movementTime = aux;
    }
}
