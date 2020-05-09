using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

    public Button weapon;
    public Button weaponBack;
    public Button towerNext;
    public Button generalNext;
    public Button towerBack;
    public Button buildNext;
    public Button buildBack;
    public Button upgradeNext;
    public Button upgradeBack;
    public Button mobNext;
    public Button mobBack;
    public Button mainMenu;

    public GameObject startUI;
    public GameObject weaponUI;
    public GameObject towerUI;
    public GameObject generalUI;
    public GameObject buildUI;
    public GameObject upgradeUI;
    public GameObject mobUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

	public void Weapon()
    {
        startUI.SetActive(false);
        weaponUI.SetActive(true);
    }

    public void WeaponBack()
    {
        towerUI.SetActive(false);
        weaponUI.SetActive(true);
    }

    public void Towers()
    {
        weaponUI.SetActive(false);
        towerUI.SetActive(true);
    }

    public void TowerBack()
    {
        generalUI.SetActive(false);
        towerUI.SetActive(true);
    }

    public void General()
    {
        towerUI.SetActive(false);
        generalUI.SetActive(true);
    }

    public void GeneralBack()
    {
        buildUI.SetActive(false);
        generalUI.SetActive(true);
    }

    public void Build()
    {
        generalUI.SetActive(false);
        buildUI.SetActive(true);
    }

    public void BuildBack()
    {
        upgradeUI.SetActive(false);
        buildUI.SetActive(true);
    }

    public void Upgrade()
    {
        buildUI.SetActive(false);
        upgradeUI.SetActive(true);
    }

    public void UpgradeBack()
    {
        mobUI.SetActive(false);
        upgradeUI.SetActive(true);
    }

    public void Mobs()
    {
        upgradeUI.SetActive(false);
        mobUI.SetActive(true);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
