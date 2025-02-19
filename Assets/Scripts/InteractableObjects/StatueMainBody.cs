using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class StatueMainBody : MonoBehaviour
{
    Rigidbody2D rb;
    private bool activated;
    [SerializeField] StatueMainBody otherStatue;
    [SerializeField] GameObject waitingRasselbande;
    [SerializeField] GameObject pathingRasselbande;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            MovementBaseState.movementEnabled = false;
        }
    }

    public void LightWaveHit(Vector2 lightWaveVelocity)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        activated = true;
        rb.velocity = lightWaveVelocity;

    }

    public void ActivateStatue()
    {
        gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LightThrowInteractable") 
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!activated) return;
            MovementBaseState.movementEnabled = true;
            otherStatue.ActivateStatue();
            waitingRasselbande.SetActive(false);
            pathingRasselbande.SetActive(true);
            this.gameObject.SetActive(false);

        }
    }
}
