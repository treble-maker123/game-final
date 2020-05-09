using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class SlowUpgrade : MonoBehaviour
{

    public Button upgradeDuration;
    public Button upgradeRange;
    public Button upgradeEffect;
    public Button close;

    private GameObject player;

    //Money
    private GameObject scene;
    private int money;

    //Upgrade Button Logic
    private int upg1Cost;
    private int upg2Cost;
    private int upg3Cost;

    // Use this for initialization
    void Start()
    {
        this.gameObject.SetActive(true);
        scene = GameObject.FindGameObjectWithTag("SC");
        money = scene.GetComponent<GameState>().Gold;

        //Get Cost Values
        initCosts();
        buttonCheck();

        //Get FPS Controller and Disable the Player
        player = GameObject.Find("Player");
        player.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Loaded");

    }

    public void durUpgrade()
    {
        int cost = upg1Cost;
        upg1Cost = upg1Cost * 2;
        string newCost = "$" + upg1Cost.ToString();
        upgradeDuration.GetComponentInChildren<Text>().text = newCost;
        //Upgrade DMG
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<SlowTurret>();
        turretVals.durCost = upg1Cost;
        //DMG
        turretVals.duration = turretVals.duration * 2;
        //Subtract Money
        money = money - cost;
        scene.GetComponent<GameState>().Gold = money;
    }

    public void rangeUpgrade()
    {
        int cost = upg2Cost;
        upg2Cost = upg2Cost * 2;
        string newCost = "$" + upg2Cost.ToString();
        upgradeRange.GetComponentInChildren<Text>().text = newCost;
        //Upgrade range
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<SlowTurret>();
        turretVals.rngCost = upg2Cost;
        //Range
        turretVals.range = turretVals.range * 1.5f;
        //Subtract Money
        money = money - cost;
        scene.GetComponent<GameState>().Gold = money;
    }

    public void effectUpgrade()
    {
        int cost = upg3Cost;
        upg3Cost = upg3Cost * 2;
        string newCost = "$" + upg3Cost.ToString();
        upgradeEffect.GetComponentInChildren<Text>().text = newCost;
        //Upgrade fire rate
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<SlowTurret>();
        turretVals.perCost = upg3Cost;
        //Fire rate
        turretVals.percentage = turretVals.percentage - 0.2f;
        //Subtract money
        money = money - cost;
        scene.GetComponent<GameState>().Gold = money;
    }

    public void closeMenu()
    {
        player = GameObject.Find("Player");
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        Destroy(this.gameObject);
    }

    void buttonCheck()
    {
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<SlowTurret>();

        if (money < upg1Cost) { upgradeDuration.interactable = false; upgradeDuration.GetComponentInChildren<Text>().color = Color.red; }
        else { upgradeDuration.GetComponentInChildren<Text>().color = Color.green; }

        if (money < upg2Cost) { upgradeRange.interactable = false; upgradeRange.GetComponentInChildren<Text>().color = Color.red; }
        else { upgradeRange.GetComponentInChildren<Text>().color = Color.green; }

        if (turretVals.percentage <= 0.3)
        {
            upgradeEffect.interactable = false;
            upgradeEffect.GetComponentInChildren<Text>().color = Color.yellow;
            upgradeEffect.GetComponentInChildren<Text>().text = "Maxed";
        }
        else
        {
            if (money < upg3Cost) { upgradeEffect.interactable = false; upgradeEffect.GetComponentInChildren<Text>().color = Color.red; }
            else { upgradeEffect.GetComponentInChildren<Text>().color = Color.green; }
        }
    }

    void initCosts()
    {
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<SlowTurret>();
        upg1Cost = turretVals.durCost;
        upgradeDuration.GetComponentInChildren<Text>().text = "$" + upg1Cost;
        upg2Cost = turretVals.rngCost;
        upgradeRange.GetComponentInChildren<Text>().text = "$" + upg2Cost;
        upg3Cost = turretVals.perCost;
        upgradeEffect.GetComponentInChildren<Text>().text = "$" + upg3Cost;
        buttonCheck();
    }

    void Update()
    {
        buttonCheck();
    }


}
