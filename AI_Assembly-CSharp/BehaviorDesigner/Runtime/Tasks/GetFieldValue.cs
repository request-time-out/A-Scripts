// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.GetFieldValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Gets the value from the field specified. Returns success if the field was retrieved.")]
  [TaskCategory("Reflection")]
  [TaskIcon("{SkinColor}ReflectionIcon.png")]
  public class GetFieldValue : Action
  {
    [Tooltip("The GameObject to get the field on")]
    public SharedGameObject targetGameObject;
    [Tooltip("The component to get the field on")]
    public SharedString componentName;
    [Tooltip("The name of the field")]
    public SharedString fieldName;
    [Tooltip("The value of the field")]
    [RequiredField]
    public SharedVariable fieldValue;

    public GetFieldValue()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.fieldValue == null)
      {
        Debug.LogWarning((object) "Unable to get field - field value is null");
        return (TaskStatus) 1;
      }
      Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.get_Value());
      if (typeWithinAssembly == (Type) null)
      {
        Debug.LogWarning((object) "Unable to get field - type is null");
        return (TaskStatus) 1;
      }
      Component component = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).GetComponent(typeWithinAssembly);
      if (Object.op_Equality((Object) component, (Object) null))
      {
        Debug.LogWarning((object) ("Unable to get the field with component " + this.componentName.get_Value()));
        return (TaskStatus) 1;
      }
      this.fieldValue.SetValue(((object) component).GetType().GetField(this.fieldName.get_Value()).GetValue((object) component));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.componentName = (SharedString) null;
      this.fieldName = (SharedString) null;
      this.fieldValue = (SharedVariable) null;
    }
  }
}
