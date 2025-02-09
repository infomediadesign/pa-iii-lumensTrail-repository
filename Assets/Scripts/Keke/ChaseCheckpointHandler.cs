using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ChaseCheckpointHandler : MonoBehaviour
{
    public bool active = false;
    bool setTarget = false;
    public GameObject chaser;
    public float reachDistance = 1f;

    private KekeAI ai;

    [SerializeField, ReadOnly(true)]
    private List<Transform> childTransforms = new List<Transform>();

    private Tuple<Transform, int> currentCheckpoint;
    

    
    // Start is called before the first frame update
    void Start()
    {
        childTransforms.AddRange(transform.GetComponentsInChildren<Transform>());
        childTransforms.Remove(transform);
        currentCheckpoint = new Tuple<Transform, int>(childTransforms[0], 0);

        ai = chaser.GetComponent<KekeAI>();

    }

    

    // Update is called once per frame
    void Update()
    {
       
        
        if (active)
        {
            if (!setTarget)
            {
              ai.target = currentCheckpoint.Item1;
              setTarget = true;
            }
            checkReachedCurrent();
        }
        else
        {
            setTarget = false;
        }
    }

    private void checkReachedCurrent()
    {
        float distance = Vector2.Distance(chaser.transform.position, currentCheckpoint.Item1.position);
        if (distance <= reachDistance)
        {
            currentCheckpoint = new Tuple<Transform, int>(childTransforms[currentCheckpoint.Item2 + 1], currentCheckpoint.Item2 + 1);
            ai.target = currentCheckpoint.Item1;
        }
    }
}
