using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SEManager : Singleton<SEManager>
    {
        [SerializeField] private AudioClip changeEquip;
        [SerializeField] private AudioClip playerGetHurt;
        [SerializeField] private AudioClip getProps;
        [SerializeField] private AudioClip handGunShot;
        [SerializeField] private AudioClip manDead;
        [SerializeField] private AudioClip raffieShot;
        [SerializeField] private AudioClip recovery;
        [SerializeField] private AudioClip reload;



        public void SE_ChangeEquip(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(changeEquip, position);
        }

        public void SE_PlayerGetHurt(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(playerGetHurt, position);
        }
        public void SE_GetProps(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(getProps, position);
        }
        public void SE_HnadGunShot(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(handGunShot, position);
        }
        public void SE_Man_Dead(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(manDead, position);
        }
        public void SE_RaffieShot(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(raffieShot, position);
        }
        public void SE_Recovery(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(recovery, position);
        }
        public void SE_Reload(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(reload, position);
        }
    }
}
