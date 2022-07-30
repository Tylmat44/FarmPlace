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

    public int startCrafting(CraftableResource craftableResource, int quantity)
    {
        int maxCraftable = Inventory.checkMaxCraftingAmount(craftableResource.Recipie);
        int makeQuantity = 0;

        if(maxCraftable <= 0 || craftableResource.Type != this.Type)
        {
            return 0;
        } else
        {
            if(maxCraftable < quantity)
            {
                makeQuantity = maxCraftable;
            } else
            {
                makeQuantity = quantity;
            }

            foreach(string key in craftableResource.Recipie.Keys)
            {
                Inventory.removeItem(key, craftableResource.Recipie[key] * makeQuantity);
            }

            this.IsCrafting = true;
            this.CraftableResource = craftableResource;
            this.QuantityLeft = makeQuantity;
            this.TimeLeft = TimeSpan.FromMinutes(this.CraftableResource.CraftingTime.TotalMinutes / this.SpeedMultiplier);
            return makeQuantity;
        }
    }

    public void refundResources(int quantity)
    {
        int refund = 0;
        if(!this.IsCrafting)
        {
            return;
        }

        if (quantity >= this.QuantityLeft)
        {
            refund = this.QuantityLeft;
        } else
        {
            refund = quantity;
            this.QuantityLeft -= refund;
        }

        foreach(string key in this.CraftableResource.Recipie.Keys)
        {
            Inventory.addItem(key, this.CraftableResource.Recipie[key] * refund);
        }

        if (quantity >= this.QuantityLeft)
        {
            this.QuantityLeft = 0;
            this.endCrafting();
        }



    }

    public void increaseQuantity(int quantity)
    {
        if (this.IsCrafting)
        {
            int maxCraftable = Inventory.checkMaxCraftingAmount(this.CraftableResource.Recipie);
            int makeQuantity = 0;

            if (maxCraftable <= 0)
            {
                return;
            }
            else
            {
                if (maxCraftable < quantity)
                {
                    makeQuantity = maxCraftable;
                }
                else
                {
                    makeQuantity = quantity;
                }

            }

            foreach (string key in craftableResource.Recipie.Keys)
            {
                Inventory.removeItem(key, craftableResource.Recipie[key] * makeQuantity);
            }

            this.QuantityLeft += makeQuantity;
        }
    }

    public void endCrafting()
    {
        this.IsCrafting = false;
        this.CraftableResource = null;
    }
}
