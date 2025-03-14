using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class ChaseExecution : MonoBehaviour
{
    private enum chaseStage {prestage1, stage1, prestage2, stage2, prestage3, stage3}
    
    public TMP_Text timerText;
    public TMP_Text startPrompt;

    public LocalizeStringEvent stringEvent;
    private LocalizedString localizedString;
    [SerializeField] private TableReference localizationTable;
    
    public float speedSegmentOne;
    public float[] speedSegmentTwo = new float[3];
    public float speedSegmentThree;
    
    [ReadOnly(true)]
    public bool inRange;
    private float timer = 0;
    public float timerGoal = 30f;

    public float transitionTime = 5f;
    public float gracePeriod = 5f;
    private bool gracePeriodPassed;

    

    private PlayerInput input;
    private InputAction activate;
    private Collider2D activationCollider;
    public KekeAIAdvanced kekeAI;
    public RangeCheck rangeCheck;
    public Transform kekeTransform;
    public Transform playerTransform;
    public ChaseCheckpointHandler stageOnePoints;
    public Transform[] stageThreePoints = new Transform[2];
    public Transform stageThreeThreshold;
    public GameObject stageThreeGoal;
    public RangeCheck goalReached;

    [SerializeField] private LumenThoughtBubbleActivation thoughtBubble;

    public SwingingActivation swingingActivation;    

     

    //private int stage = 0;
    void Start()
    {
        timerText.enabled = false;
        startPrompt.enabled = false;
        stageOnePoints.active = false;
        input = playerTransform.gameObject.GetComponent<PlayerInput>();
        activate = input.actions["Interact"];
        activationCollider = GetComponent<Collider2D>();
        localizedString = new LocalizedString();
        SetLocalizedString("she_wants_to_play");
        thoughtBubble.showPromptNow = true;
    }

    

    void OnTriggerStay2D(Collider2D collision)
    {
        startPrompt.enabled = true;
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
        thoughtBubble.DeactivatePrompt();
        kekeAI.gridGraph.Scan();
        swingingActivation.OnDisableSwing();
        ActionBaseState.LockAllActions();
        playerTransform.GetComponent<PlayerController>().inChase = true;
        stageOnePoints.active = true;
        kekeAI.OnPathfindingEnable();
        kekeAI.SetMaxSpeedX(speedSegmentOne);
        StartCoroutine(OnRunStageOne());
    }



    private IEnumerator OnRunStageOne()
    {
        MovementBaseState.LockMovement();
        startPrompt.enabled = true;
        float gracePeriodTimer = 0f;
        while (gracePeriodTimer < gracePeriod)
        {
            gracePeriodTimer += Time.deltaTime;
            startPrompt.text = ((int)gracePeriod - (int)gracePeriodTimer).ToString();
            yield return null;
            
        }
        SetLocalizedString("go");
        gracePeriodPassed = true;
        MovementBaseState.UnlockMovement();
        while (gracePeriodTimer < 3f)
        {
            gracePeriodTimer += Time.deltaTime;
            yield return null;
            
        }
        startPrompt.enabled = false;
        
        while (!(stageOnePoints.lastCheckPointReached || (rangeCheck.inRange && gracePeriodPassed)))
        {
            yield return null;
        }
        if (rangeCheck.inRange) OnExitStageOne(true);
        else OnExitStageOne();
    }
    void OnExitStageOne(bool caught = false)
    {
        kekeAI.OnPathfindingDisable();
        stageOnePoints.active = false;
        StartCoroutine(SwitchToStageTwo(caught));
    }
    private IEnumerator SwitchToStageTwo(bool caught)
    {
        MovementBaseState.LockMovement();
        startPrompt.enabled = true;
        if (caught) SetLocalizedString("caught_her");
        else SetLocalizedString("sliped_away");
        yield return new WaitForSeconds(transitionTime);
        startPrompt.enabled = false;
        MovementBaseState.UnlockMovement();
        OnActivateStageTwo();
    }
    void OnActivateStageTwo()
    {
        kekeAI.pathFindingTarget = playerTransform;
        kekeAI.SetMaxSpeedX(speedSegmentTwo[0]);
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
            int viewedTimer = (int) timerGoal - (int) elapsedTime;
            timerText.text = viewedTimer.ToString();

            // Speed adjustments
            if (elapsedTime > (timerGoal / 3) * 2) kekeAI.SetMaxSpeedX(speedSegmentTwo[2]);
            else if (elapsedTime > timerGoal / 3) kekeAI.SetMaxSpeedX(speedSegmentTwo[1]);

            yield return null;  // Wait until the next frame
        }

        timerText.enabled = false;
        OnExitStageTwo();
    }
    private IEnumerator OnRunStageTwo()
    {
        startPrompt.enabled = true;
        float gracePeriodTimer = 0f;
        while (gracePeriodTimer < gracePeriod)
        {
            gracePeriodTimer += Time.deltaTime;
            startPrompt.text = ((int)gracePeriod - (int)gracePeriodTimer).ToString();
            yield return null;
            
        }
        SetLocalizedString("go");
        gracePeriodPassed = true;
        kekeAI.OnPathfindingEnable();
        StartCoroutine(StageTwoTimer());
        gracePeriodTimer = 0f;
        while (gracePeriodTimer < 3f)
        {
            gracePeriodTimer += Time.deltaTime;
            yield return null;
            
        }
        startPrompt.enabled = false;
        
        while (!(timer >= timerGoal || (rangeCheck.inRange && gracePeriodPassed)))
        {
            yield return null;
        }
        if (rangeCheck.inRange) 
        {
            OnExitStageTwo(true);
        }
        
    }

    void OnExitStageTwo(bool caught = false)
    {
        kekeAI.OnPathfindingDisable();
        timerText.enabled = false;
        StartCoroutine(SwitchToStageThree(caught));
    }

    private IEnumerator SwitchToStageThree(bool caught)
    {
        MovementBaseState.LockMovement();
        startPrompt.enabled = true;
        if (caught) SetLocalizedString("caught_you");
        else SetLocalizedString("couldnt_caught_you");
        yield return new WaitForSeconds(transitionTime);
        startPrompt.enabled = false;
        MovementBaseState.UnlockMovement();
        playerTransform.GetComponent<PlayerController>().inChase = false;
        OnActivateStageThree();
    }

    void OnActivateStageThree()
    {
        SetLocalizedString("where_she_going");
        startPrompt.enabled = true;
        kekeAI.OnPathfindingEnable();
        gracePeriodPassed = false;
        kekeAI.SetMaxSpeedX(speedSegmentThree);
        if (playerTransform.position.x < stageThreeThreshold.position.x) kekeAI.pathFindingTarget = stageThreePoints[1];
        else kekeAI.pathFindingTarget = stageThreePoints[0];
        FindObjectOfType<BreakGroundTrigger>().triggerActive = true;
        StartCoroutine(OnRunStageThree());
    }

    private IEnumerator OnRunStageThree()
    {
        
        while (!kekeAI.ReachedTarget())
        {
            yield return null;
        }
        kekeAI.OnPathfindingDisable();
        FindObjectOfType<BreakGroundTrigger>().kekeInPlace = true;
        OnExitStageThree();
        
        
    }
    void OnExitStageThree()
    {
        startPrompt.enabled = true;
        ActionBaseState.UnlockAllActions();
        //startPrompt.text = "Boom!";
    }

    



    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetLocalizedString(string key)
    {
        if (stringEvent == null) return;

        localizedString.TableReference = localizationTable;
        localizedString.TableEntryReference = key;

        stringEvent.StringReference = localizedString;
        stringEvent.RefreshString();
    }
}
