using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public static class AccountManager
{
    private static DateTime lastLogin;
    private static int xp;
    private static int coins;
    private static double level;

    public static DateTime LastLogin { get => lastLogin; set => lastLogin = value; }
    public static int Xp { get => xp; set => xp = value; }
    public static int Coins { get => coins; set => coins = value; }
    public static double Level { get => level; set => level = value; }

    public static void loadAccountInfo()
    {
        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS ACCOUNT_INFO (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "INSERT OR IGNORE INTO ACCOUNT_INFO(info) VALUES(@accountString)";
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@accountString";
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
     
    public static void saveAccountInfo()
    {
        DataManager.DatabaseConnection.Open();
        IDbCommand command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "DROP TABLE ACCOUNT_INFO";
        command.ExecuteNonQuery();
        command = DataManager.DatabaseConnection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS ACCOUNT_INFO (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "info TEXT)";
        command.ExecuteNonQuery();
        command.CommandText = "INSERT OR IGNORE INTO ACCOUNT_INFO(info) VALUES(@accountString)";
        var parameter = command.CreateParameter();
        string encrypt = DateTime.UtcNow.Ticks.ToString() + "," + Xp.ToString() + "," + Coins.ToString();
        parameter.ParameterName = "@accountString";
        parameter.Value = EncryptedXmlSerializer.EncryptData(encrypt);
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
        DataManager.DatabaseConnection.Close();
    }

    public static void calculateLevel()
    {
        Level = Mathf.Max(1, Mathf.Floor((5 + Mathf.Sqrt(Xp+25)) / 10));
    }

}
