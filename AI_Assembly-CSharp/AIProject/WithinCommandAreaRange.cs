// Decompiled with JetBrains decompiler
// Type: AIProject.WithinCommandAreaRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class WithinCommandAreaRange : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      CommandArea commandArea = player.PlayerController.CommandArea;
      Vector3 offset = Vector3.get_zero();
      if (Object.op_Inequality((Object) commandArea.BaseTransform, (Object) null))
      {
        Vector3 eulerAngles = commandArea.BaseTransform.get_eulerAngles();
        eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
        offset = Quaternion.op_Multiply(Quaternion.Euler(eulerAngles), commandArea.Offset);
      }
      return commandArea.WithinRange(player.NavMeshAgent, (ICommandable) this.Agent, offset) ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
