using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPrefabs",menuName ="ScriptableObject")]
public class EnemyPrafabs : ScriptableObject
{


    public List<EnemyPrefab> enemyPrefabList;

    [System.Serializable]
    public class EnemyPrefab
    {
        public EnemyType type;
        public GameObject prefab;
    }
}
