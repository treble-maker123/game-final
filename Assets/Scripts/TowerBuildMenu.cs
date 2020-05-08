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
        totalMoney = scene.GetComponent<GameState>().Gold;
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
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        Destroy(this.gameObject);
    }

    public void buildReg()
    {
        var buildable = BuildMenu.buildLoc;
        Object tur = Resources.Load("RegTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Subtract Money
        scene.GetComponent<GameState>().Gold = scene.GetComponent<GameState>().Gold - 100;

        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        Destroy(this.gameObject);
    }

    public void buildBomb()
    {
        var buildable = BuildMenu.buildLoc;
        Object tur = Resources.Load("AirTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Money
        scene.GetComponent<GameState>().Gold = scene.GetComponent<GameState>().Gold - 300;

        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        Destroy(this.gameObject);
    }

    public void buildSlow()
    {
        var buildable = BuildMenu.buildLoc;
        Object tur = Resources.Load("SlowTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Money
        scene.GetComponent<GameState>().Gold = scene.GetComponent<GameState>().Gold - 200;

        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        Destroy(this.gameObject);
    }

}
