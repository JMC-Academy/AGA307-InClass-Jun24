using UnityEngine;

public class Projectile : GameBehaviour
{
    public GameObject hitParticles;
    public float lifetime = 3f;
    public int damage = 10;
    void Start()
    {
        Invoke("DestroyProjectile", lifetime);
        //_AUDIO.PlayFireballSound(GetComponent<AudioSource>());
    }

    public void DestroyProjectile()
    {
        GameObject hitParticlesInstance = Instantiate(hitParticles, transform.position, transform.rotation);
        Destroy(hitParticlesInstance, 2);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            //If the hit enemy has the Enemy script, run the Hit function
            if(collision.gameObject.GetComponent<Enemy>() != null)
                collision.gameObject.GetComponent<Enemy>().Hit(damage);
        }
        DestroyProjectile();
    }
}
