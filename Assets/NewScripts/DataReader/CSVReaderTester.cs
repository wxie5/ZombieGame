using UnityEngine;
using System.Collections.Generic;
using Model.PlayerModel;

public class CSVReaderTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        Dictionary<int, PlayerStatsCSV> temp = CSVReader.ReadPlayerStatsCSV();
        PlayerModel m = new PlayerModel(0, temp[0].baseHealth, temp[0].baseMoveSpeed);
        */

        Dictionary<int, EnemyStatsCSV> temp = CSVReader.ReadEnemyStatsCSV();
        foreach(int k in temp.Keys)
        {
            Debug.Log(temp[k].ToString());
        }
    }
}
