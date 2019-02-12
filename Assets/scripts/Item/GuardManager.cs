using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Items
{
    public class GuardManager : Item
    {
        public GameObject Guard;          //护卫预制体
        private GameObject clone_Guard;       
        public int count_Guard;           //当前护卫数量
        private void Awake()
        {   
            CoolDown = 5;
            isCoolDown = true;
            level = 2;
            count_Guard = 0;
        }
        //创建护卫
        IEnumerator CreateGuard()
        {
            clone_Guard = Instantiate(Guard) as GameObject;
            clone_Guard.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
            yield return 0;
        }
        //等待有护卫被摧毁则创建一个新的护卫并进入cd
        IEnumerator WaitDestory()
        {
            count_Guard++;
            StartCoroutine(CreateGuard());
            isCoolDown = false;
            yield return StartCoroutine(Wait(CoolDown));
            isCoolDown = true;                          
        }
        private void FixedUpdate()
        {       
            //根据技能等级创建相应数量的护卫，有CD
                for(; count_Guard < level&&isCoolDown;)
                {
                    StartCoroutine(WaitDestory());
                }
                       
        }
    }
}