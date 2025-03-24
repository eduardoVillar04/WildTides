using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int m_MaxHealthPoints;
    public int m_HealthPoints;
    public bool m_IsDead;

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
