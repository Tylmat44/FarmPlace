using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject TilePrefab;
    public static int mapSize = 20;
    public string path = "khf983fn32no3i039239f0932";
    public static Dictionary<int, Dictionary<int, Tile>> map = new Dictionary<int, Dictionary<int, Tile>>();
    public static TileManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void generateMap()
    {
        map = new Dictionary<int, Dictionary<int, Tile>>();
        for (int i = 1; i <= mapSize; i++)
        {    //i and j are the two values on the eventual vector2
            Dictionary<int, Tile> row = new Dictionary<int, Tile>();
            for (int j = 1; j <= mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j);
                tile.transform.Rotate(new Vector3(-90, 0, 0));
                tile.transform.GetComponent<Renderer>().material.color = new Color(134f / 255f, 229f / 255f, 25f / 255f);
                tile.tileManager = this;

                row.Add(j, tile);
            }
            map.Add(i, row);
        }
    }

    public void loadMapFromFile()
    {
        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS TILE_INFO (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "SELECT * FROM TILE_INFO;";
        IDataReader rdr = command.ExecuteReader();

        while (rdr.Read())
        {
            string[] results = rdr.GetString(1).Split(',');
            if (int.Parse(results[2]) == 1)
            {
                this.plowTiles(map[int.Parse(results[0])][int.Parse(results[1])].calculateTileGroup(2,2));
            }
        }
        DataManager.DatabaseConnection.Close();

    }

    public void saveMapToFile()
    {
        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "DROP TABLE TILE_INFO";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS TILE_INFO (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();

        foreach (Dictionary<int, Tile> tileRow in map.Values)
        {
            foreach (Tile tile in tileRow.Values)
            {
                command.CommandText = "INSERT OR IGNORE INTO TILE_INFO(info) VALUES(@tileString)";
                var parameter = command.CreateParameter();
                int plowed = 0;
                if (tile.IsPlowed)
                {
                    plowed = 1;
                }
                else
                {
                    plowed = 0;
                }
                string encrypt = tile.gridPosition.x.ToString() + "," + tile.gridPosition.y.ToString() + "," + plowed.ToString();
                parameter.ParameterName = "@tileString";
                parameter.Value = encrypt;
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
            }
        }
        DataManager.DatabaseConnection.Close();

    }

    public void plowTiles(Dictionary<int, Vector2> plots)
    {
        try
        {
            if (!(map[(int)plots[0].x][(int)plots[0].y].IsPlowed || map[(int)plots[1].x][(int)plots[1].y].IsPlowed || map[(int)plots[2].x][(int)plots[2].y].IsPlowed || map[(int)plots[3].x][(int)plots[3].y].IsPlowed))
            {
                map[(int)plots[0].x][(int)plots[0].y].plowTile();
                map[(int)plots[1].x][(int)plots[1].y].plowTile();
                map[(int)plots[2].x][(int)plots[2].y].plowTile();
                map[(int)plots[3].x][(int)plots[3].y].plowTile();
            }
        }
        catch
        {

        }

    }

    public void clearTiles(Dictionary<int, Vector2> plots)
    {
        try
        {
            if ((map[(int)plots[0].x][(int)plots[0].y].IsPlowed || map[(int)plots[1].x][(int)plots[1].y].IsPlowed || map[(int)plots[2].x][(int)plots[2].y].IsPlowed || map[(int)plots[3].x][(int)plots[3].y].IsPlowed))
            {
                map[(int)plots[0].x][(int)plots[0].y].clearTile();
                map[(int)plots[1].x][(int)plots[1].y].clearTile();
                map[(int)plots[2].x][(int)plots[2].y].clearTile();
                map[(int)plots[3].x][(int)plots[3].y].clearTile();
            }
        }
        catch
        {


        }
    }
}
