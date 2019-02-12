using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponType;
namespace Items
{
    public class Shell : Item
    {

        private void Awake()
        {
            LiveTime = 2;
            CoolDown = 5;
            isCoolDown = true;
        }

        /// <summary>
        /// 进入触发器反弹子弹
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("EnemyWeapon"))          //碰撞体为敌人子弹
            {
                StartCoroutine(Trigger(LiveTime,CoolDown));
                Weapon weapon = collider.GetComponent<Weapon>();
                if (weapon != null)
                {
                    weapon.Speed *= -1;
                    collider.tag = "Weapon";
                }
            }
            //触发
            IEnumerator Trigger(float duration,float cd)
            {
                //继续存在
                yield return StartCoroutine(Wait(duration));  
                //进入cd
                isCoolDown = false;                           
                yield return StartCoroutine(Wait(CoolDown));
                //cd转好
                isCoolDown = true;
            }
                
        }
        private void FixedUpdate()
        {
            //判断是否在cd
            if (isCoolDown)         
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            }
        }
    }
}