using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minJump = 5f;
    public float maxJump = 15f;
    public float chargeRate = 10f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private float jumpPower = 0f;
    private bool charging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // horizontal movement
        float move = Input.GetAxisRaw("Horizontal"); // a/d or left/right
        if (isGrounded)
        {
            rb.velocity = new Vector3(move * moveSpeed, rb.velocity.y, 0f);
        }

        // handles the charging of jumps
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            charging = true;
            jumpPower = minJump;
        }

        // the charge increases as the space bar is held down
        if (Input.GetKey(KeyCode.Space) && charging)
        {
            jumpPower += chargeRate * Time.deltaTime;
            jumpPower = Mathf.Clamp(jumpPower, minJump, maxJump);
        }

        // release the space bar to finish the jump
        if (Input.GetKeyUp(KeyCode.Space) && charging)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, 0f); // reset vertical speed
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            charging = false;
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // checks whether the player is grounded
        if (collision.contacts[0].normal == Vector3.up)
        {
            isGrounded = true;
        }
    }
}
