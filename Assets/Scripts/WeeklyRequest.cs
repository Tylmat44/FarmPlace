using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeeklyRequest
{
    private Resource resource;
    private int quantity;
    private int tier;
    private int coinReward;
    private int xpAward;
    private bool specialRequest;
    private Dictionary<Resource, int> specialRequests;

    public Resource Resource { get => resource; set => resource = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public int Tier { get => tier; set => tier = value; }
    public int CoinReward { get => coinReward; set => coinReward = value; }
    public int XpAward { get => xpAward; set => xpAward = value; }
    public bool SpecialRequest { get => specialRequest; set => specialRequest = value; }
    public Dictionary<Resource, int> SpecialRequests { get => specialRequests; set => specialRequests = value; }

    public WeeklyRequest(Resource resource, int quantity, int tier, int coinReward, int xpAward, bool specialRequest)
    {
        this.Resource = resource;
        this.Quantity = quantity;
        this.Tier = tier;
        this.CoinReward = coinReward;
        this.XpAward = xpAward;
        this.SpecialRequest = specialRequest;
    }

    public void completeRequest()
    {
        if (!this.SpecialRequest)
        {
            if (Inventory.getItemAmount(this.Resource.Key) >= this.Quantity)
            {
                AccountManager.Xp += this.XpAward;
                AccountManager.Coins += this.CoinReward;
                Inventory.removeItem(this.Resource.Key, this.Quantity);
            }
        } else
        {
            bool complete = true;

            foreach(Resource resource in this.SpecialRequests.Keys)
            {
                if (Inventory.getItemAmount(resource.Key) < this.SpecialRequests[resource])
                {
                    complete = false;
                }
            }

            if (complete)
            {
                AccountManager.Xp += this.XpAward;
                AccountManager.Coins += this.CoinReward;
                Inventory.removeItem(this.Resource.Key, this.Quantity);

                foreach (Resource resource in this.SpecialRequests.Keys)
                {
                    Inventory.removeItem(resource.Key, this.SpecialRequests[resource]);
                }
            }
        }
    }
}
