using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseableBuildingObject : MonoBehaviour
{
    private UseableBuilding useableBuilding;
    private Dictionary<int, Vector2> tileGroup;
    private bool isHover = false;

    public UseableBuilding UseableBuilding { get => useableBuilding; set => useableBuilding = value; }
    public Dictionary<int, Vector2> TileGroup { get => tileGroup; set => tileGroup = value; }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponentInChildren<Outline>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CraftingStation craftingStation in this.UseableBuilding.CraftingStations.Values)
        {
            if(craftingStation.IsCrafting)
            {
                if(craftingStation.TimeLeft > TimeSpan.Zero)
                {
                    craftingStation.TimeLeft -= TimeSpan.FromSeconds(Time.deltaTime);
                } else
                {
                    craftingStation.QuantityLeft -= 1;
                    Inventory.addItem(DataManager.resourceDB[craftingStation.CraftableResource.Key].Key, craftingStation.CraftableResource.QuantityProduced);
                    AccountManager.Xp += craftingStation.CraftableResource.Xp;

                    if(craftingStation.QuantityLeft <= 0)
                    {
                        craftingStation.IsCrafting = false;
                    } else
                    {
                        craftingStation.TimeLeft = TimeSpan.FromMinutes(craftingStation.CraftableResource.CraftingTime.TotalMinutes / craftingStation.SpeedMultiplier);
                    }
                }
            }
        }

        if (!AlwaysRunning.allowHover)
        {
            this.OnMouseExit();
        }

    }

    void OnMouseEnter()
    {
        if (AlwaysRunning.allowHover)
        {
            this.gameObject.GetComponentInChildren<Outline>().color = 2;
            this.gameObject.GetComponentInChildren<Outline>().enabled = true;
            isHover = true;
        }

    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponentInChildren<Outline>().enabled = false;
        isHover = false;
        HoverPopup._instance.hidePopup();
    }

    void OnMouseDown()
    {
        if (AlwaysRunning.allowHover)
        {
            BuildingMenuPopup._instance.setAndShowPopup(this.useableBuilding);
        }
    }

    private void readyToHarvest()
    {
        this.gameObject.GetComponentInChildren<Outline>().color = 1;
        this.gameObject.GetComponentInChildren<Outline>().enabled = true;
    }
}
