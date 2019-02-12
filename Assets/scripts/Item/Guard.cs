using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponType;
namespace Items
{
    public class Guard : Item
    {
        public GameObject weapon;
        private GameObject clone_Weapon;
        private float hp;
        private float chase_Speed;   //追击速度
        private float patrol_Speed;  //巡逻速度
        private bool isFound;       //范围内有敌人
        //private string target;
        private GameObject target;   //目标
        private bool needBack;       //超出范围
        private bool isBack;
        private bool isCanBack;
        Animator Fire;
        private void Awake()
        {
            hp = 50;
            chase_Speed = 1;
            patrol_Speed = 0.4f;
            Fire = GetComponentInChildren<Animator>();
            needBack = false;
            isBack = false;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("EnemyWeapon"))
            {
                Weapon weapon = collision.collider.GetComponent<Weapon>();
                if (weapon != null){
                    hp -= weapon.Power;
                }
            }
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// 敌人进入时标记这个敌人
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
              
                    // Debug.Log(collision.name);
                    if (target == null)
                    {
                        isFound = true;
                        isCanBack = true;
                        target = collision.gameObject;
                        //     Debug.Log(target);


                    }
                
            }
        }
        /// <summary>
        /// 敌人在视野中时追击并攻击敌人
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (isBack)                         //强制返航
            {
                if (collision.gameObject == target)
                {

                    Vector2 direction = collision.transform.position - this.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    
                    //大于攻击距离
                    if (Vector3.Distance(gameObject.transform.position, collision.transform.position) >= 4.5f)
                    {
                      //  CanFight = false;
                        transform.Translate(Vector3.up * chase_Speed * Time.deltaTime * 8);

                        Fire.SetBool("isFight", true);
                    }
                    //进行攻击
                    else
                    {
                        Fire.SetBool("isFight", false);
                        clone_Weapon = Instantiate(weapon) as GameObject;
                        //clone_Weapon.transform.parent = this.transform;
                        clone_Weapon.transform.localPosition = this.transform.position;
                        clone_Weapon.transform.localRotation = this.transform.rotation;

                    }

                    //距离过近
                    if (Vector3.Distance(collision.transform.position, transform.position) <= 2)
                    {
                        transform.Translate(Vector3.up * chase_Speed * Time.deltaTime * -5);
                    }
                    // Debug.Log(target);
                }
                //目标敌人死亡或离开视野
                if (target == null)
                {
                    //视野中有其他敌人则标记他
                    if (collision.CompareTag("Enemy"))
                    {
                        target = collision.gameObject;
                        isCanBack = true;
                    }
                    //视野中没有其他敌人
                    else 
                    {
                        if (isCanBack)   //锁定状态，不让它一直触发
                        {
                            isCanBack = false;
                            isFound = false;
                            isBack = false;
                            needBack = true;
                            //        Debug.Log("1");
                        }
                    }

                }
            }
        }
        /// <summary>
        /// 标记的敌人离开视野
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject==target)
            {

                //     Debug.Log("exit");
                isFound = false;
                target = null;
                needBack = true;
           //     Debug.Log("2");
            }
        }
        private void FixedUpdate()
        {
          //  Debug.Log(isBack);
          //  Debug.Log("back"+needBack);
          //  Debug.Log(target);
            Vector3 player_position = GameObject.FindGameObjectWithTag("Player").transform.position;
            //超出距离
            if (Vector3.Distance(transform.position, player_position) >= 20)
            {
              //      Debug.Log("3");
                isBack = false;
                needBack = true;
            }
            //是否需要返航
            if (needBack==true) {
                Fire.SetBool("isFight", true);
             //   isBack = false;
                //     Debug.Log("go");
                Vector2 direction = player_position - this.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
               
                if (Vector3.Distance(transform.position, player_position) <= 3)
                {
                   
                    needBack = false;
                   
                }

                transform.Translate(Vector3.up * chase_Speed * Time.deltaTime * 10);
            }else if (isFound)
            {

            }
            //巡逻状态
            else
            {
                isBack = true;
                Fire.SetBool("isFight", false);
                
                if (Vector3.Distance(transform.position, player_position) >= 5)
                {
                    needBack = true;
                }
                Vector2 direction = player_position - this.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (Vector3.Distance(transform.position, player_position) <= 3)
                {

                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);                    
                }
                transform.Translate(Vector3.up * patrol_Speed * Time.deltaTime);
            }
        }
        private void OnDestroy()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<GuardManager>().count_Guard--;
        }
    }
}