using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrunkMainBody : MonoBehaviour
{
    Rigidbody2D rb;
    private bool activated;
    private TrunkStump stump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stump = transform.GetChild(1).GetComponent<TrunkStump>();
        activated = false;
    }

    private void Update()
    {
        if (activated)
        {
            if (rb.velocity == Vector2.zero)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                this.gameObject.layer = 8;
                stump.DeactivateCollider();
                this.activated = false;
            }
        }
    }

    public void LightWaveHit(Vector2 lightWaveVelocity)
    {
        stump.transform.SetParent(null);
        rb.constraints = RigidbodyConstraints2D.None;
        activated = true;
        rb.velocity = lightWaveVelocity;
    }
}
