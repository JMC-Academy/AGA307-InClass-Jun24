using System.Collections;
using UnityEngine;

public class FiringPoint : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 1000f;
    [Header("Raycast Projectile")]
    public GameObject laserHitSparks;
    public LineRenderer laser;

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
            FireProjectile();

        if(Input.GetButtonDown("Fire2"))
            FireRaycast();
    }

    private void FireProjectile()
    {
        //Create a reference to hold our instantiated object
        GameObject projectileInstance;
        //Instantiate our projectile prefab
        projectileInstance = Instantiate(projectilePrefab,transform.position, transform.rotation);
        //Get the rigidbody of our projectile and add force to it
        projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
    }

    private void FireRaycast()
    {
        //Create the ray
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.magenta);
        //Create a reference to hold info on what we hit
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log("Hit " + hit.collider.name + " at point " + hit.point + "which was " + hit.distance + "units away");
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, hit.point);
            StopAllCoroutines();
            StartCoroutine(ShowLaser());

            GameObject laserParticleInstance = Instantiate(laserHitSparks, hit.point, hit.transform.rotation);
            Destroy(laserParticleInstance, 1);

            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    IEnumerator ShowLaser()
    {
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        laser.gameObject.SetActive(false);
    }
}
