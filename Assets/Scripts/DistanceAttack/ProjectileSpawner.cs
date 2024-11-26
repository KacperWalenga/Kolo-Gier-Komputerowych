using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnDistance;
    public float spawnHeight;
    public float projectileSpeed;
    public float scaleGrowthRate = 0.5f;
    [SerializeField]
    public Stats playerStats;
    public float manaCost;

    private Projectile currentProjectile;
    [SerializeField]
    private float projectileScale;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!playerStats.CheckMana(manaCost, projectileScale)) return;
            Vector3 spawnPosition = transform.position + (transform.forward * spawnDistance) + (transform.up * spawnHeight);
            GameObject newProjectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
            currentProjectile = newProjectile.GetComponent<Projectile>();

            if (currentProjectile != null)
            {
                currentProjectile.projectileSpeed = projectileSpeed;
            }
        }
        
        if (Input.GetMouseButton(0) && currentProjectile != null)
        {
            projectileScale = currentProjectile.scale;
            playerStats.regeneratingMana = false;

           
            if (playerStats.CheckMana(manaCost, projectileScale))
            {
                currentProjectile.IncreaseScale(scaleGrowthRate, Time.deltaTime);
            }
            currentProjectile.transform.position = transform.position + (transform.forward * spawnDistance) + (transform.up * spawnHeight);
        }
        
        if (Input.GetMouseButtonUp(0) && currentProjectile != null)
        {
            currentProjectile.Launch(transform.forward);
            playerStats.UseMana(manaCost, currentProjectile.scale);
            playerStats.StartRegeneration();
            currentProjectile = null;
        }
    }

}