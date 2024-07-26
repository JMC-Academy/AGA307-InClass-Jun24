using UnityEngine;

public class EnemyWeapon : GameBehaviour
{
    public int damage = 20;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _PLAYER.Hit(damage);
        }
    }
}
