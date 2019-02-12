using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rocks
{
   
    public class smallRock : Rock
    {
        public void SetSpeed(float speed)
        {
            Speed = speed;
        }
        private void Awake()
        {
          //  Speed = 0;
            life = 10;
        }
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
        }
    }
}