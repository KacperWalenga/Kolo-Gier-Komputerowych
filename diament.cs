using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttack : MonoBehaviour, IDamageable
{
    public GameObject Player;  // Gracz
    public GameObject projectilePrefab;  // Prefab pocisku
    public float detectionRange = 20f;  // Zasiêg detekcji
    public float fireRate = 1f;  // Czas miêdzy kolejnymi rzutami (w sekundach)
    public float projectileSpeed = 10f;  // Prêdkoœæ pocisków
    public Vector3 projectileOffset = new Vector3(0f, 1f, 0f); // Offset, jeœli chcesz zmieniæ miejsce, gdzie pojawia siê pocisk
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
        // SprawdŸ, czy pocisk trafi³ w gracza
       if (collision.gameObject.CompareTag("Player")) 
       {
            // Mo¿esz dodaæ tu inne efekty, np. zadanie obra¿eñ graczowi
        }
    }
    public void TakeDamage(float damangeamount)
    {
        health -= damangeamount;
        Debug.Log($"Wróg otrzyma³ {damangeamount} obra¿eñ. Pozosta³e zdrowie: {health}");

        if (health <= 0)
        {
            Debug.Log("Wróg umar³!");
            Destroy(gameObject);
            // Dodaj logikê œmierci wroga
        }
    }

}