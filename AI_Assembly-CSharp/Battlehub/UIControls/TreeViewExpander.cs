// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.TreeViewExpander
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  [RequireComponent(typeof (Toggle))]
  public class TreeViewExpander : MonoBehaviour
  {
    public Graphic OffGraphic;
    private Toggle m_toggle;
    private bool m_canExpand;
    private bool m_started;

    public TreeViewExpander()
    {
      base.\u002Ector();
    }

    public bool CanExpand
    {
      get
      {
        return this.m_canExpand;
      }
      set
      {
        this.m_canExpand = value;
        this.UpdateState();
      }
    }

    public bool IsOn
    {
      get
      {
        return this.m_toggle.get_isOn();
      }
      set
      {
        this.m_toggle.set_isOn(value);
      }
    }

    private void UpdateState()
    {
      if (this.m_started)
        this.DoUpdateState();
      else
        this.StartCoroutine(this.CoUpdateState());
    }

    [DebuggerHidden]
    private IEnumerator CoUpdateState()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TreeViewExpander.\u003CCoUpdateState\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void DoUpdateState()
    {
      if (this.CanExpand)
      {
        ((Selectable) this.m_toggle).set_interactable(true);
        if (this.IsOn)
        {
          if (!Object.op_Inequality((Object) this.OffGraphic, (Object) null))
            return;
          ((Behaviour) this.OffGraphic).set_enabled(false);
        }
        else
        {
          if (!Object.op_Inequality((Object) this.OffGraphic, (Object) null))
            return;
          ((Behaviour) this.OffGraphic).set_enabled(true);
        }
      }
      else
      {
        if (Object.op_Inequality((Object) this.m_toggle, (Object) null))
          ((Selectable) this.m_toggle).set_interactable(false);
        if (!Object.op_Inequality((Object) this.OffGraphic, (Object) null))
          return;
        ((Behaviour) this.OffGraphic).set_enabled(false);
      }
    }

    private void Awake()
    {
      this.m_toggle = (Toggle) ((Component) this).GetComponent<Toggle>();
      if (Object.op_Inequality((Object) this.OffGraphic, (Object) null))
        ((Behaviour) this.OffGraphic).set_enabled(false);
      this.UpdateState();
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.m_toggle.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChanged)));
    }

    private void Start()
    {
      this.m_started = true;
    }

    private void OnEnable()
    {
      if (!Object.op_Inequality((Object) this.m_toggle, (Object) null))
        return;
      this.UpdateState();
    }

    private void OnDestroy()
    {
      if (!Object.op_Inequality((Object) this.m_toggle, (Object) null))
        return;
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.m_toggle.onValueChanged).RemoveListener(new UnityAction<bool>((object) this, __methodptr(OnValueChanged)));
    }

    private void OnValueChanged(bool value)
    {
      this.UpdateState();
    }
  }
}
