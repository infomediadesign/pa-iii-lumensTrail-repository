using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChaseExecution : MonoBehaviour
{
    private enum chaseStage {prestage1, stage1, prestage2, stage2, prestage3, stage3}
    
    public Text timerText;
    public Text startPrompt;
    


    public float speedSegmentOne;
    public float[] speedSegmentTwo = new float[3];
    public float speedSegmentThree;
    
    [ReadOnly(true)]
    public bool inRange;
    private float timer = 0;
    public float timerGoal = 60f;

    public float transitionTime = 5f;
    public float gracePeriod = 3f;
    private bool gracePeriodPassed;

    private PlayerInput input;
    private InputAction activate;
    public KekeAI kekeAI;
    public ChaseRangeCheck rangeCheck;
    public Transform kekeTransform;
    public Transform playerTransform;
    public ChaseCheckpointHandler stageOnePoints;
    public ChaseCheckpointHandler stageThreePoints;

    private int stage = 0;
    void Start()
    {
        kekeAI.followEnabled = false;
        stageOnePoints.active = false;
        stageThreePoints.active = false;
        input = playerTransform.gameObject.GetComponent<PlayerInput>();
        activate = input.actions["Interact"];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        startPrompt.enabled = true;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (activate.IsPressed())
        {
            startPrompt.enabled = false;
            collision.enabled = false;
            OnActivateStageOne();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        startPrompt.enabled = false;
    }


    void OnActivateStageOne()
    {
        stageOnePoints.active = true;
        kekeAI.followEnabled = true;
        StartCoroutine(OnRunStageOne());
    }



    private IEnumerator OnRunStageOne()
    {
        if (!gracePeriodPassed)
        {
            MovementBaseState.movementEnabled = false;
            timerText.enabled = true;
            timerText.text = "Get Keke!";
            yield return new WaitForSeconds(gracePeriod);
            timerText.enabled = false;
            gracePeriodPassed = true;
            MovementBaseState.movementEnabled = true;
        }
        while (!(stageOnePoints.lastCheckPointReached || (rangeCheck.inRange && gracePeriodPassed)))
        {
            yield return null;
        }
        if (rangeCheck.inRange) OnExitStageOne(true);
        else OnExitStageOne();
    }
    void OnExitStageOne(bool caught = false)
    {
        kekeAI.followEnabled = false;
        stageOnePoints.active = false;
        StartCoroutine(SwitchToStageTwo(caught));
    }
    private IEnumerator SwitchToStageTwo(bool caught)
    {
        timerText.enabled = true;
        if (caught) timerText.text = "Caught Keke! Keke will now catch you!";
        else timerText.text = "Keke was too slippery! Keke will now catch you!";
        yield return new WaitForSeconds(transitionTime);
        timerText.enabled = false;
        OnActivateStageTwo();
    }
    void OnActivateStageTwo()
    {
        kekeAI.target = playerTransform;
        kekeAI.speed = speedSegmentTwo[0];
        gracePeriodPassed = false;
    }

    private IEnumerator StageTwoTimer()
    {
        while (timer < timerGoal)
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f;
            timerText.text = string.Format("{0:00.00}", timer);
            if (timer > (timerGoal/3)*2) kekeAI.speed = speedSegmentTwo[2]; 
            else if (timer > timerGoal/3) kekeAI.speed = speedSegmentTwo[1];
        }
    }
    void OnRunStageTwo()
    {
        if (!gracePeriodPassed)
        {
            MovementBaseState.movementEnabled = false;
            timerText.enabled = true;
            timerText.text = "Get Keke!";
            yield return new WaitForSeconds(gracePeriod);
            timerText.enabled = false;
            gracePeriodPassed = true;
            MovementBaseState.movementEnabled = true;
        }
        while (!(stageOnePoints.lastCheckPointReached || (rangeCheck.inRange && gracePeriodPassed)))
        {
            yield return null;
        }
        if (rangeCheck.inRange) OnExitStageOne(true);
        else OnExitStageOne();
    }

    void OnExitStageTwo()
    {
        kekeAI.followEnabled = false;
    }

    void OnActivateStageThree()
    {
        kekeAI.followEnabled = true;
        stageThreePoints.active = true;
    }

    void OnExitStageThree()
    {
        kekeAI.followEnabled = false;
        stageThreePoints.active = false;
    }

    
    void OnRunStageThree()
    {

    }



    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
