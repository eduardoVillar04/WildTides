using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionsController : MonoBehaviour
{
    public int m_MissionNumber;

    public TextMeshProUGUI TextMision1;
    private string m_PortName1 = "example";
    public GameObject TextMision2;
    public GameObject TextMision3;
    public GameObject TextMision4;
    public GameObject TextMision5;
    public GameObject TextMision6;

    // Update is called once per frame
    void Update()
    {
        //TextMision1.text = string.Format("GO TO {}", m_PortName1);
    }


}
