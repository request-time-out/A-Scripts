// Decompiled with JetBrains decompiler
// Type: AIProject.AreaLayerDefineAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  public class AreaLayerDefineAsset : ScriptableObject
  {
    [SerializeField]
    private string[] _values;

    public AreaLayerDefineAsset()
    {
      base.\u002Ector();
      this._values = new string[32];
      this._values[0] = "Normal";
    }

    public string GetLayerName(int layerID)
    {
      if (layerID >= 0 || layerID <= 31)
        return this._values[layerID];
      throw new ArgumentOutOfRangeException();
    }

    public int GetLayerByName(string name)
    {
      for (int index = 0; index < this._values.Length; ++index)
      {
        if (this._values[index] == name)
          return index;
      }
      return -1;
    }
  }
}
