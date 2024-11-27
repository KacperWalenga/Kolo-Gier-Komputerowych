using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject mainInventoryPanel;
    [SerializeField] private GameObject anvilInventoryPanel;
    private bool isMainInventoryOpen = false;
    private bool isAnvilInventoryOpen = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isMainInventoryOpen = !isMainInventoryOpen;
            mainInventoryPanel.SetActive(isMainInventoryOpen);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            isAnvilInventoryOpen = !isAnvilInventoryOpen;
            anvilInventoryPanel.SetActive(isAnvilInventoryOpen);
        }
          

    }
}
