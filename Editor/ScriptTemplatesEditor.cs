/**
 * CustomScriptTemplateEditor.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2019-07-13
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
        public const string AuthorNameKey = "com.myunitytools.cst.authorName";
        /// <summary>
        /// The name of the <see cref="authorEmail"/> string field saved on <see cref="EditorPrefs"/>
        /// </summary>
        public const string AuthorEmailKey = "com.myunitytools.cst.authorEmail";
        /// <summary>
        /// Should the date be formatted in the current locale or in the default ISO format?
        /// </summary>
        public const string UseLocalDateKey = "com.myunitytools.cst.localdate";
        /// <summary>
        /// The name of the <see cref="localGUID"/> string field saved on <see cref="EditorPrefs"/>
        /// </summary>
        public const string LocalGUIDKey = "com.myunitytools.cst.localguid";
        /// <summary>
        /// Package path for accessing script templates and visual elements
        /// </summary>
        public const string PackageRootPath = "Packages/com.myunitytools.script-template/";
        /// <summary>
        /// Project path for storing script templates
        /// </summary>
        public const string LocalTemplatesPath = "Assets/ScriptTemplates/";

        /// <summary>
        /// Editor path for storing script templates
        /// </summary>
        public static readonly string EditorTemplatesPath = EditorApplication.applicationPath.Replace("Unity.exe", "") + "/Data/Resources/ScriptTemplates/";

        const string _editorRestartMesssage = "Copying Script Templates will requre Editor restart in order to apply the changes. Do you want to restart now?";

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
            authorField.SetValueWithoutNotify(EditorPrefs.GetString(AuthorNameKey, string.Empty));
            authorField.RegisterValueChangedCallback(e => saveButton.SetEnabled(!isAuthorUpdated(e.newValue) && !string.IsNullOrEmpty(authorField.value)));

            var emailField = rootVisualElement.Q<TextField>("email");
            emailField.SetValueWithoutNotify(EditorPrefs.GetString(AuthorEmailKey, string.Empty));
            emailField.RegisterValueChangedCallback(e => saveButton.SetEnabled(!isEmailUpdated(e.newValue) && !string.IsNullOrEmpty(authorField.value)));

            var localDateToggle = rootVisualElement.Q<Toggle>("localdate");
            localDateToggle.SetValueWithoutNotify(EditorPrefs.GetBool(UseLocalDateKey, false));
            localDateToggle.RegisterValueChangedCallback(e => saveButton.SetEnabled(!isLocalDateUpdated(e.newValue)));

            var clearButton = rootVisualElement.Q<Button>("clear-button");
            clearButton.SetEnabled(EditorPrefs.HasKey(AuthorNameKey) || EditorPrefs.HasKey(AuthorEmailKey));
            clearButton.clicked += () =>
            {
                EditorPrefs.DeleteKey(AuthorNameKey);
                EditorPrefs.DeleteKey(AuthorEmailKey);
                EditorPrefs.DeleteKey(UseLocalDateKey);
                saveButton.SetEnabled(!string.IsNullOrEmpty(authorField.value));
                clearButton.SetEnabled(false);
            };

            saveButton.SetEnabled(false);
            saveButton.clicked += () =>
            {
                EditorPrefs.SetString(AuthorNameKey, authorField.value);
                EditorPrefs.SetString(AuthorEmailKey, emailField.value);
                EditorPrefs.SetBool(UseLocalDateKey, localDateToggle.value);
                saveButton.SetEnabled(false);
                clearButton.SetEnabled(true);
            };

            bool isAuthorUpdated(string author) => author == EditorPrefs.GetString(AuthorNameKey, string.Empty);

            bool isEmailUpdated(string email) => email == EditorPrefs.GetString(AuthorEmailKey, string.Empty);

            bool isLocalDateUpdated(bool value) => value == EditorPrefs.GetBool(UseLocalDateKey, false);
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
#if UNITY_2021_2_OR_NEWER
            listView.fixedItemHeight = 20;
#else
            listView.itemHeight = 20;
#endif
            listView.selectionType = SelectionType.Single;
#if UNITY_2021_2_OR_NEWER
            listView.onSelectionChange += selection => Selection.activeObject = (Object)selection.FirstOrDefault();
#else
            listView.onSelectionChanged += selection => Selection.activeObject = (Object)selection.FirstOrDefault();
#endif

            var projectButton = rootVisualElement.Q<Button>("copy-project-button");
            projectButton.clicked += () =>
            {
                var shouldRestart = EditorUtility.DisplayDialog("Editor Restart", _editorRestartMesssage, "Yes", "No");
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
                var shouldRestart = EditorUtility.DisplayDialog("Editor Restart", _editorRestartMesssage, "Yes", "No");
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