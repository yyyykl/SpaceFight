using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeaponType;
namespace Ship
{
    public class shipController : MonoBehaviour
    {
        public float MaxHp;            //最大生命
        public float CurrentHp;        //当前生命
        private GameObject ship;        //飞船
        public float MaxSpeed;         //飞船最大速度
        private float MoveSpeed;        //飞船当前移动速度
        public float Speed_a;          //飞船加速度
        public float cd;               //技能cd
        public int level;               //飞船等级
        public bool isCoolDown = true;  //技能是否冷却好

        public GameObject Weapon;      //子弹
        private GameObject res;
        private float ShootRate;       //射击间隔
        private Slider slider;         //血条
        public virtual void Start()
        {
            ship = this.gameObject;
            slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
            slider.maxValue = MaxHp;
            slider.minValue = 0;
            setSlider(CurrentHp);
        }
        void setSlider(float hp){
            slider.value = hp;
        }
        public virtual void FixedUpdate()
        {
            ShootRate -= Time.deltaTime;
            if (ShootRate <= 0)
            {
             //开火键
                if (Input.GetButton("Fire1"))
                {
                   
                    res =Instantiate(Weapon) as GameObject;
                    res.name = ("bullet" + Time.time);
                    res.transform.position = this.transform.position;
                    res.transform.rotation = this.transform.rotation;
                    Weapon weapon = res.GetComponent<Weapon>();         //Weapon要先实例化！！！！
                    if (weapon != null)
                    {
                        ShootRate = weapon.ShootRate;
                    //    Debug.Log(ShootRate);
                    }
                }
            }

            float hor = Input.GetAxis("Horizontal");     //飞船调整方向按键
            transform.Rotate(Vector3.back * hor * 80 * Time.deltaTime);

            float ver = Input.GetAxis("Vertical");       //飞船前进刹车按键
            Move(ver);
            transform.Translate(Vector3.up * MoveSpeed);
            
            GetComponent<Slider>();
        }
        
        //刚体碰撞事件
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("EnemyWeapon"))
            {
                Weapon weapon = collision.collider.GetComponent<Weapon>();
                if(weapon!=null){
                    CurrentHp-=weapon.Power;
                }
            }
            else if (collision.collider.CompareTag("Enemy"))
            {
                CurrentHp -= 4;
            }
            setSlider(CurrentHp);
            if (CurrentHp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        /// <summary>
        /// 计算当前速度
        /// </summary>
        void Move(float a)
        {
            if (a > 0)                      //按住加速键时
            {
                if (MoveSpeed >= MaxSpeed)
                {
                    MoveSpeed = MaxSpeed;
                }
                else
                {
                    MoveSpeed += Speed_a* Time.deltaTime;
                }
               
            }
            else
            if (a == 0)                     //松开加速键时
            {
                MoveSpeed += -Speed_a * Time.deltaTime;
                if (MoveSpeed <=0)
                {
                    MoveSpeed = 0;
                }           
               
                 
            }else if (a < 0)                //按减速键时
            {
                if (MoveSpeed <= -MaxSpeed)
                {
                    MoveSpeed = -MaxSpeed;
                }
                else
                {
                    MoveSpeed -=Speed_a * Time.deltaTime;
                }
               
            }
           
        }
    }
}