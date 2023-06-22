using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float _waitTime;
    [SerializeField] float _attackTimeDuration;
    [SerializeField] float _attackForce;
    [SerializeField] Transform _start;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameEvents.onAttack?.Invoke(_waitTime);
            _animator.SetBool("Attack", true);
            Force(_attackTimeDuration,collision.gameObject.GetComponent<Rigidbody2D>());
            //collision.gameObject.transform.position = _start.position;
             
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _animator.SetBool("Attack", false);
        }

    }

    private void Force(float seconds, Rigidbody2D body)
    {
        StartCoroutine(ForceCoroutine(seconds,body));
    }

    private IEnumerator ForceCoroutine(float seconds, Rigidbody2D body)
    {
        yield return new WaitForSeconds(seconds);
        Vector2 force = new Vector2(_attackForce, 0f);
        body.AddForce(force, ForceMode2D.Impulse);
    }
}
