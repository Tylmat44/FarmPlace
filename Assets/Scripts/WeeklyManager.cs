using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class WeeklyManager
{
    public static int weekNumber;
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
        parameter.Value = EncryptedXmlSerializer.EncryptData(DateTime.UtcNow.Ticks.ToString() + ",0,0");
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "SELECT * FROM ACCOUNT_INFO;";
        IDataReader rdr = command.ExecuteReader();
        rdr.Read();
        string[] results = EncryptedXmlSerializer.DecryptData(rdr.GetString(1)).Split(',');
        LastLogin = DateTime.FromBinary(long.Parse(results[0]));
        Xp = int.Parse(results[1]);
        Coins = int.Parse(results[2]);
        Debug.Log(LastLogin);
        DataManager.DatabaseConnection.Close();
    }
}
