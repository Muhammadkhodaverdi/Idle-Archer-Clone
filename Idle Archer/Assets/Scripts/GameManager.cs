using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ArcherData archerDataClone;

    public Text experienceTxt;
    public Text inGameGoldTxt;



    private void Awake()
    {
        Instance = this;
    }
    public void StartGame()
    {
        if (CurrentGameData.Instance.Load())
        {
            UICtrl.instance.resumeForm.SetActive(true);
        }
        else
        {
            archerDataClone.Clone(UserCtrl.instance.ArcherData);
            UICtrl.instance.GoGamePlay();
            WaweCtrl.instance.StartGame();
            InitArcher();
            UpdateExperienceAndGold();

        }
    }

    public void InitArcher()
    {
        Archer.Instance.Init(archerDataClone);
        TowerController.instance.Init(archerDataClone);
        UICtrl.instance.InitCategory();
    }

    public void UpdateExperienceAndGold()
    {
        experienceTxt.text = archerDataClone.Experience.ToString("0.0");
        inGameGoldTxt.text = archerDataClone.inGameGold.ToString("0.0");
    }

    public void EndGame()
    {
        CurrentGameData.Instance.DeleteData();
        UserCtrl.instance.ArcherData.gold += archerDataClone.inGameGold;
        UserCtrl.instance.ArcherData.Save();
        UICtrl.instance.GoMenu();
        WaweCtrl.instance.EndGame();

    }
    public void Pause()
    {
        List<GameObject> currentOppsObj = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        JSONObject js = new JSONObject();
        JSONArray jsArray = new JSONArray();
        for (int i = 0; i < currentOppsObj.Count; i++)
        {
            jsArray.Add(currentOppsObj[i].GetComponent<Enemy>().ToJson());
        }
        js.Add("currentOppsData", jsArray);
        js.Add("currentArcherData", archerDataClone.ToJson());
        js.Add("exp", archerDataClone.Experience);
        js.Add("gold", archerDataClone.inGameGold);
        js.Add("waweData", WaweCtrl.instance.ToJson());
        CurrentGameData.Instance.SetData(js);
        UICtrl.instance.GoMenu();
        WaweCtrl.instance.EndGame();
    }
}
