// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ItemDataBindingArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Battlehub.UIControls
{
  public class ItemDataBindingArgs : EventArgs
  {
    private bool m_canEdit = true;
    private bool m_canDrag = true;
    private bool m_canDrop = true;

    public object Item { get; set; }

    public GameObject ItemPresenter { get; set; }

    public GameObject EditorPresenter { get; set; }

    public bool CanEdit
    {
      get
      {
        return this.m_canEdit;
      }
      set
      {
        this.m_canEdit = value;
      }
    }

    public bool CanDrag
    {
      get
      {
        return this.m_canDrag;
      }
      set
      {
        this.m_canDrag = value;
      }
    }

    public bool CanBeParent
    {
      get
      {
        return this.m_canDrop;
      }
      set
      {
        this.m_canDrop = value;
      }
    }
  }
}
