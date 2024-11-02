using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount = 10f;  // Iloœæ obra¿eñ zadawanych przez pocisk

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzamy, czy trafiony obiekt ma interfejs IDamageable
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            // Zadajemy obra¿enia
            target.TakeDamage(damageAmount);
            Debug.Log($"Pocisk trafi³ {other.name}, zadano {damageAmount} obra¿eñ.");
        }

        // Zniszczenie pocisku po kolizji
        Destroy(gameObject);
    }
}