using System;
using System.IO;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace JudgementLimiter
{
    public class Setting : UnityModManager.ModSettings
    {
        public readonly HitMargin[] hitmargin =
        {
            HitMargin.TooEarly,
            HitMargin.VeryEarly,
            HitMargin.EarlyPerfect,
            HitMargin.LatePerfect,
            HitMargin.VeryLate,
            HitMargin.TooLate
        };

        public int[] InputLimit = new int[6];
        public int[] LimitChecker = new int[6];
        public bool[] LimitChoice = new bool[6];
        public bool killnofail = true;
        public string Displaytext = "{Judgement} Left:";

        public bool AccLimit = false;
        public string DisplayAccLimit = "Acc Limit:{Acc}";
        public int seqID = 0;
        public double AccGoal = 100;

        //public int[] Orders = new int[7];
        //public List<string> TextOrder = new List<string>() { "LimitText", "AccGoalText" }; // Default order

        public int EarlyCount = 0, LateCount = 0, EPerfectCount = 0, LPerfectCount = 0, 
            FailMiss = 0, FailOverload = 0, TooEarlyCount = 0, TooLateCount = 0;

        public float x = 0.96f, y = 0.98f;
        public int size = 35;
        public int align = 2;

        public bool useShadow = true;
        public bool useBold = false;

        public double ParseInput(string input)
        {
            if (!string.IsNullOrEmpty(input) && input != "." && input != "-") // Prevents invalid cases
            {
                if (double.TryParse(input, out double parsedValue))
                {
                    return Math.Max(0.0, Math.Min(100.0, Math.Round(parsedValue, 4))); // Clamp between 0-100 and round to 4 decimal places
                }
            }
            return AccGoal; // Return the existing value if input is invalid
        }
        public void SetJudgement()
        {
            EarlyCount = 0;
            LateCount = 0;
            EPerfectCount = 0;
            LPerfectCount = 0;
            TooEarlyCount = 0;
            TooLateCount = 0;
            FailOverload = 0;
            FailMiss = 0;
        }
        public void LimitAssigner()
        {
            for (int i = 0; i < InputLimit.Length; i++)
            {
                LimitChecker[i] = InputLimit[i];
            }
        }
        public string Getname(int i)
        {
            switch(i)
            {
                case 0:
                    return "Too Earlies";
                case 1:
                    return "Earlies";
                case 2:
                    return "EPerfects";
                case 3:
                    return "LPerfects";
                case 4:
                    return "Lates";
                case 5:
                    return "Too Lates";
                default:
                    return string.Empty;
            }
        }
        public override void Save(UnityModManager.ModEntry modEntry) {
            var filepath = GetPath(modEntry);
            try {
                using (var writer = new StreamWriter(filepath)) {
                    var serializer = new XmlSerializer(GetType());
                    serializer.Serialize(writer, this);
                }
            } catch {
            }
        }
        
        public override string GetPath(UnityModManager.ModEntry modEntry) {
            return Path.Combine(modEntry.Path, GetType().Name + ".xml");
        }
    }
}