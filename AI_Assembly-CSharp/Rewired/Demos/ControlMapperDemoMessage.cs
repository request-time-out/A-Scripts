// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.ControlMapperDemoMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class ControlMapperDemoMessage : MonoBehaviour
  {
    public Rewired.UI.ControlMapper.ControlMapper controlMapper;
    public Selectable defaultSelectable;

    public ControlMapperDemoMessage()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (!Object.op_Inequality((Object) this.controlMapper, (Object) null))
        return;
      this.controlMapper.ScreenClosedEvent += new Action(this.OnControlMapperClosed);
      this.controlMapper.ScreenOpenedEvent += new Action(this.OnControlMapperOpened);
    }

    private void Start()
    {
      this.SelectDefault();
    }

    private void OnControlMapperClosed()
    {
      ((Component) this).get_gameObject().SetActive(true);
      this.StartCoroutine(this.SelectDefaultDeferred());
    }

    private void OnControlMapperOpened()
    {
      ((Component) this).get_gameObject().SetActive(false);
    }

    private void SelectDefault()
    {
      if (Object.op_Equality((Object) EventSystem.get_current(), (Object) null) || !Object.op_Inequality((Object) this.defaultSelectable, (Object) null))
        return;
      EventSystem.get_current().SetSelectedGameObject(((Component) this.defaultSelectable).get_gameObject());
    }

    [DebuggerHidden]
    private IEnumerator SelectDefaultDeferred()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ControlMapperDemoMessage.\u003CSelectDefaultDeferred\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
