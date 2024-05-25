using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int m_HealthPoints;
    public bool m_IsDead;

    [Header("Audio")]
    public AudioClip m_WoodBreakSound;
    private void Update()
    {
        if(m_HealthPoints <= 0)
        {
            m_IsDead = true;
        }
        else
        {
            m_IsDead = false;
        }
    }


    public void DealDamage(int damageDealt)
    {
        m_HealthPoints -= damageDealt;
        SoundEffectsManager.instance.PlaySoundFXClip(m_WoodBreakSound, transform, 0.8f);
    }
}
