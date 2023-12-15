using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeta : MonoBehaviour
{
    public static EnemyMeta Instance;

    public TextAsset data;

    public List<EnemyData> enemyData;

    private void Awake()
    {
        Instance= this;
    }

    private void Start()
    {
        JSONNode js = JSONNode.Parse(data.text);
        for (int i = 0; i < js.Count; i++)
        {
            enemyData.Add(new EnemyData(js[i]));
        }
    }
    public EnemyData GetEnemyByType(EnemyType _enemyType)
    {
        int b = 0;
        for (int i = 0; i < enemyData.Count; i++)
        {
            if (enemyData[i].enemyType == _enemyType)
            {
                return enemyData[i];
            }
            b = i;
        }
        return enemyData[b];
    }
    public float GetEnemyDamageDataByWawe(EnemyType _enemyType, int _wawe)
    {
        int b = 0;
        for (int i = 0; i < GetEnemyByType(_enemyType).damage.Count; i++)
        {
            if (GetEnemyByType(_enemyType).damage[i].wawe == _wawe)
            {
                return GetEnemyByType(_enemyType).damage[i].amount;
            }
            b = i;
        }
        return GetEnemyByType(_enemyType).damage[b].amount;
    }

    public float GetEnemyHealthDataByWawe(EnemyType _enemyType, int _wawe)
    {
        int b = 0;
        for (int i = 0; i < GetEnemyByType(_enemyType).health.Count; i++)
        {
            if (GetEnemyByType(_enemyType).health[i].wawe == _wawe)
            {
                return GetEnemyByType(_enemyType).health[i].amount;
            }
            b= i;
        }
        return GetEnemyByType(_enemyType).health[b].amount;
    }
}

public enum EnemyType { Skeleton,Spider }

[System.Serializable]
public class EnemyData
{
    public EnemyData()
    {

    }
    public EnemyData(JSONNode js)
    {
        for (int i = 0; i < js["damage"].Count; i++)
        {
            damage.Add(new EnemyDamage(js["damage"][i]));
        }
        for (int i = 0; i < js["health"].Count; i++)
        {
            health.Add(new EnemyHealth(js["health"][i]));
        }
        enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), js["enemyType"].Value);
    }
    public EnemyType enemyType;
    public List<EnemyDamage> damage = new List<EnemyDamage>();
    public List<EnemyHealth> health = new List<EnemyHealth>();
}


[System.Serializable]
public class EnemyDamage
{
    public EnemyDamage()
    {

    }
    public EnemyDamage(JSONNode js)
    {
        wawe = js["wawe"].AsInt;
        amount = js["amount"].AsFloat;

    }
    public int wawe;
    public float amount;
}


[System.Serializable]
public class EnemyHealth
{
    public EnemyHealth()
    {

    }
    public EnemyHealth(JSONNode js)
    {
        wawe = js["wawe"].AsInt;
        amount = js["amount"].AsFloat;

    }
    public int wawe;
    public float amount;
}
