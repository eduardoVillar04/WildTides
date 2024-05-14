using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int m_HealthPoints;
    public bool m_IsDead;

    private void Update()
    {
        if(m_HealthPoints <= 0)
        {
            m_IsDead = true;
        }
    }


    public void DealDamage(int damageDealt)
    {
        m_HealthPoints -= damageDealt;
    }
}
