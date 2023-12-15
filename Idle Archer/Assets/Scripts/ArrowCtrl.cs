using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    public Transform target;
    public float arrowSpeed;

    public float damagePower;

    private void Update()
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            if (dir.magnitude <= .8f)
            {
                target.GetComponent<Enemy>().Damage(damagePower);
                Destroy(gameObject);
                return;
            }
            transform.Translate(dir.normalized * arrowSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }
}
