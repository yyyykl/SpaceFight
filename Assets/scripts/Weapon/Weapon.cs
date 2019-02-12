using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeaponType
{
    public class Weapon : MonoBehaviour
    {
        public float Speed;                //子弹速度
        public float LiveTime;             //子弹存在时间 
        public float Power;                //子弹威力   
        public float ShootRate;            //发射间隔
     // public float edge_Left, edge_Right, edge_Up, edge_Down;
     
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Awake()
        {
        //    Debug.Log(ShootRate);
            Destroy(gameObject,LiveTime);
        }
       
        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.collider.tag.CompareTo("Player") == 0)
        //    {
        //        Object.GetComponent<PolygonCollider2D>().isTrigger = true;
        //    }
        //}
        
        /// <summary>
        /// 子弹出飞船后可以进行碰撞
        /// </summary>
        public virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.CompareTo("Player") == 0)
            {
               GetComponent<PolygonCollider2D>().isTrigger =false;
            }
        }

        /// <summary>
        /// 子弹运动
        /// </summary>
        public virtual void Update()
        {
           
        }
        public virtual void FixedUpdate()
        {
            transform.Translate(Vector3.up * Speed);
        }
        public virtual void OnDestroy()
        {
            
        }
    }
}