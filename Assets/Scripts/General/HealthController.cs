using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    //something
    public int m_MaxHealthPoints = 1;
    public int m_HealthPoints = 1;
    public bool m_IsDead = false;

    [Header("Audio")]
    public AudioClip m_DamageSound;

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

    //Do this every time the enemy is enabled (spawns from a pool)
    private void OnEnable()
    {
        m_HealthPoints = m_MaxHealthPoints;
        m_IsDead = false;
    }

    public void DealDamage(int damageDealt)
    {
        if(!m_IsDead)
        {
            m_HealthPoints -= damageDealt;
            if (m_DamageSound != null)
            {
                SoundEffectsManager.instance.PlaySoundFXClip(m_DamageSound, transform, 0.8f);
            }
        }
    }

    public void heal(int healing)
    {
        if(m_HealthPoints < m_MaxHealthPoints)
        {
            m_HealthPoints += healing;
        }
    }
}
