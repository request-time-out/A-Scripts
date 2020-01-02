// Decompiled with JetBrains decompiler
// Type: AIProject.UI.DownCommandDataBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace AIProject.UI
{
  [Serializable]
  public abstract class DownCommandDataBase : CommandDataBase
  {
    [SerializeField]
    private OnDownInputEvent _triggerEvent = new OnDownInputEvent();

    public OnDownInputEvent TriggerEvent
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
      this._triggerEvent.Invoke(this.IsInput(input));
    }
  }
}
