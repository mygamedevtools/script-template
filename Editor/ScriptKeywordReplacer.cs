/**
 * ScriptKeywordReplacer.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2022-09-21
 */

using System.Globalization;
using UnityEditor;

namespace MyUnityTools.ScriptTemplates
{
    public static class ScriptKeywordReplacer
    {
        const string _scriptNameKeyword = "#SCRIPTNAME#";
        const string _signatureKeyword = "#SIGNATURE#";
        const string _authorKeyword = "#AUTHOR#";
        const string _dateKeyword = "#CREATIONDATE#";

        const string _signatureTemplate =
            "/**\n" +
            " * " + _scriptNameKeyword + ".cs\n" +
            " * Created by: " + _authorKeyword + "\n" +
            " * Created on: " + _dateKeyword + "\n" +
            " */\n\n";

        public static string AuthorString => $"{EditorPrefs.GetString(ScriptTemplatesEditor.AuthorNameKey)}{(EditorPrefs.HasKey(ScriptTemplatesEditor.AuthorEmailKey) ? $" [{EditorPrefs.GetString(ScriptTemplatesEditor.AuthorEmailKey)}]" : string.Empty)}";
        public static string DateString => EditorPrefs.GetBool(ScriptTemplatesEditor.UseLocalDateKey, false) ? $"{System.DateTime.Now.ToString("d", CultureInfo.CurrentCulture)} ({CultureInfo.CurrentCulture.Name})" : System.DateTime.Now.ToString("yyyy-MM-dd");

        public static string GetSignatureText(string scriptName)
        {
            return _signatureTemplate.Replace(_authorKeyword, AuthorString).Replace(_dateKeyword, DateString);
        }

        public static string ProcessScriptTemplate(string template, string scriptName)
        {
            return template.Replace(_signatureKeyword, GetSignatureText(scriptName))
                .Replace(_scriptNameKeyword, scriptName);
        }
    }
}