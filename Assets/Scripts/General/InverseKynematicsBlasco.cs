using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicsBlasco : MonoBehaviour
{

    //public AnimationCurve x;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(PullObject(other));
    }

    IEnumerator PullObject(Collider collider)
    {
        Vector3 directionToCenter = TwisterFocus.position - collider.transform.position;
        if (this.GetComponent<Collider>().bounds.Contains(collider.transform.position))
        {
            collider.GetComponent<Rigidbody>().AddForce(
              Time.fixedDeltaTime * ForceMagnitude *
              (
                Vector3.Cross(transform.up, directionToCenter).normalized +
                40.0f * directionToCenter.normalized +
                15.0f * transform.up)
              );
        }
        //yield return null;
        yield return new WaitForFixedUpdate();
        StartCoroutine(PullObject(collider));
    }

    // ATTRIBUTES
    public Transform TwisterFocus = null;
    public float ForceMagnitude = 0.0f;
}
