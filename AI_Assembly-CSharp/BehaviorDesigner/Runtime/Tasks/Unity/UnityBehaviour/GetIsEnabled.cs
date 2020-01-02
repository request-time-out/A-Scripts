// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour.GetIsEnabled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour
{
  [TaskCategory("Unity/Behaviour")]
  [TaskDescription("Stores the enabled state of the object. Returns Success.")]
  public class GetIsEnabled : Action
  {
    [Tooltip("The Object to use")]
    public SharedObject specifiedObject;
    [Tooltip("The enabled/disabled state")]
    [RequiredField]
    public SharedBool storeValue;

    public GetIsEnabled()
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
      this.storeValue.set_Value((this.specifiedObject.get_Value() as Behaviour).get_enabled());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      if (this.specifiedObject != null)
        this.specifiedObject.set_Value((Object) null);
      this.storeValue = (SharedBool) false;
    }
  }
}
