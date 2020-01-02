// Decompiled with JetBrains decompiler
// Type: AIProject.DevicePointAnimData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class DevicePointAnimData : MonoBehaviour
  {
    private static Dictionary<int, Animator> _animatorItemTable = new Dictionary<int, Animator>();
    [SerializeField]
    private int _id;
    [SerializeField]
    private Animator _animator;

    public DevicePointAnimData()
    {
      base.\u002Ector();
    }

    public static ReadOnlyDictionary<int, Animator> AnimatorItemTable { get; } = new ReadOnlyDictionary<int, Animator>((IDictionary<int, Animator>) DevicePointAnimData._animatorItemTable);

    protected virtual void Awake()
    {
      DevicePointAnimData._animatorItemTable[this._id] = this._animator;
    }
  }
}
