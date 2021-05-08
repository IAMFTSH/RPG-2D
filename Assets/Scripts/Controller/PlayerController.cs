
using System;
using System.Collections;
using System.Collections.Generic;
using RPG;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : AbstractCharacter
{
    // Start is called before the first frame update

    [Header("速度")]
    public float Speed;
    public float CreepingSpeed;
    [Header("跳跃")]
    public float JumpForce;
    public int JumpCount;
    public Transform UnderGroundCheck;
    [Header("受伤动画设置")]
    public float HurtTime;
    public float HurtY;
    public float HurtX;
    [Header("其他")]
    //TODO 太捞了
    public GameObject DeadDialog;
    public GameData GameData { get; set; }
    private LayerMask Ground;
    private int jumpCount;
    private bool jumpPressed;
    private bool isJump;
    private bool isGround;
    private AnimatorState animatorState;
    private GameObject canvas;
    private float hurtTimer;
    List<Collider2D> list;
    ContactFilter2D contact;
    protected override void Awake()
    {
        base.Awake();
        canvas = GameObject.Find("Canvas");
        jumpCount = JumpCount;
        //TODO 暂时先这样吧
        GameData = new GameData();
        Ground = LayerMask.GetMask("Ground");
        list=new List<Collider2D>();
        contact= new ContactFilter2D();
        contact.SetLayerMask(Ground);

    }

    void FixedUpdate()
    {
        //是否在地面
        isGround = Physics2D.OverlapCircle(UnderGroundCheck.position, 0.2f, Ground);
        //移动
        if (!animator.GetBool(AnimationConstString.HURTING))
        {
            Movement();
            Crouch();
        }
        //跳跃
        jump();
        switchAnimation();

    }

    // Update is called once per frame
    void Update()
    {
        
        //Movement(1);
        if (Input.GetButtonDown("Jump"))
        {
            if (Input.GetAxisRaw("Vertical") != -1 && jumpCount > 0)
            {
                //将跳跃指令储存，待到fixedUpdate中执行
                jumpPressed = true;
            }else if(Input.GetAxisRaw("Vertical") == -1){

                coll.OverlapCollider(contact,list);
                foreach(Collider2D c in list){
                    if(c.CompareTag("OneGround")){
                        coll.isTrigger=true;
                        break;
                    }
                }
            }
        }
        UpdateUI();
    }

    public override void Movement()
    {

        //获得输入值的方向值
        float direction = Input.GetAxisRaw("Horizontal");
        //获得输入值的向量
        float vector = Input.GetAxis("Horizontal");
        bool isSameDirection = transform.localScale.x == direction;
        if (animator.GetBool(AnimationConstString.Crouching))
        {
            RB.velocity = new Vector2(vector * CreepingSpeed, RB.velocity.y);
        }
        else
        {
            RB.velocity = new Vector2(vector * Speed, RB.velocity.y);
        }
        if (direction != 0)
        {
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    public void Crouch()
    {
        CapsuleCollider2D capsuleCollider = (CapsuleCollider2D)coll;
        if (Input.GetAxisRaw("Vertical") == -1)
        {
            capsuleCollider.offset = new Vector2(0, -0.6f);
            capsuleCollider.size = new Vector2(0.9f, 0.9f);
            animator.SetBool(AnimationConstString.Crouching, true);
        }
        else if (!Physics2D.OverlapCircle(transform.position, 0.3f, Ground))
        {
            capsuleCollider.offset = new Vector2(0, -0.3f);
            capsuleCollider.size = new Vector2(1f, 1.45f);
            animator.SetBool(AnimationConstString.Crouching, false);
        }
    }

    public override void switchAnimation()
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
        if (jumpPressed)
        {
            //isJump=true;
            RB.velocity = new Vector2(RB.velocity.x, JumpForce);
            jumpCount--;
            jumpPressed = false;
            AudioMangerController.instance.Jump();
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
            AudioMangerController.instance.Item();
        }
        if (collider.gameObject.name == "Deadline")
        {
            StartCoroutine("DieHandle");
        }
    }
        private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("OneGround"))
        {
            coll.isTrigger=false;
        }
    }
    IEnumerator DieHandle()
    {
        DeadDialog.SetActive(true);
        AudioMangerController.instance.Disable();
        AudioMangerController.instance.Death();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        asyncOperation.allowSceneActivation = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Empty"))
        {
            //玩家与敌人碰撞，判断是否踩到
            if (Physics2D.OverlapCircle(UnderGroundCheck.position, 0.2f, collision.gameObject.layer) && animator.GetBool(AnimationConstString.FALLING) && !animator.GetBool(AnimationConstString.HURTING))
            {
                RB.velocity = new Vector2(RB.velocity.x, 15);
                Animator frogAnimator = collision.gameObject.GetComponent<Animator>();
                frogAnimator.SetTrigger(AnimationConstString.DEATH);
            }
            //如果不是在伤害状体（无敌状态）则搜到伤害
            else if (!animator.GetBool(AnimationConstString.HURTING))
            {
                if (transform.position.x <= collision.gameObject.transform.position.x)
                {
                    RB.velocity = new Vector2(-HurtX, HurtY);
                }
                else
                {
                    RB.velocity = new Vector2(HurtX, HurtY);
                }
                AudioMangerController.instance.Hurt();
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
