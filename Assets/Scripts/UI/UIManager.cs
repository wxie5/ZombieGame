using UnityEngine;
using UnityEngine.UI;

//This script is wrote by Wei Xie
public class UIManager : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField] private Text weaponText;
    [SerializeField] private Text ammoText;

    private PlayerManager aManager;
    private PlayerManager bManager;

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(GameConst.PLAYER_TAG);

        foreach(GameObject go in players)
        {
            PlayerManager pm = go.GetComponent<PlayerManager>();
            if(pm.playerID == PlayerID.PlayerA)
            {
                aManager = pm;
            }
            else if(pm.playerID == PlayerID.PlayerB)
            {
                bManager = pm;
            }
        }

        aManager.OnUpdateAmmoInfoAdd(UpdatePlayerAAmmoText, true);
    }

    public void UpdatePlayerAAmmoText(int currentAmmo, int leftAmmo)
    {
        ammoText.text = currentAmmo + "/" + leftAmmo;
    }

    private void OnDisable()
    {
        aManager.OnUpdateAmmoInfoRemove(UpdatePlayerAAmmoText);
    }
}
