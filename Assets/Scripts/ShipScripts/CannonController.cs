using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CannonController : MonoBehaviour
{
    [Header("SHOOT VARIABLES")]
    public GameObject m_Projectile;
    public Transform m_CannonEndPoint;
    public Transform m_AimReferencePoint;
    public float m_ProjectileSpeed;
    public float m_RecoilFroce;

    [Header("CAMERA SHAKE")]
    public CameraShake m_MainCameraShake;
    public CameraShake m_CannonCameraShake;
    public float m_ShakeTime;
    public float m_MainCamShakeMagnitude;
    public float m_CannonCamShakeMagnitude;

    [Header("CONTROL VARIABLES")]
    public float m_ShootCooldown;
    private float m_ShootColdowntimer;
    public bool m_ShootIsPressed;
    public GameObject m_CannonCamera;

    [Header("COMPONENTS")]
    public PlayerInput m_PlayerInput;
    public Rigidbody m_ShipRigidbody;


    [Header("Audio")]
    public AudioClip m_ShootSound;
    // Start is called before the first frame update
    void Start()
    {
        m_ShootColdowntimer = Time.time + 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();

        ShootingLogic();

        if(!m_CannonCamera.activeSelf)
        {
            TrackAimReference();
        }

    }

    public void Inputs()
    {
        m_ShootIsPressed = m_PlayerInput.actions["Shoot"].IsPressed();
    }

    public void ShootingLogic()
    {
        //We can shoot when it isnt on cooldown and the player presses the button
        if (Time.time > m_ShootColdowntimer && m_ShootIsPressed && Time.timeScale != 0)
        {
            Shoot();
        }
    }

    //This method is used in the shooting button in the mobile canvas
    public void ShootingButton()
    {
        //In mobile, the player isnt able to do the press action "shoot"
        if (Time.time > m_ShootColdowntimer && Time.timeScale != 0)
        {
            m_ShootColdowntimer = Time.time + m_ShootCooldown;
            Shoot();
        }
    }

    public void Shoot()
    {
        m_ShootColdowntimer = Time.time + m_ShootCooldown;

        //We shake both the cannon and the main camera
        StartCoroutine(m_MainCameraShake.Shake(m_ShakeTime, m_MainCamShakeMagnitude));
        StartCoroutine(m_CannonCameraShake.Shake(m_ShakeTime, m_CannonCamShakeMagnitude));

        //We instantiate the projectile
        GameObject projectile = Instantiate(m_Projectile, m_CannonEndPoint.position, m_CannonEndPoint.rotation);
        //And give it force 
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.AddForce(projectile.transform.forward * m_ProjectileSpeed, ForceMode.VelocityChange);
        //Audio
        SoundEffectsManager.instance.PlaySoundFXClip(m_ShootSound, transform, 0.6f);
        Recoil();
    }

    public void TrackAimReference()
    {
        //The cannon will be aiming where the player is aiming
        Vector3 direction = new Vector3(m_AimReferencePoint.position.x, transform.position.y, m_AimReferencePoint.position.z);
        transform.LookAt(direction);
    }

    public void Recoil()
    {
        //After each attack, the boat will suffer knockback from the shot
        Vector3 direction = transform.position - m_CannonEndPoint.position;
        m_ShipRigidbody.AddForce(direction * m_RecoilFroce, ForceMode.VelocityChange);
    }

}
