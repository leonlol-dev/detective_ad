using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<Evidence> EvidenceList = new List<Evidence>();

    public GameObject ui;
    public Transform itemContent;
    public GameObject inventoryItem;
    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(ui.active)
            {
                ui.SetActive(false);
            }
            else
            {
                ui.SetActive(true);
                ListItems();
            }
        }
    }

    public void AddEvidence(Evidence _evidence)
    {
        EvidenceList.Add(_evidence);
    }

    public void RemoveEvidence(Evidence _evidence)
    {
        EvidenceList.Remove(_evidence);
    }

    public void ListItems()
    {
        //Clean up the inventory so clones don't happen.
        foreach(Transform evidence in itemContent)
        {
            Destroy(evidence.gameObject);
        }

        //Instantiate the items within the inventory.
        foreach(var evidence in EvidenceList)
        {
            GameObject obj = Instantiate(inventoryItem, itemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemImage").GetComponent<Image>();

            itemName.text = evidence.evidenceName;
            itemIcon.sprite = evidence.icon;
        }
    }

}
