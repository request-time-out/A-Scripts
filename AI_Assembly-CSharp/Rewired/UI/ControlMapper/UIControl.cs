// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class UIControl : MonoBehaviour
  {
    public Text title;
    private int _id;
    private bool _showTitle;
    private static int _uidCounter;

    public UIControl()
    {
      base.\u002Ector();
    }

    public int id
    {
      get
      {
        return this._id;
      }
    }

    private void Awake()
    {
      this._id = UIControl.GetNextUid();
    }

    public bool showTitle
    {
      get
      {
        return this._showTitle;
      }
      set
      {
        if (Object.op_Equality((Object) this.title, (Object) null))
          return;
        ((Component) this.title).get_gameObject().SetActive(value);
        this._showTitle = value;
      }
    }

    public virtual void SetCancelCallback(Action cancelCallback)
    {
    }

    private static int GetNextUid()
    {
      if (UIControl._uidCounter == int.MaxValue)
        UIControl._uidCounter = 0;
      int uidCounter = UIControl._uidCounter;
      ++UIControl._uidCounter;
      return uidCounter;
    }
  }
}
