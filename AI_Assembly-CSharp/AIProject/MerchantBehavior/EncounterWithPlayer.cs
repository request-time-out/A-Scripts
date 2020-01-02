// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.EncounterWithPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class EncounterWithPlayer : MerchantAction
  {
    [SerializeField]
    private float motivationLimit = 30f;
    [SerializeField]
    private bool hasMotivation;
    private PlayerActor _player;
    private MerchantActor _merchant;
    private float counter;
    private bool prevTalkable;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.counter = 0.0f;
      this.Merchant.ActivateNavMeshObstacle(this.Merchant.Position);
      this.prevTalkable = this.Merchant.Talkable;
      if (!this.prevTalkable)
        this.Merchant.Talkable = true;
      this._player = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
      this._merchant = this.Merchant;
      ChaControl chaControl = this.Merchant.ChaControl;
      chaControl.ChangeLookNeckTarget(1, this._player.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head), 0.5f, 0.0f, 1f, 0.8f);
      chaControl.ChangeLookNeckPtn(1, 1f);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this._player, (Object) null))
        return (TaskStatus) 1;
      if (this.hasMotivation)
      {
        this.counter += Time.get_deltaTime();
        if ((double) this.motivationLimit <= (double) this.counter)
          return (TaskStatus) 1;
      }
      return this._merchant.IsFarPlayer ? (TaskStatus) 1 : (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      this.Merchant.ChaControl.ChangeLookNeckPtn(3, 1f);
      if (!this.prevTalkable)
        this.Merchant.Talkable = false;
      ((Task) this).OnEnd();
    }
  }
}
