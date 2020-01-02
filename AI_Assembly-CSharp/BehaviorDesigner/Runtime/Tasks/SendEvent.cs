// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.SendEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Sends an event to the behavior tree, returns success after sending the event.")]
  [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/events/")]
  [TaskIcon("{SkinColor}SendEventIcon.png")]
  public class SendEvent : Action
  {
    [Tooltip("The GameObject of the behavior tree that should have the event sent to it. If null use the current behavior")]
    public SharedGameObject targetGameObject;
    [Tooltip("The event to send")]
    public SharedString eventName;
    [Tooltip("The group of the behavior tree that the event should be sent to")]
    public SharedInt group;
    [Tooltip("Optionally specify a first argument to send")]
    [SharedRequired]
    public SharedVariable argument1;
    [Tooltip("Optionally specify a second argument to send")]
    [SharedRequired]
    public SharedVariable argument2;
    [Tooltip("Optionally specify a third argument to send")]
    [SharedRequired]
    public SharedVariable argument3;
    private BehaviorTree behaviorTree;

    public SendEvent()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      BehaviorTree[] components = (BehaviorTree[]) ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).GetComponents<BehaviorTree>();
      if (components.Length == 1)
      {
        this.behaviorTree = components[0];
      }
      else
      {
        if (components.Length <= 1)
          return;
        for (int index = 0; index < components.Length; ++index)
        {
          if (components[index].get_Group() == this.group.get_Value())
          {
            this.behaviorTree = components[index];
            break;
          }
        }
        if (!Object.op_Equality((Object) this.behaviorTree, (Object) null))
          return;
        this.behaviorTree = components[0];
      }
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.argument1 == null || this.argument1.get_IsNone())
        this.behaviorTree.SendEvent(this.eventName.get_Value());
      else if (this.argument2 == null || this.argument2.get_IsNone())
        this.behaviorTree.SendEvent<object>(this.eventName.get_Value(), (M0) this.argument1.GetValue());
      else if (this.argument3 == null || this.argument3.get_IsNone())
        this.behaviorTree.SendEvent<object, object>(this.eventName.get_Value(), (M0) this.argument1.GetValue(), (M1) this.argument2.GetValue());
      else
        this.behaviorTree.SendEvent<object, object, object>(this.eventName.get_Value(), (M0) this.argument1.GetValue(), (M1) this.argument2.GetValue(), (M2) this.argument3.GetValue());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.eventName = (SharedString) string.Empty;
    }
  }
}
