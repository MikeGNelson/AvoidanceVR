using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;        // Object to orbit around
    public float distance = 5f;     // Distance from the target
    public float orbitSpeed = 30f;  // Speed of rotation in degrees per second
    public Vector3 offset = Vector3.up; // Optional vertical offset from target center

    private float currentAngle = 0f;

    void Update()
    {
        if (target == null) return;

        // Increment angle based on speed and time
        currentAngle += orbitSpeed * Time.deltaTime;

        // Convert angle to radians for Mathf functions
        float radians = currentAngle * Mathf.Deg2Rad;

        // Calculate new camera position
        Vector3 newPos = target.position + offset + new Vector3(
            Mathf.Sin(radians) * distance,
            0f,
            Mathf.Cos(radians) * distance
        );

        transform.position = newPos;

        // Always look at the target
        transform.LookAt(target.position + offset);
    }
}
