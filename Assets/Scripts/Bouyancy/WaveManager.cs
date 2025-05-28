using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public Vector3 waveOriginPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public float waveHeight = 0.25f;
    public float waveLength = 2f;
    public float waveFrequency = 0.5f;
    public float offset = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("WaveManager already exists, destroying object");
            Destroy(this);
        }
    }
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        offset += Time.deltaTime * waveFrequency;
    }

    public float GetWaveHeight(float x, float z)
    {
        Vector3 p = new Vector3(x, 0, z);
        float distance = Vector3.Distance(p, waveOriginPosition);distance = (distance % waveLength) / waveLength;

        //Oscilate the wave height via sine to create a wave effect
        return waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency + (Mathf.PI * 2.0f * distance));;

        //return amplitude * Mathf.Sin(Time.time * Mathf.PI * 2.0f * speed + (Mathf.PI * 2.0f * offset));
        //return amplitude * Mathf.Sin(height / length + offset);
    }
}
