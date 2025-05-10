using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.XR;

public class BoidFishController : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float linearVelocityMagnitude = 10.0f;

    [Range(0.0f, 100.0f)]
    public float separationFactor = 1.0f;

    [Range(0.0f, 100.0f)]
    public float cohesionFactor = 1.0f;

    [Range(0.0f, 100.0f)]
    public float alignmentFactor = 1.0f;

    [Range(0.0f, 100.0f)]
    public float separationTerrainFactor = 1.0f;

    [Range(0.0f, 200.0f)]
    public float boatAttractionFactor = 1.0f;

    [Range(0.0f, 100.0f)]
    public float queryRadius = 100.0f;

    [Range(0.0f, 100.0f)]
    public float terrainQueryRadius = 5.0f;



    void Update()
    {
        UpdateSeparation();

        //transform.position += Time.deltaTime * linearVelocityMagnitude * transform.forward;
        Vector3 vectorXZ = transform.forward;
        vectorXZ.y = 0;

        transform.position += Time.deltaTime * linearVelocityMagnitude * vectorXZ;
    }

    void UpdateSeparation()
    {
        //Standard boid behaviour
        Vector3 positionAverage = Vector3.zero;
        Vector3 directionAlignment = Vector3.zero;
        Vector3 directionSeparation = Vector3.zero;

        //Custom behaviour
        Vector3 directionTerrainSeparation = Vector3.zero;
        Vector3 directionTowardsBoat = Vector3.zero;


        int otherBoidsCount = 0;
        int terrainCount = 0;
        bool chasingPlayer = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, queryRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && (collider.transform.position - transform.position).magnitude <= queryRadius && collider.GetComponent<BoidFishController>() != null)
            {
                ++otherBoidsCount;
                positionAverage += collider.transform.position;
                directionAlignment += collider.transform.forward;
                directionSeparation += transform.position - collider.transform.position;
            }
            
            if (collider.gameObject != gameObject && (collider.transform.position - transform.position).magnitude <= terrainQueryRadius && collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                //Check for terrain so the boid can flee from its direction
                ++terrainCount;
                //Remove Y component from terrain position
                Vector3 colliderPos = new Vector3(collider.transform.position.x, transform.position.y, collider.transform.position.z);
                directionTerrainSeparation += transform.position - collider.transform.position;
            }
            
            if (collider.gameObject != gameObject && (collider.transform.position - transform.position).magnitude <= queryRadius && collider.gameObject.tag == "Player")
            {
                chasingPlayer = true;
                directionTowardsBoat = collider.transform.position - transform.position;
            }
            else
            {
                chasingPlayer = false;
            }
        }

        if (otherBoidsCount > 0)
        {
            //Boids
            positionAverage /= otherBoidsCount;
            directionAlignment.Normalize();
            directionSeparation.Normalize();

            Vector3 directionCohesion = positionAverage - transform.position;
            directionCohesion.Normalize();

            //Get Quaternion to rotate towards the specified direction rotating the up vector
            Quaternion orientationSeparation = Quaternion.LookRotation(directionSeparation, Vector3.up);
            Quaternion orientationCohesion = Quaternion.LookRotation(directionCohesion, Vector3.up);
            Quaternion orientationAlignment = Quaternion.LookRotation(directionAlignment, Vector3.up);

            //Apply rotations
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientationSeparation, separationFactor * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientationCohesion, cohesionFactor * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientationAlignment, alignmentFactor * Time.deltaTime);

        }

        if(terrainCount > 0)
        {
            directionTerrainSeparation.Normalize();
            Quaternion orientationTerrain = Quaternion.LookRotation(directionTerrainSeparation, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, orientationTerrain, separationTerrainFactor * Time.deltaTime);
            Debug.Log(terrainCount);
        }

        if(chasingPlayer)
        {
            directionTowardsBoat.Normalize();
            Quaternion oritentationBoat = Quaternion.LookRotation(directionTowardsBoat, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, oritentationBoat, boatAttractionFactor * Time.deltaTime);
        }
    }

}
