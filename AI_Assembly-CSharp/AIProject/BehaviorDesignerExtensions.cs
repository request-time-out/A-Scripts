// Decompiled with JetBrains decompiler
// Type: AIProject.BehaviorDesignerExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime;
using UnityEngine;

namespace AIProject
{
  public static class BehaviorDesignerExtensions
  {
    public static T GetVariable<T>(this Behavior behavior, string name) where T : SharedVariable, new()
    {
      return Object.op_Inequality((Object) behavior, (Object) null) ? behavior.GetVariable(name) as T : (T) null;
    }
  }
}
