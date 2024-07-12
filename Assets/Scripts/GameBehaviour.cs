using UnityEngine;
using System.Collections.Generic;

public class GameBehaviour : MonoBehaviour
{
    protected static GameManager _GM { get { return GameManager.instance; } }
    protected static EnemyManager _EM { get { return EnemyManager.instance; } }
    protected static UIManager _UI {  get { return UIManager.instance; } }


    public Transform GetClosestObject(Transform _origin, List<GameObject> _objects)
    {
        if (_objects == null || _objects.Count == 0)
            return null;

        float distance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject go in _objects)
        {
            float currentDistance = Vector3.Distance(_origin.transform.position, go.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = go.transform;
            }
        }
        return closest;
    }
}
