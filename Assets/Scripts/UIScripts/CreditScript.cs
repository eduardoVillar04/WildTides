using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour
{
    public GameObject m_Credits;
    // Start is called before the first frame update
    void Start()
    {
        m_Credits.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
