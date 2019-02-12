using UnityEngine;
using System.Collections;
using Net;

public class TestSocket : MonoBehaviour
{

    public JFSocket mJFsorket;
    private float mSynchronous;

    void Start()
    {
        mJFsorket = JFSocket.GetInstance();
    }
    
    void Update()
    {

        mSynchronous += Time.deltaTime;
        //在Update中每0.5s的时候同步一次  
        if (mSynchronous > 0.5f)
        {
            int count = mJFsorket.worldpackage.Count;
            //当接受到的数据包长度大于0 开始同步  
            if (count > 0)
            {
                //遍历数据包中 每个点的坐标  
                foreach (JFPackage.WorldPackage wp in mJFsorket.worldpackage)
                {
                    float x = (float)(wp.mPosx / 100.0f);
                    float y = (float)(wp.mPosy / 100.0f);
                    float z = (float)(wp.mPosz / 100.0f);

                    Debug.Log("x = " + x + " y = " + y + " z = " + z);
                    //同步主角的新坐标  
                    this.transform.position = new Vector3(x, y, z);
                }
                //清空数据包链表  
                mJFsorket.worldpackage.Clear();
            }
            mSynchronous = 0;
        }
    }
    

}