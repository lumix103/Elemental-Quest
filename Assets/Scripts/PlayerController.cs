// ========================================================================
//  There has to be another way to make this right? I was thrown in here
//  without learning Unity and C# so ¯\_(ツ)_/¯ ... I'll try my best to
//  make it look pretty.
//                                                               - Fabs <3
// ========================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public bool debugPrints;
    // ========================================================================
    // Movement Settings
    // ========================================================================
    public float coyoteTime;
    public float maxSpeed;
    public float heavyFallMultiper;
    public float jumpPower;
    public float slowFallMultiplyer;
    // ========================================================================
    // Movement Misc
    // ========================================================================
    private float defaultGravityScale;
    public float coyoteTimeCounter;
    // ========================================================================
    // Abilities Settings
    // ========================================================================
    public bool hasDashAbility;                     // Hang
    public bool hasDoubleJumpAbility;               // Hang
    public bool hasJumpAbility;                     // Hang, Cinder, Aqua
    public bool hasSlowFallAbility;                 // Cinder
    public bool hasLavaImmunityAbility;             // Cinder
    public bool hasWaterRopeAbility;                // Aqua
    public bool hasPhaseAbility;                    // Aqua
    public bool hasWaterImmunityAbility;            // Aqua, Hang
    public bool hasDamageResistanceAbility;         // Terra
    public bool hasCreateBridgeAbility;             // Terra
    public bool hasCreateElevatorAbility;           // Terra
    // ========================================================================
    // Character State
    // ========================================================================
    // ------------------------------------------------------------------------
    // ---> Movement State
    // ------------------------------------------------------------------------
    private bool isJumping;
    private bool isDashing;
    private bool isGrounded;
    private bool hasDoubleJumped;
    // ------------------------------------------------------------------------
    // ---> Face State
    // ------------------------------------------------------------------------
    private bool isFacingRight;
    // ------------------------------------------------------------------------
    // ---> Health State
    // ------------------------------------------------------------------------
    private int health;
    private bool isDead = false;
    // ------------------------------------------------------------------------
    // ---> Character Type State
    // ------------------------------------------------------------------------
    public enum CharacterTypeState
    {
        AQUA,
        CINDER,
        HANG,
        TERRA,
    }
    public CharacterTypeState isCharacter;
    private CharacterTypeState prevCharacter; // temp code
    // ========================================================================
    //  Game Objects, Input, and Ground Values
    // ========================================================================
    public string horizontalAxis;
    public string verticalAxis;
    private Rigidbody2D playerRB;
    private BoxCollider2D playerCollider;
    private SpriteRenderer playerSprite;
    private Vector2 dirInput;
    private Animator animator;
    public LayerMask groundLayer;
    public float groundCheckRadius;
    // ========================================================================
    //  Initialize all the player's value duh ...
    //                                                              - Fabs -_-
    // ========================================================================
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultGravityScale = 1.0f;
        isFacingRight = true;
        prevCharacter = isCharacter;
        AbilityUpdate(isCharacter);
    }

    // ========================================================================
    //  Only update the player's movement state in here and check for the
    //  player's input in here. Also it might be a good idea to flip the
    //  player's sprite here depending on what direction they're going.
    //                                                               - Fabs :O
    // ========================================================================
    void Update()
    {
        if (isDead)
        {
            StartCoroutine(DeathScene());
            return;
        }
        if (prevCharacter != isCharacter) AbilityUpdate(isCharacter);
        prevCharacter = isCharacter;
        dirInput.x = Input.GetAxis(horizontalAxis);
        //Debug.Log("Update input.x =" + input.x);
        dirInput.y = Input.GetAxisRaw(verticalAxis);
        if (dirInput.x > 0) isFacingRight = true;
        else if (dirInput.x < 0) isFacingRight = false;
        FlipSprite();
        //Debug.Log("isFacingRight " + isFacingRight);
        isGrounded = IsGrounded();
        if (isGrounded) { hasDoubleJumped = false; isDashing = false; coyoteTimeCounter = coyoteTime; }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (isJumping && playerRB.velocity.y < 0)
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0f && hasJumpAbility)
        {
            isJumping = true;
            isGrounded = false;
            if (playerRB.velocity.y < 0) playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
            playerRB.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpPower);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !hasDoubleJumped && hasDoubleJumpAbility && hasJumpAbility)
        {
            hasDoubleJumped = true;
            if (playerRB.velocity.y < 0) playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
            playerRB.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpPower);
        }
        if (playerRB.velocity.y < 0 && hasJumpAbility)
        {
            if (hasSlowFallAbility && Input.GetKey(KeyCode.Space) && !isJumping)
            {
                playerRB.gravityScale = defaultGravityScale * 0.5f * slowFallMultiplyer;

            }
            else
            {
                playerRB.gravityScale = defaultGravityScale * 4;
            }

        }
        else if (playerRB.velocity.y > 0 && Input.GetKey(KeyCode.Space) && isJumping)
        {
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            playerRB.gravityScale = defaultGravityScale * 0.75f;
            coyoteTimeCounter = 0f;
        }
        else if (!hasJumpAbility && !isGrounded)
        {
            playerRB.gravityScale = defaultGravityScale * heavyFallMultiper;
        }
        else
        {
            playerRB.gravityScale = defaultGravityScale;
        }
    }

    // ========================================================================
    //  Call all the movement code in here if you are going to edit anything.
    //  Also update the player's state in the update code unless it's
    //  physics related in here.
    //                                                               - Fabs :3
    // ========================================================================
    void FixedUpdate()
    {
        Run();
    }

    // ========================================================================
    //  Run Code
    // ========================================================================
    void Run()
    {
        if (dirInput.x != 0)
        {
            float targetSpeed = dirInput.x * maxSpeed;
            targetSpeed = Mathf.Lerp(playerRB.velocity.x, targetSpeed, 1);
            float accelRate = 7;
            float speedDif = targetSpeed - playerRB.velocity.x;
            float movement = speedDif * accelRate;
            playerRB.AddForce(movement * Vector2.right, ForceMode2D.Force);
            animator.SetBool("isRunning", true);
        }
        else
        {
            playerRB.velocity = playerRB.velocity * ((Vector2.left * Vector2.zero) + Vector2.up);
            animator.SetBool("isRunning", false);
        }
    }
    // ========================================================================
    //  Grounded Code
    // ========================================================================
    bool IsGrounded()
    {
        // These were hard coded values when I was making this so .... good luck
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.size * playerCollider.transform.localScale, 0f, Vector2.down, 0.1f, groundLayer);
    }
    // ====================================================================================================================
    //  Flip Sprite Code
    // ====================================================================================================================
    void FlipSprite()
    {
        playerSprite.flipX = !isFacingRight;
    }
    // ========================================================================
    //  Ability Upate Code
    //  ---> Update all the player's ability flags here when the player
    //  ---> changes characters here. Maybe in a future update, make sure they
    //  ---> actually change into that character some how.
    //      ---> Aqua
    //          ---> Water Rope, Jump, Water Immunity
    //      ---> Cinder
    //          ---> Slow Fall, Jump, Lava Immunity
    //      ---> Hang
    //          ---> Jump, Double Jump, Dash
    //      ---> Terra
    //          ---> Damage Immunity, Create Bridge, Create Elevator
    // ========================================================================
    void AbilityUpdate(CharacterTypeState changeTo)
    {
        switch (changeTo)
        {
            case CharacterTypeState.AQUA:
                hasDashAbility = false;
                hasDoubleJumpAbility = false;
                hasJumpAbility = true;
                hasSlowFallAbility = false;
                hasLavaImmunityAbility = false;
                hasWaterRopeAbility = true;
                hasWaterImmunityAbility = true;
                hasDamageResistanceAbility = false;
                hasCreateBridgeAbility = false;
                hasCreateElevatorAbility = false;
                break;
            case CharacterTypeState.CINDER:
                hasDashAbility = false;
                hasDoubleJumpAbility = false;
                hasJumpAbility = true;
                hasSlowFallAbility = true;
                hasLavaImmunityAbility = true;
                hasWaterRopeAbility = false;
                hasWaterImmunityAbility = false;
                hasDamageResistanceAbility = false;
                hasCreateBridgeAbility = false;
                hasCreateElevatorAbility = false;
                break;
            case CharacterTypeState.HANG:
                hasDashAbility = true;
                hasDoubleJumpAbility = true;
                hasJumpAbility = true;
                hasSlowFallAbility = false;
                hasLavaImmunityAbility = false;
                hasWaterRopeAbility = false;
                hasWaterImmunityAbility = false;
                hasDamageResistanceAbility = false;
                hasCreateBridgeAbility = false;
                hasCreateElevatorAbility = false;
                break;
            case CharacterTypeState.TERRA:
                hasDashAbility = false;
                hasDoubleJumpAbility = false;
                hasJumpAbility = false;
                hasSlowFallAbility = false;
                hasLavaImmunityAbility = false;
                hasWaterRopeAbility = false;
                hasWaterImmunityAbility = false;
                hasDamageResistanceAbility = true;
                hasCreateBridgeAbility = true;
                hasCreateElevatorAbility = true;
                break;
        }
    }

    void DebugPrint(string printable)
    {
        if (debugPrints)
            Debug.Log(printable);
    }

    IEnumerator Dash()
    {
        float ogGravity = playerRB.gravityScale;
        playerRB.gravityScale = 0;
        float targetSpeed = dirInput.x * maxSpeed;
        isDashing = true;
        playerRB.velocity = new Vector2((isFacingRight ? 1 : -1) * 75, 0f);
        //playerRB.AddForce((isFacingRight ? 1 : -1) * Vector2.right * 75f, ForceMode2D.Force);
        yield return new WaitForSeconds(2f);
        playerRB.gravityScale = ogGravity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            if (hasLavaImmunityAbility)
            {

            }
            else
            {
                isDead = true;
            }
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            if (hasWaterImmunityAbility) { } else isDead = true;
        }
        else if (collision.gameObject.CompareTag("Spikes"))
        {
            if (hasDamageResistanceAbility)
            {

            }
            else
            {
                isDead = true;
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            isDead = true;
        }
    }

    IEnumerator DeathScene()
    {
        playerRB.gravityScale = 0.0f;
        animator.SetBool("isDead", isDead);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isDead = false;
    }
}