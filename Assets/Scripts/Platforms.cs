using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class Platforms : MonoBehaviour {

    [SerializeField] private float _movementVelocity;
    [SerializeField] private float _firstHoleStartTime;
    [SerializeField] private float _holeMovementStart;

    private List<List<GameObject>> _allLevels;
    private List<List<int>> _holes;
    private bool _firstHole = false;

    private void Start() {
        Invoke("HoleCreation", _firstHoleStartTime);
        InvokeRepeating("HoleMovement", _holeMovementStart, _movementVelocity); 
    }

    private void Awake() {
        _allLevels = new List<List<GameObject>>();
        _holes = new List<List<int>>();
        foreach (Transform level in transform) {
            List<GameObject> levelPlatforms = new List<GameObject>();
            foreach (Transform platform in level) {
                levelPlatforms.Add(platform.gameObject);
            }
            _allLevels.Add(levelPlatforms);
        }
    }

    private void holeUpdate(List<int> hole) {
        _allLevels[hole[0]][hole[1]].GetComponent<Collider2D>().isTrigger = true;
        _allLevels[hole[0]][hole[1]].GetComponent<SpriteRenderer>().enabled = false;
        _allLevels[hole[2]][hole[3]].GetComponent<Collider2D>().isTrigger = true;
        _allLevels[hole[2]][hole[3]].GetComponent<SpriteRenderer>().enabled = false;
    }

    private void HoleCreation() {
        if (!_firstHole) {
            _firstHole = true;
            int orientation;
            int nPlataform = UnityEngine.Random.Range(1, _allLevels[0].Count - 1);
            if (nPlataform == 1) {
                orientation = 1;
            } else if (nPlataform == 18) {
                orientation = -1;
            } else {
                orientation = (UnityEngine.Random.Range(0, 2) * 2) - 1;
            }
            List<int> hole = new List<int>();
            hole.Add(0);
            hole.Add(nPlataform);
            hole.Add(0);
            hole.Add(nPlataform + orientation);
            hole.Add(orientation);
            hole.Add(0);
            _holes.Add(hole);
            holeUpdate(hole);
        }
        else {
            Debug.Log("NO");
        }
    }


    private void HoleMovement() {    
        foreach(List<int> hole in _holes) {
            _allLevels[hole[0]][hole[1]].GetComponent<Collider2D>().isTrigger = false;
            _allLevels[hole[0]][hole[1]].GetComponent<SpriteRenderer>().enabled = true;
            hole[0] = hole[2];
            hole[1] = hole[3];
            hole[3] = hole[1] + hole[4];
            if (hole[3] == 20) {
                hole[3] = 0;
                hole[2] = hole[2] + hole[4];
                if (hole[2] == 6) {
                    hole[2] = 0;
                }
            } else if (hole[3] == -1) {
                hole[3] = 19;
                hole[2] = hole[2] + hole[4];
                if (hole[2] == -1) {
                    hole[2] = 5;
                }
            } 
            holeUpdate(hole);
        }
    }
}
