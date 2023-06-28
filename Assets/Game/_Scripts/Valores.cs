using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valores
{
    public static bool[] personajeDesbloqueado = new bool[11];
    public static int personajeSeleccioando = 1, maxPoints = 0, coins = 0, gameOverScore = 0,timesPlayed=0,timesShocked=0;
    public static string SelectedCharFile = "ArtSprites_01", gameOverCoins = 0.ToString();
    public static bool hasMusic=true, hasSound=true;
    public static bool hasIntro,adsActivated=true;
    public static SqliteDatabase mySqlDB;
    internal static List<DataRow> achievementDT;


    internal static void LoadIntField(DataTable myDT,ref int fieldNumber,string fieldname)
    {
        fieldNumber= (int)myDT.Rows[0][fieldname];
    }
    public static void UpdateIntField(ref SqliteDatabase mysqldb, int fieldNumber, string tableName,string fieldname)
    {
        mysqldb.ExecuteQuery("UPDATE "+tableName+" SET "+fieldname+" ='" + fieldNumber + "' WHERE id=1");
    }

    internal static void LoadBoolField(DataTable dataTable, ref bool fieldBoolean, string fieldname)
    {
        fieldBoolean = Convert.ToBoolean( (int)dataTable.Rows[0][fieldname]);
    }

    public static void UpdateFloatField(ref SqliteDatabase mysqldb, float fieldNumber, string tableName, string fieldname,int id)
    {
        mysqldb.ExecuteQuery("UPDATE " + tableName + " SET " + fieldname + " ='" + fieldNumber + "' WHERE id="+id);
    }
    public static void LoadMaxScore(DataTable myDT)
    {
        maxPoints = (int)myDT.Rows[0]["points"];
    }

    public static void SaveMaxScore(ref SqliteDatabase mysqldb)
    {
        mysqldb.ExecuteQuery("UPDATE Player SET points ='" + maxPoints + "' WHERE id=1");
    }
    public static void LoadConfig(DataTable myDT) {

        hasMusic = Convert.ToBoolean( (int) myDT.Rows[0]["music"]);
        hasSound = Convert.ToBoolean((int)myDT.Rows[0]["sound"]);
    }
    public static void SaveConfig(ref SqliteDatabase mysqldb) {
        mysqldb.ExecuteQuery("UPDATE Config SET music =" + Convert.ToInt32(hasMusic) + ", sound="+Convert.ToInt32(hasSound)+" WHERE id=1");
    }

    internal static void LoadCoins(DataTable myDT)
    {
        coins = (int) myDT.Rows[0]["coins"];
    }
    internal static void LoadCharId(DataTable myDT)
    {
        personajeSeleccioando = (int)myDT.Rows[0]["char_id"];
    }
    public static void SaveCharId(ref SqliteDatabase mysqldb)
    {
        mysqldb.ExecuteQuery("UPDATE Player SET char_id ='" + personajeSeleccioando + "' WHERE id=1");
    }
    public static void SaveCoins()
    {
        mySqlDB.ExecuteQuery("UPDATE Player SET coins ='" + coins + "' WHERE id=1");
        Debug.Log("monedas guardadas");
    }

  

    internal static void LoadCompleteDT(DataTable achievementsDT,ref List<DataRow> dt_list)
    {
        dt_list = new List<DataRow>();
        foreach (DataRow dr in achievementsDT.Rows)
            dt_list.Add(dr);
    }
    internal static void UpdateAchievementsProgress(ref SqliteDatabase mysqldb, int index, int actualProgress)
    {

    }

    public static void LoadHasIntro(DataTable myDT) {
        hasIntro = Convert.ToBoolean((int)myDT.Rows[0]["IntroHappened"]);
    }
    public static void SaveHasIntro(ref SqliteDatabase mysqldb) {
        mysqldb.ExecuteQuery("UPDATE Config SET IntroHappened =" + Convert.ToInt32(hasIntro) + " WHERE id=1");
    }

    internal static void LoadCharacters(DataTable characterDT)
    {
        int i = 0;
        foreach (DataRow dr in characterDT.Rows) {
           personajeDesbloqueado[i]= Convert.ToBoolean((int)dr["purchased"]);
           i++;
        }
    }
    internal static void UpdatePurchasedCharacter(SqliteDatabase sqliteDatabase)
    {
        
        for (int i =0;i<personajeDesbloqueado.Length;i++)
            sqliteDatabase.ExecuteQuery("UPDATE Character SET purchased =" + Convert.ToInt32(personajeDesbloqueado[i]) + " WHERE id="+(i+1).ToString());
    }
}
