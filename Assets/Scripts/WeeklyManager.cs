using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class WeeklyManager
{
    public static int weekNumber;
    public static List<bool> questsCompleted = new List<bool>();
    public static DateTime STARTDATE = new DateTime(2022, 7, 18, 0, 0, 0);

    public static void loadWeeklyInfo()
    {
        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS WEEKLY_INFO (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "INSERT OR IGNORE INTO WEEKLY_INFO(info) VALUES(@weeklyString)";
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@weeklytString";
        parameter.Value = EncryptedXmlSerializer.EncryptData(((int)((DateTime.UtcNow - STARTDATE).TotalDays / 7)).ToString() + ",0,0,0,0,0");
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "SELECT * FROM WEEKLY_INFO;";
        IDataReader rdr = command.ExecuteReader();
        rdr.Read();
        string[] results = EncryptedXmlSerializer.DecryptData(rdr.GetString(1)).Split(',');

        if (int.Parse(results[0]) < (int)((DateTime.UtcNow - STARTDATE).TotalDays / 7))
        {
            weekNumber = (int)((DateTime.UtcNow - STARTDATE).TotalDays / 7);
            for(int i = 0; i < 5; i++)
            {
                questsCompleted.Add(false);
            }
        } else
        {
            for (int i = 1; i <= 5; i++)
            {
                if (int.Parse(results[i]) == 0)
                {
                    questsCompleted.Add(false);
                }
                else
                {
                    questsCompleted.Add(true);
                }
            }
        }

        DataManager.DatabaseConnection.Close();
    }

    public static void saveAccountInfo()
    {
        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "DROP TABLE WEEKLY_INFO";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS WEEKLY_INFO (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();
        command.CommandText = "INSERT OR IGNORE INTO WEEKLY_INFO(info) VALUES(@weeklyString)";
        var parameter = command.CreateParameter();
        List<int> completed = new List<int>();

        foreach(bool value in questsCompleted)
        {
            if(value)
            {
                completed.Add(1);
            } else
            {
                completed.Add(0);
            }
        }

        string encrypt = DateTime.UtcNow.Ticks.ToString() + "," + completed[0] + "," + completed[1] + "," + completed[2] + "," + completed[3] + "," + completed[4] + "," + completed[5];
        parameter.ParameterName = "@weeklyString";
        parameter.Value = EncryptedXmlSerializer.EncryptData(encrypt);
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
        DataManager.DatabaseConnection.Close();
    }
}
     