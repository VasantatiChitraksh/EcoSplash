using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    private bool isWalking;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 5f; // Jumping force
    private bool isGrounded; // To check if the player is on the ground
    private Rigidbody playerRigidbody; // Rigidbody for physics-based movement

    void Start()
    {
        // Get the Rigidbody component
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("Rigidbody not found on the player object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerHeight = 2f;
        float playerRadius = .7f;
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Attempt movement on X direction
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Attempt movement on Z direction
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        if (isWalking)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    private void HandleJump()
    {
        // Check if the jump input is pressed and the player is grounded
        if (gameInput.GetJumpInput() && isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Prevent multiple jumps
        }
    }

    // Unity's built-in methods for collision detection
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded by colliding with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
