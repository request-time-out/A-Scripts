// Decompiled with JetBrains decompiler
// Type: AIProject.IsFlavorGreater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsFlavorGreater : AgentConditional
  {
    [SerializeField]
    private float _borderValue = 50f;
    [SerializeField]
    private FlavorSkill.Type _key;
    [SerializeField]
    private bool _compareDowner;

    public virtual TaskStatus OnUpdate()
    {
      int num;
      if (!this.Agent.ChaControl.fileGameInfo.flavorState.TryGetValue((int) this._key, out num))
        return (TaskStatus) 1;
      return !this._compareDowner ? ((double) this._borderValue <= (double) num ? (TaskStatus) 2 : (TaskStatus) 1) : ((double) num <= (double) this._borderValue ? (TaskStatus) 2 : (TaskStatus) 1);
    }
  }
}
