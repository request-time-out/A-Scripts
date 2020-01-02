// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.PlayerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.MiniGames.Fishing
{
  public class PlayerInfo
  {
    public PlayerActor root;
    public GameObject hand;
    public GameObject fishingRod;
    public GameObject lurePos;
    public RuntimeAnimatorController fishingRodAnimController;
    public Animator fishingRodAnimator;

    public PlayerInfo()
    {
      this.root = (PlayerActor) null;
      this.hand = (GameObject) null;
      this.fishingRod = (GameObject) null;
    }

    public bool ActivePlayer
    {
      get
      {
        return Object.op_Inequality((Object) this.root, (Object) null);
      }
    }

    public bool ActiveHand
    {
      get
      {
        return Object.op_Inequality((Object) this.hand, (Object) null);
      }
    }

    public bool ActiveFishingRod
    {
      get
      {
        return Object.op_Inequality((Object) this.fishingRod, (Object) null);
      }
    }

    public bool ActiveLurePos
    {
      get
      {
        return Object.op_Inequality((Object) this.lurePos, (Object) null);
      }
    }

    public bool ActiveFishingRodAnimController
    {
      get
      {
        return Object.op_Inequality((Object) this.fishingRodAnimController, (Object) null);
      }
    }

    public bool ActiveFishingRodAnimator
    {
      get
      {
        return Object.op_Inequality((Object) this.fishingRodAnimator, (Object) null);
      }
    }

    public bool ActiveFishingRodInfo
    {
      get
      {
        return this.ActiveFishingRod && this.ActiveLurePos && this.ActiveFishingRodAnimController && this.ActiveFishingRodAnimator;
      }
    }

    public bool AllActive
    {
      get
      {
        return this.ActivePlayer && this.ActiveHand && this.ActiveFishingRodInfo;
      }
    }

    public void Set(AIProject.Player.Fishing _playerFishing)
    {
      this.root = _playerFishing.player;
      this.hand = _playerFishing.hand;
    }

    public bool EqualPlayer(PlayerActor _playerActor)
    {
      return Object.op_Equality((Object) this.root, (Object) _playerActor);
    }

    public bool EqualHand(GameObject _hand)
    {
      return Object.op_Equality((Object) this.hand, (Object) _hand);
    }

    public bool EqualFishingRod(GameObject _fishingRod)
    {
      return Object.op_Equality((Object) this.fishingRod, (Object) _fishingRod);
    }

    public bool EqualLurePos(GameObject _lurePos)
    {
      return Object.op_Equality((Object) this.lurePos, (Object) _lurePos);
    }

    public bool EqualFishingAnimController(RuntimeAnimatorController _con)
    {
      return Object.op_Equality((Object) this.fishingRodAnimController, (Object) _con);
    }
  }
}
