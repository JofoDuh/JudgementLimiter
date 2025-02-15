using UnityEngine;

namespace JudgementLimiter
{
    public static class MoreGUILayout
    {
        // Copyright (c) 2021 PizzaLovers007
        public static float NamedSlider(string name, float value, float leftValue, float rightValue, float sliderWidth, 
            float roundNearest = 0, float labelWidth = 0, string valueFormat = "{0}")
        {
            GUILayout.BeginHorizontal();
            float newValue = NamedSliderContent(
                    name,
                    value,
                    leftValue,
                    rightValue,
                    sliderWidth,
                    roundNearest,
                    labelWidth,
                    valueFormat);
            GUILayout.EndHorizontal();
            return newValue;
        }
        private static float NamedSliderContent(string name, float value, float leftValue, float rightValue, float sliderWidth,
            float roundNearest = 0, float labelWidth = 0, string valueFormat = "{0}")
        {
            if (labelWidth == 0)
            {
                GUILayout.Label(name);
                GUILayout.Space(4f);
            }
            else
            {
                GUILayout.Label(name, GUILayout.Width(labelWidth));
            }
            float newValue = GUILayout.HorizontalSlider(
                    value, leftValue, rightValue, GUILayout.Width(sliderWidth));
            if (roundNearest != 0)
            {
                newValue = Mathf.Round(newValue / roundNearest) * roundNearest;
            }
            GUILayout.Space(8f);
            GUILayout.Label(string.Format(valueFormat, newValue), GUILayout.Width(40f));
            GUILayout.FlexibleSpace();
            return newValue;
        }
        public static string NamedTextField(
            string name,
            string value,
            float fieldWidth,
            float labelWidth = 0)
        {
            GUILayout.BeginHorizontal();
            string newValue = NamedTextFieldContent(name, value, fieldWidth, labelWidth);
            GUILayout.EndHorizontal();
            return newValue;
        }
        private static string NamedTextFieldContent(
            string name,
            string value,
            float fieldWidth,
            float labelWidth = 0)
            {
                if (labelWidth == 0)
                {
                    GUILayout.Label(name);
                    GUILayout.Space(4f);
                }
                else
                {
                GUILayout.Label(name, GUILayout.Width(labelWidth));
                }
            string newValue = GUILayout.TextField(value, GUILayout.Width(fieldWidth));
            GUILayout.FlexibleSpace();
            return newValue;
        }
    }
}