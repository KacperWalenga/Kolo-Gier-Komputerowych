using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttack : MonoBehaviour, IDamageable
{
    public GameObject Player;  // Gracz
    public GameObject projectilePrefab;  // Prefab pocisku
    public float detectionRange = 20f;  // Zasi�g detekcji
    public float fireRate = 1f;  // Czas mi�dzy kolejnymi rzutami (w sekundach)
    public float projectileSpeed = 10f;  // Pr�dko�� pocisk�w
    public Vector3 projectileOffset = new Vector3(0f, 1f, 0f); // Offset, je�li chcesz zmieni� miejsce, gdzie pojawia si� pocisk
    public float health = 250;

    private float nextFireTime = 0f; 

    void Update()
    {
        if (IsPlayerInRange())
        {
            if (Time.time >= nextFireTime)
            {
                FireProjectile();
                nextFireTime = Time.time + fireRate; 
            }
        }
    }

    bool IsPlayerInRange()
    {

        float distance = Vector3.Distance(transform.position, Player.transform.position);
        return distance <= detectionRange;
    }

    void FireProjectile()
    {
        Vector3 spawnPosition = transform.position + projectileOffset;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Vector3 direction = (Player.transform.position - spawnPosition).normalized;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;  
        }

        Destroy(projectile, 3f);  
    }

    void OnCollisionEnter(Collision collision)
     {
        // Sprawd�, czy pocisk trafi� w gracza
       if (collision.gameObject.CompareTag("Player")) 
       {
            // Mo�esz doda� tu inne efekty, np. zadanie obra�e� graczowi
        }
    }
    public void TakeDamage(float damangeamount)
    {
        health -= damangeamount;
        Debug.Log($"Wr�g otrzyma� {damangeamount} obra�e�. Pozosta�e zdrowie: {health}");

        if (health <= 0)
        {
            Debug.Log("Wr�g umar�!");
            Destroy(gameObject);
            // Dodaj logik� �mierci wroga
        }
    }

}