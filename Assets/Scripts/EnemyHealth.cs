
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 60;
    int currentHealth;
    public AudioClip hitSound;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        var animator = GetComponent<Animator>();
        if (animator != null) animator.SetTrigger("Die");
        var agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        Destroy(gameObject, 3f);
    }
}
