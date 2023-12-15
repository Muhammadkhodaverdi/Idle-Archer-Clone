using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    public static TowerController instance;

    public Image healthBar;
    public Image manaBar;
    public Text healthText;

    public float towerHealth;
    public float towerMaxHealth;
    public float towerHealthRegeneration;
    public float towerMana;
    public float towerMaxMana;
    public float towerManaRegeneration;

    private void Awake()
    {
        instance = this;

    }
    public void Init(ArcherData data)
    {

        towerHealth = MetaData.instance.GetCtegoryItemAmountLevel(Category.Healing, SubCategory.Health, data.GetUserSubCategory(Category.Healing, SubCategory.Health).level).amount;
        towerMaxHealth = towerHealth;
        towerHealthRegeneration = MetaData.instance.GetCtegoryItemAmountLevel(Category.Healing, SubCategory.HealthRegenerationSpeed, data.GetUserSubCategory(Category.Healing, SubCategory.HealthRegenerationSpeed).level).amount;
        towerMana = MetaData.instance.GetCtegoryItemAmountLevel(Category.Healing, SubCategory.Mana, data.GetUserSubCategory(Category.Healing, SubCategory.Mana).level).amount;
        towerMaxMana = towerMana;
        towerManaRegeneration = MetaData.instance.GetCtegoryItemAmountLevel(Category.Healing, SubCategory.ManaRegenerationSpeed, data.GetUserSubCategory(Category.Healing, SubCategory.ManaRegenerationSpeed).level).amount;
        ManaBarFiller();
        HealthBarFiller();
    }
    public void Damage(float damage)
    {
        if (towerMana > 0)
        {
            towerMana -= damage;
        }
        if (towerMana <= 0)
        {
            if (towerHealth > 0)
            {
                towerHealth -= damage;
            }
            if (towerHealth <= 0)
            {
                GameManager.Instance.EndGame();
            }
        }

    }

    public void RegenerateHealth()
    {
        if (towerHealth < towerMaxHealth)
        {
            towerHealth += towerHealthRegeneration * Time.deltaTime * 1;
            HealthBarFiller();
        }
    }
    public void RegenerateMana()
    {
        if (towerMana < towerMaxMana)
        {
            towerMana += towerManaRegeneration * Time.deltaTime * 1;
            ManaBarFiller();
        }
    }
    public void HealthBarFiller()
    {
        healthBar.fillAmount = towerHealth / towerMaxHealth;
        healthText.text = towerHealth.ToString("#") + "/" + towerMaxHealth;
    }
    public void ManaBarFiller()
    {
        manaBar.fillAmount = towerMana / towerMaxMana;
    }

    private void Update()
    {
        RegenerateHealth();
        RegenerateMana();
    }
}
