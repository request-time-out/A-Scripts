// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Resources.AnimalPlayState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEx;

namespace AIProject.Animal.Resources
{
  public class AnimalPlayState
  {
    public float[] FloatList = new float[0];

    public AnimalPlayState(int _layer, string[] _inStateNames, string[] _outStateNames)
    {
      this.Layer = _layer;
      if (!((IReadOnlyList<string>) _inStateNames).IsNullOrEmpty<string>())
      {
        this.MainStateInfo.InStateInfos = new AnimalPlayState.StateInfo[_inStateNames.Length];
        for (int index = 0; index < _inStateNames.Length; ++index)
          this.MainStateInfo.InStateInfos[index] = new AnimalPlayState.StateInfo(_inStateNames[index], _layer);
      }
      if (((IReadOnlyList<string>) _outStateNames).IsNullOrEmpty<string>())
        return;
      this.MainStateInfo.OutStateInfos = new AnimalPlayState.StateInfo[_outStateNames.Length];
      for (int index = 0; index < _outStateNames.Length; ++index)
        this.MainStateInfo.OutStateInfos[index] = new AnimalPlayState.StateInfo(_outStateNames[index], _layer);
    }

    public AnimalPlayState(
      int _layer,
      int _stateID,
      string[] _inStateNames,
      string[] _outStateNames)
    {
      this.Layer = _layer;
      this.StateID = _stateID;
      if (!((IReadOnlyList<string>) _inStateNames).IsNullOrEmpty<string>())
      {
        this.MainStateInfo.InStateInfos = new AnimalPlayState.StateInfo[_inStateNames.Length];
        for (int index = 0; index < _inStateNames.Length; ++index)
          this.MainStateInfo.InStateInfos[index] = new AnimalPlayState.StateInfo(_inStateNames[index], _layer);
      }
      if (((IReadOnlyList<string>) _outStateNames).IsNullOrEmpty<string>())
        return;
      this.MainStateInfo.OutStateInfos = new AnimalPlayState.StateInfo[_outStateNames.Length];
      for (int index = 0; index < _outStateNames.Length; ++index)
        this.MainStateInfo.OutStateInfos[index] = new AnimalPlayState.StateInfo(_outStateNames[index], _layer);
    }

    public int StateID { get; set; }

    public int Layer { get; set; }

    public AnimalPlayState.PlayStateInfo MainStateInfo { get; private set; } = new AnimalPlayState.PlayStateInfo();

    public List<AnimalPlayState.PlayStateInfo> SubStateInfos { get; private set; } = new List<AnimalPlayState.PlayStateInfo>();

    public void RemakeAnimator()
    {
      this.MainStateInfo?.RemakeAnimator();
      if (((IReadOnlyList<AnimalPlayState.PlayStateInfo>) this.SubStateInfos).IsNullOrEmpty<AnimalPlayState.PlayStateInfo>())
        return;
      foreach (AnimalPlayState.PlayStateInfo subStateInfo in this.SubStateInfos)
        subStateInfo?.RemakeAnimator();
    }

    public class PlayStateInfo
    {
      public AssetBundleInfo AssetBundleInfo { get; set; }

      public RuntimeAnimatorController Controller { get; set; }

      public AnimalPlayState.StateInfo[] InStateInfos { get; set; }

      public AnimalPlayState.StateInfo[] OutStateInfos { get; set; }

      public bool ActiveInState
      {
        get
        {
          return !((IReadOnlyList<AnimalPlayState.StateInfo>) this.InStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>() && Object.op_Inequality((Object) this.Controller, (Object) null);
        }
      }

      public bool ActiveOutState
      {
        get
        {
          return !((IReadOnlyList<AnimalPlayState.StateInfo>) this.OutStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>() && Object.op_Inequality((Object) this.Controller, (Object) null);
        }
      }

      public bool InFadeEnable { get; set; }

      public float InFadeSecond { get; set; } = 0.1f;

      public bool OutFadeEnable { get; set; }

      public float OutFadeSecond { get; set; } = 0.1f;

      public bool IsLoop { get; set; }

      public int LoopMin { get; set; }

      public int LoopMax { get; set; }

      public void RemakeAnimator()
      {
        if (!Object.op_Implicit((Object) this.Controller))
          ;
      }
    }

    public struct StateInfo
    {
      public int layer;
      public string animName;
      public int animCode;

      public StateInfo(string _animName, int _layer)
      {
        this.animName = _animName;
        this.layer = _layer;
        this.animCode = Animator.StringToHash(_animName);
      }
    }

    public struct ItemInfo
    {
      public string parentName;
      public AssetBundleInfo itemABInfo;
      public AssetBundleInfo animatorABInfo;
      public bool isSync;
    }
  }
}
