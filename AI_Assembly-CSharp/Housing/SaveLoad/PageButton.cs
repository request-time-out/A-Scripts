// Decompiled with JetBrains decompiler
// Type: Housing.SaveLoad.PageButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using SuperScrollView;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Housing.SaveLoad
{
  public class PageButton : LoopListViewItem2
  {
    [SerializeField]
    private Text text;
    [SerializeField]
    private Toggle toggle;
    private Action<int> onClick;
    private int index;

    public void SetData(int _index, bool _select, Action<int> _onClick)
    {
      this.index = _index;
      this.onClick = _onClick;
      this.text.set_text(string.Format("{0}", (object) _index));
      this.toggle.set_isOn(_select);
    }

    public void Select()
    {
      this.toggle.set_isOn(true);
    }

    public void Deselect()
    {
      this.toggle.set_isOn(false);
    }

    public void OnClick()
    {
      this.toggle.set_isOn(true);
      if (this.onClick == null)
        return;
      this.onClick(this.index);
    }
  }
}
