using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool m_IsShaking;
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        m_IsShaking = true;

        float timeElapsed = 0.0f;

        while(timeElapsed < duration) 
        {
            //Calculate random shake
            float xOffset = Random.Range(-1f, 1f) * magnitude;
            float yOffset = Random.Range(-1f, 1f) * magnitude;

            //Shake the camera position
            transform.localPosition += new Vector3(xOffset, yOffset, 0);

            //Update the elpased time
            timeElapsed += Time.deltaTime;
            yield return null;
        }


        m_IsShaking = false;

        transform.localPosition = originalPos;
    }
}
