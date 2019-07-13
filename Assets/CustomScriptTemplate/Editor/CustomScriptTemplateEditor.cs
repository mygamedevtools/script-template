/**
 * CustomScriptTemplateEditor.cs
 * Created by: Joao Borks [joao.borks@gmail.com]
 * Created on: 13/07/19
 */

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class CustomScriptTemplateEditor : EditorWindow
{
    static string authorName;
    static string templatePath;

    [MenuItem("Assets/Custom Script Template Editor", false, 800)]
    public static void ShowWindow()
    {
        GetWindow<CustomScriptTemplateEditor>("Custom Script Template");
    }

    void OnGUI()
    {
        authorName = authorName ?? EditorPrefs.GetString("cst_author", "Unamed");
        templatePath = templatePath ?? GetScriptTemplatePath();

        var scriptsPath = Path.Combine(EditorApplication.applicationPath, "Data", "Resources", "ScriptTemplates");
    }

    string GetScriptTemplatePath()
    {
        var path = Directory.GetDirectories(Application.dataPath).FirstOrDefault(s => s.Contains("CustomScriptTemplate"));
        if (string.IsNullOrEmpty(path))
        {
            EditorUtility.DisplayDialog("Could not find \"CustomScriptTemplate\" folder", "The tool could not locate the \"CustomScriptTemplate\" folder, which is required to cache the script template. Please check if you have renamed the folder, otherwise reinstall the asset package", "Ok");
            return null;
        }
        path = Path.Combine(path, "Source", "81-C# Custom Script-NewBehaviourScript.cs.txt");
        if (!File.Exists(path))
        {
            EditorUtility.DisplayDialog("Could not find the template asset", "The tool could not locate the template asset. Please check if you have renamed the file, otherwise reinstall the asset package", "Ok");
            return null;
        }
        return path;
    }
}