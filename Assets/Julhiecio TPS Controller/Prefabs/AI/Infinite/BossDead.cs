using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDead : MonoBehaviour
{
    public void OnBossDead()
    {     
        InfiniteMode.Instance.OnKillEnemy();
    }
}
