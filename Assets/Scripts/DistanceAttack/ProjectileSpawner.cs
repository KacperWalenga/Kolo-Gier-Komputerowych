using UnityEngine;
public class Spawner : MonoBehaviour
{
    private Projectile currentProjectile;
    public GameObject projectilePrefab; // prefab to set in inspector;
    
    [SerializeField]
    public float projectileSpawnDistance = 2f; 
    [SerializeField]
    public float projectileSpawnHeight = 1f;
    [SerializeField]
    public float projectileSpeed = 10f;
    [SerializeField]
    public float projectileScaleGrowthRate = 0.5f;
    [SerializeField]
    public float projectileScale = 0.2f;
    [SerializeField]
    public Stats playerStats;
    [SerializeField]
    public float distanceAttackManaCost = 10f;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // on lmb click
        {
            // check if we have enough mana to make an distance attack
            if (!playerStats.CheckIfPlayerHaveEnoughManaToShoot(distanceAttackManaCost, projectileScale)) return;
            
            // spawn projectile at given position
            Vector3 spawnPosition = transform.position + (transform.forward * projectileSpawnDistance) + (transform.up * projectileSpawnHeight);
            
            GameObject newProjectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
            currentProjectile = newProjectile.GetComponent<Projectile>();
            
            // assign values to projectile if it's exists
            if (currentProjectile != null)
            {
                currentProjectile.projectileSpeed = projectileSpeed;
            }
        }
        
        if (Input.GetMouseButton(0) && currentProjectile != null) // while holding lmb
        {
            
            projectileScale = currentProjectile.scale; 
            
            // we make bigger projectile until we have enough mana
            if (playerStats.CheckIfPlayerHaveEnoughManaToShoot(distanceAttackManaCost, projectileScale))
            {
                currentProjectile.IncreaseScaleOfProjectile(projectileScaleGrowthRate, Time.deltaTime);
            }
            
            // projectile position is updated according to player position
            currentProjectile.transform.position = transform.position + (transform.forward * projectileSpawnDistance) + (transform.up * projectileSpawnHeight);
        }
        
        if (Input.GetMouseButtonUp(0) && currentProjectile != null) // on lmb release
        {
            currentProjectile.LaunchProjectile(transform.forward); // distance attack launch
            playerStats.UsePlayerMana(distanceAttackManaCost, currentProjectile.scale); // player loses defined amount of mana
            playerStats.StartManaRegeneration();
            currentProjectile = null; // we set to null to make instance of next projectile
        }
    }

}