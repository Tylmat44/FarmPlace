using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileableObjects
{
    private string key;
    private string name;
    private TileableObjectType type;
    private Dictionary<int, Vector2> tileGroup;
    private int harvestXp;

    public TileableObjects(string key, string name, TileableObjectType type, Dictionary<int, Vector2> tileGroup, int harvestXp)
    {
        this.Key = key;
        this.Name = name;
        this.Type = type;
        this.TileGroup = tileGroup;
        this.HarvestXp = harvestXp;
    }

    public string Key { get => key; set => key = value; }
    public string Name { get => name; set => name = value; }
    public TileableObjectType Type { get => type; set => type = value; }
    public Dictionary<int, Vector2> TileGroup { get => tileGroup; set => tileGroup = value; }
    public int HarvestXp { get => harvestXp; set => harvestXp = value; }
}
