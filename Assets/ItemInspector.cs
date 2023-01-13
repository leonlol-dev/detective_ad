using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInspector : MonoBehaviour, IDragHandler
{
    [SerializeField] private GameObject ui;

    private InventoryManager inventoryManager;
    private GameObject itemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
    }


    public void RenderToInspector(Evidence evidenceItem)
    {
        if (itemPrefab != null)
        {
            Destroy(itemPrefab.gameObject);
        }

        itemPrefab = Instantiate(evidenceItem.itemPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemPrefab.transform.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x);
    }

}
