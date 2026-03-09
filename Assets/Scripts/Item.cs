using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerScoreEntry
{
    public string customerId;
    public int score;
}

[System.Serializable]
public class Item
{
    public string itemName;
    public float price;
    public int quantityInStock;
    public GameObject itemPrefab;
    public Sprite itemSprite;

    [SerializeField] private List<CustomerScoreEntry> scoreEntries = new();

    private Dictionary<string, int> scoreByCustomerId;

    private void BuildScoreLookup()
    {
        if (scoreByCustomerId != null) return;

        scoreByCustomerId = new Dictionary<string, int>();

        foreach (var entry in scoreEntries)
        {
            if (string.IsNullOrWhiteSpace(entry.customerId))
                continue;

            scoreByCustomerId[entry.customerId] = entry.score;
        }
    }

    public int GetScoreForCustomer(string customerId)
    {
        BuildScoreLookup();

        if (string.IsNullOrEmpty(customerId))
            return 0;

        return scoreByCustomerId.TryGetValue(customerId, out int score) ? score : 0;
    }

    public ItemValue ToItemValue(string customerId)
    {
        return new ItemValue(
            itemName,
            price,
            itemSprite,
            GetScoreForCustomer(customerId)
        );
    }
}
public class ItemValue
{
    public string ItemName { get; }
    public float Price { get; }
    public Sprite ItemSprite { get; }
    public int CustomerScore { get; }

    public ItemValue(string itemName, float price, Sprite itemSprite, int customerScore)
    {
        ItemName = itemName;
        Price = price;
        ItemSprite = itemSprite;
        CustomerScore = customerScore;
    }
}