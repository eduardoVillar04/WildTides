using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    public RectTransform compassBarTransform;

    public RectTransform objectiveMarkerTransform;
    public RectTransform northMarkerTransform;
    public RectTransform southMarkerTransform;
    public RectTransform eastMarkerTransform;
    public RectTransform westMarkerTransform;

    public Transform cameraObjectTransform;
    public Transform objectiveObjectTransform;

    public Camera mainCam;
    public GameObject pauseMenu;

    public GameObject healthUI;
    public GameObject tideLevel;
    public GameObject missionUI;
    public GameObject deathMenu;

    public Canvas canvas;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraObjectTransform = mainCam.transform;
    }

    void Update()
    {
        SetMarkerPosition(objectiveMarkerTransform, objectiveObjectTransform.position);
        SetMarkerPosition(northMarkerTransform, Vector3.forward * 1000);
        SetMarkerPosition(southMarkerTransform, Vector3.back * 1000);
        SetMarkerPosition(eastMarkerTransform, Vector3.right * 1000);
        SetMarkerPosition(westMarkerTransform, Vector3.left * 1000);

        //We disable the compass bar when the player aims
        if (!mainCam.gameObject.activeSelf || pauseMenu.activeSelf || deathMenu.activeSelf)
        {
            compassBarTransform.gameObject.SetActive(false);
            healthUI.gameObject.SetActive(false);
            tideLevel.gameObject.SetActive(false);
            missionUI.gameObject.SetActive(false);
        } else 
        {
            compassBarTransform.gameObject.SetActive(true);
            healthUI.gameObject.SetActive(true);
            tideLevel.gameObject.SetActive(true);
            missionUI.gameObject.SetActive(true);
        }


    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;
        float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z),new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z));
        float compassPositionX = Mathf.Clamp(2 * angle / mainCam.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector2(compassBarTransform.rect.width / 2 * compassPositionX, 0);
    }
}
