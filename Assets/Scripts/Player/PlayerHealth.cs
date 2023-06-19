using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator _anim;
    
    void Start()
    {
        InitHealth();
    }

    protected override void OnTakeDamage()
    {
        base.OnTakeDamage();
        GameEvents.OnPlayerHealthChangeEvent?.Invoke(HealthPoints);
    }

    protected override void OnDeath()
    {
        base.OnDeath();    
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {   
            Debug.Log("Enemy Colission!");
            TakeHit();
        }
    }
}