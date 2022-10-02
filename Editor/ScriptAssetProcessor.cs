/**
 * ScriptAssetProcessor.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2019-02-19
 * Tips from https://forum.unity3d.com/threads/c-script-template-how-to-make-custom-changes.273191/
 */

using System.IO;
using UnityEditor;
using UnityEngine;

namespace MyUnityTools.ScriptTemplates
{
    /// <summary>
    /// This class listens to <see cref="AssetModificationProcessor"/>'s <see cref="OnWillCreateAsset(string)"/> event that executes whenever a new asset is created
    /// and replaces the keywords on scripts to what we defined on our <see cref="ScriptTemplatesEditor"/>
    /// </summary>
    public class ScriptAssetProcessor : AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            var index = path.LastIndexOf(".");
            if (index < 0)
                return;

            var file = path.Substring(index);
            if (file != ".cs")
                return;

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            if (!File.Exists(path))
                return;

            var keywordReplacer = new ScriptKeywordReplacer(ScriptTemplateSettings.FromEditorPrefs());

            var fileContent = File.ReadAllText(path);

            fileContent = keywordReplacer.ProcessScriptTemplate(fileContent, Path.GetFileNameWithoutExtension(path));

            File.WriteAllText(path, fileContent);
            AssetDatabase.Refresh();
        }
    }
}