using UnityEngine;

public class Gun : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;

    public Camera fpsCamera;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hitInfo;
        if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
            //This is where we'll check if it's a hittable game object that can take damage.
            if(hitInfo.transform.name == "Mob")
            {
                MobInteraction mob = hitInfo.transform.GetComponent<MobInteraction>();
                mob.TakeDamage(10f);
            }
        }
    }
}
