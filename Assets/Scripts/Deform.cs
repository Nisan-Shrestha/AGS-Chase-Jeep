using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deform : MonoBehaviour
{
    [Range(0, 10)]
    public float deformRadius = 0.2f;
    [Range(0, 10)]
    public float maxDeform = 0.1f;
    [Range(0, 1)]
    public float damageFalloff = 1;
    [Range(0, 10)]
    public float damageMultiplier = 1;
    [Range(0, 100000)]
    public float minDamage = 1;

    public AudioClip[] collisionSounds;
    [Range(0, 1)]
    public float AudioVolume = 1.0f;
    public MeshFilter filter;
    public Rigidbody physics;
    public MeshCollider coll;
    private Vector3[] startingVerticies;
    private Vector3[] meshVerticies;

    void Start()
    {
        if (filter == null)
            filter = GetComponent<MeshFilter>();
        if (physics == null)
            physics = GetComponent<Rigidbody>();

        if (coll == null && GetComponent<MeshCollider>())
            coll = GetComponent<MeshCollider>();

        startingVerticies = filter.mesh.vertices;
        meshVerticies = filter.mesh.vertices;
    }

    void OnCollisionEnter(Collision collision)
    {
        float collisionPower = collision.impulse.magnitude;

        if (collisionPower > minDamage)
        {
            if (collisionSounds.Length > 0)
                AudioSource.PlayClipAtPoint(collisionSounds[Random.Range(0, collisionSounds.Length)], transform.position, 1f);
            foreach (ContactPoint point in collision.contacts)
            {
                for (int i = 0; i < meshVerticies.Length; i++)
                {
                    Vector3 vertexPosition = meshVerticies[i];
                    Vector3 pointPosition = transform.InverseTransformPoint(point.point);
                    float distanceFromCollision = Vector3.Distance(vertexPosition, pointPosition);
                    float distanceFromOriginal = Vector3.Distance(startingVerticies[i], vertexPosition);

                    if (distanceFromCollision < deformRadius && distanceFromOriginal < maxDeform) // If within collision radius and within max deform
                    {
                        float falloff = 1 - (distanceFromCollision / deformRadius) * damageFalloff;

                        float xDeform = pointPosition.x * falloff;
                        float yDeform = pointPosition.y * falloff;
                        float zDeform = pointPosition.z * falloff;
                        xDeform = Random.Range(xDeform/2.0f, xDeform );
                        yDeform = Random.Range(yDeform/2.0f, yDeform );
                        zDeform = Random.Range(zDeform/2.0f, zDeform );

                        xDeform = Mathf.Clamp(xDeform, 0, maxDeform);
                        yDeform = Mathf.Clamp(yDeform, 0, maxDeform);
                        zDeform = Mathf.Clamp(zDeform, 0, maxDeform);

                        Vector3 deform = new Vector3(xDeform, yDeform, zDeform);
                        meshVerticies[i] -= deform * damageMultiplier;
                    }
                }
            }

            UpdateMeshVerticies();
        }
    }

    void UpdateMeshVerticies()
    {
        filter.mesh.vertices = meshVerticies;
        coll.sharedMesh = filter.mesh;
    }
}