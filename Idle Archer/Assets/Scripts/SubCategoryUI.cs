using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SubCategoryUI : MonoBehaviour
{
    public Text subCategoryName;
    public Text amount;
    public Text upCostTxt;

    public int upgradeTimes = 0;

    private UserSubCategoryItem _data;

    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }
    public void Init(UserSubCategoryItem data)
    {
        _data = data;
        subCategoryName.text = data.subCategory.ToString();
        amount.text = MetaData.instance.GetCtegoryItemAmountLevel(data.category, data.subCategory, data.level).amount.ToString();
        upCostTxt.text = InGameUpgradeData.Instance.GetUpToNextLevelCostByUpgradeTimes(upgradeTimes).ToString();
    }

    public void Upgrade()
    {
        if (_gameManager.archerDataClone.Experience >= InGameUpgradeData.Instance.GetUpToNextLevelCostByUpgradeTimes(upgradeTimes))
        {

            _gameManager.archerDataClone.Experience -= InGameUpgradeData.Instance.GetUpToNextLevelCostByUpgradeTimes(upgradeTimes);
            _gameManager.UpdateExperienceAndGold();
            upgradeTimes += 1;
            _gameManager.archerDataClone.GetUserSubCategory(_data.category, _data.subCategory).level += 1;
            _gameManager.InitArcher();

        }
        else
        {
            Debug.Log("Exp is not enough");
        }
    }


}

public static class ExtensionMethods
{ 
    public static string ToDate(this int x)
    {
        int total = x;
        int h = total / 86400;
        total = total % 86400;


        int m = total / 3600;
        total = total % 3600;

        return $"{h}h :{m}m: {total}s";
    }


    public static void AddElemnt(this List<int> ls,int count,int value)
    {
        for (int i = 0; i < count; i++)
        {
            ls.Add(value);
        }
    }

    public static void AddEnumElement(this List<EnemyType> ls, int count, EnemyType type)
    {
        for (int i = 0; i < count; i++)
        {
            ls.Add(type);
        }
    }
}