using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Stats : MonoBehaviour
{
    public float maxPlayerMana = 100f;
    public float currentPlayerMana;
    void Start()
    {
        currentPlayerMana = maxPlayerMana;
    }
    
    public bool CheckIfPlayerHaveEnoughManaToShoot(float projectileManaCost, float projectileScale)
    {
        return currentPlayerMana >= projectileManaCost * projectileScale;
    }
    public void UsePlayerMana(float projectileManaCost, float projectileScale)
    {   
        currentPlayerMana -= projectileManaCost * projectileScale;;
    }

    private Coroutine manaRegenerationCoroutine;
    private float manaRegenerationCooldown = 2f;
    private float manaRegenerationRate = 5f;

    public void StartManaRegeneration()
    {
        if (manaRegenerationCoroutine != null)
        {
            StopCoroutine(manaRegenerationCoroutine);
        }

        manaRegenerationCoroutine = StartCoroutine(RegeneratePlayerMana());
    }
    private IEnumerator RegeneratePlayerMana()
    {
        yield return new WaitForSeconds(manaRegenerationCooldown);

        while (currentPlayerMana < maxPlayerMana)
        {
            currentPlayerMana += manaRegenerationRate * Time.deltaTime;
            yield return null;
        }

        manaRegenerationCoroutine = null;
    }
}
