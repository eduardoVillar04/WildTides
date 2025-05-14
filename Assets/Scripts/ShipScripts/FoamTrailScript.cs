using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamTrailScript : MonoBehaviour
{
    [SerializeField] Rigidbody shipRgb;
    [SerializeField] float foamMultiplier;
    [SerializeField] float foamBase;

    [SerializeField]ParticleSystem.EmissionModule back1, back2, front;
    private void Start()
    {
        back1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
        back2 = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
        front = transform.GetChild(2).GetComponent<ParticleSystem>().emission;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        back1.rateOverTime = foamBase + foamMultiplier * shipRgb.velocity.magnitude;
        back2.rateOverTime = foamBase + foamMultiplier * shipRgb.velocity.magnitude;
        front.rateOverTime = foamBase + foamMultiplier * shipRgb.velocity.magnitude;
    }
}
