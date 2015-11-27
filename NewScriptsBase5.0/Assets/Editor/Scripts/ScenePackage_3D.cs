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

using UnityEditor;
using UnityEngine;

#endregion

public class ScenePackage_3D : Editor
{
    [MenuItem("Tools/Create All Unity3D")]
    private static void CreateAssetBundels()
    {
        Caching.CleanCache();
        Object[] selectAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Unfiltered);
        string path = Application.streamingAssetsPath;

        path = EditorUtility.OpenFolderPanel("Save", path, "");

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        foreach (Object obj in selectAsset)
        {
            Transform trans = obj as Transform;
            if(trans == null)
            {
                Debug.Log("Do not Select Object");
                return;
            }
            if(trans.childCount>0)
            {
                MeshFilter[] meshFilters = trans.GetComponentsInChildren<MeshFilter>();
            }
        }
    }
}

public class TransformHolder:ScriptableObject
{
    public int Length { get; set; }

    public Vector3[] Positions { get; set; }
    public Quaternion[] EulerAngles { get; set; }
    public Vector3[] LocalScales { get; set; }
}