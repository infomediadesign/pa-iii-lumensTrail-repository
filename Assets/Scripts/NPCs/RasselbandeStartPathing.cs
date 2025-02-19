using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasselbandeStartPathing : MonoBehaviour
{
    public KekeAI pathfinder;
    public GameObject Rasselbande;

    public Transform target;
    private bool activated = false;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        pathfinder.target = target;
        pathfinder.followEnabled = true;
        activated = true;
    }

    void Update()
    {
        if (activated)
        {
            if (pathfinder.ReachedTarget())
            {
                Rasselbande.SetActive(false);
            }
        }
    }
}
