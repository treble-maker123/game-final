using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableTile : MonoBehaviour
{

    public GameObject buildDetector;
    private GameObject build;

    // Use this for initialization
    void Start()
    {

        GameObject buildable = Instantiate(buildDetector, transform.position, Quaternion.identity);
        buildable.name = "BuildTile";
        buildable.AddComponent<BuildMenu>();
        build = buildable;
        buildable.SetActive(true);

    }

}
