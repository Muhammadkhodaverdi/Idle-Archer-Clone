using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryUI : MonoBehaviour
{
    public Category category;
    public List<SubCategoryUI> subCategories = new List<SubCategoryUI>();

    public void Init(ArcherData data)
    {
        for (int i = 0; i < subCategories.Count; i++)
        {
            subCategories[i].Init(data.GetUserCategory(category)[i]);
        }
    }
}
