// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ActionIDCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace AIProject.UI
{
  [Serializable]
  public class ActionIDCommand : DownCommandDataBase
  {
    [SerializeField]
    private ActionID _actionID = ActionID.Submit;

    public ActionID ActionID
    {
      get
      {
        return this._actionID;
      }
      set
      {
        this._actionID = value;
      }
    }

    protected override bool IsInput(Input input)
    {
      return input.IsDown(this._actionID);
    }
  }
}
