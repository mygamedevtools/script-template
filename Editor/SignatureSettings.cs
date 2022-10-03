/**
 * SignatureSettings.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2021-10-02
 */

using System;

namespace MyGameDevTools.ScriptTemplates
{
    [Serializable]
    public struct SignatureSettings
    {
        public bool Enabled;
        public string AuthorName;
        public string AuthorEmail;
        public bool UseLocalDateFormat;

        public override bool Equals(object obj)
        {
            return obj is SignatureSettings settings &&
                   Enabled == settings.Enabled &&
                   AuthorName == settings.AuthorName &&
                   AuthorEmail == settings.AuthorEmail &&
                   UseLocalDateFormat == settings.UseLocalDateFormat;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Enabled, AuthorName, AuthorEmail, UseLocalDateFormat);
        }

        public bool IsEmpty() => this == new SignatureSettings();

        public bool IsValid() => !Enabled || !string.IsNullOrWhiteSpace(AuthorName);

        public static bool operator ==(SignatureSettings left, SignatureSettings right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SignatureSettings left, SignatureSettings right)
        {
            return !(left == right);
        }
    }
}