// Decompiled with JetBrains decompiler
// Type: AIProject.AreaLayerMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace AIProject
{
  public struct AreaLayerMask
  {
    private int _value;

    public int Value
    {
      get
      {
        return this._value;
      }
      set
      {
        this._value = value;
      }
    }

    public static implicit operator int(AreaLayerMask layerMask)
    {
      return layerMask.Value;
    }

    public static implicit operator AreaLayerMask(int value)
    {
      return new AreaLayerMask() { Value = value };
    }

    public static string LayerToName(int layer)
    {
      return string.Empty;
    }

    public static int NameToLayer(string name)
    {
      return 0;
    }

    public static int GetMask(params string[] names)
    {
      if (names == null)
        throw new ArgumentNullException(nameof (names));
      int num = 0;
      for (int index = 0; index < names.Length; ++index)
      {
        int layer = AreaLayerMask.NameToLayer(names[index]);
        if (layer != -1)
          num |= 1 << layer;
      }
      return num;
    }
  }
}
