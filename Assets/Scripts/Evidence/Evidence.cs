using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EvidenceItem")]
public class Evidence : ScriptableObject
{
    string evidenceName;
    int itemId;
    public GameObject itemPrefab;

    public string getEvidenceName {  get { return evidenceName; } }
    public int getEvidenceId { get { return itemId; } }
    public GameObject getItemPrefab { get { return itemPrefab; } }

    


}
