// Decompiled with JetBrains decompiler
// Type: AIProject.IsPlayerAccessableYobai
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsPlayerAccessableYobai : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      if (Singleton<Game>.IsInstance() && Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null))
        return (TaskStatus) 1;
      if (Singleton<Manager.Map>.IsInstance() && Singleton<Manager.Map>.Instance.IsWarpProc)
        return (TaskStatus) 1;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (player.ProcessingTimeSkip)
        return (TaskStatus) 1;
      return player.Controller.State is Lie ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
