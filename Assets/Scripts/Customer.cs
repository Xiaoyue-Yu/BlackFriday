using System.Collections;
using UnityEngine;

[System.Serializable]
public class Customer
{
    public string customerName;
    public string customerId;
    public ArrayList Dialogues = new ArrayList();
    public GameObject customerPrefab;
}
