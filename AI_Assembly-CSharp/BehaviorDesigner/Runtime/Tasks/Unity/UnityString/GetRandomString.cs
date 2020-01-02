// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityString.GetRandomString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
  [TaskCategory("Unity/String")]
  [TaskDescription("Randomly selects a string from the array of strings.")]
  public class GetRandomString : Action
  {
    [Tooltip("The array of strings")]
    public SharedString[] source;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedString storeResult;

    public GetRandomString()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(this.source[Random.Range(0, this.source.Length)].get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.source = (SharedString[]) null;
      this.storeResult = (SharedString) null;
    }
  }
}
