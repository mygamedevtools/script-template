/**
 * CustomScriptTemplateEditor.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 7/13/2019 (en-US)
 */

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyUnityTools.ScriptTemplates
{
    public class ScriptTemplatesEditor : EditorWindow
    {
        /// <summary>
        /// The name of the <see cref="authorName"/> string field saved on <see cref="EditorPrefs"/>
        /// </summary>
        public const string AuthorNameField = "com.myunitytools.cst.authorName";
        /// <summary>
        /// The name of the <see cref="authorEmail"/> string field saved on <see cref="EditorPrefs"/>
        /// </summary>
        public const string AuthorEmailField = "com.myunitytools.cst.authorEmail";
        /// <summary>
        /// The name of the <see cref="localGUID"/> string field saved on <see cref="EditorPrefs"/>
        /// </summary>
        public const string LocalGUIDField = "com.myunitytools.cst.localguid";
        /// <summary>
        /// Package path for accessing script templates and visual elements
        /// </summary>
        public const string PackageRootPath = "Packages/com.myunitytools.scripttemplate/";
        /// <summary>
        /// Project path for storing script templates
        /// </summary>
        public const string LocalTemplatesPath = "Assets/ScriptTemplates/";
        /// <summary>
        /// Editor path for storing script templates
        /// </summary>
        public static readonly string EditorTemplatesPath = EditorApplication.applicationPath.Replace("Unity.exe", "") + "/Data/Resources/ScriptTemplates/";

        const string EditorRestartMesssage = "Copying Script Templates will requre Editor restart in order to apply the changes. Do you want to restart now?";

        [MenuItem("Assets/Script Templates Editor", false, 800)]
        public static void ShowWindow() => GetWindow<ScriptTemplatesEditor>("Script Templates").minSize = new Vector2(300, 250);

        void OnEnable()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PackageRootPath + "UIToolkit/ScriptTemplateWindow.uxml");
            rootVisualElement.Add(visualTree.CloneTree());
            var styles = AssetDatabase.LoadAssetAtPath<StyleSheet>(PackageRootPath + "UIToolkit/Styles/ScriptTemplateStyles.uss");
            rootVisualElement.styleSheets.Add(styles);

            DrawInfoView();
            DrawTemplateView();
        }

        void DrawInfoView()
        {
            var saveButton = rootVisualElement.Q<Button>("save-button");

            var authorField = rootVisualElement.Q<TextField>("author");
            authorField.SetValueWithoutNotify(EditorPrefs.GetString(AuthorNameField, string.Empty));
            authorField.RegisterValueChangedCallback(e => saveButton.SetEnabled(!isAuthorUpdated(e.newValue) && !string.IsNullOrEmpty(authorField.value)));

            var emailField = rootVisualElement.Q<TextField>("email");
            emailField.SetValueWithoutNotify(EditorPrefs.GetString(AuthorEmailField, string.Empty));
            emailField.RegisterValueChangedCallback(e => saveButton.SetEnabled(!isEmailUpdated(e.newValue) && !string.IsNullOrEmpty(authorField.value)));

            var clearButton = rootVisualElement.Q<Button>("clear-button");
            clearButton.SetEnabled(EditorPrefs.HasKey(AuthorNameField) || EditorPrefs.HasKey(AuthorEmailField));
            clearButton.clicked += () =>
            {
                EditorPrefs.DeleteKey(AuthorNameField);
                EditorPrefs.DeleteKey(AuthorEmailField);
                saveButton.SetEnabled(!string.IsNullOrEmpty(authorField.value));
                clearButton.SetEnabled(false);
            };

            saveButton.SetEnabled(false);
            saveButton.clicked += () =>
            {
                EditorPrefs.SetString(AuthorNameField, authorField.value);
                EditorPrefs.SetString(AuthorEmailField, emailField.value);
                saveButton.SetEnabled(false);
                clearButton.SetEnabled(true);
            };

            bool isAuthorUpdated(string author) => author == EditorPrefs.GetString(AuthorNameField, string.Empty);

            bool isEmailUpdated(string email) => email == EditorPrefs.GetString(AuthorEmailField, string.Empty);
        }

        void DrawTemplateView()
        {
            var templates = GetPackageScriptTemplates();
            var listView = rootVisualElement.Q<ListView>("template-list");
            listView.makeItem = () =>
            {
                var objectField = new ObjectField(string.Empty)
                {
                    allowSceneObjects = false,
                    objectType = typeof(TextAsset)
                };
                objectField.SetEnabled(false);
                return objectField;
            };
            listView.bindItem = (element, i) => (element as ObjectField).SetValueWithoutNotify(templates[i]);
            listView.itemsSource = templates;
            listView.itemHeight = 20;
            listView.selectionType = SelectionType.Single;
            listView.onSelectionChanged += selection => Selection.activeObject = (Object)selection.FirstOrDefault();

            var projectButton = rootVisualElement.Q<Button>("copy-project-button");
            projectButton.clicked += () =>
            {
                var shouldRestart = EditorUtility.DisplayDialog("Editor Restart", EditorRestartMesssage, "Yes", "No");
                var length = templates.Length;
                if (!AssetDatabase.IsValidFolder(LocalTemplatesPath))
                    AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
                for (int i = 0; i < length; i++)
                    AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(templates[i]), LocalTemplatesPath + templates[i].name + ".txt");
                if (shouldRestart && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorApplication.OpenProject(Path.Combine(Application.dataPath, ".."));
            };

            var editorButton = rootVisualElement.Q<Button>("copy-editor-button");
            editorButton.clicked += () =>
            {
                var shouldRestart = EditorUtility.DisplayDialog("Editor Restart", EditorRestartMesssage, "Yes", "No");
                var length = templates.Length;
                for (int i = 0; i < length; i++)
                    File.Copy(AssetDatabase.GetAssetPath(templates[i]), EditorTemplatesPath + templates[i].name + ".txt");
                if (shouldRestart && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorApplication.OpenProject(Path.Combine(Application.dataPath, ".."));
            };
        }

        /// <summary>
        /// Gets all Script Templates included in this package
        /// </summary>
        TextAsset[] GetPackageScriptTemplates()
        {
            var files = Directory.GetFiles(PackageRootPath + "ScriptTemplates/", "*.txt");
            var assets = new TextAsset[files.Length];
            var length = files.Length;
            for (int i = 0; i < length; i++)
                assets[i] = AssetDatabase.LoadAssetAtPath<TextAsset>(files[i]);
            return assets;
        }
    }
}