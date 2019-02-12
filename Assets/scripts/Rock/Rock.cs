using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using WeaponType;
namespace Rocks
{
    public class Rock : MonoBehaviour
    {
        public float life;               //陨石生命
        public GameObject[] smallRock;   //生命耗尽时生成的新陨石
        public float Speed;              //陨石的速度       
        private Vector3 Position;
        private float Speed_a;
        private void Start()
        {
            Position = this.transform.position;
        }
        /// <summary>
        /// 碰撞
        /// </summary>
        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag.CompareTo("Player") == 0)
            {
                life -= 4;
            }
            else if (collision.collider.tag.CompareTo("Weapon") == 0)
            {
                Weapon weapon = collision.collider.GetComponent<Weapon>();
                if (weapon != null)
                {
                    float power = weapon.Power;
               //     Debug.Log("power" + power);
                    life -= power;
                    
                }
            }
            if (life <= 0)
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// 根据速度直线运动
        /// </summary>
        private void FixedUpdate()
        {
            //    Debug.Log(Speed);
            
            transform.Translate(Vector3.up * Speed*Time.deltaTime);
            Move();
        }
        /// <summary>
        /// 陨石根据加速度计算速度
        /// </summary>
         void Move()
        {
            Speed_a = 1;
            if (Speed <= 0)
            {
                Speed = 0;
            }
            Speed += -Speed_a * Time.deltaTime;
        }
        /// <summary>
        ///被摧毁后
        /// </summary>
    private void OnDestroy()
        {
            for (int i = 0; i < smallRock.Length; i++)
            {
                float res_X = Mathf.Pow(-1, i);
                float res_Y = Mathf.Pow(-1, i);
                Instantiate(smallRock[i]);
                smallRock[i].transform.position = new Vector3(Position.x+i*res_X,Position.y+i*res_Y,0);
                smallRock[i].transform.rotation =Quaternion.Euler(new Vector3(0,0,120f*i+45f));
                smallRock[i].GetComponent<smallRock>().SetSpeed(5);               
                // smallRock[i].transform.Translate(Vector3.up * i);
            }
        }

    }
}