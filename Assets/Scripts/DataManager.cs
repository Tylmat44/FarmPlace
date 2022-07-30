using Mono.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public static class DataManager
{
    public static Dictionary<string, Resource> resourceDB = new Dictionary<string, Resource>();
    public static Dictionary<string, Crop> cropDB = new Dictionary<string, Crop>();
    public static Dictionary<string, Animal> animalDB = new Dictionary<string, Animal>();
    public static Dictionary<string, GameObject> prefabDB = new Dictionary<string, GameObject>();
    public static Dictionary<string, UseableBuilding> useableBuildingDB = new Dictionary<string, UseableBuilding>();
    public static Dictionary<string, CraftingStation> craftingStationDB = new Dictionary<string, CraftingStation>();
    public static Dictionary<string, CraftableResource> craftableResourceDB  = new Dictionary<string, CraftableResource>();
    public static Dictionary<string, Decor> decorDB = new Dictionary<string, Decor>();
    public static TileableObjects fill = new TileableObjects("FILL", "fill", TileableObjectType.fill, new Dictionary<int, Vector2>(), 0);
    public static List<double> xpTable = new List<double>();
    public static int MAP_SIZE = 20;
    public static string PWORD = "Fjkdj2984jJsk2939djk";
    public static IDbConnection DatabaseConnection;

    public static void loadAll()
    {
        startDBConnection("sdikajk2kJDKLsjdak2");
        loadResources();
        loadCrops();
        loadAnimals();
        loadPrefabs();
        loadCraftableResources();
        loadCraftingStations();
        loadUseableBuildings();
        loadDecor();

        xpTable.Add(0);

        for (int i = 1; i <= 100; i++)
        {
            if (i > 1)
            {
                xpTable.Add(Mathf.Floor((float)((100 * Mathf.Pow((float)(i), 2)) - (100 * (i)))) + xpTable[i - 1]);
            } else
            {
                xpTable.Add(Mathf.Floor((float)((100 * Mathf.Pow((float)(i), 2)) - (100 * (i)))));
            }
        }

        Debug.Log(xpTable[5]);
    }

    public static void loadResources()
    {
        string json = "";
        using (StreamReader r = new StreamReader("Assets\\Data\\resource_data.json"))
        {
            json = r.ReadToEnd();
        }

        dynamic array = JsonConvert.DeserializeObject(json);
        foreach (var item in array)
        {

            var value = item.Value;
            resourceDB.Add(item.Name, new Resource((string)item.Name, (string)value.name, (int)value.sell_price, (int)value.buy_price, getResourceTypeFromString((string)value.type), (string)value.source));
        }

    }

    public static void loadCrops()
    {
        string json = "";
        using (StreamReader r = new StreamReader("Assets\\Data\\crop_data.json"))
        {
            json = r.ReadToEnd();
        }

        dynamic array = JsonConvert.DeserializeObject(json);
        foreach (var item in array)
        {

            var value = item.Value;
            cropDB.Add(item.Name, new Crop((string)item.Name, (string)value.name, (int)value.cost, getTimeSpanFromString((float)value.growth_time), (int)value.harvest_quantity, (string)value.resource, (int)value.xp));
        }
    }

    public static void loadAnimals()
    {
        string json = "";
        using (StreamReader r = new StreamReader("Assets\\Data\\animal_data.json"))
        {
            json = r.ReadToEnd();
        }

        dynamic array = JsonConvert.DeserializeObject(json);
        foreach (var item in array)
        {

            var value = item.Value;
            animalDB.Add(item.Name, new Animal((string)item.Name, (string)value.name, (int)value.cost, getTimeSpanFromString((float)value.growth_time), getTimeSpanFromString((float)value.harvest_time), (int)value.base_sell_price, (int)value.harvest_quantity, (string)value.resource, (int)value.xp, new Vector2((float)value.size_x, (float)value.size_y)));
        }

    }

    public static void loadPrefabs()
    {
        var prefabFile = File.ReadAllLines("Assets\\Data\\prefab_data.data");
        var prefabList = new List<string>(prefabFile);

        foreach (string prefab in prefabList)
        {
            prefabDB.Add(prefab, (GameObject)Resources.Load(prefab));
        }


    }

    public static void loadUseableBuildings()
    {
        string json = "";
        using (StreamReader r = new StreamReader("Assets\\Data\\useablebuilding_data.json"))
        {
            json = r.ReadToEnd();
        }

        dynamic array = JsonConvert.DeserializeObject(json);
        foreach (var item in array)
        {

            var value = item.Value;
            useableBuildingDB.Add(item.Name, new UseableBuilding((string)item.Name, (string)value.name, (int)value.cost, getUseableBuildingTypeFromString((string)value.type), (int)value.slots, new Vector2((float)value.size_x, (float)value.size_y), getCraftingTypesFromString((string)value.crafting_types)));
        }
    }

    public static void loadCraftingStations()
    {
        string json = "";
        using (StreamReader r = new StreamReader("Assets\\Data\\crafting_station_data.json"))
        {
            json = r.ReadToEnd();
        }

        dynamic array = JsonConvert.DeserializeObject(json);
        foreach (var item in array)
        {

            var value = item.Value;
            craftingStationDB.Add(item.Name, new CraftingStation((string)item.Name, (string)value.name, getCraftingTypeFromString((string)value.crafting_type), (float)value.speed_multiplier, (int)value.cost));
        }
    }

    public static void loadCraftableResources()
    {
        string json = "";
        using (StreamReader r = new StreamReader("Assets\\Data\\crafting_resource_data.json"))
        {
            json = r.ReadToEnd();
        }

        dynamic array = JsonConvert.DeserializeObject(json);
        foreach (var item in array)
        {

            var value = item.Value;
            craftableResourceDB.Add(item.Name, new CraftableResource((string)item.Name, (string)value.name, getCraftingTypeFromString((string)value.crafting_type), (int)value.quantity_produced, TimeSpan.FromMinutes((double)value.crafting_time), CraftableResource.getRecipieFromString((string)value.recipie), (int)value.xp, (int)value.level));
        }
    }

    public static void loadDecor()
    {
        string json = "";
        using (StreamReader r = new StreamReader("Assets\\Data\\decor_data.json"))
        {
            json = r.ReadToEnd();
        }

        dynamic array = JsonConvert.DeserializeObject(json);
        foreach (var item in array)
        {

            var value = item.Value;
            decorDB.Add(item.Name, new Decor((string)item.Name, (string)value.name, (int)value.cost, new Vector2((float)value.size_x, (float)value.size_y)));
        }
    }

    public static TimeSpan getTimeSpanFromString(float hours)
    {
        return TimeSpan.FromMinutes(hours*60);
    }

    public static ResourceType getResourceTypeFromString(string type)
    {
        Enum.TryParse(type, out ResourceType resourceType);
        return resourceType;
    }

    public static void startDBConnection(string DatabaseName)
    {
        string filepath = Application.dataPath + DatabaseName;
        string conn = "URI=file:" + filepath;
        DatabaseConnection = new SqliteConnection(conn);
    }

    public static List<string> getListOfResourceType(ResourceType type)
    {
        List<string> resourceList = new List<string>();

        foreach (string item in resourceDB.Keys)
        {
            if (resourceDB[item].Type == type)
            {
                resourceList.Add(item);
            }
        }

        return resourceList;
    }

    public static UseableBuildingType getUseableBuildingTypeFromString(string type)
    {
        Enum.TryParse(type, out UseableBuildingType buildingType);
        return buildingType;
    }

    public static CraftingType getCraftingTypeFromString(string type)
    {
        Enum.TryParse(type, out CraftingType craftingType);
        return craftingType;
    }

    public static List<CraftingType> getCraftingTypesFromString(string types)
    {
        List <CraftingType> craftingTypes = new List<CraftingType>();

        string[] typeList = types.Split('?');

        foreach (string type in typeList)
        {
            craftingTypes.Add(getCraftingTypeFromString(type));
        }

        return craftingTypes;

    }
}
