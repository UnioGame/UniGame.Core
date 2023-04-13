namespace UniModules.UniUiSystem.Runtime.Utils
{
    using System;
    using TMPro;
    using UniCore.Runtime.Utils;
    using UnityEngine;
    using UnityEngine.UI;

    public static class UiValueExtensions
    {
        public static bool SetValue(this TextMeshPro text, string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (!text) return false;
            
            if (string.Equals(text.text, value,comparison))
                return false;

            text.text = value;
            return true;
        }
        
            
        public static bool SetValue(this TMP_Text text, string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (!text) return false;
            
            if (string.Equals(text.text, value,comparison))
                return false;

            text.text = value;
            return true;
        }
            
        public static bool SetValue(this TextMeshProUGUI text, string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (!text) return false;
            
            if (string.Equals(text.text, value,comparison))
                return false;

            text.text = value;
            return true;
        }
        
        public static bool SetValue(this TextMeshProUGUI text, Color color)
        {
            if (!text) return false;
            
            text.color = color;
            return true;
        }
        
        public static bool SetValue(this TextMeshPro text, int value)
        {
            if (!text) return false;
            
            var stringValue = value.ToStringFromCache();
            return SetValue(text, stringValue);
        }
        
        public static bool SetValue(this TextMeshProUGUI text, int value)
        {
            if (!text) return false;
            
            var stringValue = value.ToStringFromCache();
            return SetValue(text, stringValue);
        }

        public static bool SetValue(this Image target, Sprite value)
        {
            if (target == null || target.sprite == value) return false;
            
            var enabled = value != null;
            target.enabled = enabled;
            
            if(enabled) target.sprite = value;
            
            return true;
        }
        
        public static bool SetValue(this Image target, Color icon)
        {
            if (!target) return false;
            
            target.color = icon;

            return true;
        }
        
    }
}
