// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.ColorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Illusion.Extensions
{
  public static class ColorExtensions
  {
    private static string[] FormatRemoveSplit(string str)
    {
      return ColorExtensions.FormatRemove(str).Split(',');
    }

    private static string FormatRemove(string str)
    {
      return str.Replace("RGBA", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
    }

    public static string Convert(this Color self, bool isDefault = true)
    {
      int num1 = 0;
      string format = !isDefault ? "{0},{1},{2},{3}" : "RGBA({0}, {1}, {2}, {3})";
      object[] objArray = new object[4];
      ref Color local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      objArray[0] = (object) ((Color) ref local1).get_Item(num2);
      ref Color local2 = ref self;
      int num4 = num3;
      int num5 = num4 + 1;
      objArray[1] = (object) ((Color) ref local2).get_Item(num4);
      ref Color local3 = ref self;
      int num6 = num5;
      int num7 = num6 + 1;
      objArray[2] = (object) ((Color) ref local3).get_Item(num6);
      ref Color local4 = ref self;
      int num8 = num7;
      int num9 = num8 + 1;
      objArray[3] = (object) ((Color) ref local4).get_Item(num8);
      return string.Format(format, objArray);
    }

    public static Color Convert(this Color _, string str)
    {
      string[] strArray = ColorExtensions.FormatRemoveSplit(str);
      Color clear = Color.get_clear();
      for (int index = 0; index < strArray.Length && index < 4; ++index)
      {
        float result;
        if (float.TryParse(strArray[index], out result))
          ((Color) ref clear).set_Item(index, result);
      }
      return clear;
    }

    public static float[] ToArray(this Color self)
    {
      int num1 = 0;
      float[] numArray = new float[4];
      ref Color local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      numArray[0] = ((Color) ref local1).get_Item(num2);
      ref Color local2 = ref self;
      int num4 = num3;
      int num5 = num4 + 1;
      numArray[1] = ((Color) ref local2).get_Item(num4);
      ref Color local3 = ref self;
      int num6 = num5;
      int num7 = num6 + 1;
      numArray[2] = ((Color) ref local3).get_Item(num6);
      ref Color local4 = ref self;
      int num8 = num7;
      int num9 = num8 + 1;
      numArray[3] = ((Color) ref local4).get_Item(num8);
      return numArray;
    }

    public static Color RGBToHSV(this Color self)
    {
      float num1;
      float num2;
      float num3;
      Color.RGBToHSV(self, ref num1, ref num2, ref num3);
      return new Color(num1, num2, num3, (float) self.a);
    }

    public static Color HSVToRGB(this Color self)
    {
      int num1 = 0;
      ref Color local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      double num4 = (double) ((Color) ref local1).get_Item(num2);
      ref Color local2 = ref self;
      int num5 = num3;
      int num6 = num5 + 1;
      double num7 = (double) ((Color) ref local2).get_Item(num5);
      ref Color local3 = ref self;
      int num8 = num6;
      int num9 = num8 + 1;
      double num10 = (double) ((Color) ref local3).get_Item(num8);
      Color rgb = Color.HSVToRGB((float) num4, (float) num7, (float) num10);
      ((Color) ref rgb).set_Item(num9, ((Color) ref self).get_Item(num9));
      return rgb;
    }

    public static Color HSVToRGB(this Color self, bool hdr)
    {
      int num1 = 0;
      ref Color local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      double num4 = (double) ((Color) ref local1).get_Item(num2);
      ref Color local2 = ref self;
      int num5 = num3;
      int num6 = num5 + 1;
      double num7 = (double) ((Color) ref local2).get_Item(num5);
      ref Color local3 = ref self;
      int num8 = num6;
      int num9 = num8 + 1;
      double num10 = (double) ((Color) ref local3).get_Item(num8);
      int num11 = hdr ? 1 : 0;
      Color rgb = Color.HSVToRGB((float) num4, (float) num7, (float) num10, num11 != 0);
      ((Color) ref rgb).set_Item(num9, ((Color) ref self).get_Item(num9));
      return rgb;
    }

    public static Color Get(this Color self, Color set, bool a = true, bool r = true, bool g = true, bool b = true)
    {
      return new Color(!r ? (float) self.r : (float) set.r, !g ? (float) self.g : (float) set.g, !b ? (float) self.b : (float) set.b, !a ? (float) self.a : (float) set.a);
    }
  }
}
