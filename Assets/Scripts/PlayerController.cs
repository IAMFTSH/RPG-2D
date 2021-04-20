
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D RB;

    private Collider2D coll;

    private Animator animator;

    public float speed;
    public float jumpForce;
    public int JumpCount;
    public Transform underGroundCheck;
    public float HurtTime;

    public float HurtY;

    public float HurtX;
    public LayerMask ground;
    public GameData GameData { get; set; }
    private int jumpCount;
    private bool jumpPressed;
    private bool isJump;
    private bool isGround;
    private AnimatorState animatorState;
    private GameObject canvas;
    private float hurtTimer;
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        canvas = GameObject.Find("Canvas");
        jumpCount = JumpCount;
        //TODO 暂时先这样吧
        GameData = new GameData();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        //是否在地面
        Vector2 box = new Vector2(0.5f, 0.1f);
        isGround = Physics2D.OverlapBox(underGroundCheck.position, box, 0, ground);
        //移动
        if (!animator.GetBool(AnimationConstString.HURTING))
        {
            Movement();
        }
        //跳跃
        jump();
        switchAnimation();

    }

    // Update is called once per frame
    void Update()
    {


        //Movement(1);
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            //将跳跃指令储存，待到fixedUpdate中执行
            jumpPressed = true;
        }
        UpdateUI();
    }

    void Movement()
    {

        //获得输入值的方向值
        float direction = Input.GetAxisRaw("Horizontal");
        //获得输入值的向量
        float vector = Input.GetAxis("Horizontal");
        bool isSameDirection = transform.localScale.x == direction;
        RB.velocity = new Vector2(vector * speed, RB.velocity.y);
        if (direction != 0)
        {
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    void switchAnimation()
    {
        if (RB.velocity.x != 0)
        {
            animator.SetBool(AnimationConstString.RUNNING, true);
        }
        else
        {
            animator.SetBool(AnimationConstString.RUNNING, false);
        }
        if (isGround)
        {
            animator.SetBool(AnimationConstString.FALLING, false);
            animator.SetBool(AnimationConstString.JUMPING, false);
        }
        else
        {
            if (RB.velocity.y < 0)
            {
                animator.SetBool(AnimationConstString.FALLING, true);
                animator.SetBool(AnimationConstString.JUMPING, false);
            }
            if (RB.velocity.y > 0)
            {
                animator.SetBool(AnimationConstString.JUMPING, true);
                animator.SetBool(AnimationConstString.FALLING, false);
            }
        }
        if (animator.GetBool(AnimationConstString.HURTING) && hurtTimer < Time.unscaledTime)
        {
            animator.SetBool(AnimationConstString.HURTING, false);
        }

    }
    void jump()
    {
        if (isGround)
        {
            jumpCount = JumpCount;
            //isJump = false;
        }
        if (jumpPressed && isGround)
        {
            //isJump=true;
            RB.velocity = new Vector2(RB.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && !isGround)
        {
            RB.velocity = new Vector2(RB.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
    }
    void UpdateUI()
    {

        Text[] texts = canvas.GetComponentsInChildren<Text>();
        foreach (Text t in texts)
        {
            if (t.name == "CherryNum")
            {
                t.text = GameData.Cherry.ToString();
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Cherry"))
        {
            GameObject.Destroy(collider.gameObject);
            GameData.Cherry++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Frog"))
        {

            //TODO 需要判断是否是踩到 而不是侧边碰到
            if (animator.GetBool(AnimationConstString.FALLING)&&!animator.GetBool(AnimationConstString.HURTING))
            {
                GameObject.Destroy(collision.gameObject);
                RB.velocity = new Vector2(RB.velocity.x, 15);
            }
            else if(!animator.GetBool(AnimationConstString.HURTING))
            {
                if (transform.position.x <= collision.gameObject.transform.position.x)
                {
                    RB.velocity = new Vector2(-HurtX, HurtY);

                }
                else
                {
                    RB.velocity = new Vector2(HurtX, HurtY);
                }
                animator.SetBool(AnimationConstString.HURTING, true);
                hurtTimer = Time.unscaledTime + HurtTime;
            }
        }
    }

}
public enum AnimatorState
{
    IDLE = 1, RUN = 2, JUMP = 4,
}
