// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.InvokeMethod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Invokes the specified method with the specified parameters. Can optionally store the return value. Returns success if the method was invoked.")]
  [TaskCategory("Reflection")]
  [TaskIcon("{SkinColor}ReflectionIcon.png")]
  public class InvokeMethod : Action
  {
    [Tooltip("The GameObject to invoke the method on")]
    public SharedGameObject targetGameObject;
    [Tooltip("The component to invoke the method on")]
    public SharedString componentName;
    [Tooltip("The name of the method")]
    public SharedString methodName;
    [Tooltip("The first parameter of the method")]
    public SharedVariable parameter1;
    [Tooltip("The second parameter of the method")]
    public SharedVariable parameter2;
    [Tooltip("The third parameter of the method")]
    public SharedVariable parameter3;
    [Tooltip("The fourth parameter of the method")]
    public SharedVariable parameter4;
    [Tooltip("Store the result of the invoke call")]
    public SharedVariable storeResult;

    public InvokeMethod()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.get_Value());
      if (typeWithinAssembly == (Type) null)
      {
        Debug.LogWarning((object) "Unable to invoke - type is null");
        return (TaskStatus) 1;
      }
      Component component = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).GetComponent(typeWithinAssembly);
      if (Object.op_Equality((Object) component, (Object) null))
      {
        Debug.LogWarning((object) ("Unable to invoke method with component " + this.componentName.get_Value()));
        return (TaskStatus) 1;
      }
      List<object> objectList = new List<object>();
      List<Type> typeList = new List<Type>();
      for (int index = 0; index < 4 && ((object) this).GetType().GetField("parameter" + (object) (index + 1)).GetValue((object) this) is SharedVariable sharedVariable; ++index)
      {
        objectList.Add(sharedVariable.GetValue());
        typeList.Add(((object) sharedVariable).GetType().GetProperty("Value").PropertyType);
      }
      MethodInfo method = ((object) component).GetType().GetMethod(this.methodName.get_Value(), typeList.ToArray());
      if (method == (MethodInfo) null)
      {
        Debug.LogWarning((object) ("Unable to invoke method " + this.methodName.get_Value() + " on component " + this.componentName.get_Value()));
        return (TaskStatus) 1;
      }
      object obj = method.Invoke((object) component, objectList.ToArray());
      if (this.storeResult != null)
        this.storeResult.SetValue(obj);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.componentName = (SharedString) null;
      this.methodName = (SharedString) null;
      this.parameter1 = (SharedVariable) null;
      this.parameter2 = (SharedVariable) null;
      this.parameter3 = (SharedVariable) null;
      this.parameter4 = (SharedVariable) null;
      this.storeResult = (SharedVariable) null;
    }
  }
}
