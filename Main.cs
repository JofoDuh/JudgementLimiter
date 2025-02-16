using System.Reflection;
using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace JudgementLimiter
{
    public static class Main
    {
        public static bool IsEnabled = false;
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static Setting setting;
        public static JudgementLimiterText judgementLimiterText;
        internal static void Setup(UnityModManager.ModEntry modEntry)
        {
            setting = new Setting();
            setting = UnityModManager.ModSettings.Load<Setting>(modEntry);
            
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;  
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            if (value)
            {
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                judgementLimiterText = new GameObject().AddComponent<JudgementLimiterText>();
                UnityEngine.Object.DontDestroyOnLoad(judgementLimiterText);

                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;
                judgementLimiterText.TextObject.SetActive(false);
            }
            else
            {
                judgementLimiterText.TextObject.SetActive(false);
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            bool flag = false;
            foreach (bool e in setting.LimitChoice)
            {
                if (e)
                {
                    flag = true;
                    break;
                }
                else flag = false;
            }
            if (!flag) flag = setting.AccLimit;
            JudgementLimiter.JudgementLimiterSettings();
            if (flag)
            {
                GUILayout.Space(5f);
                setting.useShadow = GUILayout.Toggle(setting.useShadow, "Add Text Shadow");
                judgementLimiterText.shadowText.enabled = setting.useShadow;

                setting.useBold = GUILayout.Toggle(setting.useBold, "Make Text Bold");
                judgementLimiterText.text.fontStyle = setting.useBold ? FontStyle.Bold : FontStyle.Normal;

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                float newX =
                    MoreGUILayout.NamedSlider(
                        "X: ",
                        setting.x,
                        -0.1f,
                        1.1f,
                        300f,
                        roundNearest: 0.01f,
                        valueFormat: "{0:0.##}");
                if (newX != setting.x)
                {
                    setting.x = newX;
                    judgementLimiterText.setPosition(setting.x, setting.y);
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                float newY =
                    MoreGUILayout.NamedSlider(
                        "Y: ",
                        setting.y,
                        -0.1f,
                        1.1f,
                        300f,
                        roundNearest: 0.01f,
                        valueFormat: "{0:0.##}");

                if (newY != setting.y)
                {
                    setting.y = newY;
                    judgementLimiterText.setPosition(setting.x, setting.y);
                }

                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                float newSize =
                    MoreGUILayout.NamedSlider(
                        "Size: ",
                        setting.size,
                        1f,
                        100f,
                        300f,
                        roundNearest: 1f,
                        valueFormat: "{0:0.##}");


                if ((int)newSize != setting.size)
                {
                    setting.size = (int)newSize;
                    judgementLimiterText.setSize(setting.size);
                }

                GUILayout.EndHorizontal();

                string[] aligns = new string[] { "Left", "Middle", "Right" };
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Alignment");

                GUIStyle guiStyle = new GUIStyle(GUI.skin.button);

                foreach (string text in aligns)
                {
                    if (setting.align == Array.IndexOf(aligns, text)) guiStyle.fontStyle = FontStyle.Bold;
                    if (GUILayout.Button(text, guiStyle)) setting.align = Array.IndexOf(aligns, text);
                    guiStyle.fontStyle = FontStyle.Normal;
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                judgementLimiterText.text.alignment = judgementLimiterText.toAlign(setting.align);
            }
            GUILayout.Space(5f);
            setting.killnofail = GUILayout.Toggle(setting.killnofail, "Kill even when No-Fail is on");
        }
        
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            setting.Save(modEntry);
        }
    }
}