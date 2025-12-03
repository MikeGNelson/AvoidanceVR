using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallTiltPhysics : MonoBehaviour
{
    public Transform platform;
    public float gravityStrength = 9.81f;
    [Range(0f, 1f)] public float worldWeight = 0.5f; // Blend factor for world vs local gravity
    public float resetHeight = 0.2f;

    [Header("Roll Control")]
    public float angularDamping = 0.98f;
    public float maxRollSpeed = 20f;

    private Rigidbody rb;

    public GameObject convex;
    public GameObject concave;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        SetPlatform(0);
    }

    void FixedUpdate()
    {

        Vector3 worldDown = Physics.gravity.normalized;
        Vector3 localDown = -platform.up;

        // Blend the two directions
        Vector3 blendedDown = Vector3.Lerp(worldDown, localDown, 1f - worldWeight).normalized;


        RaycastHit hit;
        if (Physics.Raycast(rb.position, -platform.up, out hit, 2f))
        {
            // Get the actual surface normal under the ball
            Vector3 surfaceNormal = hit.normal;

            // Blend between world gravity and surface normal
            blendedDown = Vector3.Lerp(worldDown, -surfaceNormal, 1f - worldWeight).normalized;

            // Apply gravity
            rb.AddForce(blendedDown * gravityStrength, ForceMode.Acceleration);
        }
        else
        {
            // Fallback to world gravity if no surface under the ball
            rb.AddForce(blendedDown * gravityStrength, ForceMode.Acceleration);
        }

        // Apply / cap angular damping
        rb.angularVelocity *= angularDamping;
        if (rb.angularVelocity.magnitude > maxRollSpeed)
        {
            rb.angularVelocity = rb.angularVelocity.normalized * maxRollSpeed;
        }


        // Check if ball has fallen
        float distance = Vector3.Distance(rb.position, platform.position);
        if (distance > .5f) // Probaly should change to be below the platform
        {
            ResetBall();
        }
    }

    public void SetPlatform(int mode)
    {
        Debug.Log("PLatform mode: " +mode);
        switch (mode)
        {
            case 0:
                platform = null; 
                convex.SetActive(false);
                concave.SetActive(false);
                this.GetComponent<Renderer>().enabled = false;
                break;
            case 1:
                
                convex.SetActive(true);
                concave.SetActive(false);
                this.GetComponent<Renderer>().enabled = true;
                platform = convex.transform;
                break;
            case 2:
                
                convex.SetActive(false);
                concave.SetActive(true);
                this.GetComponent<Renderer>().enabled = true;
                platform = concave.transform;
                break;
        }
    }

    private void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = platform.position + Vector3.up * resetHeight;
    }
}