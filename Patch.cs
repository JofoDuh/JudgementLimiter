using ADOFAI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace JudgementLimiter
{
    public static class Patch
    {
        private static scrController Controller
        {
            get
            {
                return scrController.instance;
            }
        }

        [HarmonyPatch(typeof(scrCalibrationPlanet), "Start")]
        internal static class scrCalibrationPlanet_Start
        {
            private static void Postfix()
            {
                if (!Main.IsEnabled) return;
                Main.judgementLimiterText.TextObject.SetActive(false);
            }
        }



        [HarmonyPatch(typeof(scrUIController), "WipeToBlack")]
        internal static class scrUIController_WipeToBlack_Patch
        {
            private static void Postfix()
            {
                if (!Main.IsEnabled) return;
                Main.judgementLimiterText.TextObject.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(scnEditor), "ResetScene")]
        internal static class scnEditor_ResetScene_Patch
        {
            private static void Postfix()
            {
                if (!Main.IsEnabled) return;
                Main.judgementLimiterText.TextObject.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(scrController), "StartLoadingScene")]
        internal static class scrController_StartLoadingScene_Patch
        {
            private static void Postfix()
            {
                if (!Main.IsEnabled) return;
                Main.judgementLimiterText.TextObject.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(scnGame), "Play")]
        public static class PlayPatch
        {
            private static void Postfix()
            {
                if (!Main.IsEnabled) return;
                if (!ADOBase.controller.gameworld) return;

                Main.setting.LimitAssigner();
                LoadText();
            }
        }

        [HarmonyPatch(typeof(scrPressToStart), "ShowText")]
        public static class BossLevelStart
        {
            private static void Postfix()
            {
                if (!Main.IsEnabled) return;
                if (!ADOBase.controller.gameworld) return;

                LoadText();
            }
        }

        [HarmonyPatch(typeof(scrMisc), "GetHitMargin")]
        public static class GetHitMarginPatch
        {
            public static void Postfix(ref HitMargin __result)
            {
                foreach (var hitmargin in Main.setting.hitmargin)
                {
                    if (__result == hitmargin)
                    {
                        if (Main.setting.LimitChoice[(int)hitmargin])
                        {
                            if (Main.setting.LimitChecker[(int)hitmargin] != 0)
                            {
                                Main.setting.LimitChecker[(int)hitmargin] -= 1;
                            }
                            else
                            {
                                bool origNoFail = Controller.noFail;
                                if (Main.setting.killnofail)
                                {
                                    Controller.noFail = false;
                                }
                                Controller.FailAction(true);
                                Controller.noFail = origNoFail;
                            }
                        }
                        break;
                    }
                }
                List<string> texts = new List<string>();

                for (int i = 0; i < Main.setting.InputLimit.Length; i++)
                {
                    if (Main.setting.LimitChoice[i])
                    {
                        texts.Add(Main.setting.Displaytext.Replace("{Judgement}", Main.setting.Getname(i)) +
                            $" {Main.setting.LimitChecker[i]}");
                    }
                }
                Main.judgementLimiterText.setText(string.Join("\n", texts));
            }
        }
        private static void LoadText()
        {
            Main.judgementLimiterText.TextObject.SetActive(true);
            List<string> texts = new List<string>();

            for (int i = 0; i < Main.setting.InputLimit.Length; i++)
            {
                if (Main.setting.LimitChoice[i])
                {
                    texts.Add(Main.setting.Displaytext.Replace("{Judgement}", Main.setting.Getname(i)) +
                        $" {Main.setting.LimitChecker[i]}");
                }
            }
            Main.judgementLimiterText.setText(string.Join("\n", texts));
            Main.judgementLimiterText.setSize(Main.setting.size);
        }
    }
}