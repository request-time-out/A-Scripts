// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PressedCommandDataBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject.UI
{
  [Serializable]
  public abstract class PressedCommandDataBase : CommandDataBase
  {
    [SerializeField]
    private UnityEvent _triggerEvent = new UnityEvent();

    public UnityEvent TriggerEvent
    {
      get
      {
        return this._triggerEvent;
      }
      set
      {
        this._triggerEvent = value;
      }
    }

    protected override void OnInvoke(Input input)
    {
      if (!this.IsInput(input))
        return;
      this._triggerEvent.Invoke();
    }
  }
}
