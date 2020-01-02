// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.RestartBehaviorTree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Restarts a behavior tree, returns success after it has been restarted.")]
  [TaskIcon("{SkinColor}RestartBehaviorTreeIcon.png")]
  public class RestartBehaviorTree : Action
  {
    [Tooltip("The GameObject of the behavior tree that should be restarted. If null use the current behavior")]
    public SharedGameObject behaviorGameObject;
    [Tooltip("The group of the behavior tree that should be restarted")]
    public SharedInt group;
    private Behavior behavior;

    public RestartBehaviorTree()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      Behavior[] components = (Behavior[]) ((Task) this).GetDefaultGameObject(this.behaviorGameObject.get_Value()).GetComponents<Behavior>();
      if (components.Length == 1)
      {
        this.behavior = components[0];
      }
      else
      {
        if (components.Length <= 1)
          return;
        for (int index = 0; index < components.Length; ++index)
        {
          if (components[index].get_Group() == this.group.get_Value())
          {
            this.behavior = components[index];
            break;
          }
        }
        if (!Object.op_Equality((Object) this.behavior, (Object) null))
          return;
        this.behavior = components[0];
      }
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.behavior, (Object) null))
        return (TaskStatus) 1;
      this.behavior.DisableBehavior();
      this.behavior.EnableBehavior();
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.behavior = (Behavior) null;
    }
  }
}
