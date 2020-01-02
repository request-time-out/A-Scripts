// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour.IsEnabled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour
{
  [TaskCategory("Unity/Behaviour")]
  [TaskDescription("Returns Success if the object is enabled, otherwise Failure.")]
  public class IsEnabled : Conditional
  {
    [Tooltip("The Object to use")]
    public SharedObject specifiedObject;

    public IsEnabled()
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
      return (this.specifiedObject.get_Value() as Behaviour).get_enabled() ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      if (this.specifiedObject == null)
        return;
      this.specifiedObject.set_Value((Object) null);
    }
  }
}
