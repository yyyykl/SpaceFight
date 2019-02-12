using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAnimtor : MonoBehaviour
{
    private Animator ani;
    private void Start()
    {
        ani = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        AnimatorStateInfo animatorInfo;
        animatorInfo = ani.GetCurrentAnimatorStateInfo(0);
        if ((animatorInfo.normalizedTime > 1))
        {
            Destroy(this.gameObject);
        }


    }
}
