using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;
    public float fireRate = 0.08f;
    private float nextShot = 0.0f;

    public Camera fpsCamera;

    public AudioClip shotClip;

    private AudioSource source;

    void Start() {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShot)
        {
            nextShot = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        source.PlayOneShot(shotClip, 0.3f);

        RaycastHit hitInfo;

        Vector3 adj = new Vector3(UnityEngine.Random.Range(-0.8f, 0.8f), UnityEngine.Random.Range(-0.8f, 0.8f));
        if (Physics.Raycast(fpsCamera.transform.position + adj, fpsCamera.transform.forward, out hitInfo, range))
        {
            //var e = Resources.Load("HitSpot");
            //Instantiate(e, hitInfo.point, Quaternion.identity);
            Debug.Log(hitInfo.transform.name);
            //This is where we'll check if it's a hittable game object that can take damage.
            if (hitInfo.transform.tag == "Mobs")
            {
                MobInteraction mob = hitInfo.transform.GetComponent<MobInteraction>();
                mob.TakeDamage(10f);
            }
        }
    }
}
