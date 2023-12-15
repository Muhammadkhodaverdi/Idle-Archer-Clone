using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CurrentGameData : MonoBehaviour
{
    public static CurrentGameData Instance;

    public CurrentWawe currentWaweData;


    private void Awake()
    {
        Instance = this;
        //PlayerPrefs.DeleteKey("CurrentWawe");
    }
    public bool Load()
    {
        string loadData = PlayerPrefs.GetString("CurrentWawe", "null");
        if (loadData != "null")
        {
            JSONNode js = JSON.Parse(loadData);
            currentWaweData = new CurrentWawe(js);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Update()
    {
    }

    public void SetData(JSONObject js)
    {
        currentWaweData.Init(js);
        currentWaweData.Save();
    }
    public void DeleteData()
    {
        PlayerPrefs.DeleteKey("CurrentWawe");
        currentWaweData = new CurrentWawe();
        //currentWaweData.currentWawe = 0;
        //currentWaweData.waweEnd= 0;
        //currentWaweData.gold = 0;
        //currentWaweData.exp= 0;
        //currentWaweData.currentArcherData =  new ArcherData();
        //currentWaweData.currentOppsData= new List<CurrentEnemy>();
    }

    public void Resume()
    {
        UICtrl.instance.resumeForm.SetActive(false);

        WaweCtrl.instance.StartGame();
        GameManager.Instance.archerDataClone.Clone(currentWaweData.currentArcherData);

        GameManager.Instance.archerDataClone.Experience = currentWaweData.exp;
        GameManager.Instance.archerDataClone.inGameGold = currentWaweData.gold;
        GameManager.Instance.UpdateExperienceAndGold();

        WaweCtrl.instance.currentWawe = currentWaweData.currentWawe;
        WaweCtrl.instance.waweEnd = currentWaweData.waweEnd;
        WaweCtrl.instance.DestroyAllEnemies();
        for (int i = 0; i < currentWaweData.currentOppsData.Count; i++)
        {
            WaweCtrl.instance.SpawnCustomOpp(currentWaweData.currentOppsData[i].type, currentWaweData.currentOppsData[i].health, currentWaweData.currentOppsData[i].maxHealth, currentWaweData.currentOppsData[i].damage, currentWaweData.currentOppsData[i].enemyPos);
        }

        GameManager.Instance.InitArcher();

        UICtrl.instance.GoGamePlay();

        DeleteData();
    }

    public void Leave()
    {
        UICtrl.instance.resumeForm.SetActive(false);
        DeleteData();
        GameManager.Instance.StartGame();
    }

}

[System.Serializable]
public class CurrentWawe
{
    public CurrentWawe()
    {

    }

    public CurrentWawe(int _currentWawe, float _waweEnd, ArcherData _currentArcherData, JSONNode js)
    {
        currentWawe = _currentWawe;
        waweEnd = _waweEnd;
        currentArcherData = _currentArcherData;
        exp = _currentArcherData.Experience;
        gold = _currentArcherData.inGameGold;
    }

    public CurrentWawe(JSONNode js)
    {
        currentWawe = js["waweData"]["currentWawe"].AsInt;
        waweEnd = js["waweData"]["waweEnd"].AsFloat;
        gold = js["gold"].AsFloat;
        exp = js["exp"].AsFloat;
        currentArcherData = new ArcherData(js["currentArcherData"]);
        for (int i = 0; i < js["currentOppsData"].Count; i++)
        {
            currentOppsData.Add(new CurrentEnemy(js["currentOppsData"][i]));
        }
    }
    public void Init(JSONObject js)
    {
        currentWawe = js["waweData"]["currentWawe"].AsInt;
        waweEnd = js["waweData"]["waweEnd"].AsFloat;
        gold = js["gold"].AsFloat;
        exp = js["exp"].AsFloat;
        currentArcherData = new ArcherData(js["currentArcherData"]);
        for (int i = 0; i < currentOppsData.Count; i++)
        {
            currentOppsData.Remove(currentOppsData[i]);
        }
        for (int i = 0; i < js["currentOppsData"].Count; i++)
        {
            currentOppsData.Add(new CurrentEnemy(js["currentOppsData"][i]));
        }
    }

    public JSONObject ToJson()
    {
        JSONObject js = new JSONObject();
        JSONObject waweData = new JSONObject();
        waweData.Add("currentWawe", currentWawe);
        waweData.Add("waweEnd", waweEnd);
        js.Add("waweData", waweData);
        js.Add("exp", exp);
        js.Add("gold", gold);
        js.Add("currentArcherData", currentArcherData.ToJson());
        JSONArray jsArray = new JSONArray();
        for (int i = 0; i < currentOppsData.Count; i++)
        {
            jsArray.Add(currentOppsData[i].ToJson());
        }
        js.Add("currentOppsData", jsArray);

        return js;
    }

    public void Save()
    {
        //exp = currentArcherData.Experience;
        //gold = currentArcherData.inGameGold;
        PlayerPrefs.SetString("CurrentWawe", ToJson().ToString());
    }
    public int currentWawe;
    public float waweEnd;
    public float exp;
    public float gold;
    public ArcherData currentArcherData;
    public List<CurrentEnemy> currentOppsData = new List<CurrentEnemy>();
}

[System.Serializable]
public class CurrentEnemy
{

    public float health;
    public float maxHealth;
    public float damage;

    public EnemyType type;

    public Vector3 enemyPos;


    public CurrentEnemy()
    {

    }
    public CurrentEnemy(JSONNode js)
    {
        health = js["health"].AsFloat;
        maxHealth = js["maxHealth"].AsFloat;
        damage = js["damage"].AsFloat;

        type = (EnemyType)Enum.Parse(typeof(EnemyType), js["type"].Value);
        enemyPos = StringToVector3(js["enemyPos"].Value.ToString());
    }

    public JSONObject ToJson()
    {
        JSONObject js = new JSONObject();
        js.Add("health", health);
        js.Add("maxHealth", maxHealth);
        js.Add("damage", damage);
        js.Add("type", type.ToString());
        js.Add("enemyPos", enemyPos.ToString());
        return js;
    }
    public static Vector3 StringToVector3(string sVector)
    {
        Debug.Log(sVector);
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


}