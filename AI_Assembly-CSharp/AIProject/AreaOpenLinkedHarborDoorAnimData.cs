// Decompiled with JetBrains decompiler
// Type: AIProject.AreaOpenLinkedHarborDoorAnimData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class AreaOpenLinkedHarborDoorAnimData : HarborDoorAnimData
  {
    [SerializeField]
    private int _areaOpenID;
    private bool _openState;

    public int AreaOpenID
    {
      get
      {
        return this._areaOpenID;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this.DoorAnimator, (Object) null))
        return;
      this._openState = this.CheckAreaOpenState();
      if (this._openState)
        this.PlayOpenIdleAnimation(false, 0.0f, 0.0f, 0);
      else
        this.PlayCloseIdleAnimation(false, 0.0f, 0.0f, 0);
    }

    private void OnEnable()
    {
      this.OnCheck(true);
    }

    public bool CheckAreaOpenState()
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      Dictionary<int, bool> areaOpenState = Singleton<Game>.Instance.Environment?.AreaOpenState;
      bool flag;
      return !areaOpenState.IsNullOrEmpty<int, bool>() && areaOpenState.TryGetValue(this._areaOpenID, out flag) && flag;
    }

    private void OnCheck(bool forced = false)
    {
      bool flag = this.CheckAreaOpenState();
      if (flag == this._openState && !forced)
        return;
      if (flag)
        this.PlayOpenIdleAnimation(false, 0.0f, 0.0f, 0);
      else
        this.PlayCloseIdleAnimation(false, 0.0f, 0.0f, 0);
      this._openState = flag;
    }
  }
}
