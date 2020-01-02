// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPointAnimData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class ActionPointAnimData : MonoBehaviour
  {
    private static Dictionary<int, Animator> _animationItemTable = new Dictionary<int, Animator>();
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected Animator _animator;

    public ActionPointAnimData()
    {
      base.\u002Ector();
    }

    public static ReadOnlyDictionary<int, Animator> AnimationItemTable { get; } = new ReadOnlyDictionary<int, Animator>((IDictionary<int, Animator>) ActionPointAnimData._animationItemTable);

    protected virtual void Awake()
    {
      ActionPointAnimData._animationItemTable[this._id] = this._animator;
    }
  }
}
