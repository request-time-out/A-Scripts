// Decompiled with JetBrains decompiler
// Type: AIProject.RollTheDice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class RollTheDice : AgentConditional
  {
    [SerializeField]
    private SharedFloat _border = (SharedFloat) 100f;

    public virtual TaskStatus OnUpdate()
    {
      return (double) Random.Range(0.0f, 100f) <= (double) this._border.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
