using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Items
{
    public class Item : MonoBehaviour
    {
        public float LiveTime;    //存在时间
        public float CoolDown;    //冷却
        public bool isCoolDown;   //是否冷却完成
        public int level;         //武器等级
        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="time"></param>
        public IEnumerator Wait(float time)
        {
          for(float a = 0; a < time; a += Time.deltaTime)
             {
              yield return 0;
             }
         }
    }
}