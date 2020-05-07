using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour {

    public static GameObject buildLoc;
    public static GameObject building;

	public void Menu(GameObject obj)
    {
        buildLoc = obj;
        Object e = Resources.Load("TBMenu");
        Instantiate(e);
    }

    public void Upgrade(GameObject obj)
    {
        building = obj;
        Object e = Resources.Load("UGMenu");
        Instantiate(e);
    }
}
