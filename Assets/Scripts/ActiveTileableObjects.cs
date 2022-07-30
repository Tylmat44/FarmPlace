using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;


public static class ActiveTileableObjects
{
    public static List<TileableObjects> activeTileableObjects = new List<TileableObjects>();
    private static string path = "kSAld934JK2lkljrE02";


    public static void loadActiveTileableObjects()
    {

        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS ACTIVE_OBJECTS (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "SELECT * FROM ACTIVE_OBJECTS;";
        IDataReader rdr = command.ExecuteReader();
        List<TileableObjects> tempList = new List<TileableObjects>();

        while (rdr.Read())
        {
            string[] results = EncryptedXmlSerializer.DecryptData(rdr.GetString(1)).Split(',');
            Enum.TryParse(results[1], out TileableObjectType tempType);

            if (tempType == TileableObjectType.crop)
            {
                Crop newObj = DataManager.cropDB[results[0]].Copy();
                newObj.Parse(results[2]);
                tempList.Add(newObj);

            }
            else if (tempType == TileableObjectType.animal)
            {
                Animal newObj = DataManager.animalDB[results[0]].Copy();
                newObj.Parse(results[2]);
                tempList.Add(newObj);
            }
            else if (tempType == TileableObjectType.useableBuilding)
            {
                UseableBuilding newObj = DataManager.useableBuildingDB[results[0]].Copy();
                newObj.Parse(results[2]);
                tempList.Add(newObj);
            }
            else if (tempType == TileableObjectType.decor)
            {
                Decor newObj = DataManager.decorDB[results[0]].Copy();
                newObj.Parse(results[2]);
                tempList.Add(newObj);
            }
        }
        DataManager.DatabaseConnection.Close();

        foreach (TileableObjects tileableObject in tempList)
        {
            if (tileableObject.Type == TileableObjectType.crop)
            {
                TileManager.map[(int)((Crop)tileableObject).TileGroup[0].x][(int)((Crop)tileableObject).TileGroup[0].y].addTileableObject(tileableObject);
            } else if (tileableObject.Type == TileableObjectType.animal)
            {
                TileManager.map[(int)((Animal)tileableObject).TileGroup[0].x][(int)((Animal)tileableObject).TileGroup[0].y].addTileableObject(tileableObject);
            }
            else if (tileableObject.Type == TileableObjectType.useableBuilding)
            {
                TileManager.map[(int)((UseableBuilding)tileableObject).TileGroup[0].x][(int)((UseableBuilding)tileableObject).TileGroup[0].y].addTileableObject(tileableObject);
            }
            else if (tileableObject.Type == TileableObjectType.decor)
            {
                TileManager.map[(int)((Decor)tileableObject).TileGroup[0].x][(int)((Decor)tileableObject).TileGroup[0].y].addTileableObject(tileableObject);
            }
        }
    }

    public static void saveActiveTileableObjects()
    {

        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "DROP TABLE ACTIVE_OBJECTS";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS ACTIVE_OBJECTS (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();

        foreach (TileableObjects tileableObject in activeTileableObjects)
        {
            command.CommandText = "INSERT OR IGNORE INTO ACTIVE_OBJECTS(info) VALUES(@activeObjectString)";
            var parameter = command.CreateParameter();
            string encrypt = tileableObject.Key + "," + tileableObject.Type.ToString() + "," + tileableObject.ToString();
            Debug.Log(encrypt);
            parameter.ParameterName = "@activeObjectString";
            parameter.Value = EncryptedXmlSerializer.EncryptData(encrypt);
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();
        }
        DataManager.DatabaseConnection.Close();

    }

}
