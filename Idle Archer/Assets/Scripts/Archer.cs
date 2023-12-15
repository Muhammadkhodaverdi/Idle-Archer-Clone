using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public static Archer Instance;

    public Transform target;
    private Animator animator;

    public float attackPower;
    public float attackSpeed;
    public float range;

    private float rotationSpeed = 50;
    private float fire;

    public GameObject arrowPrefab;
    public Transform arrowSpawnPos;

    public Transform rangeSpriteTranform;

    public void Init(ArcherData data)
    {
        attackPower = MetaData.instance.GetCtegoryItemAmountLevel(Category.Attacking, SubCategory.Damage, data.GetUserSubCategory(Category.Attacking, SubCategory.Damage).level).amount;
        attackSpeed = MetaData.instance.GetCtegoryItemAmountLevel(Category.Attacking, SubCategory.AttackSpeed, data.GetUserSubCategory(Category.Attacking, SubCategory.AttackSpeed).level).amount;
        range = MetaData.instance.GetCtegoryItemAmountLevel(Category.Attacking, SubCategory.AttackRange, data.GetUserSubCategory(Category.Attacking, SubCategory.AttackRange).level).amount;
        RangeSize();

    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateTarget();
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if (fire <= 0)
            {
                Shoot();
                fire = attackSpeed;
            }
            fire -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        animator.Play("shootArrow");
    }
    public void attack()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPos.position, Quaternion.identity);
        ArrowCtrl arrowCtrl = arrow.GetComponent<ArrowCtrl>();
        if (arrowCtrl != null)
        {
            arrowCtrl.target = target;
            arrowCtrl.damagePower = attackPower;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 bace = transform.position;
        bace.y = 0;
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(bace, range);
    }

    public void UpdateTarget()
    {
        Vector3 bace = transform.position;
        bace.y = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (var Enemy in enemies)
        {
            float distance = Vector3.Distance(bace, Enemy.transform.position);

            if (distance <= shortDistance)
            {
                shortDistance = distance;
                closestEnemy = Enemy;
            }
        }

        if (closestEnemy != null && shortDistance < (range+1))
        {
            target = closestEnemy.transform;
        }
    }

    public void RangeSize()
    {
        float x = 10 / .75f /*12 / .92f*/;
        float y = range / x;
        rangeSpriteTranform.localScale = new Vector3(y, y, y);
    }
}


