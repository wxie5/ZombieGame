using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Please aware that this CSVReader is built only for this project
/// Different projects may have different excel chart style
/// So do not use this class outside of this project
/// </summary>
public class CSVReader
{
    /// <param name="filePath">this should not include "CSVFiles" in the path, this method will handle that prefix</param>
    private static string[,] ReadCSVFile(string filePath)
    {
        // remove some prefix that might cause errors
        if(filePath[0] == '/') 
        { 
            filePath = filePath.Substring(1, filePath.Length - 1); 
        }

        // get absolute path (also check if someone accidentally add some unnecessary prefix)
        string absFilePath = "";
        if(filePath.Length < 9 || filePath.Substring(0, 9) != "CSVFiles/")
        {
            absFilePath = "CSVFiles/" + filePath;
        }
        else
        {
            absFilePath = filePath;
        }

        TextAsset csvFile = Resources.Load<TextAsset>(absFilePath);
        if(csvFile == null) { Debug.LogError("File path does not exist!"); }

        string csvText = csvFile.text;
        string titles = "";
        string contents = "";
        for(int i = 0; i < csvText.Length; i++)
        {
            if(csvText[i] == '\n')
            {
                titles = csvText.Substring(0, i + 1);
                contents = csvText.Substring(i + 1, csvText.Length - i - 1);
                break;
            }
        }

        if(titles == "" || contents == "")
        {
            Debug.LogError("CSV file is empty, no information to read!");
        }

        string[] contentStrs = contents.Substring(0, contents.Length - 1).Split('\n', ',');
        string[] titleStrs = titles.Substring(0, titles.Length - 1).Split(',');
        int colCount = titleStrs.Length;
        int rowCount = contentStrs.Length / colCount;

        string[,] result = new string[rowCount, colCount];
        for(int r = 0; r < rowCount; r++)
        {
            for(int c = 0; c < colCount; c++)
            {
                int index = r * colCount + c;
                result[r, c] = contentStrs[index];
            }
        }

        return result;
    }

    public static void PrintReadCSVFile(string[,] readCSV)
    {
        int rowCount = readCSV.GetLength(0);
        int colCount = readCSV.GetLength(1);

        string outputString = "";

        for(int r = 0; r < rowCount; r++)
        {
            outputString += "[";
            for(int c = 0; c < colCount; c++)
            {
                outputString += readCSV[r, c];
                if(c < colCount - 1)
                {
                    outputString += "\t";
                }
            }
            outputString += "]";
            if(r < rowCount - 1)
            {
                outputString += "\n";
            }
        }

        Debug.Log(outputString);
    }

    #region Specific Data Reader
    // Modify these method when CSV file changed
    public static Dictionary<int, PlayerStatsCSV> ReadPlayerStatsCSV()
    {
        string[,] playerStatsCSV = ReadCSVFile("PlayerStats");
        int rowCount = playerStatsCSV.GetLength(0);

        Dictionary<int, PlayerStatsCSV> resultDict = new Dictionary<int, PlayerStatsCSV>();

        for(int r = 0; r < rowCount; r++)
        {
            // read primary key
            int playerID = int.Parse(playerStatsCSV[r, 0]);

            // read data
            float baseHealth = float.Parse(playerStatsCSV[r, 1]);
            float baseMoveSpeed = float.Parse(playerStatsCSV[r, 2]);
            PlayerStatsCSV newPlayerStatsCSV = new PlayerStatsCSV(baseHealth, baseMoveSpeed);

            // add into dict
            resultDict.Add(playerID, newPlayerStatsCSV);
        }

        return resultDict;
    }

    public static Dictionary<int, EnemyStatsCSV> ReadEnemyStatsCSV()
    {
        string[,] enemyStatsCSV = ReadCSVFile("EnemyStats");
        int rowCount = enemyStatsCSV.GetLength(0);

        Dictionary<int, EnemyStatsCSV> resultDict = new Dictionary<int, EnemyStatsCSV>();

        for (int r = 0; r < rowCount; r++)
        {
            // read primary key
            int enemyID = int.Parse(enemyStatsCSV[r, 0]);

            // read data
            string enemyName = enemyStatsCSV[r, 1];
            float baseHealth = float.Parse(enemyStatsCSV[r, 2]);
            float moveSpeed = float.Parse(enemyStatsCSV[r, 3]);
            float baseDmg = float.Parse(enemyStatsCSV[r, 4]);
            float alertDistance = float.Parse(enemyStatsCSV[r, 5]);
            float attackRange = float.Parse(enemyStatsCSV[r, 6]);
            float stopDistance = float.Parse(enemyStatsCSV[r, 7]);
            float attackRate = float.Parse(enemyStatsCSV[r, 8]);
            float attackRateDefault = float.Parse(enemyStatsCSV[r, 9]);
            int score = int.Parse(enemyStatsCSV[r, 10]);
            float baseHealthMulti = float.Parse(enemyStatsCSV[r, 11]);
            float baseDmgMulti = float.Parse(enemyStatsCSV[r, 12]);
            int defaultLevel = int.Parse(enemyStatsCSV[r, 13]);
            int maxLevel = int.Parse(enemyStatsCSV[r, 14]);
            EnemyStatsCSV newEnemyStatsCSV = new EnemyStatsCSV(enemyName, baseHealth, moveSpeed, baseDmg,
                alertDistance, attackRange, stopDistance, attackRate, attackRateDefault, score, baseHealthMulti,
                baseDmgMulti, defaultLevel, maxLevel);

            // add into dict
            resultDict.Add(enemyID, newEnemyStatsCSV);
        }

        return resultDict;
    }
    #endregion
}

#region Data Classes
public class PlayerStatsCSV
{
    public float baseHealth;
    public float baseMoveSpeed;

    public PlayerStatsCSV(float baseHealth, float baseMoveSpeed)
    {
        this.baseHealth = baseHealth;
        this.baseMoveSpeed = baseMoveSpeed;
    }

    public override string ToString()
    {
        string result = string.Format("BaseHealth: {0}\t BaseMoveSpeed: {1}", baseHealth, baseMoveSpeed);
        return result;
    }
}

public class EnemyStatsCSV
{
    public string enemyName;
    public float baseHealth;
    public float moveSpeed;
    public float baseDmg;
    public float alertDistance;
    public float attackRange;
    public float stopDistance;
    public float attackRate;
    public float attackRateDefault;
    public int score;
    public float baseHealthMulti;
    public float baseDmgMulti;
    public int defaultLevel;
    public int maxLevel;

    public EnemyStatsCSV(string enemyName, float baseHealth, float moveSpeed, float baseDmg, float alertDistance, 
        float attackRange, float stopDistance, float attackRate, float attackRateDefault, int score, float baseHealthMulti, 
        float baseDmgMulti, int defaultLevel, int maxLevel)
    {
        this.enemyName = enemyName;
        this.baseHealth = baseHealth;
        this.moveSpeed = moveSpeed;
        this.baseDmg = baseDmg;
        this.alertDistance = alertDistance;
        this.attackRange = attackRange;
        this.stopDistance = stopDistance;
        this.attackRate = attackRate;
        this.attackRateDefault = attackRateDefault;
        this.score = score;
        this.baseHealthMulti = baseHealthMulti;
        this.baseDmgMulti = baseDmgMulti;
        this.defaultLevel = defaultLevel;
        this.maxLevel = maxLevel;
    }

    public override string ToString()
    {
        string result = string.Format("Enemy Info: \n" +
                                      "\tenemyName: {0}\n" +
                                      "\tbaseHealth: {1}\n" +
                                      "\tmoveSpeed: {2}\n" +
                                      "\tbaseDmg: {3}\n" +
                                      "\talertDistance: {4}\n" +
                                      "\tattackRange: {5}\n" +
                                      "\tstopDistance: {6}\n" +
                                      "\tattackRate: {7}\n" +
                                      "\tattackRateDefault: {8}\n" +
                                      "\tscore: {9}\n" +
                                      "\tbaseHealthMulti: {10}\n" +
                                      "\tbaseDmgMulti: {11}\n" +
                                      "\tdefaultLevel: {12}\n" +
                                      "\tmaxLevel: {13}\n", enemyName, baseHealth, moveSpeed, baseDmg,
                                      alertDistance, attackRange, stopDistance, attackRate, attackRateDefault, score,
                                      baseHealthMulti, baseDmgMulti, defaultLevel, maxLevel);

        return result;                              
    }
}
#endregion