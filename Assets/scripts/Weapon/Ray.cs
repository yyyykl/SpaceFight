using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using Rocks;
namespace WeaponType
{
    public class Ray : Weapon
    {
        private LineRenderer line;     //LineRenderer
        private Ray2D ray;             //射线检测
        private RaycastHit2D hit;   
        private float Radius=5;        //武器攻击范围
        public GameObject spark;       //火花
        private GameObject clone;
    //    private float interval;
        public override void Awake()
        {
           
            LiveTime = Time.deltaTime ;
            Power = 1 / LiveTime;
            line = this.GetComponent<LineRenderer>();
            base.Awake();
        }
        void Start()
        {
            line.positionCount=2;
            line.startWidth=0.1f;
            line.endWidth = 0.1f;
            line.SetPosition(0,this.transform.position);

        }

        public override void FixedUpdate()
        {
            Vector3 forward_begin = transform.up * Radius;
            ray = new Ray2D(transform.position, forward_begin);
            int mask = LayerMask.GetMask("Enemy","Rock");
            hit = Physics2D.Raycast(transform.position, forward_begin,Radius,mask);
            line.SetPosition(0, this.transform.position);
            if (hit.collider != null) {
                if (hit.collider.CompareTag("Enemy")|| hit.collider.CompareTag("Rock"))
                {
                  //  Debug.Log(hit.point);
                  //  interval += Time.deltaTime;
                    line.SetPosition(1, hit.point);
                    Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Hp -= Power;
                        if (enemy.Hp <= 0)
                        {
                            Destroy(hit.collider.gameObject);
                        }
                    }
                    Rock rock = hit.collider.gameObject.GetComponent<Rock>();
                    if (rock != null)
                    {
                        rock.life -= Power;
                        if (rock.life <= 0)
                        {
                            Destroy(hit.collider.gameObject);
                        }
                    }
                    //  if (interval > 0.5f)
                    //  {
                    clone = Instantiate(spark) as GameObject;
                        clone.transform.position = new Vector3(hit.point.x,hit.point .y,-1);
                        clone.transform.rotation = transform.rotation;
                  //      interval = 0;
                  //  }
                }
            }
            
            else
            {
             //   Debug.Log("no target");
                line.SetPosition(1, transform.position + forward_begin);
            }
            Debug.DrawLine(transform.position, transform.position+forward_begin, Color.black);
        }
    }
}