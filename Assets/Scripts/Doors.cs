using UnityEngine;

public class Doors : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            anim.SetTrigger("DoorOpen");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.CompareTag("Enemy"))
        {
            anim.SetTrigger("DoorClose");
        }
    }

}
