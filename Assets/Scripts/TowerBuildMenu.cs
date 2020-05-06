using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TowerBuildMenu : MonoBehaviour {

    public int totalMoney;
    public Button reg;
    public Button bomb;
    public Button slow;
    public Button close;

    private GameObject player;
    private GameObject scene;

    void Start()
    {
        scene = GameObject.FindGameObjectWithTag("SC");
        totalMoney = scene.GetComponent<GameState>().StartingGold;
        //Get FPS Controller and Disable the Player
        player = GameObject.Find("Player");
        player.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
    }

    public void buildReg()
    {
        var buildable = BuildMenu.buildLoc;
        Object tur = Resources.Load("RegTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Subtract Money
        scene.GetComponent<GameState>().StartingGold = scene.GetComponent<GameState>().StartingGold - 100;

        this.gameObject.SetActive(false);
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
    }

    public void buildBomb()
    {
        var buildable = BuildMenu.buildLoc;
        Object tur = Resources.Load("BombTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Money
        scene.GetComponent<GameState>().StartingGold = scene.GetComponent<GameState>().StartingGold - 300;

        this.gameObject.SetActive(false);
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
    }

    public void buildSlow()
    {
        var buildable = BuildMenu.buildLoc;
        Object tur = Resources.Load("SlowTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Money
        scene.GetComponent<GameState>().StartingGold = scene.GetComponent<GameState>().StartingGold - 200;

        this.gameObject.SetActive(false);
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
    }

}
