using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Decor : TileableObjects
{
    private Vector2 size;
    private float rotation;
    private int cost;

    public Vector2 Size { get => size; set => size = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public int Cost { get => cost; set => cost = value; }

    public Decor(string key, string name, int cost, Vector2 size) 
        : base(key, name, TileableObjectType.decor, new Dictionary<int, Vector2>(), 0)
    {
        this.Cost = cost;
        this.Size = size;
    }

    public Decor Copy()
    {
        return new Decor(this.Key, this.Name, this.Cost, this.Size);
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(this.TileGroup[0].x + "?" + this.TileGroup[0].y + "?" + this.Rotation);

        return builder.ToString();
    }

    public void Parse(string data)
    {
        string[] dataList = data.Split('?');

        this.Rotation = float.Parse(dataList[2]);

        if (this.Rotation == 0 || this.rotation == 180)
        {
            this.TileGroup = TileManager.map[int.Parse(dataList[0])][int.Parse(dataList[1])].calculateTileGroup((int)this.Size.x, (int)this.Size.y);
        }
        else
        {
            this.TileGroup = TileManager.map[int.Parse(dataList[0])][int.Parse(dataList[1])].calculateTileGroup((int)this.Size.y, (int)this.Size.x);
        }
    }
}
