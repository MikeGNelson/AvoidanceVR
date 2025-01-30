using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public Transform head; // Reference to the head of the character
    public float lookSpeed = 5f; // Speed of rotation when transitioning between modes
    public float minRotationY = -45f; // Minimum Y-axis rotation constraint
    public float maxRotationY = 45f; // Maximum Y-axis rotation constraint
    public bool isLookingAtPlayer = true; // Flag to switch between modes
    public Transform player; // Reference to the player

    void Start()
    {
        // Find the player dynamically once it spawns
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)  // Ensure the player is found
        {
            if (isLookingAtPlayer)
            {
                LookAtPlayer();
            }
            else
            {
                LookAwayFromPlayer();
            }
        }
    }

    // Makes the head look at the player
    void LookAtPlayer()
    {
        // Calculate the direction from the head to the player
        Vector3 direction = player.position - head.position;

        // Make sure the head only rotates on the y-axis (to keep a realistic head movement)
        direction.y = 0;

        // Calculate the desired rotation to look at the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Apply the rotation with constraints
        head.rotation = Quaternion.Euler(
            head.rotation.eulerAngles.x,
            Mathf.Clamp(targetRotation.eulerAngles.y, minRotationY, maxRotationY),
            head.rotation.eulerAngles.z
        );

        // Smoothly interpolate the current rotation to the target rotation
        head.rotation = Quaternion.Slerp(head.rotation, targetRotation, Time.deltaTime * lookSpeed);
    }

    // Makes the head look away from the player
    void LookAwayFromPlayer()
    {
        // Calculate the direction opposite to the player
        Vector3 direction = head.position - player.position;

        // Make sure the head only rotates on the y-axis (to keep a realistic head movement)
        direction.y = 0;

        // Calculate the desired rotation to look away from the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Apply the rotation with constraints
        head.rotation = Quaternion.Euler(
            head.rotation.eulerAngles.x,
            Mathf.Clamp(targetRotation.eulerAngles.y, minRotationY, maxRotationY),
            head.rotation.eulerAngles.z
        );

        // Smoothly interpolate the current rotation to the target rotation
        head.rotation = Quaternion.Slerp(head.rotation, targetRotation, Time.deltaTime * lookSpeed);
    }

    // Apply rotation after animations are applied
    void LateUpdate()
    {
        // Only update the head rotation after the animation is done
        if (player != null)
        {
            if (isLookingAtPlayer)
            {
                LookAtPlayer();
            }
            else
            {
                LookAwayFromPlayer();
            }
        }
    }

    // Toggle between looking at the player and looking away
    public void ToggleHeadMode()
    {
        isLookingAtPlayer = !isLookingAtPlayer;
    }
}
