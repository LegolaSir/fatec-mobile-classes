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
        MoveForwards();
        //MoveByKeyboard();
        MoveByMouse();
    }

    public void MoveByKeyboard()
    {
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed * Time.fixedDeltaTime;

        // Check if we're moving to the side 
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(horizontalSpeed, 0, 0);
        }
    }

    public void MoveByMouse()
    {
        float horizontalSpeed = dodgeSpeed * Time.fixedDeltaTime;

        if (Input.GetMouseButton(0))
        {
            // Converting mouse click or touch into a viewport point, in order to simplify interaction towards game world space
            Vector3 worldPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            // Proper movement in X axis, related to 'worldPos'
            int xMove;

            // Setting player direction towards X axis according to 'worldPos' value
            _ = (worldPos.x < 0.5f) ? xMove = -1 : xMove = 1;

            // X value to be implemented into new player location
            horizontalSpeed *= xMove;
            this.transform.position += new Vector3(horizontalSpeed, 0, 0);
        }
    }

    public void MoveForwards()
    {
        rb.AddForce(0, 0, rollSpeed);
        if (rb.velocity.z >= rollSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rollSpeed);
    }
}