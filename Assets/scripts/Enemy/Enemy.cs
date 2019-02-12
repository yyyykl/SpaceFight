using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponType;
namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        public float Hp;                 //生命值
        public float MoveSpeed;          //徘徊速度
        public float RushSpeed;          //追击速度
        private float CurrentSpeed;      //当前速度 
        public float viewRadius;         //视野
        public int viewAngleStep;        //射线条数
        public GameObject EnemyWeapon;   //武器
        private GameObject useWeapon;
        private float ShootRate;         //射击间隔
        private bool isHit;
        private Vector3 target;

        /// <summary>
        /// 画视野射线
        /// </summary>
        void DrawFieldOfView()
        {
            //最开始射线的向量
            Vector3 forward_begin = transform.up * viewRadius;
            //依次处理每一条射线
            for (int i = 0; i <= viewAngleStep; i++)
            {
                //每条射线偏转（360/射线条数）度
                Vector3 v = Quaternion.Euler(0, 0, (360f / viewAngleStep) * i) * forward_begin;

                //创建射线
                Ray2D ray = new Ray2D(transform.position, v);
                int mask = LayerMask.GetMask("Player","Rock");
                RaycastHit2D hitt = Physics2D.Raycast(transform.position, v, viewRadius,mask);

                //射线终点
                Vector3 pos = transform.position + v;
                if (hitt.collider != null)
                {
                    //射线碰撞,射线终点变为碰撞点
                    pos = hitt.point;
                    
                    //射线发现玩家
                    if (hitt.collider.CompareTag("Player")|| hitt.collider.CompareTag("Guard"))
                    {
                        //开始攻击
                        isHit = true;
                        
                        target = hitt.collider.transform.position;
                        CurrentSpeed = RushSpeed;

                        //保持安全车距
                        if (Vector3.Distance(gameObject.transform.position, target) >= 3)  
                        {
                            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, CurrentSpeed * Time.deltaTime);
                        }
                        //（暂定解决方案）当目标位置大于视野半径时停止攻击
                        if (Vector3.Distance(gameObject.transform.position, target) >=viewRadius*0.95)   
                        {
                            isHit = false;
                        }
                    }
                    
                }
               
                Debug.DrawLine(transform.position, pos, Color.black);
            }
            //攻击
            if (isHit == true)
            {             
                //攻击间隔
                ShootRate -= Time.deltaTime;
                if (ShootRate <= 0)
                {
                    useWeapon = Instantiate(EnemyWeapon) as GameObject;
                    useWeapon.name = ("bullet" + Time.time);
                    useWeapon.transform.position = this.transform.position;
                    //子弹正对目标
                    Vector2 direction = target - this.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    useWeapon.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
                   
                
                    Weapon weapon = useWeapon.GetComponent<Weapon>();
                    if (weapon != null)
                    {
                        ShootRate = weapon.ShootRate;
                      //  Debug.Log(ShootRate);
                    }

                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //碰撞到子弹
            if (collision.collider.CompareTag("Weapon"))
            {
                Weapon weapon = collision.collider.GetComponent<Weapon>();
                if (weapon != null)
                {
                    Hp -= weapon.Power;
                }
            }else if(collision.collider.CompareTag("Player"))
            //碰撞到玩家
            {
                Hp -= 4;
            }
            if (Hp <= 0)
            {
                Destroy(gameObject);
            }
        }
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            DrawFieldOfView();
        }
    }
}
