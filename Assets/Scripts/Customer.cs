using System.Collections;
using UnityEngine;
using System.Collections.Generic;

// Just the data for customer not the customer gameobject
[System.Serializable]
public class Customer
{
    public string customerName;
    public string customerId;               // used for indexing the item score
    // public ArrayList Dialogues = new ArrayList();   // adjust as you need
    public List<string> dialogues = new List<string>(); //using ink
    public GameObject customerPrefab;
    public float budget;

    // add extra field as you need
}
