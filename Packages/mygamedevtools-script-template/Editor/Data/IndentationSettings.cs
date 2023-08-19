/**
 * IndentationSettings.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2023-08-19
 */

using System;

namespace MyGameDevTools.ScriptTemplates
{
    [Serializable]
    public struct IndentationSettings
    {
        public IndentPattern IndentPattern;
        public int IndentMultiplier;
    
        public override bool Equals(object obj)
        {
            return obj is IndentationSettings settings &&
                   IndentPattern == settings.IndentPattern &&
                   IndentMultiplier == settings.IndentMultiplier;
        }

        public static IndentationSettings Default()
        {
            return new IndentationSettings
            {
                IndentPattern = IndentPattern.Spaces,
                IndentMultiplier = 4
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IndentPattern, IndentMultiplier);
        }

        public bool IsEmpty() => this == new IndentationSettings();

        public static bool operator ==(IndentationSettings left, IndentationSettings right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IndentationSettings left, IndentationSettings right)
        {
            return !(left == right);
        }
    }
}