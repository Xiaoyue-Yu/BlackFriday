using System.Collections;
using UnityEngine;

// Just the data for customer not the customer gameobject
[System.Serializable]
public class Customer
{
    public string customerName;
    public string customerId;               // used for indexing the item score
    public ArrayList Dialogues = new ArrayList();   // adjust as you need
    public GameObject customerPrefab;
    public float budget;

    // add extra field as you need
}
