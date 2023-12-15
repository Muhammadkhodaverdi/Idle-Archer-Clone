using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryMenuUI : MonoBehaviour
{
    public Category category;
    public List<SubCategoryMenuUI> subcategories = new List<SubCategoryMenuUI>();

    public void Init(ArcherData data)
    {
        for (int i = 0; i < subcategories.Count; i++)
        {
            subcategories[i].Init(data.GetUserCategory(category)[i]);
        }

    }
}
