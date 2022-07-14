using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableResource
{
    private string key;
    private string name;
    private CraftingType type;
    private int quantityProduced;
    private TimeSpan craftingTime;
    private Dictionary<string, int> recipie;
    private int sellPrice;
    private int xp;
    private int level;

    public string Key { get => key; set => key = value; }
    public string Name { get => name; set => name = value; }
    public CraftingType Type { get => type; set => type = value; }
    public int QuantityProduced { get => quantityProduced; set => quantityProduced = value; }
    public TimeSpan CraftingTime { get => craftingTime; set => craftingTime = value; }
    public Dictionary<string, int> Recipie { get => recipie; set => recipie = value; }
    public int Xp { get => xp; set => xp = value; }
    public int Level { get => level; set => level = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }

    public CraftableResource(string key, string name, CraftingType type, int quantityProduced, TimeSpan craftingTime, Dictionary<string, int> recipie, int xp, int level)
    {
        this.Key = key;
        this.Name = name;
        this.Type = type;
        this.QuantityProduced = quantityProduced;
        this.CraftingTime = craftingTime;
        this.Recipie = recipie;
        this.Xp = xp;
        this.Level = level;
    }

    public CraftableResource Copy()
    {
        return new CraftableResource(this.Key, this.Name, this.Type, this.QuantityProduced, this.CraftingTime, this.Recipie, this.Xp, this.Level);
    }

    public static Dictionary<string, int> getRecipieFromString(string r)
    {
        Dictionary<string, int> recipieList = new Dictionary<string, int>();

        if (r != "")
        {
            string[] dataList = r.Split('?');

            foreach (string str in dataList)
            {
                string[] dList = str.Split('!');
                recipieList.Add(dList[0], int.Parse(dList[1]));
            }
        }

        return recipieList;
    }
}
