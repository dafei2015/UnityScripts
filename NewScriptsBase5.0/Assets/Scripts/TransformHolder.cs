#region 描述
// **********************************************************************
// 
// 文件名(File Name)：    		TransformHolder
// 
// 作者(Author)：           	    da_fei
// 
// 创建时间(CreateTime):    	    2015-12-01 15:41:45Z
//
// 描述(Description):            记录所打包物体的位置信息                  								
//
// **********************************************************************
#endregion

#region 引用的命名空间
using UnityEngine;
#endregion

public class TransformHolder : ScriptableObject 
{
    public int Length { get; set; }

    public Vector3[] Positions { get; set; }
    public Quaternion[] EulerAngles { get; set; }
    public Vector3[] LocalScales { get; set; }
}

