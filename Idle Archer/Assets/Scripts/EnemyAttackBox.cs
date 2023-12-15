using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    public float damageAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TowerController.instance.gameObject)
        {
            TowerController.instance.Damage(damageAmount);

            gameObject.SetActive(false);
        }
    }
}