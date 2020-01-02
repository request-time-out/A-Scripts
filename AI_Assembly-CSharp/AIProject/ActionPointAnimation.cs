// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPointAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class ActionPointAnimation : ActionPointComponentBase
  {
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected Animator _animator;

    public Animator Animator
    {
      get
      {
        return this._animator;
      }
    }

    protected override void OnStart()
    {
      this._animator = ActionPointAnimData.AnimationItemTable.get_Item(this._id);
    }

    protected void PlayAnimation(string stateName, int layer, float normalizedTime)
    {
      if (Debug.get_isDebugBuild())
        Debug.Log((object) string.Format("{0}: CrossFade to {1}", (object) ((Object) ((Component) this).get_gameObject()).get_name(), (object) stateName));
      this.Animator.Play(stateName, layer, normalizedTime);
    }

    protected void CrossFadeAnimation(
      string stateName,
      float fadeTime,
      int layer,
      float fixedTimeOffset)
    {
      if (Debug.get_isDebugBuild())
        Debug.Log((object) string.Format("{0}: CrossFade to {1}", (object) ((Object) ((Component) this).get_gameObject()).get_name(), (object) stateName));
      this.Animator.CrossFadeInFixedTime(stateName, fadeTime, layer, fixedTimeOffset);
    }
  }
}
