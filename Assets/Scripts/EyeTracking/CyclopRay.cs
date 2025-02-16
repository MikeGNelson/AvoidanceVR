using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopRay : MonoBehaviour
{
    public Transform REye, LEye;

    [SerializeField]
    private float rayDistance = 1.0f;
    [SerializeField]
    private float rayWidth = 0.01f;
    [SerializeField]
    private LayerMask layersToInclude;
    [SerializeField]
    private Color rayColorDefaultState = Color.yellow;

    private LineRenderer lineRenderer;
    private Vector3 currentGazePoint;
    private string currentGazeGameObject;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupRay();
    }

    void SetupRay()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColorDefaultState;
        lineRenderer.endColor = rayColorDefaultState;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistance));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GetAveragePosition();
        transform.eulerAngles = GetAverageRotation();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance;

        if (Physics.Raycast(transform.position, rayCastDirection, out hit, Mathf.Infinity))
        {
            currentGazePoint = hit.point;

            if (hit.transform != null)
            {
                currentGazeGameObject = hit.transform.gameObject.name;
            }
        }
    }

    public Vector3 GetAveragePosition()
    {
        return (REye.position + LEye.position) / 2.0f;
    }

    public Vector3 GetAverageRotation()
    {
        return (REye.eulerAngles + LEye.eulerAngles) / 2.0f;
    }

    public Vector3 GetCurrentGazePoint()
    {
        return currentGazePoint;
    }

    public string GetCurrentGazeGameObject()
    {
        return currentGazeGameObject;
    }
    
    public Vector3 Direction()
    {
        return transform.TransformDirection(Vector3.forward);
    }
}
