using System.Data;
using UnityEngine;

namespace RPG
{
    public abstract class AbstractCharacter : MonoBehaviour
    {
        protected Animator animator;
        protected Collider2D coll;
        protected Rigidbody2D RB;

        protected AudioSource audioSource;
        protected virtual void Awake() {
            RB = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            coll = GetComponent<Collider2D>();
            audioSource = GetComponent<AudioSource>();
        }

        public abstract void Movement();
        public abstract void switchAnimation();
    }
}