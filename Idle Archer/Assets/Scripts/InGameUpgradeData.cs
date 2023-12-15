using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUpgradeData : MonoBehaviour
{
    public static InGameUpgradeData Instance;
    public TextAsset data;

    public UpgradeData upgradeData;

    private void Awake()
    {
        Instance= this;
    }
    private void Start()
    {
        JSONNode js = JSONNode.Parse(data.text);
        upgradeData = new UpgradeData(js);
    }

    public int GetUpToNextLevelCostByUpgradeTimes(int upgTimes)
    {
        for (int i = 0; i < upgradeData.upgradeValueList.Count; i++)
        {
            if (upgradeData.upgradeValueList[i].upgradeTimes == upgTimes)
            {
                return upgradeData.upgradeValueList[i].upToNextLevelCost;
            }
        }
        return 0;
    }
}

[System.Serializable]
public class UpgradeData
{
    public UpgradeData()
    {

    }

    public UpgradeData(JSONNode js)
    {
        for (int i = 0; i < js.Count; i++)
        {
            upgradeValueList.Add(new UpgradeValue(js[i]));
        }
    }
    public List<UpgradeValue> upgradeValueList = new List<UpgradeValue>();
}

[System.Serializable]
public class UpgradeValue
{
    public UpgradeValue()
    {

    }
    public UpgradeValue(JSONNode js)
    {
        upgradeTimes = js["upgradeTimes"].AsInt;
        upToNextLevelCost = js["upToNextLevelCost"].AsInt;
    }
    public int upgradeTimes;
    public int upToNextLevelCost;
}
