using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class Boom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy=collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Hp -= 10;
             //   Debug.Log("Boom");
            }
        }
    }

}
