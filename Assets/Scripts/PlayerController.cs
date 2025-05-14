using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minJump = 5f;
    public float maxJump = 15f;
    public float chargeRate = 10f;
    public float maxChargeTime = 2f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private float jumpPower = 0f;
    private bool charging = false;
    private float chargeTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // only allows movement if player isnt charging a jump
        if (!charging && isGrounded)
        {
            float move = Input.GetAxisRaw("Horizontal"); // a/d or left/right
            rb.velocity = new Vector3(move * moveSpeed, rb.velocity.y, 0f);
        }

        // start charging a jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            charging = true;
            jumpPower = minJump;
            chargeTimer = 0f;
        }

        // while holding, increase the jump power
        if (Input.GetKey(KeyCode.Space) && charging)
        {
            chargeTimer += Time.deltaTime;
            jumpPower += chargeRate * Time.deltaTime;
            jumpPower = Mathf.Clamp(jumpPower, minJump, maxJump);

            if (chargeTimer >= maxChargeTime)
            {
                ReleaseJump();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && charging)
        {
            ReleaseJump();
        }


       // not currently being used, keeping in case things go wrong lol//

       // if (Input.GetKeyUp(KeyCode.Space) && charging)
       // {
       //     rb.velocity = new Vector3(rb.velocity.x, 0f, 0f); // reset vertical speed
       //     rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
       //     charging = false;
       //     isGrounded = false;
       // }

    }

    void ReleaseJump()
    {
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        charging = false;
        isGrounded = false;
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
