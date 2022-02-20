using UnityEngine;
//This Script is create and wrote by Jiacheng Sun
public class MultiplayerCamara: MonoBehaviour
{
    private bool player0Dead;
    private bool player1Dead;
    private Transform player0Trans;
    private Transform player1Trans;
    [SerializeField] private float cof_y;
    [SerializeField] private float cof_z;
    [SerializeField] private float cameraMovingSpeed;


    public Transform Player0Trans
    {
        set { player0Trans = value; }
    }
    public Transform Player1Trans
    {
        set { player1Trans = value; }
    }

    public bool Player0Dead
    {
        set { player0Dead = value; }
    }
    public bool Player1Dead
    {
        set { player1Dead = value; }
    }

    private void LateUpdate()
    {
        if(player0Dead && !player1Dead)
        {
            MoveCamera(player1Trans.position);
        }
        else if(player1Dead && !player0Dead)
        {
            MoveCamera(player0Trans.position);
        }
        else
        {
            Vector3 final_position;
            final_position = (player0Trans.position + player1Trans.position) / 2;
            float distance = Vector3.Distance(player0Trans.position, player1Trans.position);
            final_position += new Vector3(0, distance * cof_y, distance * cof_z);
            MoveCamera(final_position);
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