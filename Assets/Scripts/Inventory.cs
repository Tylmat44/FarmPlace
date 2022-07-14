using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public static class Inventory
{
    public static Dictionary<string, int> inventory = new Dictionary<string, int>();
    private static string path = "JH3j9djDO929dkLmk228Fh";

    public static void loadInventory()
    {
        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS INVENTORY (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();

        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "SELECT * FROM INVENTORY;";
        IDataReader rdr = command.ExecuteReader();

        while (rdr.Read())
        {
            string[] results = EncryptedXmlSerializer.DecryptData(rdr.GetString(1)).Split(',');
            inventory.Add(results[0], int.Parse(results[1]));
        }
        DataManager.DatabaseConnection.Close();

    }

    public static void saveInventory()
    {
        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "DROP TABLE INVENTORY";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS INVENTORY (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();

        foreach (string key in inventory.Keys)
        {
            command.CommandText = "INSERT OR IGNORE INTO INVENTORY(info) VALUES(@inventoryString)";
            var parameter = command.CreateParameter();
            string encrypt = key + "," + inventory[key].ToString();
            parameter.ParameterName = "@inventoryString";
            parameter.Value = EncryptedXmlSerializer.EncryptData(encrypt);
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();
        }
        DataManager.DatabaseConnection.Close();
    }

    public static List<string> getListOfResourceType(ResourceType type)
    {
        List<string> resourceList = new List<string>();

        foreach (string item in inventory.Keys)
        {
            if (DataManager.resourceDB[item].Type == type)
            {
                resourceList.Add(item);
            }
        }

        return resourceList;
    }

    public static void addItem(string resource, int quantity)
    {
        if (inventory.ContainsKey(resource))
        {
            inventory[resource] += quantity;
        } else
        {
            inventory.Add(resource, quantity);
        }
    }

    public static void removeItem(string resource, int quantity)
    {
        if (inventory.ContainsKey(resource))
        {
            inventory[resource] -= quantity;
        }
    }

    public static int checkMaxCraftingAmount(Dictionary<string, int> recipie)
    {
        List<int> craftingValues = new List<int>();
        bool canCraft = true;

        foreach(string key in recipie.Keys)
        {
            if (inventory.ContainsKey(key))
            {
                craftingValues.Add(recipie[key] / inventory[key]); 
            } else
            {
                canCraft = false;
            }
        }

        if (!canCraft)
        {
            return 0;
        } else
        {
            craftingValues.Sort();
            return craftingValues[0];
        }
    }
}
