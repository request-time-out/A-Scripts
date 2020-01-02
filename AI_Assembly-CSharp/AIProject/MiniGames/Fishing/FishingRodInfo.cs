// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.FishingRodInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using UnityEngine;

namespace AIProject.MiniGames.Fishing
{
  public class FishingRodInfo
  {
    public FishingRodInfo(GameObject _rod, RuntimeAnimatorController _controller, string _tipName)
    {
      this.Rod = _rod;
      this.AnimController = _controller;
      this.TipName = _tipName;
    }

    public GameObject Rod { get; private set; }

    public RuntimeAnimatorController AnimController { get; private set; }

    public string TipName { get; private set; }

    public GameObject GetObj(string _name)
    {
      GameObject rod = this.Rod;
      return rod == null ? (GameObject) null : rod.get_transform().FindLoop(_name);
    }
  }
}
