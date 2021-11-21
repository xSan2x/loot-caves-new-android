using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.ExpressionParser;

namespace GameLib
{
    
    public class Evaluator
    {
        public enum Rarity
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary
        }
        public static string[] rarityStr = new string[5] { "common", "uncommon", "rare", "epic", "legendary"};
        public static string[] typeStr = new string[8] { "helmet", "fullbody", "gloves", "boots", "shield", "weapon", "ring", "necklace" };
        public static int[] maxWearableIndex = new int[8] { 29, 83, 11, 8, 30, 394, 49, 41 };
        public static Color[] rarityColor = new Color[5] {new Color32(128, 128, 128, 255), new Color32(30, 255, 0, 255), new Color32(0, 112, 221, 255), new Color32(163, 53, 238, 255), new Color32(255, 128, 0, 255)};
        public static string[] shortAttributes = new string[13] { "STR", "AGI", "INT", "TRK", "END", "ALL", "ATK", "SPD", "CRIT", "EVD", "MDMG", "HP", "REG" };
        public static string[] longAttributes = new string[13] { "Strength", "Agility", "Intelligence", "Trickery", "Endurance", "All Stats", "Attack", "Attack Speed", "Critical Damage", "Evasion", "Magical Damage", "Health", "Regeneration" };
        

        public static Rarity RandRarity(float legChance, float multiplier = 1) //chance on less rarity increaces 100%*multiplier. LegChance = chance to obtain Legendary
        {
            float rnd = Random.Range(0f, 100f);
            if (rnd < legChance * 100 * multiplier) // Get legendary?
            {
                return Rarity.Legendary;
            }
            else if (rnd < legChance * 200 * multiplier) // Get epic?
            {
                return Rarity.Epic;
            }
            else if (rnd < legChance * 300 * multiplier) // Get rare?
            {
                return Rarity.Rare;
            }
            else if (rnd < legChance * 400 * multiplier) // Get uncommon?
            {
                return Rarity.Uncommon;
            }
            else // Get common rarity
            {
                return Rarity.Common;
            }
        }

        public static float FormulaSolution(string formula)
        {
            var parser = new ExpressionParser();
            if (formula.Contains("STR"))
            {
                formula = formula.Replace("STR", Hero.instance.STR.ToString());
            }
            if (formula.Contains("AGI"))
            {
                formula = formula.Replace("AGI", Hero.instance.AGI.ToString());
            }
            if (formula.Contains("INT"))
            {
                formula = formula.Replace("INT", Hero.instance.INT.ToString());
            }
            if (formula.Contains("END"))
            {
                formula = formula.Replace("END", Hero.instance.END.ToString());
            }
            if (formula.Contains("TRK"))
            {
                formula = formula.Replace("TRK", Hero.instance.TRK.ToString());
            }
            Expression exp = parser.EvaluateExpression(formula);
            return (float)exp.Value; //returns parsed value
        }

        public static string CheckInt(int number)
        {
            string returnedNum = number.ToString();
            if (number < 1000)
            {
                returnedNum = number.ToString();
            }
            else if (number < 1000000)
            {
                returnedNum = ((float)number / 1000).ToString("F2") + " K";
            }
            else if (number < 1000000000)
            {
                returnedNum = ((float)number / 1000000).ToString("F2") + " M";
            }
            else
            {
                returnedNum = ((float)number / 1000000000).ToString("F2") + " B";
            }
            return returnedNum;
        }
        public static string CheckFloat(float number)
        {
            string returnedNum = number.ToString();
            if (number < 1000)
            {
                returnedNum = number.ToString("F2");
            }
            else if (number < 1000000)
            {
                returnedNum = (number / 1000).ToString("F2") + " K";
            }
            else if (number < 1000000000)
            {
                returnedNum = (number / 1000000).ToString("F2") + " M";
            }
            else
            {
                returnedNum = (number / 1000000000).ToString("F2") + " B";
            }
            return returnedNum;
        }
    }
}
    
