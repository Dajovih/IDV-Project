using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int HealthPoints = GameManager.Instance.playerHealth;

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
            TakeHit();
        }
    }
}