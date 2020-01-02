// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPointComponentBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public abstract class ActionPointComponentBase : MonoBehaviour
  {
    protected ActionPoint _actionPoint;

    protected ActionPointComponentBase()
    {
      base.\u002Ector();
    }

    protected void Start()
    {
      this._actionPoint = (ActionPoint) ((Component) this).GetComponent<ActionPoint>();
      this.OnStart();
    }

    protected virtual void OnStart()
    {
    }
  }
}
