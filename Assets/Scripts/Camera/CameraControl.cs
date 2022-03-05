using UnityEngine;
using System.Collections.Generic;
//This Script is create and wrote by Jiacheng Sun
public class CameraControl: MonoBehaviour
{
    private GameObject[] playerInstance;
    [SerializeField] private float cof_y;
    [SerializeField] private float cof_z;
    [SerializeField] private float cameraMovingSpeed;
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
        for(int i=0;i< playerInstance.Length;i++)
        {
            if (!playerInstance[i].GetComponent<PlayerStats>().IsDead)
            {
                final_position += playerInstance[i].transform.position;
                number_of_alive++;
            }
        }
        if(number_of_alive == 0)
        {
            return new Vector3(-0.5f, 26f, -14.5f);
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
        return final_position;
    }
    private void MoveCamera(Vector3 position)
    {
        if(transform.position != position)
        {
            transform.position += (position - transform.position) * cameraMovingSpeed * Time.deltaTime;
        }
    }
}