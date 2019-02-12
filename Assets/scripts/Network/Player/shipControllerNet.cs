using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeaponType;
using Net;
namespace Ship
{
    public class shipControllerNet : MonoBehaviour
    {
        public float MaxHp;            //最大生命
        public float CurrentHp;        //当前生命
        private GameObject ship;        //飞船
        public float MaxSpeed;         //飞船最大速度
        private float MoveSpeed;        //飞船当前移动速度
        public float Speed_a;          //飞船加速度

        public GameObject Weapon;      //子弹
        private GameObject res;
        private float ShootRate;       //射击间隔
        private Slider slider;         //血条
        public JFSocket mJFsorket;
        private float mSynchronous;


        public virtual void Start()
        {
            mJFsorket = JFSocket.GetInstance();
            MaxHp = 100;
            CurrentHp = 100;
            MaxSpeed = 0.3f;
            Speed_a = 1;
            ship = this.gameObject;
            slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
            slider.maxValue = MaxHp;
            slider.minValue = 0;
            setSlider(CurrentHp);
        }
        void setSlider(float hp)
        {
            slider.value = hp;
        }
        /// <summary>
        /// 同步服务器坐标
        /// </summary>
        void Update()
        {

            mSynchronous += Time.deltaTime;
            //在Update中每0.5s的时候同步一次  
            if (mSynchronous > 0.5f)
            {
                int count = mJFsorket.worldpackage.Count;
                //当接受到的数据包长度大于0 开始同步  
                if (count > 0)
                {
                    //遍历数据包中 每个点的坐标  
                    foreach (JFPackage.WorldPackage wp in mJFsorket.worldpackage)
                    {
                        float x = (float)(wp.mPosx / 100.0f);
                        float y = (float)(wp.mPosy / 100.0f);
                        float z = (float)(wp.mPosz / 100.0f);

                        Debug.Log("x = " + x + " y = " + y + " z = " + z);
                        //同步主角的新坐标  
                        this.transform.position = new Vector3(x, y, z);
                    }
                    //清空数据包链表  
                    mJFsorket.worldpackage.Clear();
                }
                mSynchronous = 0;
            }
        }

        void FixedUpdate()
        {
            ShootRate -= Time.deltaTime;
            if (ShootRate <= 0)
            {

                if (Input.GetButton("Fire1"))
                {

                    res = Instantiate(Weapon) as GameObject;
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
            SendPlayerWorldMessage();
            GetComponent<Slider>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Weapon"))
            {
                Weapon weapon = collision.collider.GetComponent<Weapon>();
                if (weapon != null)
                {
                    CurrentHp -= weapon.Power;
                }
            }
            else
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
        /// Socket发送坐标
        /// </summary>
        void SendPlayerWorldMessage()
        {
            //组成新的结构体对象，包括主角坐标旋转等。  
            Vector3 PlayerTransform = transform.localPosition;
            Vector3 PlayerRotation = transform.localRotation.eulerAngles;
            //用short的话是2字节，为了节省包的长度。这里乘以100 避免使用float 4字节。当服务器接受到的时候小数点向前移动两位就是真实的float数据  
            short px = (short)(PlayerTransform.x * 100);
            short py = (short)(PlayerTransform.y * 100);
            short pz = (short)(PlayerTransform.z * 100);
            short rx = (short)(PlayerRotation.x * 100);
            short ry = (short)(PlayerRotation.y * 100);
            short rz = (short)(PlayerRotation.z * 100);
            byte equipID = 1;
            byte animationID = 9;
            byte hp = 2;
            JFPackage.WorldPackage wordPackage = new JFPackage.WorldPackage(px, py, pz, rx, ry, rz, equipID, animationID, hp);
            //通过Socket发送结构体对象  
            mJFsorket.SendMessage(wordPackage);
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
                    MoveSpeed += Speed_a * Time.deltaTime;
                }

            }
            else
            if (a == 0)                     //松开加速键时
            {
                MoveSpeed += -Speed_a * Time.deltaTime;
                if (MoveSpeed <= 0)
                {
                    MoveSpeed = 0;
                }


            }
            else if (a < 0)                //按减速键时
            {
                if (MoveSpeed <= 0)
                {
                    MoveSpeed = 0;
                }
                else
                {
                    MoveSpeed -= 10 * Speed_a * Time.deltaTime;
                }

            }

        }
    }
}