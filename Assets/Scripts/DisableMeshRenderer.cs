using UnityEngine;

public class DisableMeshRenderer : MonoBehaviour
{
    private void Start()
    {
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
