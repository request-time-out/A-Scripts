// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour.SetIsEnabled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour
{
  [TaskCategory("Unity/Behaviour")]
  [TaskDescription("Enables/Disables the object. Returns Success.")]
  public class SetIsEnabled : Action
  {
    [Tooltip("The Object to use")]
    public SharedObject specifiedObject;
    [Tooltip("The enabled/disabled state")]
    public SharedBool enabled;

    public SetIsEnabled()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.specifiedObject == null && !(this.specifiedObject.get_Value() is Behaviour))
      {
        Debug.LogWarning((object) "SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
        return (TaskStatus) 1;
      }
      (this.specifiedObject.get_Value() as Behaviour).set_enabled(this.enabled.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      if (this.specifiedObject != null)
        this.specifiedObject.set_Value((Object) null);
      this.enabled = (SharedBool) false;
    }
  }
}
