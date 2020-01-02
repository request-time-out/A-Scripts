// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.GetPropertyValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Gets the value from the property specified. Returns success if the property was retrieved.")]
  [TaskCategory("Reflection")]
  [TaskIcon("{SkinColor}ReflectionIcon.png")]
  public class GetPropertyValue : Action
  {
    [Tooltip("The GameObject to get the property of")]
    public SharedGameObject targetGameObject;
    [Tooltip("The component to get the property of")]
    public SharedString componentName;
    [Tooltip("The name of the property")]
    public SharedString propertyName;
    [Tooltip("The value of the property")]
    [RequiredField]
    public SharedVariable propertyValue;

    public GetPropertyValue()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.propertyValue == null)
      {
        Debug.LogWarning((object) "Unable to get property - property value is null");
        return (TaskStatus) 1;
      }
      Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.get_Value());
      if (typeWithinAssembly == (Type) null)
      {
        Debug.LogWarning((object) "Unable to get property - type is null");
        return (TaskStatus) 1;
      }
      Component component = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).GetComponent(typeWithinAssembly);
      if (Object.op_Equality((Object) component, (Object) null))
      {
        Debug.LogWarning((object) ("Unable to get the property with component " + this.componentName.get_Value()));
        return (TaskStatus) 1;
      }
      this.propertyValue.SetValue(((object) component).GetType().GetProperty(this.propertyName.get_Value()).GetValue((object) component, (object[]) null));
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
