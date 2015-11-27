#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			ScriptsTemplate.cs
// 
// 作者(Author)：  				da_fei
// 
// 创建时间(CreateTime):			2015-11-27 11:11:30Z
//
// 描述(Description):            改变Unity自建脚本的模板，参考游客学院的脚本编写教程             
//
// **********************************************************************

#endregion


namespace CustomEditor
{
    public class ScriptsTemplate : UnityEditor.AssetModificationProcessor
    {
        
        private static void OnWillCreateAsset(string currentPath)
        {
            currentPath = currentPath.Replace(".meta", "");
            if (!currentPath.EndsWith(".cs")) return;
            string text = System.IO.File.ReadAllText(currentPath);
            text = text.Replace("#AuthorName#", "da_fei").Replace("#CreateTime#", System.DateTime.Now.ToString("u"));
            System.IO.File.WriteAllText(currentPath, text);
        }
    }
}