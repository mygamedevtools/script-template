/**
 * ScriptKeywordReplacer.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2022-09-21
 */

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Compilation;

namespace MyUnityTools.ScriptTemplates
{
    public class ScriptKeywordReplacer
    {
        const string _scriptNameKeyword = "#SCRIPTNAME#";
        const string _namespaceKeyword = "#NAMESPACE#";
        const string _signatureKeyword = "#SIGNATURE#";
        const string _authorKeyword = "#AUTHOR#";
        const string _dateKeyword = "#CREATIONDATE#";

        static readonly string _signatureTemplate =
            "/**\n" +
            " * " + _scriptNameKeyword + ".cs\n" +
            " * Created by: " + _authorKeyword + "\n" +
            " * Created on: " + _dateKeyword + "\n" +
            " */\n\n";

        public string AuthorString => $"{_templateSettings.Signature.AuthorName}{(!string.IsNullOrWhiteSpace(_templateSettings.Signature.AuthorEmail) ? $" [{_templateSettings.Signature.AuthorEmail}]" : string.Empty)}";
        public string DateString => _templateSettings.Signature.UseLocalDateFormat ? $"{DateTime.Now.ToString("d", CultureInfo.CurrentCulture)} ({CultureInfo.CurrentCulture.Name})" : DateTime.Now.ToString("yyyy-MM-dd");

        readonly ScriptTemplateSettings _templateSettings;

        public ScriptKeywordReplacer(ScriptTemplateSettings templateSettings)
        {
            _templateSettings = templateSettings;
        }

        public string ProcessScriptTemplate(string scriptContent, string path)
        {
            Regex regex;
            if (_templateSettings.Namespace.Enabled)
            {
                var namespaceString = string.Empty;
                if (_templateSettings.Namespace.UseAssemblyDefinition)
                {
                    namespaceString = CompilationPipeline.GetAssemblyRootNamespaceFromScriptPath(path);
                    if (string.IsNullOrWhiteSpace(namespaceString))
                        namespaceString = CompilationPipeline.GetAssemblyNameFromScriptPath(path).Replace(".dll", string.Empty);
                }
                if (string.IsNullOrWhiteSpace(namespaceString) || namespaceString.StartsWith("Assembly-CSharp"))
                    namespaceString = _templateSettings.Namespace.DefaultNamespace;

                regex = new Regex(@"(?<=#NAMESPACE#)([\s\S]*?(\n|\r|\r\n)}(\n|\r|\r\n)?)");
                if (regex.IsMatch(scriptContent))
                {
                    var match = regex.Match(scriptContent);
                    var indentString = GetIndentReplacement();
                    var indentedMatch = match.Value.Replace("\n", indentString);
                    scriptContent = scriptContent
                        .Replace(match.Value, indentedMatch)
                        .Replace(_namespaceKeyword, "namespace " + namespaceString + "\n{" + indentString);

                    scriptContent = scriptContent.Insert(scriptContent.Length, "\n}");
                }
            }

            scriptContent = scriptContent.Replace(_namespaceKeyword, string.Empty);

            var stringBuilder = new StringBuilder(scriptContent);
            stringBuilder
                .Replace(_signatureKeyword, _templateSettings.Signature.Enabled ? _signatureTemplate.Replace(_authorKeyword, AuthorString).Replace(_dateKeyword, DateString) : string.Empty)
                .Replace(_scriptNameKeyword, System.IO.Path.GetFileNameWithoutExtension(path));
            return Regex.Replace(stringBuilder.ToString(), @"\r\n|\n\r|\n|\r", Environment.NewLine);
        }

        string GetIndentReplacement()
        {
            string indentValue = _templateSettings.Namespace.IndentPattern == IndentPattern.Spaces ? " " : "\t";
            var stringBuilder = new StringBuilder("\n");

            for (int i = 0; i < _templateSettings.Namespace.IndentMultiplier; i++)
                stringBuilder.Append(indentValue);
            return stringBuilder.ToString();
        }
    }
}