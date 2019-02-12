using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rocks
{
    public class bigRock : Rock
    {
        private void Awake()
        {
            life = 40;
        }
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
        }
    }
}