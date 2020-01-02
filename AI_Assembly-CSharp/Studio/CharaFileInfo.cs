// Decompiled with JetBrains decompiler
// Type: Studio.CharaFileInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class CharaFileInfo
  {
    public string file = string.Empty;
    public string name = string.Empty;
    public int index = -1;
    public DateTime time;
    public Button button;

    public CharaFileInfo(string _file = "", string _name = "")
    {
      this.file = _file;
      this.name = _name;
    }

    public ListNode node { get; set; }

    public bool select
    {
      get
      {
        return this.node.select;
      }
      set
      {
        this.node.select = value;
        if (!Object.op_Implicit((Object) this.button))
          return;
        ((Graphic) ((Selectable) this.button).get_image()).set_color(!value ? Color.get_white() : Color.get_green());
      }
    }

    public int siblingIndex
    {
      set
      {
        ((Component) this.node).get_transform().SetSiblingIndex(value);
      }
    }
  }
}
