using System.Data;
using UnityEngine;

namespace RPG
{
    public abstract class AbstractEmpty : AbstractCharacter, IEmpty
    {
        protected float timer;

        public float CD;
        protected override void Awake()
        {
            base.Awake();
            timer = Time.time;
        }
        //TODO 死亡前还会执行方法
        public void BeforeDestroy()
        {
            coll.enabled=false;
            RB.Sleep();
            audioSource.Play();
        }
        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}