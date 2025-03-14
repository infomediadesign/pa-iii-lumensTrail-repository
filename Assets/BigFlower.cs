using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BigFlower : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject flower;
    [SerializeField] private GameObject topPart;
    [SerializeField] private DesignerPlayerScriptableObject dData;
    private Animator animator;
    public float inRangeDistance = 3f;
    private bool movingPlayer = false;
    private bool playerInPosition = false;
    private bool flowerOpen = false;

    public float startX;
    public float endX;
    private bool movingToEnd = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        topPart.SetActive(false);
    }

    public float duration = 2f; // Zeit in Sekunden
    private float elapsedTime = 0f;

    void Update()
    {
        if (movingPlayer) 
        {
            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration; 

                player.transform.localPosition = new Vector2(Mathf.Lerp(startX, endX, t), player.transform.localPosition.y);
                Debug.Log(player.transform.localPosition.x);
            }
            else
            {
                movingToEnd = !movingToEnd; // Richtung umkehren
                movingPlayer = false;
                elapsedTime = 0f; // Zeit zurücksetzen
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && CheckInRange())
        {
            movingPlayer = true;
            player.GetComponent<PlayerInput>().enabled = false;
            startX = player.transform.localPosition.x;
            endX = flower.transform.position.x;
            Debug.Log(player.transform.localPosition.x);
        }
    }

    private bool CheckInRange()
    {
        float check = flower.transform.position.x - player.transform.localPosition.x + player.transform.parent.transform.position.x;
        Debug.Log(player.transform.localPosition + " and " + flower.transform.position);
        Debug.Log(check);
        return check < inRangeDistance && check > -inRangeDistance;
    }

    /*
    IEnumerator MovePlayerToFlower() 
    {
        

        if (flowerX - playerX < 0)
        {
            player.GetComponent<PlayerController>().FlipPlayerCharacter();
        }

        player.GetComponent<Animator>().SetFloat("horizontalSpeed", playerSpeed);

        while (true)
        {
            float distCovered = (Time.time - startTime) * playerSpeed;
            float fractionOfTravel = distCovered / travelLength;

            player.transform.localPosition = new Vector2(Mathf.Lerp(playerX, flowerX, fractionOfTravel), player.transform.position.y);

            if (fractionOfTravel >= 1f) break;

            yield return null;
        }
        while (!flowerOpen)
        {
            Debug.Log("WaitingForFlower");
        }
        animator.SetTrigger("elevate");
        yield return null;
        player.GetComponent<Animator>().SetFloat("horizontalSpeed", 0);
        player.gameObject.SetActive(false);
    }*/

    public void FlowerOpen()
    {
        flowerOpen = true;
    }

    public void ActivateTopCollision()
    {
        topPart.SetActive(true);
        player.SetActive(true);
        if (player.GetComponent<PlayerController>().GetIsFacingRight()) {}
        player.GetComponent<PlayerInput>().enabled = true;
    }


}
