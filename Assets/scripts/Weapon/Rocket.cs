using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeaponType
{
    public class Rocket : Weapon
    {
        public GameObject Boom;    //爆炸特效
        private Vector3 place;   //撞击位置
        private bool isFound;
        private Vector3 Target;
        public override void Awake()
        {
            Speed = 0.5f;
            LiveTime = 10;
            Power = 5;
            ShootRate = 1;
            isFound = false;
            base.Awake();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.CompareTag("Enemy")||collision.collider.CompareTag("Rock"))
            Destroy(this.gameObject);
            
        }

        public override void FixedUpdate()
        {
           
            
            place = gameObject.transform.position;
            if (isFound == true)
            {
                Vector2 direction = Target - this.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                gameObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
              
            }
           
            base.FixedUpdate();
        }
        /// <summary>
        /// 找到目标就改变状态
        /// </summary>
        public void FindTarget(Vector3 target)
        {
            isFound = true;
            Target=target;
        }
        //销毁时产生爆炸
        public override void OnDestroy()
        {
          //  Debug.Log(place );
            Boom=Instantiate(Boom) as GameObject;
            Boom.transform.position =place;
        }
    }
}