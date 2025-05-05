using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool m_IsShaking;
    public Transform m_ShipTransform;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        Vector3 originalShipPos = m_ShipTransform.position;

        m_IsShaking = true;

        float timeElapsed = 0.0f;

        while(timeElapsed < duration) 
        {
            //Calculate random shake
            float xOffset = Random.Range(-1f, 1f) * magnitude;
            float yOffset = Random.Range(-1f, 1f) * magnitude;

            //Shake the camera position
            transform.position += new Vector3(xOffset, yOffset, 0);

            //Update the elpased time
            timeElapsed += Time.deltaTime;


            yield return null;
        }

        //To maintain relative position to ship adjust position in relation to how much the ship has moved
        transform.position = originalPos + (m_ShipTransform.position - originalShipPos);

        m_IsShaking = false;

    }
}
