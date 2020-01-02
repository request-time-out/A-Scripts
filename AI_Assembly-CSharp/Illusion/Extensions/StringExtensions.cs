// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.StringExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Illusion.Extensions
{
  public static class StringExtensions
  {
    public static string RemoveExtension(this string self)
    {
      return self.Substring(0, self.LastIndexOf("."));
    }

    public static string SetExtension(this string self, string extension)
    {
      return self.Substring(0, self.LastIndexOf(".")) + "." + extension;
    }

    public static string RemoveNewLine(this string self)
    {
      return self.Replace("\r", string.Empty).Replace("\n", string.Empty);
    }

    public static bool Compare(this string self, string str, bool ignoreCase = false)
    {
      return string.Compare(self, str, ignoreCase) == 0;
    }

    public static bool Compare(this string self, string str, StringComparison comparison)
    {
      return string.Compare(self, str, comparison) == 0;
    }

    public static bool CompareParts(this string self, string str, bool ignoreCase = false)
    {
      return !ignoreCase ? self.IndexOf(str) != -1 : self.IndexOf(str, StringComparison.OrdinalIgnoreCase) != -1;
    }

    public static bool CompareParts(this string self, string str, StringComparison comparison)
    {
      return self.IndexOf(str, comparison) != -1;
    }

    public static string[] LastStringEmptyRemove(this string[] self)
    {
      int length1 = self.Length;
      do
        ;
      while (--length1 >= 0 && self[length1].IsNullOrEmpty());
      int length2;
      string[] strArray = new string[length2 = length1 + 1];
      Array.Copy((Array) self, 0, (Array) strArray, 0, length2);
      return strArray;
    }

    public static List<string> LastStringEmptyRemove(this List<string> self)
    {
      int count = self.Count;
      do
        ;
      while (--count >= 0 && self[count].IsNullOrEmpty());
      return self.GetRange(0, count + 1);
    }

    public static string[] LastStringEmptySpaceRemove(this string[] self)
    {
      int length1 = self.Length;
      do
        ;
      while (--length1 >= 0 && self[length1].IsNullOrWhiteSpace());
      int length2;
      string[] strArray = new string[length2 = length1 + 1];
      Array.Copy((Array) self, 0, (Array) strArray, 0, length2);
      return strArray;
    }

    public static List<string> LastStringEmptySpaceRemove(this List<string> self)
    {
      int count = self.Count;
      do
        ;
      while (--count >= 0 && self[count].IsNullOrWhiteSpace());
      return self.GetRange(0, count + 1);
    }

    public static string Coloring(this string self, string color)
    {
      return string.Format("<color={0}>{1}</color>", (object) color, (object) self);
    }

    public static string Size(this string self, int size)
    {
      return string.Format("<size={0}>{1}</size>", (object) size, (object) self);
    }

    public static string Bold(this string self)
    {
      return string.Format("<b>{0}</b>", (object) self);
    }

    public static string Italic(this string self)
    {
      return string.Format("<i>{0}</i>", (object) self);
    }

    public static Color GetColor(this string self)
    {
      Color? colorCheck = self.GetColorCheck();
      return colorCheck.HasValue ? colorCheck.Value : Color.get_clear();
    }

    public static Color? GetColorCheck(this string self)
    {
      if (self.IsNullOrEmpty())
        return new Color?();
      string[] strArray = self.Split(',');
      if (strArray.Length >= 3)
      {
        int num1 = 0;
        string[] array1 = strArray;
        int index1 = num1;
        int num2 = index1 + 1;
        Color color;
        // ISSUE: cast to a reference type
        float.TryParse(array1.SafeGet<string>(index1), (float&) ref color.r);
        string[] array2 = strArray;
        int index2 = num2;
        int num3 = index2 + 1;
        // ISSUE: cast to a reference type
        float.TryParse(array2.SafeGet<string>(index2), (float&) ref color.g);
        string[] array3 = strArray;
        int index3 = num3;
        int num4 = index3 + 1;
        // ISSUE: cast to a reference type
        float.TryParse(array3.SafeGet<string>(index3), (float&) ref color.b);
        string[] array4 = strArray;
        int index4 = num4;
        int num5 = index4 + 1;
        // ISSUE: cast to a reference type
        if (!float.TryParse(array4.SafeGet<string>(index4), (float&) ref color.a))
          color.a = (__Null) 1.0;
        for (int index5 = 0; index5 < num5; ++index5)
        {
          if ((double) ((Color) ref color).get_Item(index5) > 1.0)
            ((Color) ref color).set_Item(index5, Mathf.InverseLerp(0.0f, (float) byte.MaxValue, ((Color) ref color).get_Item(index5)));
        }
        return new Color?(color);
      }
      Color color1;
      return ColorUtility.TryParseHtmlString(self, ref color1) ? new Color?(color1) : new Color?();
    }

    public static Vector2 GetVector2(this string self)
    {
      string[] strArray = StringExtensions.StringVectorReplace(self);
      return new Vector2(float.Parse(strArray[0]), float.Parse(strArray[1]));
    }

    public static Vector3 GetVector3(this string self)
    {
      string[] strArray = StringExtensions.StringVectorReplace(self);
      return new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
    }

    private static string[] StringVectorReplace(string str)
    {
      return str.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Split(',');
    }

    public static int Check(this string self, bool ignoreCase, params string[] strs)
    {
      return self.Check(ignoreCase, (Func<string, string>) (s => s), strs);
    }

    public static int Check(
      this string self,
      bool ignoreCase,
      Func<string, string> func,
      params string[] strs)
    {
      int index = -1;
      do
        ;
      while (++index < strs.Length && !self.Compare(func(strs[index]), ignoreCase));
      return index >= strs.Length ? -1 : index;
    }

    public static string ToTitleCase(this string self)
    {
      return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(self);
    }

    public static string CopyNameReplace(this string self, int cnt)
    {
      return cnt > 0 ? string.Format("{0} {1}", (object) self, (object) cnt) : self;
    }
  }
}
