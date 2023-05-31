using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    float groundRadius;
    [SerializeField]
    LayerMask groundLayerMask;

    [SerializeField]
    float speed;

    [SerializeField]
    float jumpForce;

    public bool onGround = false;

    string currentAnimation;
    Animator animator;
    Rigidbody2D rb;

    float hAxis;

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer) { 
            hAxis = Input.GetAxis("Horizontal");
            if (onGround)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
        ChangeAnimation();
        Gravity();
    }

    void Jump() 
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position,
            groundRadius,
            groundLayerMask);

    }

    void Move() 
    {
        rb.velocity = new Vector2(hAxis * speed, rb.velocity.y);
    }

    void ChangeAnimation() 
    {
        if (Mathf.Abs(hAxis) > 0)
        {
            DoAnimation(PlayerAnimations.walk);
            if (hAxis > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (hAxis < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            DoAnimation(PlayerAnimations.idle);
        }
    }

    void Gravity() {
        if (!onGround) {
            rb.AddForce(Vector2.down * 100);
        }
    }

    void DoAnimation(string animation) 
    {
        if (animation == currentAnimation)
            return;
        currentAnimation = animation;
        animator.Play(currentAnimation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundRadius);
    }

}
