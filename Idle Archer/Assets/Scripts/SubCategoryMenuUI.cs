using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubCategoryMenuUI : MonoBehaviour
{
    public Text subCategoryName;
    public Text amount;
    public Text upCostTxt;

    UserSubCategoryItem _data;
    public void Init(UserSubCategoryItem data)
    {
        _data = data;
        subCategoryName.text = data.subCategory.ToString();
        amount.text = MetaData.instance.GetCtegoryItemAmountLevel(data.category, data.subCategory, data.level).amount.ToString();
        upCostTxt.text = MetaData.instance.GetCtegoryItemUpgradeCostLevel(data.category, data.subCategory, data.level).amount.ToString();
    }

    public void Upgrade()
    {
        if (UserCtrl.instance.ArcherData.gold >= MetaData.instance.GetCtegoryItemUpgradeCostLevel(_data.category, _data.subCategory, _data.level).amount)
        {
            UserCtrl.instance.ArcherData.gold -= MetaData.instance.GetCtegoryItemUpgradeCostLevel(_data.category, _data.subCategory, _data.level).amount;
            UserCtrl.instance.ArcherData.GetUserSubCategory(_data.category, _data.subCategory).level += 1;
            UserCtrl.instance.ArcherData.Save();
            UICtrl.instance.InitCategoryMenu();
        }
    }
}
