// Decompiled with JetBrains decompiler
// Type: Studio.ChangeFocus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class ChangeFocus : MonoBehaviour
  {
    [SerializeField]
    private Selectable[] selectable;
    private int m_Select;

    public ChangeFocus()
    {
      base.\u002Ector();
    }

    public int select
    {
      get
      {
        return this.m_Select;
      }
      set
      {
        this.m_Select = value;
        ((Behaviour) this).set_enabled(this.m_Select != -1);
      }
    }

    private void ChangeTarget()
    {
      if (Input.GetKey((KeyCode) 304) | Input.GetKey((KeyCode) 303))
      {
        --this.m_Select;
        if (this.m_Select < 0)
          this.m_Select = this.selectable.Length - 1;
      }
      else
        this.m_Select = (this.m_Select + 1) % this.selectable.Length;
      if (!Object.op_Implicit((Object) this.selectable[this.m_Select]))
        return;
      this.selectable[this.m_Select].Select();
    }

    private void Start()
    {
      this.select = -1;
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_enabled())), (Func<M0, bool>) (_ => this.select != -1)), (Func<M0, bool>) (_ => Input.GetKeyDown((KeyCode) 9))), (Action<M0>) (_ => this.ChangeTarget()));
    }
  }
}
