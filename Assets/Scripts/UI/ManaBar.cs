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
        currentMana = playerStats.currentPlayerMana;
        maxMana = playerStats.maxPlayerMana;
        manaBar = GetComponent<Image>();
    }
    void Update()
    {
        currentMana = playerStats.currentPlayerMana;
        maxMana = playerStats.maxPlayerMana;         
        manaBar.fillAmount = currentMana / maxMana;
    }

}
