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
        }
    }
}
