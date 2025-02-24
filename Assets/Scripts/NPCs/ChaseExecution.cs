using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChaseExecution : MonoBehaviour
{
    private enum chaseStage {prestage1, stage1, prestage2, stage2, prestage3, stage3}
    
    public TMP_Text timerText;
    public TMP_Text startPrompt;
    


    public float speedSegmentOne;
    public float[] speedSegmentTwo = new float[3];
    public float speedSegmentThree;
    
    [ReadOnly(true)]
    public bool inRange;
    private float timer = 0;
    public float timerGoal = 30f;

    public float transitionTime = 5f;
    public float gracePeriod = 3f;
    private bool gracePeriodPassed;

    private PlayerInput input;
    private InputAction activate;
    private Collider2D activationCollider;
    public KekeAI kekeAI;
    public ChaseRangeCheck rangeCheck;
    public Transform kekeTransform;
    public Transform playerTransform;
    public ChaseCheckpointHandler stageOnePoints;
    public Transform[] stageThreePoints = new Transform[2];
    public Transform stageThreeThreshold;
    public GameObject stageThreeGoal;
    public ChaseRangeCheck goalReached;    

     

    private int stage = 0;
    void Start()
    {
        kekeAI.followEnabled = false;
        timerText.enabled = false;
        stageOnePoints.active = false;
        input = playerTransform.gameObject.GetComponent<PlayerInput>();
        activate = input.actions["Interact"];
        activationCollider = GetComponent<Collider2D>();
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
            activationCollider.enabled = false;
            OnActivateStageOne();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        startPrompt.enabled = false;
    }


    void OnActivateStageOne()
    {
        ActionBaseState.LockAllActions();
        kekeAI.gridGraph.Scan();
        stageOnePoints.active = true;
        kekeAI.followEnabled = true;
        kekeAI.speed = speedSegmentOne;
        StartCoroutine(OnRunStageOne());
    }



    private IEnumerator OnRunStageOne()
    {
        if (!gracePeriodPassed)
        {
            MovementBaseState.LockMovement();
            startPrompt.enabled = true;
            startPrompt.text = "Get Keke!";
            yield return new WaitForSeconds(gracePeriod);
            startPrompt.enabled = false;
            gracePeriodPassed = true;
            MovementBaseState.UnlockMovement();
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
        MovementBaseState.LockMovement();
        startPrompt.enabled = true;
        if (caught) startPrompt.text = "Caught Keke! Keke will now catch you!";
        else startPrompt.text = "Keke was too slippery! Keke will now catch you!";
        yield return new WaitForSeconds(transitionTime);
        startPrompt.enabled = false;
        MovementBaseState.UnlockMovement();
        OnActivateStageTwo();
    }
    void OnActivateStageTwo()
    {
        kekeAI.target = playerTransform;
        kekeAI.speed = speedSegmentTwo[0];
        gracePeriodPassed = false;
        StartCoroutine(OnRunStageTwo());
        
    }

    private IEnumerator StageTwoTimer()
    {
        timerText.enabled = true;
        float elapsedTime = 0f;

        while (elapsedTime < timerGoal)
        {
            elapsedTime += Time.deltaTime;  // Increment by the time passed since last frame
            timerText.text = string.Format("{0:00.00}", elapsedTime);

            // Speed adjustments
            if (elapsedTime > (timerGoal / 3) * 2) kekeAI.speed = speedSegmentTwo[2];
            else if (elapsedTime > timerGoal / 3) kekeAI.speed = speedSegmentTwo[1];

            yield return null;  // Wait until the next frame
        }

        timerText.enabled = false;
    }
    private IEnumerator OnRunStageTwo()
    {
        if (!gracePeriodPassed)
        {
            startPrompt.enabled = true;
            startPrompt.text = "Run from Keke!";
            yield return new WaitForSeconds(gracePeriod);
            startPrompt.enabled = false;
            gracePeriodPassed = true;
            kekeAI.followEnabled = true;
            StartCoroutine(StageTwoTimer());
        }
        while (!(timer >= timerGoal || (rangeCheck.inRange && gracePeriodPassed)))
        {
            yield return null;
        }
        if (rangeCheck.inRange) OnExitStageTwo(true);
        else OnExitStageTwo();
    }

    void OnExitStageTwo(bool caught = false)
    {
        kekeAI.followEnabled = false;
        StartCoroutine(SwitchToStageThree(caught));
    }

    private IEnumerator SwitchToStageThree(bool caught)
    {
        MovementBaseState.LockMovement();
        startPrompt.enabled = true;
        if (caught) startPrompt.text = "Keke caught you! Now follow Keke!";
        else startPrompt.text = "You were too slippery! Now follow Keke!";
        yield return new WaitForSeconds(transitionTime);
        startPrompt.enabled = false;
        MovementBaseState.UnlockMovement();
        OnActivateStageThree();
    }

    void OnActivateStageThree()
    {
        kekeAI.followEnabled = true;
        gracePeriodPassed = false;
        kekeAI.speed = speedSegmentThree;
        if (playerTransform.position.x < stageThreeThreshold.position.x) kekeAI.target = stageThreePoints[1];
        else kekeAI.target = stageThreePoints[0];
        StartCoroutine(OnRunStageThree());
    }

    private IEnumerator OnRunStageThree()
    {
        while (!goalReached.inRange)
        {
            if (kekeAI.ReachedTarget())kekeAI.followEnabled = false;
            yield return null;
        }
        OnExitStageThree();
        
    }
    void OnExitStageThree()
    {
        kekeAI.followEnabled = false;
        startPrompt.enabled = true;
        startPrompt.text = "Boom!";
    }

    



    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
