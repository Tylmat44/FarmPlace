using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : TileableObjects
{
    private int cost;
    private TimeSpan growthTime;
    private TimeSpan growthTimeLeft;
    private int harvestQuantity;
    private string resource;


    public int Cost { get => cost; set => cost = value; }
    public TimeSpan GrowthTime { get => growthTime; set => growthTime = value; }
    public TimeSpan GrowthTimeLeft { get => growthTimeLeft; set => growthTimeLeft = value; }
    public int HarvestQuantity { get => harvestQuantity; set => harvestQuantity = value; }
    public string Resource { get => resource; set => resource = value; }

    public Crop(string key, string name, int cost, TimeSpan growthTime, int harvestQuantity, string resource, int harvestXp)
        : base (key, name, TileableObjectType.crop, new Dictionary<int, Vector2>(), harvestXp)
    {

        this.Cost = cost;
        this.GrowthTime = growthTime;
        this.GrowthTimeLeft = growthTime;
        this.HarvestQuantity = harvestQuantity;
        this.Resource = resource;
    }

    public override string ToString()
    {
        return growthTimeLeft.ToString(@"hh\:mm\:ss") + "?" + this.TileGroup[0].x + "?" + this.TileGroup[0].y;
    }

    public Crop Copy()
    {
        return new Crop(this.Key, this.Name, this.Cost, this.GrowthTime, this.HarvestQuantity, this.Resource, this.HarvestXp);
    }

    public void Parse(string data)
    {
        string[] dataList = data.Split('?');

        this.GrowthTimeLeft = TimeSpan.Parse(dataList[0]);
        this.TileGroup = new Dictionary<int, Vector2>();
        this.TileGroup.Add(0, new Vector2(float.Parse(dataList[1]), float.Parse(dataList[2])));
        this.TileGroup.Add(1, new Vector2(float.Parse(dataList[1]), float.Parse(dataList[2]) + 1));
        this.TileGroup.Add(2, new Vector2(float.Parse(dataList[1] + 1), float.Parse(dataList[2])));
        this.TileGroup.Add(3, new Vector2(float.Parse(dataList[1] + 1), float.Parse(dataList[2] + 1)));

        TimeSpan timeSinceLastLogin = DateTime.UtcNow - AccountManager.LastLogin;

        if (this.GrowthTimeLeft - timeSinceLastLogin <= TimeSpan.Zero)
        {
            this.GrowthTimeLeft = TimeSpan.Zero;
        } else
        {
            this.GrowthTimeLeft = TimeSpan.Zero;
            /*            this.GrowthTimeLeft = this.GrowthTimeLeft - timeSinceLastLogin;
            */
        }
    }
}
