using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ship
{
    public class defaultShip : shipController
    {
        public GameObject missile;         //导弹预制体
        private GameObject clone_missile;          
        private void Awake()
        {
            level = 1;
            MaxHp = 100;
            CurrentHp = 100;
            MaxSpeed = 0.3f;
            Speed_a = 1;
            cd = 4;
            
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //按下技能键
            if (Input.GetButtonUp("Fire2"))
            {
                if (isCoolDown == true)
                {
                    StartCoroutine(Skill(level));
                }

            }
            ///<summary>
            ///释放技能
            /// </summary>
            IEnumerator Skill(int i)
            {
                isCoolDown = false;
                StartCoroutine(CoolDown(cd));
                for (int a = 0; a < i + 1; a++)
                {
                    StartCoroutine(Fire());
                    yield return StartCoroutine(Wait(0.5f));
                }

            }
            ///<summary>
            ///技能进入cd
            /// </summary>
            IEnumerator CoolDown(float cd)
            {
                yield return StartCoroutine(Wait(cd));
                isCoolDown = true;
            }
            ///<summary>
            ///一轮发射
            /// </summary>
            IEnumerator Fire()
            {             
                for (int i = 0; i < 5; i++)
                {
                    float sin_angle = Mathf.Sin(transform.rotation.eulerAngles.z);
                    float cos_angle = Mathf.Cos(transform.rotation.eulerAngles.z);
                    clone_missile = Instantiate(missile) as GameObject;
                    clone_missile.transform.rotation = transform.rotation;
                    clone_missile.transform.position = new Vector3(cos_angle*( i- 2.5f )+ transform.position.x, sin_angle*(i-2.5f)+transform.position.y, transform.position.z+1);
                    yield return StartCoroutine(Wait(0.1f));
                }
            }
            ///<summary>
            ///计时器协程
            /// </summary>
            IEnumerator Wait(float duration)
            {
                for(float time = 0; time < duration; time += Time.deltaTime)
                {
                    yield return 0;
                }
            }
        }
    }
}