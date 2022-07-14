using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimalObject : MonoBehaviour
{
    private Animal animal;
    private Dictionary<int, Vector2> tileGroup;
    private bool isHover = false;

    public Animal Animal { get => animal; set => animal = value; }
    public Dictionary<int, Vector2> TileGroup { get => tileGroup; set => tileGroup = value; }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponentInChildren<Outline>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Animal.IsChild)
        {
            if (this.Animal.GrowthTimeLeft > TimeSpan.Zero)
            {
                this.Animal.GrowthTimeLeft -= TimeSpan.FromSeconds(Time.deltaTime);
            } else
            {
                this.growAnimal();
            }

            if (isHover)
            {
                HoverPopup._instance.setAndShowPopup(this.Animal.Name + "\n" + this.Animal.GrowthTimeLeft.ToString(@"hh\:mm\:ss"));
            }
        } else
        {
            if (this.Animal.HarvestTimeLeft > TimeSpan.Zero)
            {
                this.Animal.HarvestTimeLeft -= TimeSpan.FromSeconds(Time.deltaTime);
            }
            else
            {
                this.readyToHarvest();
            }

            if (isHover)
            {
                HoverPopup._instance.setAndShowPopup(this.Animal.Name + "\n" + this.Animal.HarvestTimeLeft.ToString(@"hh\:mm\:ss"));
            }
        }




    }

    void OnMouseEnter()
    {
        this.gameObject.GetComponentInChildren<Outline>().color = 2;
        this.gameObject.GetComponentInChildren<Outline>().enabled = true;
        if (this.Animal.IsChild)
        {
            HoverPopup._instance.setAndShowPopup(this.Animal.Name + "\n" + this.Animal.GrowthTimeLeft.ToString(@"hh\:mm\:ss"));
        } else
        {
            HoverPopup._instance.setAndShowPopup(this.Animal.Name + "\n" + this.Animal.HarvestTimeLeft.ToString(@"hh\:mm\:ss"));
        }
        isHover = true;

    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponentInChildren<Outline>().enabled = false;
        if (!this.Animal.IsChild)
        {
            if (this.Animal.HarvestTimeLeft <= TimeSpan.Zero)
            {
                this.readyToHarvest();
            }
        }
        isHover = false;
        HoverPopup._instance.hidePopup();
    }

    void OnMouseDown()
    {
        if (!this.Animal.IsChild)
            if (this.Animal.HarvestTimeLeft <= TimeSpan.Zero)
            {
                Inventory.addItem(this.Animal.Resource, this.Animal.HarvestQuantity);
        {
                AccountManager.Xp += this.Animal.HarvestXp;
                ActiveTileableObjects.activeTileableObjects.Remove(this.Animal);

                foreach (Vector2 tile in this.TileGroup.Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].hasObject = false;
                }
                HoverPopup._instance.hidePopup();
                Destroy(this.gameObject);

            }
        }
    }

    private void readyToHarvest()
    {
        this.gameObject.GetComponentInChildren<Outline>().color = 1;
        this.gameObject.GetComponentInChildren<Outline>().enabled = true;
    }

    private void growAnimal()
    {
        ActiveTileableObjects.activeTileableObjects.Remove(this.Animal);
        Animal newAnimal = DataManager.animalDB[this.Animal.Resource].Copy();
        newAnimal.Rotation = this.Animal.Rotation;
        newAnimal.TileGroup = this.Animal.TileGroup;
        foreach (Vector2 tile in this.TileGroup.Values)
        {
            TileManager.map[(int)tile.x][(int)tile.y].hasObject = false;
        }
        TileManager.map[(int)this.Animal.TileGroup[0].x][(int)this.Animal.TileGroup[0].y].addTileableObject(newAnimal);
        Destroy(this.gameObject);

    }
}
