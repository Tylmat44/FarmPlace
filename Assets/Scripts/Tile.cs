using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public Vector2 gridPosition = Vector2.zero;
    public bool hasObject;
    private bool isPlowed;
    public TileManager tileManager;
    public Color color = new Color(134f / 255f, 229f / 255f, 25f / 255f);

    public bool IsPlowed { get => isPlowed; set => isPlowed = value; }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (ToolChest.PlowSelected)
        {
            tileManager.plowTiles(this.calculateTileGroup(2, 2));
        }

        if (ToolChest.ClearSelected)
        {
            tileManager.clearTiles(this.calculateTileGroup(2, 2));
        }

        if (ToolChest.SeedSelected != null)
        {
            if (TileManager.map[(int)this.calculateTileGroup(2, 2)[0].x][(int)this.calculateTileGroup(2, 2)[0].y].isPlowed && !TileManager.map[(int)this.calculateTileGroup(2, 2)[0].x][(int)this.calculateTileGroup(2, 2)[0].y].hasObject)
            {
                if (AccountManager.Coins >= DataManager.resourceDB[ToolChest.SeedSelected.Key].BuyPrice)
                {
                    addTileableObject(DataManager.cropDB[ToolChest.SeedSelected.Source].Copy());
                    AccountManager.Coins -= DataManager.resourceDB[ToolChest.SeedSelected.Key].BuyPrice;
                }
            }

        }

        if (ToolChest.AnimalSelected != null)
        {
            int x;
            int y;
            if (ToolChest.AnimalSelected.Rotation == 0 || ToolChest.AnimalSelected.Rotation == 180)
            {
                x = (int)ToolChest.AnimalSelected.Size.x;
                y = (int)ToolChest.AnimalSelected.Size.y;
            } else
            {
                x = (int)ToolChest.AnimalSelected.Size.y;
                y = (int)ToolChest.AnimalSelected.Size.x;
            }

            Dictionary<int, Vector2> group = this.calculateTileGroup(x, y);
            bool isOccupied = false;

            foreach (Vector2 tile in group.Values)
            {
                if (TileManager.map[(int)tile.x][(int)tile.y].hasObject)
                {
                    isOccupied = true;
                }
            }
            if (!isOccupied)
            {
                if (AccountManager.Coins >= DataManager.animalDB[ToolChest.AnimalSelected.Key].Cost)
                {
                    addTileableObject(DataManager.animalDB[ToolChest.AnimalSelected.Key].Copy());
                    AccountManager.Coins -= DataManager.animalDB[ToolChest.AnimalSelected.Key].Cost;
                }
            }

        }

        if (ToolChest.UseableBuildingSelected != null)
        {
            int x;
            int y;
            if (ToolChest.UseableBuildingSelected.Rotation == 0 || ToolChest.UseableBuildingSelected.Rotation == 180)
            {
                x = (int)ToolChest.UseableBuildingSelected.Size.x;
                y = (int)ToolChest.UseableBuildingSelected.Size.y;
            }
            else
            {
                x = (int)ToolChest.UseableBuildingSelected.Size.y;
                y = (int)ToolChest.UseableBuildingSelected.Size.x;
            }

            Dictionary<int, Vector2> group = this.calculateTileGroup(x, y);
            bool isOccupied = false;

            foreach (Vector2 tile in group.Values)
            {
                if (TileManager.map[(int)tile.x][(int)tile.y].hasObject)
                {
                    isOccupied = true;
                }
            }
            if (!isOccupied)
            {
                if (AccountManager.Coins >= DataManager.useableBuildingDB[ToolChest.UseableBuildingSelected.Key].Cost)
                {
                    addTileableObject(DataManager.useableBuildingDB[ToolChest.UseableBuildingSelected.Key].Copy());
                    AccountManager.Coins -= DataManager.useableBuildingDB[ToolChest.UseableBuildingSelected.Key].Cost;
                }
            }

        }

        if (ToolChest.DecorSelected != null)
        {
            int x;
            int y;
            if (ToolChest.DecorSelected.Rotation == 0 || ToolChest.DecorSelected.Rotation == 180)
            {
                x = (int)ToolChest.DecorSelected.Size.x;
                y = (int)ToolChest.DecorSelected.Size.y;
            }
            else
            {
                x = (int)ToolChest.DecorSelected.Size.y;
                y = (int)ToolChest.DecorSelected.Size.x;
            }

            Dictionary<int, Vector2> group = this.calculateTileGroup(x, y);
            bool isOccupied = false;

            foreach (Vector2 tile in group.Values)
            {
                if (TileManager.map[(int)tile.x][(int)tile.y].hasObject)
                {
                    isOccupied = true;
                }
            }
            if (!isOccupied)
            {
                if (AccountManager.Coins >= DataManager.decorDB[ToolChest.DecorSelected.Key].Cost)
                {
                    addTileableObject(DataManager.decorDB[ToolChest.DecorSelected.Key].Copy());
                    AccountManager.Coins -= DataManager.decorDB[ToolChest.DecorSelected.Key].Cost;
                }
            }

        }

    }

    void OnMouseEnter()
    {
        if (ToolChest.PlowSelected || ToolChest.ClearSelected || ToolChest.SeedSelected != null || ToolChest.AnimalSelected != null || ToolChest.UseableBuildingSelected != null || ToolChest.DecorSelected != null)
        {
            if (ToolChest.AnimalSelected != null)
            {
                foreach (Vector2 tile in this.calculateTileGroup((int)ToolChest.AnimalSelected.Size.x, (int)ToolChest.AnimalSelected.Size.y).Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].gameObject.GetComponent<Outline>().enabled = true;
                }
            } else if (ToolChest.UseableBuildingSelected != null)
            {
                foreach (Vector2 tile in this.calculateTileGroup((int)ToolChest.UseableBuildingSelected.Size.x, (int)ToolChest.UseableBuildingSelected.Size.y).Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].gameObject.GetComponent<Outline>().enabled = true;
                }
            }
            else if (ToolChest.DecorSelected != null)
            {
                foreach (Vector2 tile in this.calculateTileGroup((int)ToolChest.DecorSelected.Size.x, (int)ToolChest.DecorSelected.Size.y).Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].gameObject.GetComponent<Outline>().enabled = true;
                }
            }
            else
            {
                foreach (Vector2 tile in this.calculateTileGroup(2, 2).Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].gameObject.GetComponent<Outline>().enabled = true;
                }
            }
        } else
        {

        }
    }

    void OnMouseExit()

    {
        if (ToolChest.PlowSelected || ToolChest.ClearSelected || ToolChest.SeedSelected != null || ToolChest.AnimalSelected != null || ToolChest.UseableBuildingSelected != null || ToolChest.DecorSelected != null)
        {
            if (ToolChest.AnimalSelected != null)
            {
                foreach (Vector2 tile in this.calculateTileGroup((int)ToolChest.AnimalSelected.Size.x, (int)ToolChest.AnimalSelected.Size.y).Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].gameObject.GetComponent<Outline>().enabled = false;
                }
            }
            else if (ToolChest.UseableBuildingSelected != null)
            {
                foreach (Vector2 tile in this.calculateTileGroup((int)ToolChest.UseableBuildingSelected.Size.x, (int)ToolChest.UseableBuildingSelected.Size.y).Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].gameObject.GetComponent<Outline>().enabled = false;
                }
            }
            else if (ToolChest.DecorSelected != null)
            {
                foreach (Vector2 tile in this.calculateTileGroup((int)ToolChest.DecorSelected.Size.x, (int)ToolChest.DecorSelected.Size.y).Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].gameObject.GetComponent<Outline>().enabled = false;
                }
            }
            else
            {
                foreach (Vector2 tile in this.calculateTileGroup(2, 2).Values)
                {
                    TileManager.map[(int)tile.x][(int)tile.y].gameObject.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }

    public void addTileableObject(TileableObjects addObject)
    {
        Dictionary<int, Vector2> group = null;

        if (addObject.Type == TileableObjectType.crop)
        {
            group = this.calculateTileGroup(2, 2); ;
        } else if (addObject.Type == TileableObjectType.animal)
        {
            group = this.calculateTileGroup((int)((Animal)addObject).Size.x, (int)((Animal)addObject).Size.y);
        } else if (addObject.Type == TileableObjectType.useableBuilding)
        {
            group = this.calculateTileGroup((int)((UseableBuilding)addObject).Size.x, (int)((UseableBuilding)addObject).Size.y);
        }
        else if (addObject.Type == TileableObjectType.decor)
        {
            group = this.calculateTileGroup((int)((Decor)addObject).Size.x, (int)((Decor)addObject).Size.y);
        }

        bool objectInPlace = false;

        foreach (Vector2 tile in group.Values) { 
            if (TileManager.map[(int)tile.x][(int)tile.y].hasObject)
            {
                objectInPlace = true;
            }
        }

        if (!objectInPlace)
        {
            foreach (Vector2 tile in group.Values)
            {
                TileManager.map[(int)tile.x][(int)tile.y].hasObject = true;
            }

            if (addObject.Type == TileableObjectType.crop)
            {
                GameObject gameObjectSlot = Instantiate(DataManager.prefabDB[addObject.Key], TileManager.map[(int)group[0].x][(int)group[0].y].gameObject.transform.position, Quaternion.identity, (TileManager.map[(int)group[0].x][(int)group[0].y]).transform);
                gameObjectSlot.GetComponent<CropObject>().Crop = (Crop)addObject;
                gameObjectSlot.GetComponent<CropObject>().TileGroup = group;
                gameObjectSlot.GetComponent<CropObject>().Crop.TileGroup = group;
                ActiveTileableObjects.activeTileableObjects.Add(gameObjectSlot.GetComponent<CropObject>().Crop);
            } else if (addObject.Type == TileableObjectType.animal)
            {
                GameObject gameObjectSlot = Instantiate(DataManager.prefabDB[addObject.Key], TileManager.map[(int)group[0].x][(int)group[0].y].gameObject.transform.position, Quaternion.identity, (TileManager.map[(int)group[0].x][(int)group[0].y]).transform);
                gameObjectSlot.GetComponent<AnimalObject>().Animal = (Animal)addObject;
                gameObjectSlot.GetComponent<AnimalObject>().TileGroup = group;
                gameObjectSlot.GetComponent<AnimalObject>().Animal.TileGroup = group;
                ActiveTileableObjects.activeTileableObjects.Add(gameObjectSlot.GetComponent<AnimalObject>().Animal);
            } else if (addObject.Type == TileableObjectType.useableBuilding)
            {
                GameObject gameObjectSlot = Instantiate(DataManager.prefabDB[addObject.Key], TileManager.map[(int)group[0].x][(int)group[0].y].gameObject.transform.position, Quaternion.identity, (TileManager.map[(int)group[0].x][(int)group[0].y]).transform);
                gameObjectSlot.GetComponent<UseableBuildingObject>().UseableBuilding = (UseableBuilding)addObject;
                gameObjectSlot.GetComponent<UseableBuildingObject>().TileGroup = group;
                gameObjectSlot.GetComponent<UseableBuildingObject>().UseableBuilding.TileGroup = group;
                ActiveTileableObjects.activeTileableObjects.Add(gameObjectSlot.GetComponent<UseableBuildingObject>().UseableBuilding);
            }
            else if (addObject.Type == TileableObjectType.decor)
            {
                GameObject gameObjectSlot = Instantiate(DataManager.prefabDB[addObject.Key], TileManager.map[(int)group[0].x][(int)group[0].y].gameObject.transform.position, Quaternion.identity, (TileManager.map[(int)group[0].x][(int)group[0].y]).transform);
                gameObjectSlot.GetComponent<DecorObject>().Decor = (Decor)addObject;
                gameObjectSlot.GetComponent<DecorObject>().TileGroup = group;
                gameObjectSlot.GetComponent<DecorObject>().Decor.TileGroup = group;
                ActiveTileableObjects.activeTileableObjects.Add(gameObjectSlot.GetComponent<DecorObject>().Decor);
            }

        }
    }

    public void plowTile()
    {
        this.IsPlowed = true;
        this.color = new Color(165f / 255f, 42f / 255f, 42f / 255f);
        this.transform.GetComponent<Renderer>().material.color = color;

    }

    public void clearTile()
    {
        this.IsPlowed = false;
        this.color = new Color(134f / 255f, 229f / 255f, 25f / 255f);
        transform.GetComponent<Renderer>().material.color = color;
    }

    public Dictionary<int, Vector2> calculateTileGroup(int x, int y)
    {
        if ((x % 2 != 0) || (y % 2 != 0 ))
        {
            if (x != 1 && y != 1)
            {
                Debug.Log("Odd Tile Dummy");
                return null;
            }
        }

        Dictionary<int, Vector2> list = new Dictionary<int, Vector2>();
        Vector2 baseT = new Vector2();
        int position = 0;

        if (x <= 2 && y <= 2)
        {
            if ((this.gridPosition.x % x) == 0)
            {
                baseT.x = this.gridPosition.x - (x - 1);

            }
            else
            {
                if (((this.gridPosition.x + (x - 1)) % x) == 0)
                {
                    baseT.x = this.gridPosition.x;
                }
                else
                {
                    for (int i = 1; i < x; i++)
                    {
                        if (((this.gridPosition.x + i) % x) == 0)
                        {
                            baseT.x = (this.gridPosition.x + i) - (x - 1);
                        }
                    }
                }
            }

            if ((this.gridPosition.y % y) == 0)
            {
                baseT.y = this.gridPosition.y - (y - 1);

            }
            else
            {
                if (((this.gridPosition.y + (y - 1)) % y) == 0)
                {
                    baseT.y = this.gridPosition.y;
                }
                else
                {
                    for (int i = 1; i < y; i++)
                    {
                        if (((this.gridPosition.y + i) % y) == 0)
                        {
                            baseT.y = (this.gridPosition.y + i) - (y - 1);
                        }
                    }
                }
            }

            position = 0;
            for (int j = 0; j < x; j++)
            {
                for (int k = 0; k < y; k++)
                {
                    list.Add(position, new Vector2(baseT.x + j, baseT.y + k));
                    position++;
                }
            }

            return list;
        }

        if ((this.gridPosition.x % 2) == 0 )
        {
            baseT.x = this.gridPosition.x - 1;
        } else
        {
            baseT.x = this.gridPosition.x;
        }

        if ((this.gridPosition.y % 2) == 0)
        {
            baseT.y = this.gridPosition.y - 1;
        }
        else
        {
            baseT.y = this.gridPosition.y;
        }


        if (baseT.x + x > TileManager.mapSize)
        {
            baseT.x = TileManager.mapSize - x + 1;
        }

        if (baseT.y + y > TileManager.mapSize)
        {
            baseT.y = TileManager.mapSize - y + 1;
        }

        position = 0;
        for (int j = 0; j < x; j++)
        {
            for (int k = 0; k < y; k++)
            {
                list.Add(position, new Vector2(baseT.x + j, baseT.y + k));
                position++;
            }
        }

        return list;
    }
}
