using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;

    public Camera fpsCamera;

    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hitInfo;

        Vector3 adj = new Vector3(Random.RandomRange(-0.8f, 0.8f), Random.RandomRange(-0.8f, 0.8f));
        if(Physics.Raycast(fpsCamera.transform.position + adj, fpsCamera.transform.forward, out hitInfo, range))
        {
            var e = Resources.Load("HitSpot");
            Instantiate(e, hitInfo.point, Quaternion.identity);
            Debug.Log(hitInfo.transform.name);
            //This is where we'll check if it's a hittable game object that can take damage.
            if(hitInfo.transform.tag == "Mobs")
            {
                MobInteraction mob = hitInfo.transform.GetComponent<MobInteraction>();
                mob.TakeDamage(10f);
            }
        }
    }
}
