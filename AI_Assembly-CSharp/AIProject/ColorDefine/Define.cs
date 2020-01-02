// Decompiled with JetBrains decompiler
// Type: AIProject.ColorDefine.Define
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace AIProject.ColorDefine
{
  public static class Define
  {
    private static readonly Dictionary<Colors, Color> _colorDefine;

    public static Color Get(Colors colors)
    {
      return Define._colorDefine[colors];
    }

    public static void Set(ref Color color, Colors colors, bool setAlpha = false)
    {
      float a = (float) color.a;
      color = Define._colorDefine[colors];
      if (!setAlpha)
        return;
      color.a = (__Null) (double) a;
    }

    static Define()
    {
      Dictionary<Colors, Color> dictionary = new Dictionary<Colors, Color>();
      dictionary.Add(Colors.White, Color32.op_Implicit(new Color32((byte) 235, (byte) 226, (byte) 215, byte.MaxValue)));
      dictionary.Add(Colors.LightGreen, Color32.op_Implicit(new Color32((byte) 133, (byte) 214, (byte) 83, byte.MaxValue)));
      dictionary.Add(Colors.Green, Color32.op_Implicit(new Color32((byte) 100, (byte) 185, (byte) 22, byte.MaxValue)));
      dictionary.Add(Colors.Yellow, Color32.op_Implicit(new Color32((byte) 204, (byte) 197, (byte) 59, byte.MaxValue)));
      dictionary.Add(Colors.Blue, Color32.op_Implicit(new Color32((byte) 0, (byte) 183, (byte) 238, byte.MaxValue)));
      dictionary.Add(Colors.Cian, Color32.op_Implicit(new Color32((byte) 98, (byte) 215, (byte) 245, byte.MaxValue)));
      dictionary.Add(Colors.DarkRed, Color32.op_Implicit(new Color32((byte) 198, (byte) 69, (byte) 73, byte.MaxValue)));
      dictionary.Add(Colors.Red, Color32.op_Implicit(new Color32((byte) 222, (byte) 69, (byte) 41, byte.MaxValue)));
      dictionary.Add(Colors.Black, Color32.op_Implicit(new Color32((byte) 23, (byte) 30, (byte) 36, byte.MaxValue)));
      dictionary.Add(Colors.DarkBlack, Color32.op_Implicit(new Color32((byte) 3, (byte) 4, (byte) 5, byte.MaxValue)));
      dictionary.Add(Colors.Orange, Color32.op_Implicit(new Color32((byte) 237, (byte) 122, (byte) 35, byte.MaxValue)));
      Define._colorDefine = dictionary;
    }
  }
}
