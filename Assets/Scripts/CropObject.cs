using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CropObject : MonoBehaviour
{
    private Crop crop;
    private Dictionary<int, Vector2> tileGroup;
    private bool isHover = false;
    private double growthStep;
    private int step;

    public Crop Crop { get => crop; set => crop = value; }
    public Dictionary<int, Vector2> TileGroup { get => tileGroup; set => tileGroup = value; }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponentInChildren<Outline>().enabled = false;
        if (this.Crop.GrowthTimeLeft <= TimeSpan.Zero)
        {
            this.readyToHarvest();
            step = 10;
        } else
        {
            growthStep = this.Crop.GrowthTime.TotalMinutes / 10;
            step = 10;
            while ((this.Crop.GrowthTime.TotalMinutes - this.Crop.GrowthTimeLeft.TotalMinutes) < growthStep * step)
            {
                step -= 1;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (this.Crop.GrowthTimeLeft > TimeSpan.Zero)
        {
            this.Crop.GrowthTimeLeft -= TimeSpan.FromSeconds(Time.deltaTime);
            this.transform.localScale = new Vector3(10f, step, 10f);
            if (this.Crop.GrowthTime.TotalMinutes - this.Crop.GrowthTimeLeft.TotalMinutes > growthStep * step)
            {
                step += 1;
            }
        }

        if (isHover)
        {
            HoverPopup._instance.setAndShowPopup(this.Crop.Name + "\n" + this.Crop.GrowthTimeLeft.ToString(@"hh\:mm\:ss"));
        }


    }

    void OnMouseEnter()
    {
        this.gameObject.GetComponentInChildren<Outline>().color = 2;
        this.gameObject.GetComponentInChildren<Outline>().enabled = true;
        HoverPopup._instance.setAndShowPopup(this.Crop.Name + "\n" + this.Crop.GrowthTimeLeft.ToString(@"hh\:mm\:ss"));
        isHover = true;

    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponentInChildren<Outline>().enabled = false;
        if (this.Crop.GrowthTimeLeft <= TimeSpan.Zero)
        {
            this.readyToHarvest();
        }
        isHover = false;
        HoverPopup._instance.hidePopup();
    }

    void OnMouseDown()
    {
        if (this.Crop.GrowthTimeLeft <= TimeSpan.Zero)
        {
            Inventory.addItem(this.Crop.Resource, this.Crop.HarvestQuantity);
            AccountManager.Xp += this.Crop.HarvestXp;
            ActiveTileableObjects.activeTileableObjects.Remove(this.Crop);
            TileManager.map[(int)this.TileGroup[0].x][(int)this.TileGroup[0].y].hasObject = false;
            TileManager.map[(int)this.TileGroup[1].x][(int)this.TileGroup[1].y].hasObject = false;
            TileManager.map[(int)this.TileGroup[2].x][(int)this.TileGroup[2].y].hasObject = false;
            TileManager.map[(int)this.TileGroup[3].x][(int)this.TileGroup[3].y].hasObject = false;
            HoverPopup._instance.hidePopup();
            Destroy(this.gameObject);

        }
    }

    private void readyToHarvest()
    {
        this.gameObject.GetComponentInChildren<Outline>().color = 1;
        this.gameObject.GetComponentInChildren<Outline>().enabled = true;
    }
}
