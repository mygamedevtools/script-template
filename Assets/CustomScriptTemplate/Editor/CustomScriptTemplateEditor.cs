/**
 * CustomScriptTemplateEditor.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 7/13/2019 (en-US)
 */

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

namespace CustomScriptTemplate
{
    public class CustomScriptTemplateEditor : EditorWindow
    {
        public const string AuthorNameField = "cst_authorName";
        public const string AuthorEmailField = "cst_authorEmail";

        public static Object ScriptTemplate
        {
            get
            {
                scriptTemplate = scriptTemplate ?? AssetDatabase.LoadAssetAtPath<TextAsset>(TemplatePath.Replace(Application.dataPath, "Assets"));
                return scriptTemplate;
            }
        }
        public static string TemplatePath
        {
            get
            {
                templatePath = templatePath ?? GetSourceScriptTemplatePath();
                return templatePath;
            }
        }

        static Object scriptTemplate;
        static string authorName;
        static string authorEmail;
        static string templatePath;

        [MenuItem("Assets/Custom Script Template Editor", false, 800)]
        public static void ShowWindow()
        {
            GetWindow<CustomScriptTemplateEditor>("Custom Script Template").minSize = new Vector2(300, 300);
        }

        public static void GenerateScriptTemplate()
        {
            File.Copy(TemplatePath, GetTargetScriptTemplatePath(), true);
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorApplication.OpenProject(Path.Combine(Application.dataPath, ".."));
        }

        static void SaveInfo()
        {
            EditorPrefs.SetString(AuthorNameField, authorName);
            EditorPrefs.SetString(AuthorEmailField, authorEmail);
        }

        static void ClearInfos()
        {
            EditorPrefs.DeleteKey(AuthorNameField);
            EditorPrefs.DeleteKey(AuthorEmailField);
        }

        static string GetSourceScriptTemplatePath()
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

        static string GetTargetScriptTemplatePath()
        {
            return Path.Combine(EditorApplication.applicationPath.Replace("Unity.exe", ""), "Data", "Resources", "ScriptTemplates", Path.GetFileName(TemplatePath));
        }

        static bool IsInfoUpdated()
        {
            return EditorPrefs.HasKey(AuthorNameField) && EditorPrefs.GetString(AuthorNameField) == authorName && EditorPrefs.HasKey(AuthorEmailField) && EditorPrefs.GetString(AuthorEmailField) == authorEmail;
        }

        static bool HasTemplate()
        {
            return File.Exists(GetTargetScriptTemplatePath());
        }

        void OnGUI()
        {
            authorName = authorName ?? EditorPrefs.GetString(AuthorNameField, "Unamed");
            authorEmail = authorEmail ?? EditorPrefs.GetString(AuthorEmailField, "");
            bool updated = IsInfoUpdated();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(450));
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
            authorName = EditorGUILayout.TextField("Author", authorName);
            authorEmail = EditorGUILayout.TextField("Email (optional)", authorEmail);
            if (!updated)
                EditorGUILayout.HelpBox("Your info is not yet saved. Please save below.", MessageType.Warning);
            else
                EditorGUILayout.HelpBox("Your info is updated.", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear"))
                ClearInfos();
            GUI.enabled = !updated;
            if (GUILayout.Button("Save"))
                SaveInfo();
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(450));
            EditorGUILayout.LabelField("Template", EditorStyles.boldLabel);
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Custom Template", ScriptTemplate, typeof(TextAsset), false);
            GUI.enabled = true;
            EditorGUILayout.HelpBox("Generating the Script Template will restart the Editor in order to apply the changes.", MessageType.Info);
            if (HasTemplate())
                EditorGUILayout.HelpBox("A Script Template already exists and will be overwritten by the new generated one.", MessageType.Warning);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Edit"))
                AssetDatabase.OpenAsset(ScriptTemplate);
            if (GUILayout.Button("Generate"))
                GenerateScriptTemplate();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}