/**
 * ScriptTemplateSettings.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2021-10-02
 */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyGameDevTools.ScriptTemplates
{
    [Serializable]
    public class ScriptTemplateSettings
    {
        const string _editorPrefsKey = "com.myunitytools.script-template:settings";

        public SignatureSettings Signature;
        public NamespaceSettings Namespace;

        public void SaveToEditorPrefs() => EditorPrefs.SetString(_editorPrefsKey, JsonUtility.ToJson(this));

        public void DeleteEditorPrefs() => EditorPrefs.DeleteKey(_editorPrefsKey);

        public static ScriptTemplateSettings FromEditorPrefs() => JsonUtility.FromJson<ScriptTemplateSettings>(EditorPrefs.GetString(_editorPrefsKey, "{}"));

        public override bool Equals(object obj)
        {
            return obj is ScriptTemplateSettings settings &&
                   EqualityComparer<SignatureSettings>.Default.Equals(Signature, settings.Signature) &&
                   EqualityComparer<NamespaceSettings>.Default.Equals(Namespace, settings.Namespace);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Signature, Namespace);
        }

        public bool IsEmpty() => Signature.IsEmpty() && Namespace.IsEmpty();

        public bool IsValid() => Signature.IsValid() && Namespace.IsValid();

        public static bool operator ==(ScriptTemplateSettings left, ScriptTemplateSettings right)
        {
            return EqualityComparer<ScriptTemplateSettings>.Default.Equals(left, right);
        }

        public static bool operator !=(ScriptTemplateSettings left, ScriptTemplateSettings right)
        {
            return !(left == right);
        }
    }
}