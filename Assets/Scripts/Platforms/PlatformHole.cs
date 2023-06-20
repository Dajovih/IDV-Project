using System;
using UnityEngine;
using System.Collections.Generic;

public class PlatformHole : MonoBehaviour {
    //Estos son los par�metros de cuando se genera el primer hueco, cuando se empieza a mover y la velocidad de movimiento
    [SerializeField] private float _movementVelocity;
    [SerializeField] private float _firstHoleStartTime;
    [SerializeField] private float _holeMovementStart;

    private List<List<GameObject>> _allLevels;  //Este contiene todas las plataformas en listas por niveles
    private List<List<int>> _holes; //Todos los huecos que hay
    private bool _firstHole = false;

    private void Awake()
    {
        GameEvents.onNewPlatformEvent += HoleCreation;
        _allLevels = new List<List<GameObject>>();
        _holes = new List<List<int>>();
        foreach (Transform level in transform) //Por cada hijo level en el Empty Object Platforms
        {
            List<GameObject> levelPlatforms = new List<GameObject>(); //Se crea la lista que almacena las plataformas por cada nivel
            foreach (Transform platform in level) //Por cada plataforma en en level
            {
                levelPlatforms.Add(platform.gameObject);
            }
            _allLevels.Add(levelPlatforms); 
        }
        Debug.Log("Total number of platforms: " + _allLevels.Count.ToString());
    }

    private void Start()
    {
        Invoke("HoleCreation", _firstHoleStartTime);    //Se invoca al segundo _firstsHoleStartTime el m�todo hole creation, que crea huecos
        InvokeRepeating("HoleMovement", _holeMovementStart, _movementVelocity); //Se invoca cada movementVelocity segundos el movimiento de los huecos. Es decir que est� es la velocidad con la que se mueven
    }

    private void OnDestroy()
    {
        GameEvents.onNewPlatformEvent -= HoleCreation;
    }

    private void HoleUpdate(List<int> hole) //Es necesario que el collider se vuelva trigger y que la imagen desaparezca para que parezca hueco
    {
        _allLevels[hole[0]][hole[1]].GetComponent<Collider2D>().isTrigger = true;
        _allLevels[hole[0]][hole[1]].GetComponent<SpriteRenderer>().enabled = false;
        _allLevels[hole[2]][hole[3]].GetComponent<Collider2D>().isTrigger = true;
        _allLevels[hole[2]][hole[3]].GetComponent<SpriteRenderer>().enabled = false;
    }

    private void HoleCreation() 
    {   
        Debug.Log("Hole Created!");
        List<int> hole = new List<int>();   //Almacena informaci�n del hueco
        int nLevel, nPlatform, orientation; //Es necesario saber el nivel donde se encuentra el hueco, la plataforma donde se va a generar y la orientacion de movimiento (izquierda, derecha)
        while (true)
        {
            if (!_firstHole) { nLevel = 0; } else { nLevel = UnityEngine.Random.Range(0, _allLevels.Count); } //Si se trata del primer hueco, este se va a generar en el nivel 1, en caso contrario escoge aleatoriamente un nivel
            nPlatform = UnityEngine.Random.Range(1, _allLevels[nLevel].Count - 1);  //Se escoge aleatoriamente una plataforma
            if (nPlatform == 1) { orientation = 1; } else if (nPlatform == 18) { orientation = -1;} else { orientation = (UnityEngine.Random.Range(0, 2) * 2) - 1; }    //Se escoge aleatoriamente una direcci�n. Si se trata de los extremos, se escogen direcciones hacia el centro
            if (!_allLevels[nLevel][nPlatform].GetComponent<Collider2D>().isTrigger && !_allLevels[nLevel][nPlatform + orientation].GetComponent<Collider2D>().isTrigger) 
            {
                break;  //Si las plataformas no son huecos se sale del ciclo, en caso contrario se buscan nuevas plataformas
            }
        }
        if (!_firstHole) { _firstHole = true; }
        //Se guarda el nivel de la primera plataforma, la posici�n de la primera plataforma, el nivel de la segunda plataforma, la posici�n de la segunda plataforma, la orientaci�n y un bool representado por 0  y 1 que indica 
        //si ese hueco ya fue pasado
        hole.Add(nLevel);
        hole.Add(nPlatform);
        hole.Add(nLevel);
        hole.Add(nPlatform + orientation);
        hole.Add(orientation);
        hole.Add(0);
        _holes.Add(hole);
        HoleUpdate(hole);
    }


    private void HoleMovement() {
        //Por cada uno de los huecos se debe de hacer el movimiento
        foreach(List<int> hole in _holes) {
            //El hueco m�s hacia el centro debe de volverse plataforma y los huecos se generan en los extremos
            _allLevels[hole[0]][hole[1]].GetComponent<Collider2D>().isTrigger = false;
            _allLevels[hole[0]][hole[1]].GetComponent<SpriteRenderer>().enabled = true;
            hole[0] = hole[2];
            hole[1] = hole[3];
            hole[3] = hole[1] + hole[4];
            //En los casos en que se obtenga 20 o -1 es porque hay un cambio de nivel. �Si se obtiene -1 o 6 en las plataformas es porque se vuelve a empezar desde los niveles extremos
            
            if (hole[3] == -1) {
                hole[3] = 19;
                hole[2] = hole[2] - hole[4];
                if (hole[2] == 6) {
                    hole[2] = 0;
                }
            } else if (hole[3] == 20) {
                hole[3] = 0;
                hole[2] = hole[2] - hole[4];
                if (hole[2] == -1) {
                    hole[2] = 5;
                }
            }
            HoleUpdate(hole);
        }
    }
}
