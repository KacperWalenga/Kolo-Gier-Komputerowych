using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float scale = 0.2f; // Initial scale of the projectile
    public float maxScale = 1.5f; // Maximum scale
    public float damage = 10f; // Initial damage
    public float maxDamage = 30f; // Maximum damage
    public float projectileSpeed; // Speed of the projectile

    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Disable gravity initially
        }
    }

    public void IncreaseScaleOfProjectile(float scaleGrowthRate, float deltaTime)
    {
        scale += scaleGrowthRate * deltaTime;
        scale = Mathf.Min(scale, maxScale);

        damage += 0.1f * scale; // Increase damage proportionally
        damage = Mathf.Min(damage, maxDamage);

        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void LaunchProjectile(Vector3 direction)
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // destroy the projectile on any collision
        Destroy(gameObject);
    }
}