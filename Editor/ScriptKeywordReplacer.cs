/**
 * ScriptKeywordReplacer.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2022-09-21
 */

using System.Globalization;

namespace MyUnityTools.ScriptTemplates
{
    public class ScriptKeywordReplacer
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

        public string AuthorString => $"{_templateSettings.Signature.AuthorName}{(!string.IsNullOrWhiteSpace(_templateSettings.Signature.AuthorEmail) ? $" [{_templateSettings.Signature.AuthorEmail}]" : string.Empty)}";
        public string DateString => _templateSettings.Signature.UseLocalDateFormat ? $"{System.DateTime.Now.ToString("d", CultureInfo.CurrentCulture)} ({CultureInfo.CurrentCulture.Name})" : System.DateTime.Now.ToString("yyyy-MM-dd");

        readonly ScriptTemplateSettings _templateSettings;

        public ScriptKeywordReplacer(ScriptTemplateSettings templateSettings)
        {
            _templateSettings = templateSettings;
        }

        public string ReplaceSignature()
        {
            return _templateSettings.Signature.Enabled ? _signatureTemplate.Replace(_authorKeyword, AuthorString).Replace(_dateKeyword, DateString) : string.Empty;
        }

        public string ProcessScriptTemplate(string template, string scriptName)
        {
            return template
                .Replace(_signatureKeyword, ReplaceSignature())
                .Replace(_scriptNameKeyword, scriptName);
        }
    }
}