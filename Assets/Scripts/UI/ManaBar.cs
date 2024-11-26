using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField]
    private Stats playerStats;
    public float currentMana;
    public float maxMana;
    
    [SerializeField]
    public Image manaBar;
    void Start()
    {
        currentMana = playerStats.currentMana;
        maxMana = playerStats.maxMana;
        manaBar = GetComponent<Image>();
    }
    void Update()
    {
        currentMana = playerStats.currentMana;
        maxMana = playerStats.maxMana;         
        manaBar.fillAmount = currentMana / maxMana;
    }

}
