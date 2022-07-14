using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation
{
    private string key;
    private string name;
    private CraftingType type;
    private CraftableResource craftableResource;
    private bool isCrafting;
    private int quantityLeft;
    private TimeSpan timeLeft;
    private float speedMultiplier;
    private int cost;

    public string Key { get => key; set => key = value; }
    public string Name { get => name; set => name = value; }
    public CraftingType Type { get => type; set => type = value; }
    public CraftableResource CraftableResource { get => craftableResource; set => craftableResource = value; }
    public bool IsCrafting { get => isCrafting; set => isCrafting = value; }
    public int QuantityLeft { get => quantityLeft; set => quantityLeft = value; }
    public TimeSpan TimeLeft { get => timeLeft; set => timeLeft = value; }
    public float SpeedMultiplier { get => speedMultiplier; set => speedMultiplier = value; }
    public int Cost { get => cost; set => cost = value; }

    public CraftingStation(string key, string name, CraftingType type, float speedMultiplier, int cost)
    {
        this.Key = key;
        this.Name = name;
        this.Type = type;
        this.IsCrafting = false;
        this.QuantityLeft = 0;
        this.SpeedMultiplier = speedMultiplier;
        this.Cost = cost;
    }

    public CraftingStation Copy()
    {
        return new CraftingStation(this.Key, this.Name, this.Type, this.SpeedMultiplier, this.Cost);
    }

    public bool startCrafting(CraftableResource craftableResource, int quantity)
    {
        int maxCraftable = Inventory.checkMaxCraftingAmount(craftableResource.Recipie);

        if(maxCraftable <= 0 || maxCraftable < quantity || craftableResource.Type != this.Type)
        {
            return false;
        } else
        {
            this.IsCrafting = true;
            this.CraftableResource = craftableResource;
            this.QuantityLeft = quantity;
            this.TimeLeft = TimeSpan.FromMinutes(this.CraftableResource.CraftingTime.TotalMinutes / this.SpeedMultiplier);
            return true;
        }
    }
}
