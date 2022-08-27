using UnityEngine;

/// <summary> 
/// Responsible for moving the player automatically and 
/// reciving input. 
/// </summary> 
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary> 
    /// A reference to the Rigidbody component 
    /// </summary> 
    private Rigidbody rb;

    [Tooltip("How fast the ball moves left/right")]
    public float dodgeSpeed = 5;

    [Tooltip("How fast the ball moves forwards automatically")]
    [Range(0, 10)]
    public float rollSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        // Get access to our Rigidbody component 
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// FixedUpdate is called at a fixed framerate and is a prime place to put
    /// Anything based on time.
    /// </summary>
    void FixedUpdate()
    {
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed * Time.fixedDeltaTime;

        // Check if we're moving to the side 
        rb.AddForce(0, 0, rollSpeed);
        if (rb.velocity.z >= rollSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rollSpeed);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(horizontalSpeed, 0, 0);
        }
    }
}