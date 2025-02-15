using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
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
        };

        public int[] InputLimit = new int[5];
        public int[] LimitChecker = new int[5];
        public bool[] LimitChoice = new bool[5];
        public bool killnofail = true;
        public string Displaytext = "{Judgement} Left: ";

        public float x = 0.96f, y = 0.98f;
        public int size = 35;
        public int align = 2;

        public bool useShadow = true;
        public bool useBold = false;

        public void LimitAssigner()
        {
            for (int i = 0; i < InputLimit.Length; i++)
            {
                LimitChecker[i] = InputLimit[i];
            }
            foreach (int i in LimitChecker)
            {
                Main.Logger.Log(i.ToString());
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