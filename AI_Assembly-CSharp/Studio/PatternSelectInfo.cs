// Decompiled with JetBrains decompiler
// Type: Studio.PatternSelectInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class PatternSelectInfo
  {
    public int index = -1;
    public string name = string.Empty;
    public string assetBundle = string.Empty;
    public string assetName = string.Empty;
    public int category;
    public bool disable;
    public bool disvisible;
    public PatternSelectInfoComponent sic;

    public bool activeSelf
    {
      get
      {
        return ((Component) this.sic).get_gameObject().get_activeSelf();
      }
    }

    public bool interactable
    {
      get
      {
        return ((Selectable) this.sic.tgl).get_interactable();
      }
    }

    public bool isOn
    {
      get
      {
        return this.sic.tgl.get_isOn();
      }
    }
  }
}
