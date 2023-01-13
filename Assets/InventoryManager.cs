using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class InventoryManager : MonoBehaviour, IDragHandler
{
    //Inventory Instance
    public static InventoryManager Instance { get; private set; }
    public List<Evidence> EvidenceList = new List<Evidence>();


    public GameObject ui, itemUi;
    public Transform itemContent;
    public GameObject itemSlot;
    public GameObject inventory3DRenderSelect { get; set; }

    
    private ItemInspector itemInspector;
    private void Awake()
    {
        Instance = this;
        itemInspector = itemUi.GetComponent<ItemInspector>();
    }

    public void Update()
    {
        //ListItems();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(ui.active)
            {
                ui.SetActive(false);
                itemUi.SetActive(false);
            }
            else
            {
                ui.SetActive(true);
                itemUi.SetActive(true) ;
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
            itemSlot.GetComponent<ItemSlotController>().thisEvidence = evidence;
            GameObject obj = Instantiate(itemSlot, itemContent);
            
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemImage").GetComponent<Image>();

            

            itemName.text = evidence.evidenceName;
            itemIcon.sprite = evidence.icon;
        }
    }

    public void SelectTo3DRender(GameObject obj)
    {
        inventory3DRenderSelect = obj;
        Evidence currentEvidence = inventory3DRenderSelect.GetComponent<ItemSlotController>().thisEvidence;
        itemInspector.RenderToInspector(currentEvidence);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
