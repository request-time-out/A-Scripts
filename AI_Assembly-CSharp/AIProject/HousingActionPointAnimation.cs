// Decompiled with JetBrains decompiler
// Type: AIProject.HousingActionPointAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace AIProject
{
  public class HousingActionPointAnimation : ActionPointAnimation
  {
    [SerializeField]
    protected int _animatorID;

    protected override void OnStart()
    {
      if (!Object.op_Inequality((Object) this._animator, (Object) null))
        return;
      this._animator.set_runtimeAnimatorController(Singleton<Resources>.Instance.Animation.GetItemAnimator(this._animatorID));
    }
  }
}
