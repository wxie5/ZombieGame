using UnityEngine;
using System.Collections.Generic;
//This Script is create and wrote by Jiacheng Sun
public class CameraControl: MonoBehaviour
{
    private GameObject[] playerInstance;
    [SerializeField] private float cof_y;
    [SerializeField] private float cof_z;
    [SerializeField] private float cameraMovingSpeed;
    [SerializeField] private Vector3 GlobalViewPosition;
    private Vector3 offset;
    private void Start()
    {
        playerInstance = GameObject.FindGameObjectsWithTag("Player");
        offset = gameObject.transform.position;
    }


    private void LateUpdate()
    {
        MoveCamera(GetFinalPosition());
    }


    private Vector3 GetFinalPosition()
    {
        Vector3 final_position = offset;
        int number_of_alive = 0;
        int number_of_player_alive = 0;
        for (int i=0;i< playerInstance.Length;i++)
        {
            if (!playerInstance[i].GetComponent<PlayerStats>().IsDead)
            {
                final_position += playerInstance[i].transform.position;
                number_of_alive++;
                if (playerInstance[i].GetComponent<PlayerStats>().ID != PlayerID.AI)
                {
                    number_of_player_alive++;
                }
            }
        }
        if(number_of_player_alive == 0)
        {
            return GlobalViewPosition;
        }
        final_position /= number_of_alive;

        float distance = 0;
        for(int i=0;i< playerInstance.Length; i++)
        {
            if (playerInstance[i].GetComponent<PlayerStats>().IsDead)
                continue;

            for (int j = 0; j < playerInstance.Length; j++)
            {
                if (playerInstance[j].GetComponent<PlayerStats>().IsDead)
                    continue;
                if (Vector3.Distance(playerInstance[i].transform.position, playerInstance[j].transform.position) > distance)
                {
                    distance = Vector3.Distance(playerInstance[i].transform.position, playerInstance[j].transform.position);
                }
            }
        }
        final_position += new Vector3(0, distance * cof_y, distance * cof_z);
        if (Vector3.Magnitude(final_position) > Vector3.Magnitude(GlobalViewPosition))
        {
            return GlobalViewPosition;
        }
        else
        {
            return final_position;
        }
    }
    private void MoveCamera(Vector3 position)
    {
        if(transform.position != position)
        {
            transform.position += (position - transform.position) * cameraMovingSpeed * Time.deltaTime;
        }
    }
}