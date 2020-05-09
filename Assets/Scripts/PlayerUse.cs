using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PlayerUse : MonoBehaviour {

    public float range = 10f;

    public Camera fpsCamera;
    public Transform[] weapons;
    private int startWeapon = 0;

    public RawImage rifle;
    public RawImage pistol;
    public RawImage heavy;

    void Start()
    {
        FetchIcons();
        WeaponSwap(startWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponSwap(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponSwap(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponSwap(2);
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

            if (hitInfo.transform.name == "SlowTurret(Clone)")
            {
                BuildMenu tile = hitInfo.transform.GetComponent<BuildMenu>();
                GameObject hitObj = hitInfo.transform.gameObject;
                tile.UpgradeSlow(hitObj);
            }
        }
    }

    void WeaponSwap(int activeWep)
    {
        Debug.Log("Switch to: ");
        Debug.Log(activeWep);
        for (int i = 0; i < weapons.Length; i++)
        {
            if (activeWep == i)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    void FetchIcons() {
        Texture2D rifleIcon = AssetPreview.GetAssetPreview(Resources.Load("Guns/Rifle"));
        Texture2D pistolIcon = AssetPreview.GetAssetPreview(Resources.Load("Guns/Preview/PistolPrev"));
        Texture2D heavyIcon = AssetPreview.GetAssetPreview(Resources.Load("Guns/Preview/HeavyPrev"));

        rifle.texture = rifleIcon;
        pistol.texture = pistolIcon;
        heavy.texture = heavyIcon;
    }
}
