using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TowerBuildMenu : MonoBehaviour {

    public int totalMoney = 600;
    public Button reg;
    public Button bomb;
    public Button slow;
    public Button close;

    private GameObject player;

	// Use this for initialization
	void Start () {
        Debug.Log(totalMoney);
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
        Debug.Log("Building Regular!");
        Object tur = Resources.Load("RegTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Subtract Money
        this.gameObject.SetActive(false);
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
    }

    public void buildBomb()
    {
        var buildable = BuildMenu.buildLoc;
        //Debug.Log("Building Bomb!");
        Object tur = Resources.Load("BombTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Money
        this.gameObject.SetActive(false);
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
    }

    public void buildSlow()
    {
        var buildable = BuildMenu.buildLoc;
        Debug.Log("Building Slow!");
        Object tur = Resources.Load("SlowTurret");
        Instantiate(tur, buildable.transform.position + new Vector3(0f, 0.5f, 0), buildable.transform.rotation);
        buildable.name = "Tile";
        //Money
        this.gameObject.SetActive(false);
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
    }

}
