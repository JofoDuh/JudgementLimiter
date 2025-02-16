using System;
using UnityEngine;

namespace JudgementLimiter
{
    public static class JudgementLimiter
    {
        public static void JudgementLimiterSettings()
        {
            Main.setting.AccLimit = GUILayout.Toggle(Main.setting.AccLimit, "Limit X-Accuracy");

            if (Main.setting.AccLimit)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30f);
                GUILayout.BeginVertical(); // Nested layout for content
                // Determine how many decimal places to display dynamically
                string formattedAccGoal = Main.setting.AccGoal.ToString("0.0###"); // Shows only necessary decimals

                // Accuracy Goal (as percentage)
                string newAccGoalText = MoreGUILayout.NamedTextField("Limit: ", formattedAccGoal + "%", 100, 35);

                // Remove "%" and trim spaces
                string numericValue = newAccGoalText.Replace("%", "").Trim();

                // Parse and update
                double result = Main.setting.ParseInput(numericValue);
                if (Math.Abs(Main.setting.AccGoal - result) > 0.0001) // Small tolerance for float comparison
                {
                    Main.setting.AccGoal = result;
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            if (Main.setting.AccLimit)
            {
                GUILayout.Space(5f);
                GUILayout.BeginHorizontal();
                GUILayout.Space(10f);
                GUILayout.BeginVertical(); // Nested layout for content
                Main.setting.DisplayAccLimit = MoreGUILayout.NamedTextField("Display Text:", Main.setting.DisplayAccLimit, 500, 80);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(5f);
            for (int i = 0; i < Main.setting.hitmargin.Length; i++)
            {
                if (i % 2 == 0) GUILayout.BeginHorizontal();  // Start horizontal layout for pairs of toggles
                GUILayout.BeginVertical();  // Wrap the toggle in a vertical layout
                Main.setting.LimitChoice[i] = GUILayout.Toggle(Main.setting.LimitChoice[i], $"Limit {Main.setting.Getname(i)}", GUILayout.Width(700f));

                if (Main.setting.LimitChoice[i])
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(30f);
                    GUILayout.BeginVertical();
                    string NewValue = MoreGUILayout.NamedTextField("Limit: ", Main.setting.InputLimit[i].ToString(), 50, 35);
                    if (!string.IsNullOrEmpty(NewValue) && int.TryParse(NewValue, out int parsedValue))
                    {
                        Main.setting.InputLimit[i] = Math.Max(0, parsedValue); // Clamp value between 0 and 100
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();  // End the vertical layout
                if (i % 2 == 1) GUILayout.EndHorizontal();  // Close the horizontal layout for pairs of toggles
            }
            foreach (bool e in Main.setting.LimitChoice)
            {
                if (e)
                {
                    GUILayout.Space(5f);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10f);
                    GUILayout.BeginVertical(); // Nested layout for content
                    Main.setting.Displaytext = MoreGUILayout.NamedTextField("Display Text: ", Main.setting.Displaytext, 500, 80);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    break;
                }
            }
        }

        public static double XAcc(int TileCount, int CurrentTile, int EPerfects, int LPerfects, int Early, int Late, int TooEarly, int TooLate, 
            int Miss, int OverLoad)
        {
            int LevelTileCount = TileCount - CurrentTile;
            int count = LevelTileCount + TooEarly + TooLate + OverLoad;
            // Ensure no division by zero
            if (count == 0)
                return 100.0; // If no tiles played, assume 100% (perfect acc)

            int PurePerfects = LevelTileCount - (EPerfects + LPerfects + Early + Late + Miss); // Correct Pure Perfect Calculation

            double num5 = (
                (1.0 * PurePerfects) +
                (0.75 * EPerfects) +
                (0.75 * LPerfects) +
                (0.4 * Early) +
                (0.4 * Late) +
                (0.2 * TooEarly) +
                (0.2 * TooLate)
            ) / count;  // Entire sum is divided by count

            return num5 * 100;
        }
    }
}