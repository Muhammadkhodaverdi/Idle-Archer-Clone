using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class MetaData : MonoBehaviour
{
    public static MetaData instance;

    public TextAsset metaData;

    public List<SubCategoryItem> subCategoryItems = new List<SubCategoryItem>();

    private void Awake()
    {
        instance = this;
        Init();
    }
    public void Init()
    {
        JSONNode  js = JSONNode.Parse(metaData.text);
        for (int i = 0; i < js.Count; i++)
        {
            subCategoryItems.Add(new SubCategoryItem(js[i]));
        }
    }

    public List<SubCategoryItem> GetCategoryItems(Category _category)
    {
        List<SubCategoryItem> categoryItems = new List<SubCategoryItem>();
        for (int i = 0; i < subCategoryItems.Count; i++)
        {
            if (subCategoryItems[i].category == _category)
            {
                categoryItems.Add(subCategoryItems[i]);
            }
        }
        return categoryItems;
    }

    public SubCategoryItem GetCategoryItem(Category _category, SubCategory _subCategory)
    {
        int b = 0;
        for (int i = 0; i < GetCategoryItems(_category).Count; i++)
        {
            if (GetCategoryItems(_category)[i].subCategory == _subCategory)
            {
                return GetCategoryItems(_category)[i];
            }
            b = i;
        }
        return GetCategoryItems(_category)[b];
    }

    public Amount GetCtegoryItemAmountLevel(Category _category, SubCategory _subCategory,int lvl)
    {
        int b = 0;
        for (int i = 0; i < GetCategoryItem(_category,_subCategory).amount.Count; i++)
        {
            if (GetCategoryItem(_category, _subCategory).amount[i].level == lvl)
            {
                return GetCategoryItem(_category, _subCategory).amount[i];
            }
            b = i;
        }
        return GetCategoryItem(_category, _subCategory).amount[b];
    }

    public UpgradeCost GetCtegoryItemUpgradeCostLevel(Category _category, SubCategory _subCategory, int lvl)
    {
        int b = 0;
        for (int i = 0; i < GetCategoryItem(_category, _subCategory).upgradeCost.Count; i++)
        {
            if (GetCategoryItem(_category, _subCategory).upgradeCost[i].level == lvl)
            {
                return GetCategoryItem(_category, _subCategory).upgradeCost[i];
            }
            b = i;
        }
        return GetCategoryItem(_category, _subCategory).upgradeCost[b];
    }

}
public enum Category { Attacking, Healing, Collecting }
public enum SubCategory
{
    //Attacking
    Damage,
    AttackSpeed,
    AttackRange,

    //Healing
    Health,
    HealthRegenerationSpeed,
    Mana,
    ManaRegenerationSpeed,

    //Collecting
    ExperiencePerEnemy,
    GoldPerEnemy
}
[System.Serializable]
public class SubCategoryItem
{
    public SubCategoryItem()
    {

    }
    public SubCategoryItem(JSONNode js)
    {
        category = (Category)Enum.Parse(typeof(Category), js["category"].Value);
        subCategory = (SubCategory)Enum.Parse(typeof(SubCategory), js["subCategory"].Value);
        for (int i = 0; i < js["amount"].Count; i++)
        {
            amount.Add(new Amount(js["amount"][i]));
        }
        for (int i = 0; i < js["upgradeCost"].Count; i++)
        {
            upgradeCost.Add(new UpgradeCost(js["upgradeCost"][i]));
        }
    }
    public Category category;
    public SubCategory subCategory;
    public List<Amount> amount = new List<Amount>();
    public List<UpgradeCost> upgradeCost = new List<UpgradeCost>();
}

[System.Serializable]
public class Amount
{
    public Amount()
    {

    }
    public Amount(JSONNode js)
    {
        level = js["level"].AsInt;
        amount = js["amount"].AsFloat;
    }
    public int level;
    public float amount;
}

[System.Serializable]
public class UpgradeCost
{
    public UpgradeCost()
    {

    }
    public UpgradeCost(JSONNode js)
    {
        level = js["level"].AsInt;
        amount = js["amount"].AsFloat;
    }
    public int level;
    public float amount;
}
