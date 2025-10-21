
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 25;
    public float life = 5f;
    public AudioClip impactSound;

    void Start()
    {
        Destroy(gameObject, life);
    }

    void OnTriggerEnter(Collider other)
    {
        var eh = other.GetComponent<EnemyHealth>();
        if (eh != null)
        {
            eh.TakeDamage(damage);
            if (impactSound != null) AudioSource.PlayClipAtPoint(impactSound, transform.position);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
