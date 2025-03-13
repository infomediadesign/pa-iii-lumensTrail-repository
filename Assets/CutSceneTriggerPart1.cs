using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutSceneTriggerPart1 : MonoBehaviour
{
    [SerializeField] private ProgrammerPlayerScriptableObject pData;
    private bool cutsceneActive = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInput>().enabled = false;
            cutsceneActive = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (cutsceneActive)
            {
                if (pData.isGrounded)
                {
                    StartCoroutine(WaitASecond(collision));
                }
            }
        }
    }

    IEnumerator WaitASecond(Collider2D collision)
    {
        yield return new WaitForSeconds(1);
        collision.gameObject.GetComponent<PlayerController>().CutsceneWalking();
                    Destroy(gameObject);
    }

}
