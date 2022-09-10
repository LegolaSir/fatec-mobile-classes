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

    [Header("Swipe Config.")]
    [SerializeField] private float swipeMove = 2.0f;
    [SerializeField] private float minSwipeDistance = 2.0f;
    private Vector2 touchStart;


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
        float horizontalSpeed = dodgeSpeed * Time.fixedDeltaTime;

        MoveForwards();

        #if UNITY_STANDALONE || UNITY_WEBPLAYER
        MoveByKeyboard(horizontalSpeed);
        MoveByMouse(horizontalSpeed);
        #elif UNITY_IOS || UNITY_ANDROID
        MoveExclusiveByTouch(horizontalSpeed);
        #endif
    }

    public void MoveForwards()
    {
        rb.AddForce(0, 0, rollSpeed);
        if (rb.velocity.z >= rollSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rollSpeed);
    }

    public void MoveByKeyboard(float horizontalSpeed)
    {
        // Check if we're moving to the side 
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            horizontalSpeed *= Input.GetAxis("Horizontal");
            this.transform.position += new Vector3(horizontalSpeed, 0, 0);
        }
    }

    public void MoveByMouse(float horizontalSpeed)
    {
        if (Input.GetMouseButton(0))
        {
            horizontalSpeed *= SetDirectionByScreenPixel(Input.mousePosition);
            this.transform.position += new Vector3(horizontalSpeed, 0, 0);
        }
    }

    public void MoveExclusiveByTouch(float horizontalSpeed)
    {
        // Checking if there's at least a touch into the screen
        if(Input.touchCount > 0)
        {
            // Storing first touch into the screen
            Touch myTouch = Input.touches[0];

            SwipeTeleport(myTouch);

            //horizontalSpeed *= SetDirectionByScreenPixel(myTouch.position);
            //this.transform.position += new Vector3(horizontalSpeed, 0, 0);
        }
    }

    private int SetDirectionByScreenPixel(Vector3 screenPoint)
    {
        Vector3 worldPos = Camera.main.ScreenToViewportPoint(screenPoint);

        // Proper movement in X axis, related to 'worldPos'
        int xMove = 0;

        // Setting player direction towards X axis according to 'worldPos' value
        _ = (worldPos.x < 0.5f) ? xMove = -1 : xMove = 1;

        return xMove;
    }

    private void SwipeTeleport(Touch myTouch)
    {
        if (myTouch.phase == TouchPhase.Began) touchStart = myTouch.position;
        else if (myTouch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnd = myTouch.position;

            float xTrack = touchEnd.x - touchStart.x;
            if (Mathf.Abs(xTrack) < minSwipeDistance) return;

            Vector3 moveDirection = new Vector3();
            _ = (xTrack < 0.0f) ? (moveDirection = Vector3.left) : (moveDirection = Vector3.right);

            RaycastHit hit;
            if(!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (moveDirection * swipeMove));
            }
        }
    }
}