using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float runSpeed = 6f;

    [SerializeField]
    float jumpSpeed = 15f;

    [SerializeField]
    float climbSpeed = 6f;

    [SerializeField]
    Vector2 deathKick = new Vector2(0f, 13f);

    float coyoteTime = 0.2f; // Duration of coyote time
    float coyoteTimeCounter;
    public AudioSource deathSound;

    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    Animator playerAnimation;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float defaultGravity;
    bool isJumping = false;
    bool isAlive = true;
    bool canDash = true;
    bool isDashing = false;

    [SerializeField]
    float dashingPower = 24f;
    float dashingTime = 0.15f;
    float dashingCooldown = 1f;

    const float ZeroGravity = 0f;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        defaultGravity = playerRigidBody.gravityScale;
    }

    void Update()
    {
        if (FindObjectOfType<GameSession>().isPaused)
            return;

        if (isAlive && !isDashing)
        {
            Run();
            FlipSprite();
            ClimbLadder();
            Die();

            // Update coyote time counter
            if (IsTouchingGroundLayer())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            isJumping = false; // Reset isJumping when grounded
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isAlive && !isDashing)
            moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (FindObjectOfType<GameSession>().isPaused)
            return;

        if (isAlive && !isDashing)
        {
            if (context.performed && coyoteTimeCounter > 0f)
            {
                isJumping = true;
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
            }

            if (context.canceled && playerRigidBody.velocity.y > 0f)
            {
                playerRigidBody.velocity = new Vector2(
                    playerRigidBody.velocity.x,
                    playerRigidBody.velocity.y * 0.5f
                );
                coyoteTimeCounter = 0f;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (isAlive && canDash && !isDashing && buildIndex > 1)
            {
                StartCoroutine(Dash());
            }
        }
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = playerRigidBody.gravityScale;
        playerRigidBody.gravityScale = ZeroGravity;
        playerRigidBody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);
        playerRigidBody.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;

        playerAnimation.SetBool("isRunning", HasHorizontalSpeed());
    }

    void FlipSprite()
    {
        if (HasHorizontalSpeed())
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1f);
    }

    void ClimbLadder()
    {
        if (IsTouchingClimbingLayer() && !isJumping)
        {
            Vector2 climbVelocity = new Vector2(
                playerRigidBody.velocity.x,
                moveInput.y * climbSpeed
            );

            playerRigidBody.velocity = climbVelocity;
            playerRigidBody.gravityScale = ZeroGravity;

            playerAnimation.SetBool("isClimbing", HasVerticalSpeed());
        }
        else
        {
            playerRigidBody.gravityScale = defaultGravity;
            playerAnimation.SetBool("isClimbing", false);
        }
    }

    void Die()
    {
        if (IsTouchingEnemyAndTrapLayer())
        {
            isAlive = false;
            playerAnimation.SetTrigger("Dying");
            playerRigidBody.velocity = deathKick;
            deathSound.Play();

            Invoke("DeathProc", 1f);
        }
    }

    void DeathProc()
    {
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    bool HasHorizontalSpeed()
    {
        return (Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon);
    }

    bool HasVerticalSpeed()
    {
        return (Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon);
    }

    bool IsTouchingGroundLayer()
    {
        return (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")));
    }

    bool IsTouchingClimbingLayer()
    {
        return (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")));
    }

    bool IsTouchingEnemyAndTrapLayer()
    {
        return (
            playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Trap", "Water"))
            || playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Trap", "Water"))
        );
    }

    bool IsTouchClimbLayer()
    {
        return playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
    }
}