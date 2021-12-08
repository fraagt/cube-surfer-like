using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollower : MonoBehaviour
{
    public List<Transform> targets;

    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public float smoothTime = .5f;

    public float minZoom;
    public float maxZoom;
    public float zoomLimiter;

    private Vector3 velocity;
    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets.Count == 0) return;

        Move();
        Rotation();
        Zoom();
    }

    private void Rotation()
    {
        var rotation = targets[0].rotation;
        transform.rotation = rotation * Quaternion.Euler(rotationOffset);
    }

    private void Zoom()
    {
        float newZoom =
            Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        camera.fieldOfView =
            Mathf.Lerp(camera.fieldOfView, newZoom, Time.deltaTime);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + positionOffset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 1; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        
        return bounds.size.y;
    }
}
