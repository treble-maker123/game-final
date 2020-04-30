﻿using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TowerBuildMenu : MonoBehaviour {

    public int totalMoney = 200;
    public Button reg;
    public Button bomb;
    public Button slow;
    public Button close;

	// Use this for initialization
	void Start () {
        //Get FPS Controller and Disable the Player

        //Regular Tower
        if (totalMoney >= 100)
        {
            reg.GetComponentInChildren<Text>().color = Color.green;
        }
        else
        {
            reg.interactable = false;
        }
        //Bomb Tower
        if (totalMoney >= 300)
        {
            bomb.GetComponentInChildren<Text>().color = Color.green;
        }
        else
        {
            bomb.interactable = false;
        }
        //Slow Tower
        if (totalMoney >= 200)
        {
            slow.GetComponentInChildren<Text>().color = Color.green;
        }
        else
        {
            slow.interactable = false;
        }
    }

    public void closeMenu()
    {
        this.gameObject.SetActive(false);
        //Enable FPS Controller
    }

    public void buildReg()
    {
        var buildable = BuildMenu.buildLoc;
        //Debug.Log("Building Regular!");
        Object tur = Resources.Load("RegTurret");
        Instantiate(tur, buildable.transform.position, buildable.transform.rotation);
        Destroy(buildable);
        //Subtract Money
        this.gameObject.SetActive(false);
    }

    public void buildBomb()
    {
        var buildable = BuildMenu.buildLoc;
        //Debug.Log("Building Bomb!");
        Object tur = Resources.Load("BombTurret");
        Instantiate(tur, buildable.transform.position, buildable.transform.rotation);
        Destroy(buildable);
        //Money
        this.gameObject.SetActive(false);
    }

    public void buildSlow()
    {
        var buildable = BuildMenu.buildLoc;
        //Debug.Log("Building Slow!");
        Object tur = Resources.Load("SlowTurret");
        Instantiate(tur, buildable.transform.position, buildable.transform.rotation);
        Destroy(buildable);
        //Money
        this.gameObject.SetActive(false);
    }

}