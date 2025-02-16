using HarmonyLib;
using System.Collections.Generic;

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
            private static void Postfix(int seqID)
            {

                Main.setting.seqID = seqID;
                if (!Main.IsEnabled) return;
                if (!ADOBase.controller.gameworld) return;
                Main.setting.LimitAssigner();
                Main.setting.SetJudgement();
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
        private static int FloorCount
        {
            get
            {
                return scrLevelMaker.instance.listFloors.Count - 1;
            }
        }

        [HarmonyPatch(typeof(scrMistakesManager), "AddHit")]
        public static class AddHitPatch
        {
            public static void Postfix(HitMargin hit)
            {
                int i = 0;
                foreach (HitMargin hitmargin in Main.setting.hitmargin)
                {
                    if (hit == hitmargin)
                    {
                        if (Main.setting.LimitChoice[i])
                        {
                            if (Main.setting.LimitChecker[i] != 0)
                            {
                                Main.setting.LimitChecker[i] -= 1;
                            }
                            else
                            {
                                KillPlayer();
                            }
                        }
                        break;
                    }
                    i++;
                }

                List<string> texts = GetText(new List<string>());
                Main.judgementLimiterText.setText(string.Join("\n", texts));

                switch(hit)
                {
                    case HitMargin.TooEarly:
                        Main.setting.TooEarlyCount++;
                        break;
                    case HitMargin.VeryEarly:
                        Main.setting.EarlyCount++;
                        break;
                    case HitMargin.EarlyPerfect:
                        Main.setting.EPerfectCount++;
                        break;
                    case HitMargin.LatePerfect:
                        Main.setting.LPerfectCount++;
                        break;
                    case HitMargin.VeryLate:
                        Main.setting.LateCount++;
                        break;
                    case HitMargin.TooLate:
                        Main.setting.TooEarlyCount++;
                        break;
                    case HitMargin.FailMiss:
                        Main.setting.FailMiss++;
                        break;
                    case HitMargin.FailOverload:
                        Main.setting.FailOverload++;
                        break;
                }
                if (Main.setting.AccLimit)
                {
                    double GetCurrentAcc = JudgementLimiter.XAcc(FloorCount, Main.setting.seqID, Main.setting.EPerfectCount, Main.setting.LPerfectCount,
                        Main.setting.EarlyCount, Main.setting.LateCount, Main.setting.TooEarlyCount, Main.setting.TooLateCount, Main.setting.FailMiss,
                        Main.setting.FailOverload);

                    if (GetCurrentAcc < Main.setting.AccGoal)
                    {
                        KillPlayer();
                    }
                }
            }
        }

        private static void KillPlayer()
        {
            bool origNoFail = Controller.noFail;
            if (Main.setting.killnofail)
            {
                Controller.noFail = false;
            }
            Controller.FailAction(true);
            Controller.noFail = origNoFail;
        }
        private static void LoadText()
        {
            Main.judgementLimiterText.TextObject.SetActive(true);
            List<string> texts = GetText(new List<string>());
            Main.judgementLimiterText.setText(string.Join("\n", texts));
            Main.judgementLimiterText.setSize(Main.setting.size);
        }
        private static List<string> GetText(List<string> texts)
        {
            if (Main.setting.AccLimit)
            {
                // Add Accuracy Goal Text if enabled
                string accGoalText = Main.setting.DisplayAccLimit.Replace("{Acc}", Main.setting.AccGoal.ToString("0.0###") + "%");
                texts.Add(accGoalText);
            }

            // Add Limit Text based on choices
            for (int i = 0; i < Main.setting.InputLimit.Length; i++)
            {
                if (Main.setting.LimitChoice[i])
                {
                    texts.Add(Main.setting.Displaytext.Replace("{Judgement}", Main.setting.Getname(i)) +
                        $" {Main.setting.LimitChecker[i]}");
                }
            }
            return texts;
        }
    }
}