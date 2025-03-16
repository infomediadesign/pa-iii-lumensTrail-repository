using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class BigFlower : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject flower;
    [SerializeField] private GameObject topPart;
    [SerializeField] private Light2D spotLight;
    [SerializeField] private DesignerPlayerScriptableObject dData;
    private Animator animator;
    public float inRangeDistance = 3f;
    private bool movingPlayer = false;
    private bool reduceLightRadius = false;

    private float startX;
    private float endX;
    private bool movingToEnd = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        topPart.SetActive(false);
    }

    public float duration = 1f; // Zeit in Sekunden
    private float elapsedTime = 0f;

    void Update()
    {
        if (movingPlayer) 
        {
            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration; 
                player.transform.position = new Vector2(Mathf.Lerp(startX, endX, t), player.transform.position.y);
                spotLight.pointLightOuterRadius = Mathf.Lerp(3, 9, t);
                Debug.Log(player.transform.position.x);
            }
            else
            {
                movingToEnd = !movingToEnd; // Richtung umkehren
                movingPlayer = false;
                animator.SetTrigger("elevate");
                player.GetComponent<Animator>().SetFloat("horizontalSpeed", 0);
                player.gameObject.SetActive(false);
                elapsedTime = 0;
            }
        }

        if (reduceLightRadius)
        {
            if (elapsedTime < duration) 
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration; 
                spotLight.pointLightOuterRadius = Mathf.Lerp(9, 3, t);
            }
            else
            {
                reduceLightRadius = false;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && CheckInRange())
        {
            movingPlayer = true;
            player.GetComponent<PlayerInput>().enabled = false;
            startX = player.transform.position.x;
            endX = flower.transform.position.x;
            if (endX - startX < 0) player.GetComponent<PlayerController>().FlipPlayerCharacter();
            animator.SetTrigger("openUp");
            player.GetComponent<Animator>().SetFloat("horizontalSpeed", 1);
            LumenThoughtBubbleActivation bubble = flower.transform.GetChild(0).GetComponent<LumenThoughtBubbleActivation>();
            if (bubble != null) bubble.DeactivatePrompt();

        }
    }

    private bool CheckInRange()
    {
        float check = flower.transform.position.x - player.transform.position.x;
        Debug.Log(player.transform.position + " and " + flower.transform.position);
        Debug.Log(check);
        return check < inRangeDistance && check > -inRangeDistance;
    }

    public void ActivateTopCollision()
    {
        topPart.SetActive(true);
        player.SetActive(true);
        player.transform.position = topPart.transform.GetChild(0).transform.position;
        if (player.GetComponent<PlayerController>().GetIsFacingRight()) 
        {
            player.GetComponent<PlayerController>().FlipPlayerCharacter();
        }
        player.GetComponent<PlayerInput>().enabled = true;
        reduceLightRadius = true;
    }


}
