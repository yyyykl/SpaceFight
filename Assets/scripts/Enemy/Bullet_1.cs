﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeaponType
{
    public class Bullet_1 : Weapon
    {
        public override void Awake()
        {
            Speed = 0.5f;
            LiveTime = 5;
            Power = 1;
            ShootRate = 0.3f;
            base.Awake();
        }
        public override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.CompareTo("Enemy") == 0)
            {
                GetComponent<CapsuleCollider2D>().isTrigger = false;
            }
        }
    }
}