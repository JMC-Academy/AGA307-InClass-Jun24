using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public UnityEvent triggerEnterEvent;
    public UnityEvent triggerStayEvent;
    public UnityEvent triggerExitEvent;
    public string triggerTag;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerTag == "")
            return;

        if (other.CompareTag(triggerTag))
            triggerEnterEvent?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerTag == "")
            return;

        if (other.CompareTag(triggerTag))
            triggerStayEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerTag == "")
            return;

        if (other.CompareTag(triggerTag))
            triggerExitEvent?.Invoke();
    }
}
