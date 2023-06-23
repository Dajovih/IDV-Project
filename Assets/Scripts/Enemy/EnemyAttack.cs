using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float _waitTime;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1,1,1);
            }
            _animator.SetBool("Attack", true);
            GameEvents.OnEnemyAttack?.Invoke(_waitTime);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _animator.SetBool("Attack", false);
        }

    }
}
