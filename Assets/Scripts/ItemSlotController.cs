using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotController : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public Evidence thisEvidence;


    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
    }

    public void FocusThis()
    {
        inventoryManager.SelectTo3DRender(this.gameObject);
    }
}
