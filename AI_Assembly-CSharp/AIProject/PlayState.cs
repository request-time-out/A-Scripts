// Decompiled with JetBrains decompiler
// Type: AIProject.PlayState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [Serializable]
  public class PlayState
  {
    private List<PlayState.ItemInfo> _itemInfos = new List<PlayState.ItemInfo>();

    public PlayState()
    {
    }

    public PlayState(int layer, string[] inStateNames, string[] outStateNames)
    {
      this.Layer = layer;
      this.MainStateInfo.InStateInfo = new PlayState.AnimStateInfo();
      if (!inStateNames.IsNullOrEmpty<string>())
      {
        PlayState.Info[] infoArray1 = new PlayState.Info[inStateNames.Length];
        this.MainStateInfo.InStateInfo.StateInfos = infoArray1;
        PlayState.Info[] infoArray2 = infoArray1;
        for (int index = 0; index < infoArray2.Length; ++index)
          infoArray2[index] = new PlayState.Info(inStateNames[index], layer);
      }
      this.MainStateInfo.OutStateInfo = new PlayState.AnimStateInfo();
      if (outStateNames.IsNullOrEmpty<string>())
        return;
      PlayState.Info[] infoArray3 = new PlayState.Info[outStateNames.Length];
      this.MainStateInfo.OutStateInfo.StateInfos = infoArray3;
      PlayState.Info[] infoArray4 = infoArray3;
      for (int index = 0; index < infoArray4.Length; ++index)
        infoArray4[index] = new PlayState.Info(outStateNames[index], layer);
    }

    public int Layer { get; set; }

    public int DirectionType { get; set; }

    public PlayState.PlayStateInfo MainStateInfo { get; private set; } = new PlayState.PlayStateInfo();

    public List<PlayState.PlayStateInfo> SubStateInfos { get; private set; } = new List<PlayState.PlayStateInfo>();

    public ActionInfo ActionInfo { get; set; }

    public PlayState.Info MaskStateInfo { get; set; }

    public PlayState.ItemInfo GetItemInfo(int index)
    {
      PlayState.ItemInfo? nullable = this._itemInfos != null ? new PlayState.ItemInfo?(this._itemInfos.GetElement<PlayState.ItemInfo>(index)) : new PlayState.ItemInfo?();
      return nullable.HasValue ? nullable.Value : new PlayState.ItemInfo();
    }

    public int ItemInfoCount
    {
      get
      {
        int? nullable = this._itemInfos != null ? new int?(this._itemInfos.Count) : new int?();
        return nullable.HasValue ? nullable.Value : 0;
      }
    }

    public bool EndEnableBlend { get; set; }

    public float EndBlendRate { get; set; }

    public bool UseNeckLook { get; set; }

    ~PlayState()
    {
    }

    public void AddItemInfo(PlayState.ItemInfo itemInfo)
    {
      this._itemInfos.Add(itemInfo);
    }

    [Serializable]
    public class PlayStateInfo
    {
      public AssetBundleInfo AssetBundleInfo { get; set; }

      public PlayState.AnimStateInfo InStateInfo { get; set; }

      public PlayState.AnimStateInfo OutStateInfo { get; set; }

      public float FadeOutTime { get; set; } = 1f;

      public bool IsLoop { get; set; }

      public int LoopMin { get; set; }

      public int LoopMax { get; set; }
    }

    [Serializable]
    public class AnimStateInfo
    {
      [SerializeField]
      private PlayState.Info[] _stateInfos;
      [SerializeField]
      private bool _enableFade;
      [SerializeField]
      private float _fadeTime;

      public PlayState.Info[] StateInfos
      {
        get
        {
          return this._stateInfos;
        }
        set
        {
          this._stateInfos = value;
        }
      }

      public bool EnableFade
      {
        get
        {
          return this._enableFade;
        }
        set
        {
          this._enableFade = value;
        }
      }

      public float FadeSecond
      {
        get
        {
          return this._fadeTime;
        }
        set
        {
          this._fadeTime = value;
        }
      }
    }

    [Serializable]
    public struct Info
    {
      public string stateName;
      public int layer;

      public Info(string name_, int layer_)
      {
        this.stateName = name_;
        this.layer = layer_;
      }

      public int ShortNameStateHash
      {
        get
        {
          return Animator.StringToHash(this.stateName);
        }
      }
    }

    public struct ItemInfo
    {
      public string parentName;
      public bool fromEquipedItem;
      public int itemID;
      public bool isSync;
    }
  }
}
