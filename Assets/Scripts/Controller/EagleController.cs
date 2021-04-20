
using RPG;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EagleController : AbstractEmpty
{
    public float velocityFlyY;

    private void Update()
    {
        Movement();
    }
    public override void Movement()
    {
        if (timer < Time.time)
        {
            RB.velocity=new Vector2(RB.velocity.x,velocityFlyY);
            timer += CD;
        }
    }

    public override void switchAnimation()
    {

    }
}