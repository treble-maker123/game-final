using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableTile : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        this.gameObject.name = "BuildTile";
        this.gameObject.AddComponent<BuildMenu>();
    }

}
