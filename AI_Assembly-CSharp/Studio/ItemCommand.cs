// Decompiled with JetBrains decompiler
// Type: Studio.ItemCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public static class ItemCommand
  {
    public class ColorInfo
    {
      public ColorInfo(int _dicKey, Color _oldValue, Color _newValue)
      {
        this.dicKey = _dicKey;
        this.oldValue = _oldValue;
        this.newValue = _newValue;
      }

      public int dicKey { get; protected set; }

      public Color oldValue { get; protected set; }

      public Color newValue { get; protected set; }
    }
  }
}
