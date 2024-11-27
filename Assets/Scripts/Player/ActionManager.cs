using System;
using UnityEngine;
public class ActionManager : MonoBehaviour
{
    public DistanceAttack distanceAttack;

    public void Start()
    {
        distanceAttack = FindFirstObjectByType<DistanceAttack>();

        if (distanceAttack == null)
        {
            Debug.LogError("DistanceAttack component not found");
        }
    }

    private void Update()
    {
        // Distance attack
        if (Input.GetKeyDown(KeyCode.Alpha1) && !distanceAttack.isActive)
        {
            distanceAttack?.Activate();
        }

        // Melee attack 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // meleeAttack.PerformMeleeAttack();
            distanceAttack?.Deactivate();
        }
    }
}