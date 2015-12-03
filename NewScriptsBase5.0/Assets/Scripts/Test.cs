#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			Test.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2015-12-02 14:45:42Z
//
// 描述(Description):							
//
// **********************************************************************

#endregion

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

#endregion

public class Test : MonoBehaviour
{
    private Dictionary<string, GameObject> GameObjectPool = new Dictionary<string, GameObject>();

    private void Start()
    {
        //要加载的资源的队列
        Queue<string> needLoadQueue = new Queue<string>();
       
        needLoadQueue.Enqueue("islands/island 1");
        needLoadQueue.Enqueue("islands/island 2");
        needLoadQueue.Enqueue("islands/island 3");
        needLoadQueue.Enqueue("islands/island 4");


        Load(needLoadQueue);
    }

    private void Load(Queue<string> needLoadQueue)
    {
        TimeSpan ts1 = Process.GetCurrentProcess().TotalProcessorTime;
        Stopwatch stw = new Stopwatch();
        stw.Start();
        if (needLoadQueue.Count > 0)
        {
            string needLoadAssetName = needLoadQueue.Dequeue();
            int index = needLoadAssetName.LastIndexOf("/");
            string assetName = needLoadAssetName.Substring(index + 1);
            GameObject go;

            if (GameObjectPool.ContainsKey(assetName))
            {
                go = Instantiate(GameObjectPool[assetName]);
                //                    go = Instantiate(obj) as GameObject;
                Load(needLoadQueue);
            }
            else
            {
                LoadAssetBundle.Instance.Load(needLoadAssetName, obj =>
                {
                    go = Instantiate(obj) as GameObject;
                    //加载出来的GameObject放到GameObjectPool存储
                    GameObjectPool.Add(assetName, go);
                    Load(needLoadQueue);
                });
            }
           
        }
        else
        {
            double Msecs = Process.GetCurrentProcess().TotalProcessorTime.Subtract(ts1).TotalMilliseconds;
            stw.Stop();
            Debug.LogError(string.Format("CPU时间(毫秒)={1} 实际时间(毫秒)={2}", Msecs, stw.Elapsed.TotalMilliseconds,
                stw.ElapsedTicks));
            Debug.Log("all finished");
        }
    }
}