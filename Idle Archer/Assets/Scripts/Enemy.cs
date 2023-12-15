using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public float health;
    public float maxHealth;
    public float damage;

    public EnemyType type;

    public Vector3 enemyPos;


    public Animator animator;

    public bool canMove;



    public void Init(int wawe)
    {
        agent = GetComponent<NavMeshAgent>();
        canMove = true;
        health = EnemyMeta.Instance.GetEnemyHealthDataByWawe(type, wawe);
        maxHealth = health;
        damage = EnemyMeta.Instance.GetEnemyDamageDataByWawe(type, wawe);
    }

    public void Init(float _health, float _maxHealth, float _damage)
    {
        agent = GetComponent<NavMeshAgent>();
        canMove = true;
        health = _health;
        maxHealth = _maxHealth;
        damage = _damage;
    }

    public void Init(JSONNode js)
    {
        agent = GetComponent<NavMeshAgent>();
        canMove = true;
        health = js["health"].AsFloat;
        maxHealth = js["maxHealth"].AsFloat;
        damage = js["damage"].AsFloat;
        type = (EnemyType)Enum.Parse(typeof(EnemyType), js["Type"].Value);
        enemyPos = StringToVector3(js["enemyPos"].Value.ToString());

    }
    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        if (canMove == true)
        {
            agent.destination = TowerController.instance.gameObject.transform.position;
        }
        else
        {
            agent.destination = transform.position;
        }
    }

    public void Damage(float damageValue)
    {
        health -= damageValue;

        if (health <= 0)
        {
            Archer.Instance.target = null;
            GameManager.Instance.archerDataClone.Experience += MetaData.instance.GetCtegoryItemAmountLevel(Category.Collecting, SubCategory.ExperiencePerEnemy, GameManager.Instance.archerDataClone.GetUserSubCategory(Category.Collecting, SubCategory.ExperiencePerEnemy).level).amount * maxHealth;
            GameManager.Instance.archerDataClone.inGameGold += MetaData.instance.GetCtegoryItemAmountLevel(Category.Collecting, SubCategory.GoldPerEnemy, GameManager.Instance.archerDataClone.GetUserSubCategory(Category.Collecting, SubCategory.GoldPerEnemy).level).amount * maxHealth;
            GameManager.Instance.UpdateExperienceAndGold();
            canMove = false;
            animator.Play("Die");
            Destroy(gameObject, 1);
        }
    }

    public JSONObject ToJson()
    {
        JSONObject js = new JSONObject();
        js.Add("health", health);
        js.Add("maxHealth", maxHealth);
        js.Add("damage", damage);
        js.Add("type", type.ToString());
        js.Add("enemyPos", transform.position.ToString());
        return js;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TowerController.instance.gameObject)
        {
            animator.Play("Attack");
            canMove = false;
        }
    }
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public void EnemyAttack()
    {
        TowerController.instance.Damage(damage);
    }


}
