using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUse : MonoBehaviour {

    public float range = 10f;

    public Camera fpsCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Interact();
        }
    }

    void Interact()
    {

        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hitInfo, range))
        {
            //This is where we'll check if it's a hittable game object that can take damage.
            if (hitInfo.transform.name == "BuildTile")
            {
                BuildMenu tile = hitInfo.transform.GetComponent<BuildMenu>();
                GameObject hitObj = hitInfo.transform.gameObject;
                tile.Menu(hitObj);
            }

            if (hitInfo.transform.name == "RegTurret(Clone)" || hitInfo.transform.name == "AirTurret(Clone)")
            {
                BuildMenu tile = hitInfo.transform.GetComponent<BuildMenu>();
                GameObject hitObj = hitInfo.transform.gameObject;
                tile.Upgrade(hitObj);
            }
        }
    }
}
