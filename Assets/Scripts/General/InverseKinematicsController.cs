using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InverseKinematicsController : MonoBehaviour
{
    [SerializeField]
    Transform[] bones;
    float[] bonesLength;


    [SerializeField]
    int solverIterations = 5;


    [SerializeField]
    Transform targetPostion;
    public Transform Position1;
    public Transform Position2;
    public bool goingRight = false;


    //public AnimationCurve x;


    // Start is called before the first frame update
    void Start()
    {
        bonesLength = new float[bones.Length];

        for (int i = 0; i < bonesLength.Length; i++)
        {
            if(i < bones.Length - 1)
            {
                bonesLength[i] = (bones[i + 1].position - bones[i].position).magnitude;
            }
            else
            {
                bonesLength[i] = 0f;
            }
        }

        goingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        SolveIK();
        MoveSphere();
    }

    void SolveIK()
    {
        Vector3[] finalBonesPositions = new Vector3[bones.Length];

        for(int i = 0;i < bones.Length;i++)
        {
            finalBonesPositions[i] = bones[i].position;
        }

        for (int i = 0;i < solverIterations;i++)
        {
            finalBonesPositions = SolveForwardPositions(SolveInversePositions(finalBonesPositions));
        }
        for(int i = 0; i < bones.Length;i++)
        {
            bones[i].position = finalBonesPositions[i];
            if(i != bones.Length - 1)
            {
                bones[i].rotation = Quaternion.LookRotation(finalBonesPositions[i+1] - bones[i].position);
            }
            else
            {
                bones[i].rotation = Quaternion.LookRotation(targetPostion.position - bones[i].position);
            }
        }
    }

    Vector3[] SolveInversePositions(Vector3[] forwardPositions)
    {
        Vector3[] inversePositions = new Vector3[forwardPositions.Length];

        for(int i = (forwardPositions.Length - 1); i >= 0;i--)
        {
            if( i == forwardPositions.Length - 1 )
            {
                inversePositions[i] = targetPostion.position;
            }
            else
            {
                Vector3 posPrimaSiguiente = inversePositions[i + 1];
                Vector3 posBaseActual = forwardPositions[i];
                Vector3 direccion = (posBaseActual - posPrimaSiguiente).normalized;
                float longitud = bonesLength[i];
                inversePositions[i] = posPrimaSiguiente + (direccion * longitud);
            }
        }

        return inversePositions;
    }

    Vector3[] SolveForwardPositions(Vector3[] inversePositions)
    {
        Vector3[] forwardPostions = new Vector3[inversePositions.Length];

        for (int i = 0; i < inversePositions.Length; i++)
        {
            if(i == 0)
            {
                forwardPostions[i] = bones[0].position;
            }
            else
            {
                Vector3 posPrimaActual = inversePositions[i];
                Vector3 posPrimaSegundaAnterior = forwardPostions[i - 1];
                Vector3 direccion = (posPrimaActual - forwardPostions[i - 1]).normalized;
                float longitud = bonesLength[i - 1];
                forwardPostions[i] = posPrimaSegundaAnterior + (direccion * longitud);
            }
        }

        return forwardPostions;
    }

    private void MoveSphere()
    {
        if(goingRight)
        {
            if (targetPostion.position.x > Position2.position.x)
            {
                goingRight = false;
            }
            else
            {
                targetPostion.position = new Vector3(targetPostion.position.x + 0.1f, targetPostion.position.y, targetPostion.position.z);
            }
        }
        else
        {
            if (targetPostion.position.x < Position1.position.x)
            {
                goingRight = true;
            }
            else
            {
                targetPostion.position = new Vector3(targetPostion.position.x - 0.1f, targetPostion.position.y, targetPostion.position.z);
            }

        }
            
    }
}
