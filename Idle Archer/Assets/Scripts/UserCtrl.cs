using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserCtrl : MonoBehaviour
{
    public static UserCtrl instance;

    public TextAsset data;
    public ArcherData ArcherData;

    private void Awake()
    {
        instance = this;
        Load();
    }
    public void Load()
    {
        //PlayerPrefs.DeleteKey("Archer");
        string loadData = PlayerPrefs.GetString("Archer", "null");
        if (loadData != "null")
        {
            JSONNode js = JSON.Parse(loadData);
            ArcherData = new ArcherData(js);
        }
        else
        {
            JSONNode js = JSONNode.Parse(data.text);
            ArcherData = new ArcherData(js);
        }
    }
}
[System.Serializable]
public class ArcherData
{
    public ArcherData()
    {

    }
    public ArcherData(JSONNode js)
    {
        for (int i = 0; i < js["userSubCategory"].Count; i++)
        {
            userSubCategoryItemslvl.Add(new UserSubCategoryItem(js["userSubCategory"][i]));
        }
        gold = js["gold"].AsFloat;
    }
    public ArcherData(ArcherData data)
    {
        userSubCategoryItemslvl = data.userSubCategoryItemslvl;
        gold = data.gold;
    }
    public JSONObject ToJson()
    {
        JSONArray jsArray = new JSONArray();
        for (int i = 0; i < userSubCategoryItemslvl.Count; i++)
        {
            jsArray.Add(userSubCategoryItemslvl[i].ToJson());
        }
        JSONObject js = new JSONObject();
        js.Add("userSubCategory", jsArray);
        js.Add("gold", gold);
        return js;
    }

    public void Save()
    {
        PlayerPrefs.SetString("Archer", ToJson().ToString());
    }

    public void Clone(ArcherData data)
    {
        userSubCategoryItemslvl = new List<UserSubCategoryItem>();
        for (int i = 0; i < data.userSubCategoryItemslvl.Count; i++)
        {
            userSubCategoryItemslvl.Add(new UserSubCategoryItem());
        }
        for (int i = 0; i < userSubCategoryItemslvl.Count; i++)
        {
            userSubCategoryItemslvl[i].Clone(data.userSubCategoryItemslvl[i]);
        }

        inGameGold = data.inGameGold;
        Experience = data.Experience;
        gold = data.gold;
    }
    public List<UserSubCategoryItem> GetUserCategory(Category _category)
    {
        List<UserSubCategoryItem> categoryItems = new List<UserSubCategoryItem>();
        for (int i = 0; i < userSubCategoryItemslvl.Count; i++)
        {
            if (userSubCategoryItemslvl[i].category == _category)
            {
                categoryItems.Add(userSubCategoryItemslvl[i]);
            }
        }
        return categoryItems;
    }

    public UserSubCategoryItem GetUserSubCategory(Category _category, SubCategory _subCategory)
    {
        for (int i = 0; i < GetUserCategory(_category).Count; i++)
        {
            if (GetUserCategory(_category)[i].subCategory == _subCategory)
            {
                return GetUserCategory(_category)[i];
            }
        }
        return null;
    }

    public List<UserSubCategoryItem> userSubCategoryItemslvl = new List<UserSubCategoryItem>();
    public float Experience;
    public float inGameGold;
    public float gold;
}

[System.Serializable]
public class UserSubCategoryItem
{
    public UserSubCategoryItem()
    {

    }

    public UserSubCategoryItem(JSONNode js)
    {
        category = (Category)Enum.Parse(typeof(Category), js["category"].Value);
        subCategory = (SubCategory)Enum.Parse(typeof(SubCategory), js["subCategory"].Value);
        level = js["level"].AsInt;
    }

    public JSONObject ToJson()
    {
        JSONObject js = new JSONObject();
        js.Add("category", category.ToString());
        js.Add("subCategory", subCategory.ToString());
        js.Add("level", level);

        return js;
    }
    public void Clone(UserSubCategoryItem data)
    {
        category = data.category;
        subCategory = data.subCategory;
        level = data.level;

    }
    public Category category;
    public SubCategory subCategory;
    public int level;
    public int upgradeTimes;
}