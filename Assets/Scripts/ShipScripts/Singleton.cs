using System;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton player_Sensitivity;

    public static implicit operator Singleton(float v)
    {
        throw new NotImplementedException();
    }
}
