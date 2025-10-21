
using UnityEngine;

public class AnimationDamage : MonoBehaviour
{
    public int damage = 25;
    public float radius = 1.5f;

    public void DoMeleeDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 0.8f, radius);
        foreach (var h in hits)
        {
            var eh = h.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(damage);
            }
        }
    }
}
