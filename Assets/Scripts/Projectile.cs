using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitParticles;
    public float lifetime = 3f;
    void Start()
    {
        Invoke("DestroyProjectile", lifetime);
    }

    public void DestroyProjectile()
    {
        GameObject hitParticlesInstance = Instantiate(hitParticles, transform.position, transform.rotation);
        Destroy(hitParticlesInstance, 2);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.gameObject.name);
        if(collision.collider.CompareTag("Enemy"))
        {
            //Change the collided objects material colour to red
            collision.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            //Destroy the collision object after 1 second
            Destroy(collision.gameObject,1);
        }
        DestroyProjectile();
    }

}
