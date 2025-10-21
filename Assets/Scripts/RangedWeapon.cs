
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;

    public void Fire(Transform origin)
    {
        if (projectilePrefab == null) return;
        var proj = Instantiate(projectilePrefab, origin.position + origin.forward * 0.8f, origin.rotation);
        var rb = proj.GetComponent<Rigidbody>();
        if (rb == null) rb = proj.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearVelocity = origin.forward * projectileSpeed;
    }
}
