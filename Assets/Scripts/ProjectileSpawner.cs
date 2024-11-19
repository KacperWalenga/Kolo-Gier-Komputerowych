using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign the projectile prefab in the Inspector
    public float spawnDistance;
    public float spawnHeight;
    public float projectileSpeed;
    public float scaleGrowthRate = 0.5f;

    private Projectile currentProjectile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPosition = transform.position + (transform.forward * spawnDistance) + (transform.up * spawnHeight);
            GameObject newProjectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
            currentProjectile = newProjectile.GetComponent<Projectile>();

            if (currentProjectile != null)
            {
                currentProjectile.projectileSpeed = projectileSpeed; // Set initial speed
            }
        }

        if (Input.GetMouseButton(0) && currentProjectile != null)
        {
            currentProjectile.IncreaseScale(scaleGrowthRate, Time.deltaTime);
            currentProjectile.transform.position = transform.position + (transform.forward * spawnDistance) + (transform.up * spawnHeight);
        }

        if (Input.GetMouseButtonUp(0) && currentProjectile != null)
        {
            currentProjectile.Launch(transform.forward);
            currentProjectile = null;
        }
    }
}