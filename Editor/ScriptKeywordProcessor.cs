/**
 * ScriptKeywordProcessor.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2/19/2019 (en-US)
 * Tips from https://forum.unity3d.com/threads/c-script-template-how-to-make-custom-changes.273191/
 */

using UnityEngine;
using UnityEditor;
using System.Globalization;

namespace MyUnityTools.ScriptTemplates
{
    /// <summary>
    /// This class listens to <see cref="UnityEditor.AssetModificationProcessor"/>'s <see cref="OnWillCreateAsset(string)"/> event that executes whenever a new asset is created
    /// and replaces the keywords on scripts to what we defined on our <see cref="ScriptTemplatesEditor"/>
    /// </summary>
    public class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
            if (index < 0)
                return;

            string file = path.Substring(index);
            if (file != ".cs")
                return;

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            if (!System.IO.File.Exists(path))
                return;

            string fileContent = System.IO.File.ReadAllText(path);
            fileContent = fileContent.Replace("#AUTHOR#", $"{EditorPrefs.GetString(ScriptTemplatesEditor.AuthorNameField)}{(EditorPrefs.HasKey(ScriptTemplatesEditor.AuthorEmailField) ? $" [{EditorPrefs.GetString(ScriptTemplatesEditor.AuthorEmailField)}]" : string.Empty)}");
            fileContent = fileContent.Replace("#CREATIONDATE#", $"{System.DateTime.Now.ToString("d", CultureInfo.CurrentCulture)} ({CultureInfo.CurrentCulture.Name})");

            System.IO.File.WriteAllText(path, fileContent);
            AssetDatabase.Refresh();
        }
    }
}