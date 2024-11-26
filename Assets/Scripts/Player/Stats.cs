using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Stats : MonoBehaviour
{

    public float currentMana;
    public float maxMana = 100f;
    public bool regeneratingMana = false;

    public event Action<float, float> OnManaChanged;

    public void SetCurretMana(float mana)
    {
        currentMana = Mathf.Clamp(mana, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }


    void Start()
    {
        currentMana = maxMana;
    }
    

    public bool CheckMana(float projectileManaCost, float projectileScale)
    {
        return currentMana >= projectileManaCost * projectileScale;
    }
    public float UseMana(float projectileManaCost, float projectileScale)
    {   
        return currentMana -= projectileManaCost * projectileScale;;
    }

    private Coroutine manaRegenerationCoroutine;
    private float regenerationCooldown = 2f;
    private float regenerationRate = 5f;

    public void StartRegeneration()
    {
        if (manaRegenerationCoroutine != null)
        {
            StopCoroutine(manaRegenerationCoroutine);
        }

        manaRegenerationCoroutine = StartCoroutine(RegenerateMana());
    }

    private IEnumerator RegenerateMana()
    {
        yield return new WaitForSeconds(regenerationCooldown);

        while (currentMana < maxMana)
        {
            currentMana += regenerationRate * Time.deltaTime;
            yield return null;
        }

        manaRegenerationCoroutine = null;
    }
}
