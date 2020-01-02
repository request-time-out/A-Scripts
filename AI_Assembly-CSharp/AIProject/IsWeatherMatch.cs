// Decompiled with JetBrains decompiler
// Type: AIProject.IsWeatherMatch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class IsWeatherMatch : Conditional
  {
    [SerializeField]
    private Weather _weather;

    public IsWeatherMatch()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return this._weather == Singleton<Manager.Map>.Instance.Simulator.Weather ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
