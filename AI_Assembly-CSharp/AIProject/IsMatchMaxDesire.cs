// Decompiled with JetBrains decompiler
// Type: AIProject.IsMatchMaxDesire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsMatchMaxDesire : AgentConditional
  {
    [SerializeField]
    private Desire.Type _desire;
    [SerializeField]
    private bool _withPlayer;

    public virtual TaskStatus OnUpdate()
    {
      float num1 = 0.0f;
      Desire.Type type = Desire.WithPlayerDesireTable[0];
      if (this._withPlayer)
      {
        foreach (Desire.Type key in Desire.WithPlayerDesireTable)
        {
          float num2;
          if (this.Agent.AgentData.DesireTable.TryGetValue(Desire.GetDesireKey(key), out num2) && (double) num1 < (double) num2)
          {
            num1 = num2;
            type = key;
          }
        }
      }
      else
      {
        foreach (KeyValuePair<int, float> keyValuePair in this.Agent.AgentData.DesireTable)
        {
          if ((double) num1 < (double) keyValuePair.Value)
          {
            num1 = keyValuePair.Value;
            type = (Desire.Type) keyValuePair.Key;
          }
        }
      }
      if (type == Desire.Type.None)
        return (TaskStatus) 1;
      return type == this._desire ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
