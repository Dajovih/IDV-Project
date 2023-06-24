using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{   
    [SerializeField] private int _deductedPoints = 1000;
    [SerializeField] float _wait;
    [SerializeField] Transform _start;

    public static int HealthPoints;
    private Animator _animator;
    private float _time = 0f;
    private bool _isTeleporting = false;
    private bool _hit = false;

    private void Start()
    {   
        PlayerHealth.HealthPoints = GameManager.Instance.playerHealth;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _wait && _isTeleporting)
        {
            transform.position = _start.position;
            _isTeleporting = false;
            if (_hit)
            {
                TakeHit();
                _hit = false;
            }
            
        }

    }

    private void TakeHit(int damage = 1)
    {
        if (PlayerHealth.HealthPoints <= 0)
            return;
    
        PlayerHealth.HealthPoints -= damage;
        OnTakeDamage();  

        if (PlayerHealth.HealthPoints <= 0)
        {

            OnDeath();
        }
    }

    private void OnTakeDamage()
    {
        GameEvents.OnPlayerHealthChangeEvent?.Invoke(PlayerHealth.HealthPoints);
        GameEvents.OnPointsChangeEvent?.Invoke(-(_deductedPoints));
        GameManager.Instance.playerHealth = PlayerHealth.HealthPoints;
    }

    private void OnDeath()
    {   
        AudioManager.Instance.PlaySound2D("DeathSFX");

        GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in objectsToDisable) { 
    	    obj.SetActive(false);  
        }

        GameManager.Instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {   
            Debug.Log("Enemy Colission!");
            AudioManager.Instance.PlaySound2D("AttackSFX");
            _hit = true;
            _animator.SetBool("InAttack", true);
            _time = 0;
            _isTeleporting = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _animator.SetBool("InAttack", false);
        }
    }
}