using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{   

    [field: SerializeField] public static int TotalHealthPoints = 3;

    public static int HealthPoints = PlayerHealth.TotalHealthPoints;

    private void TakeHit(int damage = 1)
    {
        if(PlayerHealth.HealthPoints <= 0)
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
            TakeHit();
        }
    }
}