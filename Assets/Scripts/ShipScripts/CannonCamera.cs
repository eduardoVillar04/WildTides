using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCamera : MonoBehaviour
{
    public GameObject m_Sails;
    public GameObject m_Sight;
    private float m_Sensitivity;

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Update the sensitivity selected by the player
        m_Sensitivity = SingletonOptions.m_Instance.m_SensitivityValue;

        rotation.x += Input.GetAxis(xAxis) * m_Sensitivity * 3;
        rotation.y += Input.GetAxis(yAxis) * m_Sensitivity * 3;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat; 
    }

    private void OnEnable()
    {
        m_Sails.SetActive(false);
        m_Sight.SetActive(true);
    }

    private void OnDisable()
    {
        m_Sails.SetActive(true);
        m_Sight.SetActive(false);
    }
}
