// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.StartBehaviorTree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Start a new behavior tree and return success after it has been started.")]
  [TaskIcon("{SkinColor}StartBehaviorTreeIcon.png")]
  public class StartBehaviorTree : Action
  {
    [Tooltip("The GameObject of the behavior tree that should be started. If null use the current behavior")]
    public SharedGameObject behaviorGameObject;
    [Tooltip("The group of the behavior tree that should be started")]
    public SharedInt group;
    [Tooltip("Should this task wait for the behavior tree to complete?")]
    public SharedBool waitForCompletion;
    [Tooltip("Should the variables be synchronized?")]
    public SharedBool synchronizeVariables;
    private bool behaviorComplete;
    private Behavior behavior;

    public StartBehaviorTree()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      Behavior[] components = (Behavior[]) ((Task) this).GetDefaultGameObject(this.behaviorGameObject.get_Value()).GetComponents<Behavior>();
      if (components.Length == 1)
        this.behavior = components[0];
      else if (components.Length > 1)
      {
        for (int index = 0; index < components.Length; ++index)
        {
          if (components[index].get_Group() == this.group.get_Value())
          {
            this.behavior = components[index];
            break;
          }
        }
        if (Object.op_Equality((Object) this.behavior, (Object) null))
          this.behavior = components[0];
      }
      if (!Object.op_Inequality((Object) this.behavior, (Object) null))
        return;
      List<SharedVariable> allVariables = ((Task) this).get_Owner().GetAllVariables();
      if (allVariables != null && this.synchronizeVariables.get_Value())
      {
        for (int index = 0; index < allVariables.Count; ++index)
          this.behavior.SetVariableValue(allVariables[index].get_Name(), (object) allVariables[index]);
      }
      this.behavior.EnableBehavior();
      if (!this.waitForCompletion.get_Value())
        return;
      this.behaviorComplete = false;
      // ISSUE: method pointer
      this.behavior.add_OnBehaviorEnd(new Behavior.BehaviorHandler((object) this, __methodptr(BehaviorEnded)));
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.behavior, (Object) null))
        return (TaskStatus) 1;
      return this.waitForCompletion.get_Value() && !this.behaviorComplete ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    private void BehaviorEnded(Behavior behavior)
    {
      this.behaviorComplete = true;
    }

    public virtual void OnEnd()
    {
      if (!Object.op_Inequality((Object) this.behavior, (Object) null) || !this.waitForCompletion.get_Value())
        return;
      // ISSUE: method pointer
      this.behavior.remove_OnBehaviorEnd(new Behavior.BehaviorHandler((object) this, __methodptr(BehaviorEnded)));
    }

    public virtual void OnReset()
    {
      this.behaviorGameObject = (SharedGameObject) null;
      this.group = (SharedInt) 0;
      this.waitForCompletion = (SharedBool) false;
      this.synchronizeVariables = (SharedBool) false;
    }
  }
}
