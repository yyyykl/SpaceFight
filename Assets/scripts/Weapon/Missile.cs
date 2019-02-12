using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeaponType
{
    public class Missile : Weapon
    {
        public GameObject Boom;    //爆炸特效
        private Vector3 place;   //撞击位置
        private bool isFound;
        private Vector3 Target;
        private float a_speed=0.2f;
        public override void Awake()
        {
            Speed = 0;
            LiveTime = 4;
            Power = 5;
            ShootRate = 1;
            base.Awake();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Rock"))
                Destroy(this.gameObject);

        }
        public override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
        }
        public override void FixedUpdate()
        {
            Speed += a_speed * Time.deltaTime;
            a_speed += 0.05f;
            place = gameObject.transform.position;
            transform.Translate(Vector3.up*Speed);

        }
        public override void OnDestroy()
        {
            //  Debug.Log(place );
            Boom = Instantiate(Boom) as GameObject;
            Boom.transform.position = place;
        }
    }
}