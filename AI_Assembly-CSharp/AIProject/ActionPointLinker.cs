// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPointLinker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (ActionPoint))]
  public class ActionPointLinker : ActionPointComponentBase
  {
    [SerializeField]
    private ActionPoint _connectedPoint;

    public ActionPoint ConnectedPoint
    {
      get
      {
        return this._connectedPoint;
      }
    }

    protected override void OnStart()
    {
      if (!Object.op_Inequality((Object) this._connectedPoint, (Object) null))
        return;
      this._actionPoint.ConnectedActionPoints.Add(this._connectedPoint);
    }
  }
}
