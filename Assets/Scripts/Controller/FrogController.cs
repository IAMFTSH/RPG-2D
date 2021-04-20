using System.Security.Cryptography;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

public class FrogController :AbstractEmpty{
    public float velocityJumpX;
    public float velocityJumpY;
    public LayerMask ground;

    // Start is called before the first frame update
    // Update is called once per frame
    private void FixedUpdate()
    {
        switchAnimation();
        if (timer < Time.time)
        {
            Movement();
        }
    }

    void Update()
    {
    }

    public override void switchAnimation()
    {
        if (coll.IsTouchingLayers(ground))
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
    }

    public override void Movement()
    {
        Jump();
    }

    void Jump()
    {
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        RB.velocity = new Vector2(-velocityJumpX * transform.localScale.x, velocityJumpY);
        timer = Time.time + CD;
    }
}
