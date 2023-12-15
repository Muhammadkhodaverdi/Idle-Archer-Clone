using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaweCtrl;

public class WaweCtrl : MonoBehaviour
{
    public static WaweCtrl instance;
    public TextAsset data;
    public List<Transform> spawnPoints = new List<Transform>();

    public float x;
    public float spawnTime;

    public List<WaweData> waweDatas;

    public int currentWawe;
    public float waweEnd;
    public float waweDuration;

    public EnemyPrafabs enemyPrefabs;

    private bool endGame;

    public AnimationCurve spawnCurve;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        JSONNode js = JSONNode.Parse(data.text);
        for (int i = 0; i < js.Count; i++)
        {
            waweDatas.Add(new WaweData(js[i]));
        }
    }
    private void Update()
    {
        if (!endGame)
        {
            if (x <= 0)
            {
                SpawnRandomOpp();
                x = spawnTime;
            }
            x -= Time.deltaTime;

            if (waweEnd <= 0)
            {
                ChangeWawe();
                waweEnd = waweDuration;
            }
            waweEnd -= Time.deltaTime;
            UICtrl.instance.WaweInit();
        }

    }

    public void StartGame()
    {
        endGame = false;
        DestroyAllEnemies();
        currentWawe = 0;
        waweEnd = 0;
    }

    public void DestroyAllEnemies()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            for (int b = 0; b < spawnPoints[i].childCount; b++)
            {
                Destroy(spawnPoints[i].GetChild(b).gameObject);
            }
        }
    }

    public void EndGame()
    {
        endGame = true;
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            for (int b = 0; b < spawnPoints[i].childCount; b++)
            {
                Destroy(spawnPoints[i].GetChild(b).gameObject);
            }
        }
        currentWawe = 0;
        waweEnd = 0;
    }

    [System.Serializable]
    public class ChoseSpawnPoint
    {

        public List<int> posible = new List<int>();
        public void init()
        {
            posible.AddElemnt(1, 0);
            posible.AddElemnt(2, 1);
            posible.AddElemnt(2, 2);
            posible.AddElemnt(3, 3);
            posible.AddElemnt(4, 4);
            posible.AddElemnt(3, 5);
            posible.AddElemnt(1, 6);
        }
        public int Chose()
        {
            if (posible.Count == 0) init();

            int rnd = UnityEngine.Random.Range(0, posible.Count);
            int res = posible[rnd];
            posible.RemoveAt(rnd);
            return res;
        }
    }
    public ChoseSpawnPoint choseSpawnPoint = new ChoseSpawnPoint();

    public class ChoseEnemy
    {
        public List<EnemyType> posible = new List<EnemyType>();

        public void Init()
        {
            posible.AddEnumElement(3, EnemyType.Skeleton);
            posible.AddEnumElement(1, EnemyType.Spider);
        }

        public EnemyType Chose()
        {
            if (posible.Count == 0)
            {
                Init();
            }
            int randomChose = UnityEngine.Random.Range(0, posible.Count);
            EnemyType enemyChose = posible[randomChose];
            posible.RemoveAt(randomChose);
            return enemyChose;
        }
    }
    public ChoseEnemy choseEnemy = new ChoseEnemy();
    public GameObject ChoseRandomEnemy()
    {
        EnemyType spawnEnemy = choseEnemy.Chose();
        for (int i = 0; i < enemyPrefabs.enemyPrefabList.Count; i++)
        {
            if (enemyPrefabs.enemyPrefabList[i].type == spawnEnemy)
            {
                return enemyPrefabs.enemyPrefabList[i].prefab;
            }
        }
        return null;
    }
    public GameObject ChoseCustomEnemy(EnemyType _type)
    {
        for (int i = 0; i < enemyPrefabs.enemyPrefabList.Count; i++)
        {
            if (enemyPrefabs.enemyPrefabList[i].type == _type)
            {
                return enemyPrefabs.enemyPrefabList[i].prefab;
            }
        }
        return null;
    }
    public void SpawnRandomOpp()
    {
        Transform finalSpawnPoint = spawnPoints[choseSpawnPoint.Chose()];

        GameObject obj = Instantiate(ChoseRandomEnemy(), finalSpawnPoint.position, Quaternion.identity);
        obj.transform.SetParent(finalSpawnPoint);
        obj.GetComponent<Enemy>().Init(currentWawe);
    }

    public void SpawnCustomOpp(EnemyType type, float health, float maxHealth, float damage, Vector3 spawnPos)
    {
        GameObject obj = Instantiate(ChoseCustomEnemy(type), spawnPos, Quaternion.identity);
        obj.GetComponent<Enemy>().Init(health, maxHealth, damage);
    }
    public void ChangeWawe()
    {
        currentWawe += 1;
        waweDuration = GetWaweDuration(currentWawe);
    }

    public float GetWaweDuration(int _wawe)
    {
        for (int i = 0; i < waweDatas.Count; i++)
        {
            if (waweDatas[i].wawe == _wawe)
            {
                return waweDatas[i].duration;
            }
        }
        return 0;
    }

    public JSONObject ToJson()
    {
        JSONObject js = new JSONObject();

        js.Add("currentWawe", currentWawe);
        js.Add("waweEnd", waweEnd);

        return js;
    }
}

[System.Serializable]
public class WaweData
{
    public WaweData()
    {

    }
    public WaweData(JSONNode js)
    {
        wawe = js["wawe"].AsInt;
        duration = js["duration"].AsInt;
    }

    public int wawe;
    public float duration;
}
