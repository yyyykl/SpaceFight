using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponType;

public class RocketFindEnemy : MonoBehaviour
{
    public GameObject rocket;   //火箭预制体
    public GameObject smoke;    //尾气
    private float spaceTime;    //尾气产生间隔
    private void Start()
    {

    }
    //触发器内有敌人
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //向rocket传递敌人信息
            rocket.GetComponent<Rocket>().FindTarget(collision.transform.position);

        }
    }
    private void FixedUpdate()
    {
        spaceTime += Time.deltaTime;
        if (spaceTime > 0.1f)
        {
            smoke = Instantiate(smoke) as GameObject;
            smoke.transform.position = gameObject.transform.position;
            spaceTime = 0;
        }
      }
}
