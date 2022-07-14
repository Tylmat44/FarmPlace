using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : TileableObjects
{
    private int cost;
    bool isChild;
    private TimeSpan growthTime;
    private TimeSpan growthTimeLeft;
    private TimeSpan harvestTime;
    private TimeSpan harvestTimeLeft;
    private int baseSellPrice;
    private int harvestQuantity;
    private string resource;
    private Vector2 size;
    private float rotation = 0;

    public int Cost { get => cost; set => cost = value; }
    public bool IsChild { get => isChild; set => isChild = value; }
    public TimeSpan GrowthTime { get => growthTime; set => growthTime = value; }
    public TimeSpan GrowthTimeLeft { get => growthTimeLeft; set => growthTimeLeft = value; }
    public TimeSpan HarvestTime { get => harvestTime; set => harvestTime = value; }
    public TimeSpan HarvestTimeLeft { get => harvestTimeLeft; set => harvestTimeLeft = value; }
    public int BaseSellPrice { get => baseSellPrice; set => baseSellPrice = value; }
    public int HarvestQuantity { get => harvestQuantity; set => harvestQuantity = value; }
    public string Resource { get => resource; set => resource = value; }
    public Vector2 Size { get => size; set => size = value; }
    public float Rotation { get => rotation; set => rotation = value; }

    public Animal(string key, string name, int cost, TimeSpan growthTime, TimeSpan harvestTime, int baseSellPrice, int harvestQuantity, string resource, int harvestXp, Vector2 size)
        : base (key, name, TileableObjectType.animal, new Dictionary<int,Vector2>(), harvestXp)
    {
        this.Cost = cost;
        this.GrowthTime = growthTime;
        if (growthTime.TotalMinutes > 0)
        {
            this.GrowthTimeLeft = growthTime;
            this.IsChild = true;
        } else
        {
            this.GrowthTimeLeft = TimeSpan.Zero;
            this.IsChild = false;
        }

        this.HarvestTime = harvestTime;
        this.HarvestTimeLeft = harvestTime;
        this.BaseSellPrice = baseSellPrice;
        this.HarvestQuantity = harvestQuantity;
        this.Resource = resource;
        this.Size = size;
    }

    public override string ToString()
    {
        int tempIsChild = 0;
        if (this.IsChild)
        {
            tempIsChild = 1;
        }
        else
        {
            tempIsChild = 0;
        }

        return tempIsChild + "?" + this.GrowthTimeLeft.ToString() + "?" + this.HarvestTimeLeft.ToString() + "?" + this.TileGroup[0].x + "?" + this.TileGroup[0].y + "?" + this.Rotation;
    }

    public Animal Copy()
    {
        return new Animal(this.Key, this.Name, this.Cost, this.GrowthTime, this.HarvestTime, this.BaseSellPrice, this.harvestQuantity, this.Resource, this.HarvestXp, this.Size);
    }

    public void Parse(string data)
    {
        string[] dataList = data.Split('?');

        if (int.Parse(dataList[0]) == 1)
        {
            this.IsChild = true;
        } else
        {
            this.IsChild = false;
        }

        this.GrowthTimeLeft = TimeSpan.Parse(dataList[1]);
        this.HarvestTimeLeft = TimeSpan.Parse(dataList[2]);

        this.Rotation = float.Parse(dataList[5]);

        Dictionary<int, Vector2> tempGroup = new Dictionary<int, Vector2>();

        if (this.Rotation == 0 || this.rotation == 180)
        {
            this.TileGroup = TileManager.map[int.Parse(dataList[3])][int.Parse(dataList[4])].calculateTileGroup((int)this.Size.x, (int)this.Size.y);
        } else
        {
            this.TileGroup = TileManager.map[int.Parse(dataList[3])][int.Parse(dataList[4])].calculateTileGroup((int)this.Size.y, (int)this.Size.x);
        }

        TimeSpan timeSinceLastLogin = DateTime.UtcNow - AccountManager.LastLogin;

        if (this.IsChild)
        {
            if (this.GrowthTimeLeft - timeSinceLastLogin <= TimeSpan.Zero)
            {
                this.GrowthTimeLeft = TimeSpan.Zero;
            }
            else
            {
                this.GrowthTimeLeft = TimeSpan.Zero;
                /*            this.GrowthTimeLeft = this.GrowthTimeLeft - timeSinceLastLogin;
                */
            }
        } else
        {
            if (this.HarvestTimeLeft - timeSinceLastLogin <= TimeSpan.Zero)
            {
                this.HarvestTimeLeft = TimeSpan.Zero;
            }
            else
            {
                this.HarvestTimeLeft = TimeSpan.Zero;
                /*            this.HarvestTimeLeft = this.HarvestTimeLeft - timeSinceLastLogin;
                */
            }
        }
    }
}
