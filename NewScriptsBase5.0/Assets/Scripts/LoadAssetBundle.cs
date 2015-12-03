#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			LoadAssetBundle.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2015-12-02 13:54:22Z
//
// 描述(Description):							
//
// **********************************************************************

#endregion

#region

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

public class LoadAssetBundle : MonoBehaviour
{
    public string assetPath;
    private string expandedName ;

    public static LoadAssetBundle Instance;

    void Awake()
    {
        assetPath = Application.streamingAssetsPath;
        expandedName = ".unity3d";
        if(Instance == null)
        {
            Instance = this;
        }
    }
    /// <summary>
    ///     加载目标资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callBack"></param>
    public void Load(string name, System.Action<Object> callBack)
    {
        name = name + expandedName;
        Action<List<AssetBundle>> action = delegate(List<AssetBundle> depenceAssetBundles)
        {
            string realName = this.GetRuntimePlatform()+"/"+ name;

            LoadResReturnWWW(realName, delegate(WWW www)
            {
                int index = realName.LastIndexOf("/");
                string assetName = realName.Substring(index + 1).Replace(expandedName, "");
                AssetBundle assetBundle = www.assetBundle;
                Object obj = assetBundle.LoadAsset(assetName);

                assetBundle.Unload(false);
                for (int i = 0; i < depenceAssetBundles.Count; i++)
                {
                    depenceAssetBundles[i].Unload(false);
                }

                callBack(obj);
            });
        };

        LoadDepenceAssets(name, action);
    }

    /// <summary>
    ///     加载目标资源依赖资源
    /// </summary>
    /// <param name="targetName"></param>
    /// <param name="action"></param>
    private void LoadDepenceAssets(string targetName, Action<List<AssetBundle>> action)
    {
        Debug.Log("要加载的目标资源：" + targetName);
        Caching.CleanCache();
        Action<AssetBundleManifest> dependenceAction = mainfest =>
        {
            List<AssetBundle> depenceAssetBundles = new List<AssetBundle>(); //用来存放加载出来的依赖资源的ab
            string[] depences = mainfest.GetAllDependencies(targetName);

            Debug.Log("依赖文件的个数：" + depences.Length);

            int length = depences.Length;
            int finishedCount = 0;

            if (length == 0)
            {
                action(depenceAssetBundles);
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    string depenceAssetName = depences[i];
                    depenceAssetName = GetRuntimePlatform() + "/" + depenceAssetName;

                    //加载到assetpool
                    LoadResReturnWWW(depenceAssetName, www =>
                    {
                        int index = depenceAssetName.LastIndexOf("/");
                        string assetName = depenceAssetName.Substring(index + 1).Replace(expandedName, "");
                        AssetBundle assetBundle = www.assetBundle;
                        Object obj = assetBundle.LoadAsset(assetName);

                        depenceAssetBundles.Add(assetBundle);
                        finishedCount++;
                        if (finishedCount == length)
                        {
                            action(depenceAssetBundles);
                        }
                    });
                }
            }
        };
        LoadAssetBundleMainfest(dependenceAction);
    }

    /// <summary>
    ///     加载AssetBundlesMainfest
    /// </summary>
    /// <param name="action"></param>
    private void LoadAssetBundleMainfest(Action<AssetBundleManifest> action)
    {
        string manifestName = this.GetRuntimePlatform();
        manifestName = manifestName + "/" + manifestName;
        LoadResReturnWWW(manifestName, www =>
        {
            AssetBundle assetBundle = www.assetBundle;
            Object obj = assetBundle.LoadAsset("AssetBundleManifest");
            assetBundle.Unload(false);
            AssetBundleManifest manifest = obj as AssetBundleManifest;
            action(manifest);
        });
    }

    private void LoadResReturnWWW(string realName, Action<WWW> action)
    {
        string path = "file://"+this.assetPath + "/" + realName;
        Debug.Log("加载：" + path);
        StartCoroutine(LoaderRes(path, action));
    }

    private IEnumerator LoaderRes(string path, Action<WWW> callBack)
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.isDone)
        {
            callBack(www);
        }
    }

    /// <summary>
    ///     对应平台
    /// </summary>
    /// <returns></returns>
    private string GetRuntimePlatform()
    {
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "IOS";
            case BuildTarget.WebPlayer:
                return "WebPlayer";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            default:
                return null;
        }
    }
}