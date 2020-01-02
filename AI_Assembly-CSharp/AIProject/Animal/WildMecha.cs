// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.WildMecha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using AIProject.UI;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  public class WildMecha : AnimalGround
  {
    private MechaHabitatPoint habitatPoint;
    protected CommandLabel.CommandInfo[] getLabels;

    public override bool WaitPossible
    {
      get
      {
        return this.CurrentState == AnimalState.Idle;
      }
    }

    public override bool ParamRisePossible
    {
      get
      {
        return false;
      }
    }

    public override bool DepopPossible
    {
      get
      {
        return false;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (!Object.op_Inequality((Object) this.Agent, (Object) null))
        return;
      NavMeshAgent agent = this.Agent;
      int num1 = 0;
      this.Priority = num1;
      int num2 = num1;
      agent.set_avoidancePriority(num2);
    }

    protected override void OnDestroy()
    {
      if (Object.op_Inequality((Object) this.habitatPoint, (Object) null))
        this.habitatPoint.StopUse(this);
      base.OnDestroy();
    }

    public void Initialize(MechaHabitatPoint _habitatPoint)
    {
      this.Clear();
      if (Object.op_Equality((Object) (this.habitatPoint = _habitatPoint), (Object) null))
        this.SetState(AnimalState.Destroyed, (System.Action) null);
      else if (!this.habitatPoint.SetUse(this))
      {
        this.SetState(AnimalState.Destroyed, (System.Action) null);
      }
      else
      {
        MapArea ownerArea = this.habitatPoint.OwnerArea;
        this.ChunkID = !Object.op_Inequality((Object) ownerArea, (Object) null) ? 0 : ownerArea.ChunkID;
        this.DeactivateNavMeshElements();
        Transform transform = ((Component) this.habitatPoint).get_transform();
        this.Position = transform.get_position();
        this.Rotation = transform.get_rotation();
        this.Relocate(LocateTypes.NavMesh);
        this.stateController.Initialize((AnimalBase) this);
        this.LoadBody();
        this.SetStateData();
        bool flag = false;
        this.MarkerEnabled = flag;
        this.BodyEnabled = flag;
        this.SetState(AnimalState.Start, (System.Action) null);
      }
    }

    public override bool IsNeutralCommand
    {
      get
      {
        return !this.BadMood && this.CurrentState == AnimalState.Idle;
      }
    }

    public override CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return !this.IsNeutralCommand || this.LabelType != LabelTypes.GetAnimal ? AnimalBase.emptyLabels : this.getLabels;
      }
    }

    protected override void InitializeCommandLabels()
    {
      if (!((IReadOnlyList<CommandLabel.CommandInfo>) this.getLabels).IsNullOrEmpty<CommandLabel.CommandInfo>())
        return;
      CommonDefine commonDefine = !Singleton<Resources>.IsInstance() ? (CommonDefine) null : Singleton<Resources>.Instance.CommonDefine;
      CommonDefine.CommonIconGroup commonIconGroup = !Object.op_Inequality((Object) commonDefine, (Object) null) ? (CommonDefine.CommonIconGroup) null : commonDefine.Icon;
      Resources instance = Singleton<Resources>.Instance;
      int guideCancelId = commonIconGroup.GuideCancelID;
      Sprite sprite;
      instance.itemIconTables.InputIconTable.TryGetValue(guideCancelId, out sprite);
      List<string> source;
      instance.Map.EventPointCommandLabelTextTable.TryGetValue(0, out source);
      int index = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
      this.getLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = source.GetElement<string>(index),
          Transform = this.LabelPoint,
          IsHold = false,
          Icon = sprite,
          TargetSpriteInfo = commonIconGroup?.CharaSpriteInfo,
          Event = (System.Action) (() =>
          {
            PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
            PlayerController playerController = !Object.op_Inequality((Object) playerActor, (Object) null) ? (PlayerController) null : playerActor.PlayerController;
            EventPoint.SetCurrentPlayerStateName();
            if (Object.op_Inequality((Object) playerController, (Object) null))
              playerController.ChangeState("Idle");
            RequestUI requestUi = MapUIContainer.RequestUI;
            requestUi.SubmitEvent = (System.Action) (() => this.GetMecha());
            requestUi.SubmitCondition = (Func<bool>) (() => this.CanGet());
            requestUi.ClosedEvent = (System.Action) (() => EventPoint.ChangePrevPlayerMode());
            requestUi.Open(Popup.Request.Type.Cuby);
            if (!requestUi.IsImpossible)
              return;
            MapUIContainer.PushWarningMessage(Popup.Warning.Type.InsufficientBattery);
          })
        }
      };
    }

    protected override void EnterStart()
    {
      bool flag = true;
      this.MarkerEnabled = flag;
      this.BodyEnabled = flag;
      this.LabelType = LabelTypes.GetAnimal;
      this.ActivateNavMeshObstacle();
      this.Active = true;
      this.SetState(AnimalState.Idle, (System.Action) null);
    }

    protected override void OnStart()
    {
    }

    protected override void ExitStart()
    {
    }

    protected override void AnimationStart()
    {
    }

    protected override void EnterIdle()
    {
      this.PlayInAnim(AnimationCategoryID.Etc, 0, (System.Action) null);
    }

    protected override void OnIdle()
    {
    }

    protected override void ExitIdle()
    {
    }

    protected override void AnimationIdle()
    {
    }

    protected override void EnterDepop()
    {
      this.AutoChangeAnimation = false;
      StuffItemInfo _itemInfo = this.ItemInfo;
      StuffItem _addItem = (StuffItem) null;
      if (_itemInfo != null)
        _addItem = new StuffItem(_itemInfo.CategoryID, _itemInfo.ID, 1);
      PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
      if ((!Object.op_Inequality((Object) playerActor, (Object) null) ? (List<StuffItem>) null : playerActor.PlayerData?.ItemList) != null && _addItem != null)
        playerActor.PlayerData.ItemList.AddItem(_addItem);
      if (Singleton<Resources>.IsInstance())
      {
        AnimalDefinePack animalDefinePack = Singleton<Resources>.Instance.AnimalDefinePack;
        if (Object.op_Inequality((Object) animalDefinePack, (Object) null))
        {
          AudioSource audioSource = this.Play3DSound(animalDefinePack.SoundID.MechaStartup);
          if (Object.op_Inequality((Object) audioSource, (Object) null))
          {
            audioSource.Stop();
            ((Component) audioSource).get_transform().SetPositionAndRotation(this.Position, this.Rotation);
            audioSource.Play();
          }
        }
      }
      this.PlayInAnim(AnimationCategoryID.Etc, 1, (System.Action) (() =>
      {
        this.AutoChangeAnimation = true;
        if (_itemInfo != null && _addItem != null)
          MapUIContainer.AddSystemItemLog(_itemInfo, _addItem.Count, true);
        else
          MapUIContainer.AddNotify(MapUIContainer.ItemGetEmptyText);
        this.SetState(AnimalState.Destroyed, (System.Action) null);
      }));
    }

    protected override void OnDepop()
    {
    }

    protected override void ExitDepop()
    {
    }

    protected override void AnimationDepop()
    {
    }

    public bool CanGet()
    {
      PlayerActor player = Manager.Map.GetPlayer();
      if (Object.op_Equality((Object) player, (Object) null))
        return false;
      PlayerData playerData = player.PlayerData;
      if (playerData == null)
        return false;
      List<StuffItem> itemList = playerData.ItemList;
      if (itemList == null)
        return false;
      StuffItem stuffItem = new StuffItem(this.ItemID.categoryID, this.ItemID.itemID, 1);
      int possible;
      return StuffItemExtensions.CanAddItem((IReadOnlyCollection<StuffItem>) itemList, playerData.InventorySlotMax, stuffItem, out possible) && 0 < possible;
    }

    protected void GetMecha()
    {
      if (!this.CanGet())
      {
        MapUIContainer.PushWarningMessage(Popup.Warning.Type.PouchIsFull);
      }
      else
      {
        this.LabelType = LabelTypes.None;
        this.SetState(AnimalState.Depop, (System.Action) null);
      }
    }
  }
}
