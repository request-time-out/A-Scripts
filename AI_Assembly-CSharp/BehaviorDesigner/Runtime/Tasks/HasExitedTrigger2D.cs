﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.HasExitedTrigger2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Returns success when an object exits the 2D trigger. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
  [TaskCategory("Physics")]
  public class HasExitedTrigger2D : Conditional
  {
    [Tooltip("The tag of the GameObject to check for a trigger against")]
    public SharedString tag;
    [Tooltip("The object that exited the trigger")]
    public SharedGameObject otherGameObject;
    private bool exitedTrigger;

    public HasExitedTrigger2D()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.exitedTrigger ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnEnd()
    {
      this.exitedTrigger = false;
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
      if (!string.IsNullOrEmpty(this.tag.get_Value()) && !this.tag.get_Value().Equals(((Component) other).get_gameObject().get_tag()))
        return;
      this.otherGameObject.set_Value(((Component) other).get_gameObject());
      this.exitedTrigger = true;
    }

    public virtual void OnReset()
    {
      this.tag = (SharedString) string.Empty;
      this.otherGameObject = (SharedGameObject) null;
    }
  }
}
