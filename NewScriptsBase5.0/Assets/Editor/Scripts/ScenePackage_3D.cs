#region 描述

// **********************************************************************
// 
// 文件名(File Name)：    		ScenePackage_3D
// 
// 作者(Author)：           	    da_fei
// 
// 创建时间(CreateTime):    	    2015-11-27 17:32:18Z
//
// 描述(Description):            3D游戏场景打包                              								
//
// **********************************************************************

#endregion

#region

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#endregion

public class ScenePackage_3D : Editor
{
    private static string sourcePath = Application.dataPath + "/Resources";
    private static string outPath = Application.streamingAssetsPath;
    [MenuItem("Tools/AssetBuilt/Build")]

    private static void BuildAssetBundels()
    {
        Dictionary<string, List<Transform>> mDicTrans = new Dictionary<string, List<Transform>>();
        Caching.CleanCache();
//        Object[] selectAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Unfiltered);

        outPath = EditorUtility.OpenFolderPanel("Save", outPath, "");

        if (string.IsNullOrEmpty(sourcePath))
        {
            return;
        }
//        ClearAssetBundleName();

        PackBundles(sourcePath);

        outPath = Path.Combine(outPath, Platform.GetPlatformInfo(EditorUserBuildSettings.activeBuildTarget));
        if(!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        BuildPipeline.BuildAssetBundles(outPath);
        AssetDatabase.Refresh();
        Debug.Log("打包完成");
    }

    private static void PackBundles(string sourcePath)
    {
        DirectoryInfo folder = new DirectoryInfo(sourcePath);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;
        for (int i = 0; i < length; i++)
        {
            if(files[i] is DirectoryInfo)
            {
                PackBundles(files[i].FullName);
            }
            else
            {
                if(!files[i].Name.EndsWith(".meta"))
                {
                    SetAssetName(files[i].FullName);
                }
            }
        }
    }

    private static void SetAssetName(string fullName)
    {
        string source = fullName.Replace("\\", "/");
        string assetPath = "Assets" + source.Substring(Application.dataPath.Length);
        string assetPath2 = source.Substring(Application.dataPath.Length + 1);

        AssetImporter assetInImporter = AssetImporter.GetAtPath(assetPath);
        string assetName = assetPath2.Substring(assetPath2.IndexOf("/", StringComparison.Ordinal) + 1);
        assetName = assetName.Replace(Path.GetExtension(assetName), ".unity3d");
        assetInImporter.assetBundleName = assetName;
    }

    /// <summary>
    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包
    /// 之前说过，只要设置了AssetBundleName的，都会进行打包，不论在什么目录下
    /// </summary>
    private static void ClearAssetBundleName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        Debug.Log("old AssetNames is " + AssetDatabase.GetAllAssetBundleNames().Length);

        for (int i = 0; i < AssetDatabase.GetAllAssetBundleNames().Length; )
        {
            AssetDatabase.RemoveAssetBundleName(AssetDatabase.GetAllAssetBundleNames()[0], true);
        }

        Debug.Log("old AssetNames is " + AssetDatabase.GetAllAssetBundleNames().Length);


    }

    public class Platform
    {
        public static string GetPlatformInfo(BuildTarget target)
        {
            switch (target)
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

//    private static void CreateAssetBundels()
//    {
//        Dictionary<string, List<Transform>> mDicTrans = new Dictionary<string, List<Transform>>();
//        Caching.CleanCache();
//        Object[] selectAsset = Selection.GetFiltered(typeof (Object), SelectionMode.Unfiltered);
//        string path = Application.streamingAssetsPath;
//
//        path = EditorUtility.OpenFolderPanel("Save", path, "");
//
//        if (string.IsNullOrEmpty(path))
//        {
//            return;
//        }
//
//        foreach (Object obj in selectAsset)
//        {
//            Transform trans = (obj as GameObject).transform;
//            if (trans == null)
//            {
//                Debug.Log("Do not Select Object");
//                return;
//            }
//            if (trans.childCount > 0)
//            {
//                MeshFilter[] meshFilters = trans.GetComponentsInChildren<MeshFilter>(true);
//
//                foreach (MeshFilter var in meshFilters)
//                {
//                    if (mDicTrans.ContainsKey(var.sharedMesh.name))
//                    {
//                        mDicTrans[var.sharedMesh.name].Add(var.transform);
//                    }
//                    else
//                    {
//                        mDicTrans.Add(var.sharedMesh.name, new List<Transform> { var.transform });
//                    }
//                }
//            }
//        }
//
//        foreach (KeyValuePair<string, List<Transform>> t in mDicTrans)
//        {
//            
//            Debug.Log(t.Value[0].name);
//            if(t.Value.Count>1)
//            {
//                Debug.Log(t.Key.ToString() +":"+t.Value.Count);
//            }
//        }
//    }
}