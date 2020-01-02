// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.ChangeParamState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public struct ChangeParamState
  {
    public ChangeType changeType;
    public float minRange;
    public float maxRange;

    public ChangeParamState(ChangeType _changeType, float _minRange, float _maxRange)
    {
      this.changeType = _changeType;
      this.minRange = Mathf.Min(_minRange, _maxRange);
      this.maxRange = Mathf.Max(_minRange, _maxRange);
    }

    public float RandomValue
    {
      get
      {
        return Random.Range(this.minRange, this.maxRange);
      }
    }
  }
}
