using UnityEngine;

public class ScreenWrap : MonoBehaviour {
    [SerializeField] private Collider2D _inCamera;
    [SerializeField] private Collider2D _leftPortal;
    [SerializeField] private Collider2D _rightPortal;
    [SerializeField] private Transform _leftPosition;
    [SerializeField] private Transform _rightPosition;
    [SerializeField] private GameObject _clone;
    private Transform _player;
    private bool _view;
    private Renderer _render;
    SpriteRenderer playerSprite; 
    void Start() {
        _player = GetComponent<Transform>();
        _view = true;
        _render = GetComponent<Renderer>();
        playerSprite = GetComponentInParent<SpriteRenderer>();
    }
    
    void Update()
    {
     
    }
    void OnBecameInvisible()
    {
       
    }

    void OnTriggerEnter2D(Collider2D col) {
        Vector3 nPosition = new Vector3(0, _player.position.y, 0);
        if (col == _inCamera)
        {
            Debug.Log("Dentro");
        }
        if (col == _leftPortal) {
            Debug.Log(col.gameObject.name);
            Debug.Log("Left");
            _clone.SetActive(true);
            nPosition.x=_rightPosition.position.x;
            _clone.transform.position = nPosition;
        } else if (col == _rightPortal) {
            Debug.Log(col.gameObject.name);
            Debug.Log("Right");
            _clone.SetActive(true);
            nPosition.x = _leftPosition.position.x;
            _clone.transform.position = nPosition;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col == _rightPortal)  {
            Debug.Log("Exit Right");
        
        } else if (col == _leftPortal)
        {
            Debug.Log("Exit Left");
        }
        if (col == _inCamera)
        {
            Debug.Log("Sale de cámara");
        }
    }
}