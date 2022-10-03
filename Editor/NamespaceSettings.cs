/**
 * NamespaceSettings.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2021-10-02
 */

using System;

namespace MyUnityTools.ScriptTemplates
{
    [Serializable]
    public struct NamespaceSettings
    {
        public bool Enabled;
        public bool UseAssemblyDefinition;
        public string DefaultNamespace;
        public IndentPattern IndentPattern;
        public int IndentMultiplier;

        public override bool Equals(object obj)
        {
            return obj is NamespaceSettings settings &&
                   Enabled == settings.Enabled &&
                   UseAssemblyDefinition == settings.UseAssemblyDefinition &&
                   DefaultNamespace == settings.DefaultNamespace &&
                   IndentPattern == settings.IndentPattern &&
                   IndentMultiplier == settings.IndentMultiplier;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Enabled, UseAssemblyDefinition, DefaultNamespace, IndentPattern, IndentMultiplier);
        }

        public bool IsValid() => !Enabled || !string.IsNullOrWhiteSpace(DefaultNamespace);

        public bool IsEmpty() => this == new NamespaceSettings();

        public static bool operator ==(NamespaceSettings left, NamespaceSettings right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NamespaceSettings left, NamespaceSettings right)
        {
            return !(left == right);
        }
    }
}