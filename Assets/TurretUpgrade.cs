using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TurretUpgrade : MonoBehaviour {

    public Button upgradeDamage;
    public Button upgradeRange;
    public Button rateOfFire;
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
    void Start () {
        this.gameObject.SetActive(true);
        scene = GameObject.FindGameObjectWithTag("SC");
        money = scene.GetComponent<GameState>().StartingGold;

        //Get Cost Values
        initCosts();

        //Get FPS Controller and Disable the Player
        player = GameObject.Find("Player");
        player.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Loaded");

    }

    public void dmgUpgrade()
    {
        int cost = upg1Cost;
        upg1Cost = upg1Cost * 2;
        string newCost = "$" + upg1Cost.ToString();
        upgradeDamage.GetComponentInChildren<Text>().text = newCost;
        //Upgrade DMG
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<Turret>();
        turretVals.dmgCost = upg1Cost;
        //DMG
        turretVals.dmg = turretVals.dmg * 2;
        //Subtract Money
        money = money - cost;
        scene.GetComponent<GameState>().StartingGold = money;
    }

    public void rangeUpgrade()
    {
        int cost = upg2Cost;
        upg2Cost = upg2Cost * 2;
        string newCost = "$" + upg2Cost.ToString();
        upgradeRange.GetComponentInChildren<Text>().text = newCost;
        //Upgrade range
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<Turret>();
        turretVals.rngCost = upg2Cost;
        //Range
        turretVals.range = turretVals.range * 2;
        //Subtract Money
        money = money - cost;
        scene.GetComponent<GameState>().StartingGold = money;
    }

    public void fireUpgrade()
    {
        int cost = upg3Cost;
        upg3Cost = upg3Cost * 2;
        string newCost = "$" + upg3Cost.ToString();
        rateOfFire.GetComponentInChildren<Text>().text = newCost;
        //Upgrade fire rate
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<Turret>();
        turretVals.frCost = upg3Cost;
        //Fire rate
        turretVals.fireRate = turretVals.fireRate * 2;
        //Subtract money
        money = money - cost;
        scene.GetComponent<GameState>().StartingGold = money;
    }

    public void closeMenu()
    {
        player = GameObject.Find("Player");
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        Destroy(this.gameObject);
    }
    
    void buttonCheck()
    {
        if (money < upg1Cost) { upgradeDamage.interactable = false; upgradeDamage.GetComponentInChildren<Text>().color = Color.red; }
        else { upgradeDamage.GetComponentInChildren<Text>().color = Color.green; }

        if (money < upg2Cost) { upgradeRange.interactable = false; upgradeRange.GetComponentInChildren<Text>().color = Color.red; }
        else { upgradeRange.GetComponentInChildren<Text>().color = Color.green; }

        if (money < upg3Cost) { rateOfFire.interactable = false; rateOfFire.GetComponentInChildren<Text>().color = Color.red; }
        else { rateOfFire.GetComponentInChildren<Text>().color = Color.green; }
    }

    void initCosts()
    {
        var turret = BuildMenu.building;
        var turretVals = turret.GetComponent<Turret>();
        upg1Cost = turretVals.dmgCost;
        upgradeDamage.GetComponentInChildren<Text>().text = "$" + upg1Cost;
        upg2Cost = turretVals.rngCost;
        upgradeRange.GetComponentInChildren<Text>().text = "$" + upg2Cost;
        upg3Cost = turretVals.frCost;
        rateOfFire.GetComponentInChildren<Text>().text = "$" + upg3Cost;
        buttonCheck();
    }

    void Update()
    {
        buttonCheck();
    }


}
