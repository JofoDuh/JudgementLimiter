using System;
using UnityEngine;

namespace JudgementLimiter
{
    public static class JudgementLimiter
    {
        public static void JudgementLimiterSettings()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            Main.setting.Displaytext = MoreGUILayout.NamedTextField("Display Text: ", Main.setting.Displaytext, 500, 80);
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);

            for (int i = 0; i < Main.setting.InputLimit.Length; i++)
            {
                Main.setting.LimitChoice[i] = GUILayout.Toggle(Main.setting.LimitChoice[i], $"Limit {Main.setting.Getname(i)}");

                if (Main.setting.LimitChoice[i])
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(30f);
                    string NewValue = MoreGUILayout.NamedTextField("Limit: ", Main.setting.InputLimit[i].ToString(), 50, 40);
                    Main.setting.InputLimit[i] = int.Parse(NewValue);
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.Space(5f);
            Main.setting.killnofail = GUILayout.Toggle(Main.setting.killnofail, "Kill even when No-Fail is on");
        }
    }
}