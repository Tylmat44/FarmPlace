using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    private string key;
    private string name;
    private int sellPrice;
    private int buyPrice;
    private ResourceType type;
    private string source;

    public string Key { get => key; set => key = value; }
    public string Name { get => name; set => name = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    public int BuyPrice { get => buyPrice; set => buyPrice = value; }
    public ResourceType Type { get => type; set => type = value; }
    public string Source { get => source; set => source = value; }

    public Resource(string key, string name, int sellPrice, int buyPrice, ResourceType type, string source)
    {
        this.Key = key;
        this.Name = name;
        this.SellPrice = sellPrice;
        this.BuyPrice = buyPrice;
        this.Type = type;
        this.Source = source;
    }
}
