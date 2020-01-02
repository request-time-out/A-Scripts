// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.ComparePropertyValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Compares the property value to the value specified. Returns success if the values are the same.")]
  [TaskCategory("Reflection")]
  [TaskIcon("{SkinColor}ReflectionIcon.png")]
  public class ComparePropertyValue : Conditional
  {
    [Tooltip("The GameObject to compare the property of")]
    public SharedGameObject targetGameObject;
    [Tooltip("The component to compare the property of")]
    public SharedString componentName;
    [Tooltip("The name of the property")]
    public SharedString propertyName;
    [Tooltip("The value to compare to")]
    public SharedVariable compareValue;

    public ComparePropertyValue()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.compareValue == null)
      {
        Debug.LogWarning((object) "Unable to compare field - compare value is null");
        return (TaskStatus) 1;
      }
      Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.get_Value());
      if (typeWithinAssembly == (Type) null)
      {
        Debug.LogWarning((object) "Unable to compare property - type is null");
        return (TaskStatus) 1;
      }
      Component component = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).GetComponent(typeWithinAssembly);
      if (Object.op_Equality((Object) component, (Object) null))
      {
        Debug.LogWarning((object) ("Unable to compare the property with component " + this.componentName.get_Value()));
        return (TaskStatus) 1;
      }
      object obj = ((object) component).GetType().GetProperty(this.propertyName.get_Value()).GetValue((object) component, (object[]) null);
      if (obj == null && this.compareValue.GetValue() == null)
        return (TaskStatus) 2;
      return obj.Equals(this.compareValue.GetValue()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.componentName = (SharedString) null;
      this.propertyName = (SharedString) null;
      this.compareValue = (SharedVariable) null;
    }
  }
}
