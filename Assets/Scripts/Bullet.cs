using UnityEngine;

public class Bullet : MonoBehaviour {

    private Transform target;

    public float speed = 70f;
    public GameObject bulletExplosionEffect;

    private float damage;

    public void Seek(Transform _target, float dmg)
    {
        target = _target;
        damage = dmg;
    }
	
	// Update is called once per frame
	void Update () {
		if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        //Hit the target if this is true
        if (direction.magnitude <= distanceThisFrame)
        {
            MobInteraction mb = target.GetComponent<MobInteraction>();
            Debug.Log(damage);
            mb.TakeDamage(damage);
            HitTarget();
            return;
        }

        //Haven't hit target yet, so move towards it
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
	}

    void HitTarget()
    {
        GameObject bulletExplosion = (GameObject) Instantiate(bulletExplosionEffect, transform.position, transform.rotation);
        Destroy(bulletExplosion, 2f);
        Destroy(gameObject);
    }
}
