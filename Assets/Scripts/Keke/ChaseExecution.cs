using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ChaseExecution : MonoBehaviour
{
    public Text timerText;
    public Text kekePrompt;
    private float timer = 0;
    public float timerGoal = 60f;
    public KekeAI kekeAI;
    public GameObject player;
    public ChaseCheckpointHandler stageOnePoints;
    public ChaseCheckpointHandler stageThreePoints;

    private int stage = 0;
    


    void OnActivateStageOne()
    {
        stageOnePoints.active = true;
        kekeAI.followEnabled = true;
    }

    void OnExitStageOne()
    {
        kekeAI.followEnabled = false;
        stageOnePoints.active = false;
    }

    void OnActivateStageTwo()
    {
        kekeAI.target = player.transform;
        kekeAI.followEnabled = true;
    }

    void OnExitStageTwo()
    {
        kekeAI.followEnabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
