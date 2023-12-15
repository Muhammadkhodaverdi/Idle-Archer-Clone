using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimatorCtrl : MonoBehaviour
{
    public Enemy root;

    public void EnemyAttack()
    {
        root.EnemyAttack();
    }
}
