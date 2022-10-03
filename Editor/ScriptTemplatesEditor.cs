/**
 * ScriptTemplatesEditor.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2019-07-13
 */

using MyUnityTools.ScriptTemplates.UIToolkit;
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
        public const string PackageRootPath = "Packages/com.myunitytools.script-template/";
        public const string LocalTemplatesPath = "Assets/ScriptTemplates/";

        /// <summary>
        /// Editor path for storing script templates
        /// </summary>
        public static readonly string EditorTemplatesPath = EditorApplication.applicationPath.Replace("Unity.exe", "") + "/Data/Resources/ScriptTemplates/";

        const string _editorRestartMesssage = "Copying Script Templates will requre Editor restart in order to apply the changes. Do you want to restart now?";
        const string _previewTemplate =
            "<color=#608B4E>#SIGNATURE#</color><color=#569cd6>using</color> UnityEngine;\n" +
            "\n" +
            "#NAMESPACE#<color=#569cd6>public class</color> <color=#4ec9b0>#SCRIPTNAME#</color> : <color=#4ec9b0>MonoBehaviour</color>\n" +
            "{\n" +
            "    <color=#569cd6>public void</color> <color=#dcdcaa>Start</color>()\n" +
            "    {\n" +
            "    }\n" +
            "}";

        ScriptTemplateSettings _cachedTemplateSettings;
        ScriptTemplateSettings _previewTemplateSettings;
        ScriptKeywordReplacer _keywordReplacer;

        [MenuItem("Assets/Script Templates Editor", false, 800)]
        public static void ShowWindow() => GetWindow<ScriptTemplatesEditor>("Script Templates").minSize = new Vector2(400, 650);

        void OnEnable()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PackageRootPath + "UIToolkit/ScriptTemplateWindow.uxml");
            rootVisualElement.Add(visualTree.CloneTree());

            _cachedTemplateSettings = ScriptTemplateSettings.FromEditorPrefs();
            _previewTemplateSettings = new ScriptTemplateSettings()
            {
                Signature = _cachedTemplateSettings.Signature,
                Namespace = _cachedTemplateSettings.Namespace
            };
            _keywordReplacer = new ScriptKeywordReplacer(_previewTemplateSettings);

            BindSettingsView();
            BindTemplateView();
        }

        void BindSettingsView()
        {
            var previewLabel = rootVisualElement.Q<Label>("preview-template");
            var saveButton = rootVisualElement.Q<Button>("save-button");
            var clearButton = rootVisualElement.Q<Button>("clear-button");

            var signatureModule = rootVisualElement.Q<Module>("signature-module");
            signatureModule.SetValueWithoutNotify(_previewTemplateSettings.Signature.Enabled);

            var authorField = signatureModule.Q<TextField>("author");
            authorField.SetValueWithoutNotify(_previewTemplateSettings.Signature.AuthorName);

            var emailField = signatureModule.Q<TextField>("email");
            emailField.SetValueWithoutNotify(_previewTemplateSettings.Signature.AuthorEmail);

            var localDateToggle = signatureModule.Q<Toggle>("localdate");
            localDateToggle.SetValueWithoutNotify(_previewTemplateSettings.Signature.UseLocalDateFormat);

            var namespaceModule = rootVisualElement.Q<Module>("namespace-module");
            namespaceModule.SetValueWithoutNotify(_previewTemplateSettings.Namespace.Enabled);

            var assembly = namespaceModule.Q<Toggle>("assembly");
            assembly.SetValueWithoutNotify(_previewTemplateSettings.Namespace.UseAssemblyDefinition);

            var defaultNamespace = namespaceModule.Q<TextField>("default");
            defaultNamespace.SetValueWithoutNotify(_previewTemplateSettings.Namespace.DefaultNamespace);

            var indentType = namespaceModule.Q<EnumField>("indent-type");
            indentType.SetValueWithoutNotify(_previewTemplateSettings.Namespace.IndentPattern);

            var indentMult = namespaceModule.Q<SliderInt>("indent-mult");
            indentMult.SetValueWithoutNotify(_previewTemplateSettings.Namespace.IndentMultiplier);

            clearButton.SetEnabled(!_cachedTemplateSettings.IsEmpty());
            clearButton.clicked += () =>
            {
                _cachedTemplateSettings.Signature = new SignatureSettings();
                _cachedTemplateSettings.DeleteEditorPrefs();
                saveButton.SetEnabled(_cachedTemplateSettings != _previewTemplateSettings);
                clearButton.SetEnabled(false);
            };

            signatureModule.RegisterValueChangedCallback(e => updatePreview());
            authorField.RegisterValueChangedCallback(e => updatePreview());
            emailField.RegisterValueChangedCallback(e => updatePreview());
            localDateToggle.RegisterValueChangedCallback(e => updatePreview());

            namespaceModule.RegisterValueChangedCallback(e => updatePreview());
            assembly.RegisterValueChangedCallback(e => updatePreview());
            defaultNamespace.RegisterValueChangedCallback(e => updatePreview());
            indentType.RegisterValueChangedCallback(e => updatePreview());
            indentMult.RegisterValueChangedCallback(e => updatePreview());

            saveButton.SetEnabled(false);
            saveButton.clicked += () =>
            {
                _cachedTemplateSettings.Signature = _previewTemplateSettings.Signature;
                _cachedTemplateSettings.Namespace = _previewTemplateSettings.Namespace;
                _previewTemplateSettings.SaveToEditorPrefs();
                saveButton.SetEnabled(false);
                clearButton.SetEnabled(!_cachedTemplateSettings.IsEmpty());
            };

            updatePreview();

            void updatePreview()
            {
                _previewTemplateSettings.Signature = new SignatureSettings
                {
                    Enabled = signatureModule.value,
                    AuthorName = authorField.value,
                    AuthorEmail = emailField.value,
                    UseLocalDateFormat = localDateToggle.value
                };
                _previewTemplateSettings.Namespace = new NamespaceSettings
                {
                    Enabled = namespaceModule.value,
                    UseAssemblyDefinition = assembly.value,
                    DefaultNamespace = defaultNamespace.value,
                    IndentPattern = (IndentPattern)indentType.value,
                    IndentMultiplier = indentMult.value
                };
                previewLabel.text = _keywordReplacer.ProcessScriptTemplate(_previewTemplate, "Assets/Scripts/ExampleScript.cs").Replace("namespace", "<color=#569cd6>namespace</color>").Replace(" ", "<color=#144852>·</color>").Replace("\t", "<color=#144852>→   </color>");
                saveButton.SetEnabled(_cachedTemplateSettings != _previewTemplateSettings && _previewTemplateSettings.IsValid());
            }
        }

        void BindTemplateView()
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
            listView.onSelectionChange += selection => Selection.activeObject = (UnityEngine.Object)selection.FirstOrDefault();
#else
            listView.onSelectionChanged += selection => Selection.activeObject = (UnityEngine.Object)selection.FirstOrDefault();
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