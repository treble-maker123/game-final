using UnityEngine;
/**
 * Attached to tiles that are along the border and contains logic such as building trees.
 */

public class Border : MonoBehaviour {

    private static string[] TreeTypes = new string[] {
        "Trees/Prefabs/Fir_Tree",
        "Trees/Prefabs/Oak_Tree",
        "Trees/Prefabs/Palm_Tree",
        "Trees/Prefabs/Poplar_Tree"
    };

    private GameObject[] trees;

    private static readonly float y = 0.5f;

    // Use this for initialization
    void Start () {
        trees = new GameObject[5];

        for (int i = 0; i < 5; i++) {
            trees[i] = BuildRandomTree();
            trees[i].transform.parent = transform;
            trees[i].AddComponent<BoxCollider>();
        }

        trees[0].transform.localPosition = new Vector3(Random.Range(-0.5f, 0.0f), y, Random.Range(-0.5f, 0.0f));
        trees[1].transform.localPosition = new Vector3(Random.Range(-0.5f, 0.0f), y, Random.Range(0.0f, 0.5f));
        trees[2].transform.localPosition = new Vector3(Random.Range(0.5f, 0.0f), y, Random.Range(-0.5f, 0.0f));
        trees[3].transform.localPosition = new Vector3(Random.Range(0.5f, 0.0f), y, Random.Range(0.5f, 0.0f));
        trees[4].transform.localPosition = new Vector3(0f, y, 0f);
    }

    private GameObject BuildRandomTree() {
        string target = TreeTypes[Random.Range(0, TreeTypes.Length)];
        GameObject tree = Instantiate(Resources.Load(target)) as GameObject;
        tree.name = "Tree";
        tree.tag = "Trees";

        return tree;
    }
}
