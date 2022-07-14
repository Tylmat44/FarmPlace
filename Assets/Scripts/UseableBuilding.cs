using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UseableBuilding : TileableObjects
{
    private int cost;
    private int slots;
    private Vector2 size;
    private UseableBuildingType Buildingtype;
    private float rotation = 0;
    private Dictionary<int, CraftingStation> craftingStations;
    private List<CraftingType> craftingTypes;

    public int Cost { get => cost; set => cost = value; }
    public Vector2 Size { get => size; set => size = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public int Slots { get => slots; set => slots = value; }
    public UseableBuildingType BuildingType { get => Buildingtype; set => Buildingtype = value; }
    public Dictionary<int, CraftingStation> CraftingStations { get => craftingStations; set => craftingStations = value; }
    public List<CraftingType> CraftingTypes { get => craftingTypes; set => craftingTypes = value; }

    public UseableBuilding(string key, string name, int cost, UseableBuildingType buildingType, int slots, Vector2 size, List<CraftingType> craftingTypes)
        : base(key, name, TileableObjectType.useableBuilding, new Dictionary<int, Vector2>(), 0)
    {
        this.Cost = cost;
        this.BuildingType = buildingType;
        this.Slots = slots;
        this.Size = size;
        this.CraftingStations = new Dictionary<int, CraftingStation>();
        for (int i = 1; i <= this.Slots; i++)
        {
            this.CraftingStations[i] = DataManager.craftingStationDB["NONE"].Copy();
        }
        this.craftingTypes = craftingTypes;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(this.TileGroup[0].x + "?" + this.TileGroup[0].y + "?" + this.Rotation + "?" + this.CraftingStations.Count + "?");

        foreach (int key in this.CraftingStations.Keys)
        {
            builder.Append(this.CraftingStations[key].Key);

            if (this.CraftingStations[key].IsCrafting)
            {
                builder.Append("!true!");
                builder.Append(this.craftingStations[key].CraftableResource.Key);
                builder.Append("!");
                builder.Append(this.craftingStations[key].QuantityLeft);
                builder.Append("!");
                builder.Append(this.craftingStations[key].TimeLeft.TotalMinutes);

            } else
            {
                builder.Append("!false");
            }
        }

        return builder.ToString();
    }

    public UseableBuilding Copy()
    {
        return new UseableBuilding(this.Key, this.Name, this.Cost, this.BuildingType, this.Slots, this.Size, this.CraftingTypes);
    }

    public void Parse(string data)
    {
        string[] dataList = data.Split('?');

        if (this.Rotation == 0 || this.rotation == 180)
        {
            this.TileGroup = TileManager.map[int.Parse(dataList[0])][int.Parse(dataList[1])].calculateTileGroup((int)this.Size.x, (int)this.Size.y);
        }
        else
        {
            this.TileGroup = TileManager.map[int.Parse(dataList[0])][int.Parse(dataList[1])].calculateTileGroup((int)this.Size.y, (int)this.Size.x);
        }

        if(int.Parse(dataList[3]) > 0)
        {
            string[] craftingStationList = dataList[4].Split('!');
            for (int i = 0; i < int.Parse(dataList[3]); i++)
            {
                this.CraftingStations.Add(i, DataManager.craftingStationDB[craftingStationList[0]].Copy());

                if(craftingStationList[1] == "true")
                {
                    this.CraftingStations[i].IsCrafting = true;
                    this.CraftingStations[i].CraftableResource = DataManager.craftableResourceDB[craftingStationList[2]].Copy();
                    this.CraftingStations[i].QuantityLeft = int.Parse(craftingStationList[3]);
                    this.CraftingStations[i].TimeLeft = TimeSpan.FromMinutes(double.Parse(craftingStationList[4]));

                    double minutesSinceLastLogin = (DateTime.UtcNow - AccountManager.LastLogin).TotalMinutes;
                    int totalProduced = 0;

                    if (this.CraftingStations[i].TimeLeft.TotalMinutes - minutesSinceLastLogin <= 0)
                    {
                        totalProduced += 1;
                        minutesSinceLastLogin -= this.CraftingStations[i].TimeLeft.TotalMinutes;
                        totalProduced = Math.Min((int)(minutesSinceLastLogin / (this.CraftingStations[i].CraftableResource.CraftingTime.TotalMinutes / this.CraftingStations[i].SpeedMultiplier)), this.CraftingStations[i].QuantityLeft);

                        if (totalProduced < this.CraftingStations[i].QuantityLeft)
                        {
                            this.CraftingStations[i].QuantityLeft -= totalProduced;
                            this.CraftingStations[i].TimeLeft = TimeSpan.FromMinutes((this.CraftingStations[i].CraftableResource.CraftingTime.TotalMinutes / this.CraftingStations[i].SpeedMultiplier) - (((minutesSinceLastLogin / (this.CraftingStations[i].CraftableResource.CraftingTime.TotalMinutes / this.CraftingStations[i].SpeedMultiplier)) % 1) * (this.CraftingStations[i].CraftableResource.CraftingTime.TotalMinutes / this.CraftingStations[i].SpeedMultiplier)));
                        } else
                        {
                            this.CraftingStations[i].IsCrafting = false;
                        }

                        Inventory.addItem(DataManager.resourceDB[this.CraftingStations[i].CraftableResource.Key].Key, totalProduced * this.CraftingStations[i].CraftableResource.QuantityProduced);
                        AccountManager.Xp += this.CraftingStations[i].CraftableResource.Xp * totalProduced;

                    } else
                    {
                        this.CraftingStations[i].TimeLeft -= (DateTime.UtcNow - AccountManager.LastLogin);
                    }
                }
            }
        }

    }

    public void purchaseCraftingStation(CraftingStation craftingStation, int slot)
    {
        if(craftingStation.Cost > AccountManager.Coins)
        {
            return;
        } else
        {

            if (CraftingStationConfirmationBox.selection)
            {
                AccountManager.Coins -= craftingStation.Cost;
                this.CraftingStations[slot] = craftingStation;
            }
        }
    }
}
