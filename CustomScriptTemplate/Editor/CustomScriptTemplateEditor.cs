/**
 * CustomScriptTemplateEditor.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 7/13/2019 (en-US)
 */

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

namespace CustomScriptTemplate
{
    /// <summary>
    /// Controls the settings of the Script Templates and the Visual of the <see cref="EditorWindow"/> for Custom Script Templates
    /// </summary>
    public class CustomScriptTemplateEditor : EditorWindow
    {
        /// <summary>
        /// The name of the <see cref="authorName"/> string field saved on <see cref="EditorPrefs"/>
        /// </summary>
        public const string AuthorNameField = "com.joaoborks.cst.authorName";
        /// <summary>
        /// The name of the <see cref="authorEmail"/> string field saved on <see cref="EditorPrefs"/>
        /// </summary>
        public const string AuthorEmailField = "com.joaoborks.cst.authorEmail";
        /// <summary>
        /// The name of the <see cref="isPackage"/> bool field saved on <see cref="EditorPrefs"/>
        /// </summary>
        public const string IsPackageField = "com.joaoborks.cst.ispackage";

        /// <summary>
        /// Reference to the Script Template Object, which is essentialy a <see cref="TextAsset"/>
        /// </summary>
        public static Object ScriptTemplate
        {
            get
            {
                scriptTemplate = scriptTemplate ?? AssetDatabase.LoadAssetAtPath<TextAsset>(TemplatePath.Replace(Application.dataPath, "Assets"));
                return scriptTemplate;
            }
        }
        /// <summary>
        /// Path to the <see cref="ScriptTemplate"/> object
        /// </summary>
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
        static bool isPackage;

        [MenuItem("Assets/Custom Script Template Editor", false, 800)]
        public static void ShowWindow()
        {
            GetWindow<CustomScriptTemplateEditor>("Custom Script Template").minSize = new Vector2(300, 300);
        }

        /// <summary>
        /// Copies the <see cref="ScriptTemplate"/> file to Unity's Script Template folder and reloads the Editor
        /// </summary>
        public static void GenerateScriptTemplate()
        {
            File.Copy(TemplatePath, GetTargetScriptTemplatePath(), true);
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorApplication.OpenProject(Path.Combine(Application.dataPath, ".."));
        }

        /// <summary>
        /// Saves both <see cref="authorName"/> and <see cref="authorEmail"/> to the <see cref="EditorPrefs"/>
        /// </summary>
        static void SaveInfo()
        {
            EditorPrefs.SetString(AuthorNameField, authorName);
            EditorPrefs.SetString(AuthorEmailField, authorEmail);
        }

        /// <summary>
        /// Deletes both <see cref="authorName"/> and <see cref="authorEmail"/> from <see cref="EditorPrefs"/>
        /// </summary>
        static void ClearInfos()
        {
            EditorPrefs.DeleteKey(AuthorNameField);
            EditorPrefs.DeleteKey(AuthorEmailField);
            EditorPrefs.DeleteKey(IsPackageField);
        }

        /// <summary>
        /// Sets if the tool is installed as a regular asset or as a custom package through the Package Manager, in order to load the files
        /// </summary>
        static void SetIsPackage()
        {
            if (EditorPrefs.HasKey(IsPackageField))
            {
                isPackage = EditorPrefs.GetBool(IsPackageField);
                return;
            }

            var paths = Directory.GetFiles(Application.dataPath, "CustomScriptTemplate.asmdef", SearchOption.AllDirectories);
            if (paths == null || paths.Length == 0)
            {
                paths = Directory.GetFiles(Path.Combine(Application.dataPath, "..", "Library"), "CustomScriptTemplate.asmdef", SearchOption.AllDirectories);
                if (paths == null || paths.Length == 0)
                {
                    EditorUtility.DisplayDialog("Could not find package files", "The tool could not locate the package files. Please check if you have renamed the files, otherwise reinstall the asset package.", "Ok");
                    GetWindow<CustomScriptTemplateEditor>().Close();
                }
                else
                {
                    EditorPrefs.SetBool(IsPackageField, true);
                    isPackage = true;
                }
            }
            else
            {
                EditorPrefs.SetBool(IsPackageField, false);
                isPackage = false;
            }

        }

        /// <summary>
        /// Attempts to get the path to the <see cref="ScriptTemplate"/> file. The system should be able to work even if the files got moved, but never renamed or deleted.
        /// </summary>
        static string GetSourceScriptTemplatePath()
        {
            SetIsPackage();

            string[] paths;
            if (isPackage)
                paths = Directory.GetDirectories(Path.Combine(Application.dataPath, "..", "Library", "PackageCache"), "customscripttemplate", SearchOption.AllDirectories);
            else
                paths = Directory.GetDirectories(Application.dataPath, "CustomScriptTemplate", SearchOption.AllDirectories);

            if (paths == null || paths.Length == 0)
            {
                EditorUtility.DisplayDialog("Could not find \"CustomScriptTemplate\" folder", "The tool could not locate the \"CustomScriptTemplate\" folder, which is required to cache the script template. Please check if you have renamed the folder, otherwise reinstall the asset package.", "Ok");
                GetWindow<CustomScriptTemplateEditor>().Close();
                return null;
            }
            var path = paths[0];
            path = Path.Combine(path, "Source", "81-C# Custom Script-NewBehaviourScript.cs.txt");
            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog("Could not find the template asset", "The tool could not locate the template asset. Please check if you have renamed the file, otherwise reinstall the asset package.", "Ok");
                GetWindow<CustomScriptTemplateEditor>().Close();
                return null;
            }
            return path;
        }

        /// <summary>
        /// Gets the path to Unity's Script Template folder
        /// </summary>
        static string GetTargetScriptTemplatePath()
        {
            return Path.Combine(EditorApplication.applicationPath.Replace("Unity.exe", ""), "Data", "Resources", "ScriptTemplates", Path.GetFileName(TemplatePath));
        }

        /// <summary>
        /// Checks whether the <see cref="authorName"/> and <see cref="authorEmail"/> values matches the <see cref="EditorPrefs"/> data
        /// </summary>
        static bool IsInfoUpdated()
        {
            return EditorPrefs.HasKey(AuthorNameField) && EditorPrefs.GetString(AuthorNameField) == authorName && EditorPrefs.HasKey(AuthorEmailField) && EditorPrefs.GetString(AuthorEmailField) == authorEmail;
        }

        /// <summary>
        /// Checks whether a custom Script Template has already been created
        /// </summary>
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