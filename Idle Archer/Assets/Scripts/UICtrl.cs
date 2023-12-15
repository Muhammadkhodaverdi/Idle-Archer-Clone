using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    public static UICtrl instance;
    public GameObject menuRoot;
    public GameObject gamePlayRoot;
    public GameObject GamePlayUI;

    public Text GoldTxt;

    public List<CategoryUI> categoryUI;

    //Wawe UI
    public Text currentWaweTxt;
    public Text WaweTimer;

    public GameObject resumeForm;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GoMenu();
    }
    public void GoMenu()
    {
        InitCategoryMenu();
        menuRoot.SetActive(true);
        gamePlayRoot.SetActive(false);
        GamePlayUI.SetActive(false);
    }

    public void GoGamePlay()
    {
        menuRoot.SetActive(false);
        gamePlayRoot.SetActive(true);
        GamePlayUI.SetActive(true);
        resumeForm.SetActive(false);
    }




    public void WaweInit()
    {
        currentWaweTxt.text = "Wawe : " + WaweCtrl.instance.currentWawe.ToString();
        WaweTimer.text = "Time : " + WaweCtrl.instance.waweEnd.ToString("#") + "s";
    }


    public void InitCategory()
    {
        for (int i = 0; i < categoryUI.Count; i++)
        {
            categoryUI[i].Init(GameManager.Instance.archerDataClone);
        }
    }

    public List<CategoryMenuUI> categoryMenuUIList = new List<CategoryMenuUI>();
    public void InitCategoryMenu()
    {
        GoldTxt.text = UserCtrl.instance.ArcherData.gold.ToString();
        for (int i = 0; i < categoryMenuUIList.Capacity; i++)
        {
            categoryMenuUIList[i].Init(UserCtrl.instance.ArcherData);
        }
    }
}
