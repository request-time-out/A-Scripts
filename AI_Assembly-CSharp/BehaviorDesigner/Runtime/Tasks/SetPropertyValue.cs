// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.SetPropertyValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Sets the property to the value specified. Returns success if the property was set.")]
  [TaskCategory("Reflection")]
  [TaskIcon("{SkinColor}ReflectionIcon.png")]
  public class SetPropertyValue : Action
  {
    [Tooltip("The GameObject to set the property on")]
    public SharedGameObject targetGameObject;
    [Tooltip("The component to set the property on")]
    public SharedString componentName;
    [Tooltip("The name of the property")]
    public SharedString propertyName;
    [Tooltip("The value to set")]
    public SharedVariable propertyValue;

    public SetPropertyValue()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.propertyValue == null)
      {
        Debug.LogWarning((object) "Unable to get field - field value is null");
        return (TaskStatus) 1;
      }
      Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.get_Value());
      if (typeWithinAssembly == (Type) null)
      {
        Debug.LogWarning((object) "Unable to set property - type is null");
        return (TaskStatus) 1;
      }
      Component component = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).GetComponent(typeWithinAssembly);
      if (Object.op_Equality((Object) component, (Object) null))
      {
        Debug.LogWarning((object) ("Unable to set the property with component " + this.componentName.get_Value()));
        return (TaskStatus) 1;
      }
      ((object) component).GetType().GetProperty(this.propertyName.get_Value()).SetValue((object) component, this.propertyValue.GetValue(), (object[]) null);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.componentName = (SharedString) null;
      this.propertyName = (SharedString) null;
      this.propertyValue = (SharedVariable) null;
    }
  }
}
