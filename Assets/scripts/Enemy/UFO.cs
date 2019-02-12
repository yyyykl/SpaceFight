using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies
{
    public class UFO : Enemy
    {
        private void Awake()
        {
            Hp = 20;
            MoveSpeed = 0.5f;
            RushSpeed = 1f;
            viewRadius = 20;
            viewAngleStep = 120;

        }

    }
}