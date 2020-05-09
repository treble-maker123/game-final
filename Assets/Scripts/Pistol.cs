using UnityEngine;
using System.Collections;

public class Pistol : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;

    public Camera fpsCamera;

    public AudioClip shotClip;
    private AudioSource source;

    void Start() {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        source.PlayOneShot(shotClip);

        RaycastHit hitInfo;

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hitInfo, range))
        {
            //var e = Resources.Load("HitSpot");
            //Instantiate(e, hitInfo.point, Quaternion.identity);
            //This is where we'll check if it's a hittable game object that can take damage.
            if (hitInfo.transform.name == "Air")
            {
                MobInteraction mob = hitInfo.transform.GetComponent<MobInteraction>();
                mob.TakeDamage(30f);
            }
        }
    }
}
