// Decompiled with JetBrains decompiler
// Type: AIProject.CommandArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.Player;
using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class CommandArea : MonoBehaviour
  {
    [SerializeField]
    private Transform _baseTransform;
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private float _agentRadius;
    [SerializeField]
    private float _allaroundRadius;
    [SerializeField]
    private ObjectLayer _layerMask;
    [SerializeField]
    private float _commandFovAngle;
    [SerializeField]
    private float _height;
    private List<ICommandable> _commandableObjects;
    private List<ICommandable> _considerationObjects;
    private Dictionary<int, CollisionState> _collisionStateTable;
    private ReactiveProperty<bool> _isFishableState;
    [SerializeField]
    private Transform _bobberTransform;
    private CommandLabel.CommandInfo _fishingLabel;

    public CommandArea()
    {
      base.\u002Ector();
    }

    public Transform BaseTransform
    {
      get
      {
        return this._baseTransform;
      }
      set
      {
        this._baseTransform = value;
      }
    }

    public Vector3 Offset
    {
      get
      {
        return this._offset;
      }
    }

    public float Radius
    {
      get
      {
        return this._radius;
      }
    }

    public float AgentRadius
    {
      get
      {
        return this._agentRadius;
      }
    }

    public float AllAroundRadius
    {
      get
      {
        return this._allaroundRadius;
      }
    }

    public float Height
    {
      get
      {
        return this._height;
      }
    }

    public Vector3 BobberPosition { get; private set; }

    public Transform BobberTransform
    {
      get
      {
        return this._bobberTransform;
      }
    }

    public CommandLabel.CommandInfo FishingLabel
    {
      get
      {
        if (this._fishingLabel == null)
        {
          CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
          Sprite sprite;
          Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(22, out sprite);
          this._fishingLabel = new CommandLabel.CommandInfo()
          {
            Text = "釣りを始める",
            Icon = sprite,
            IsHold = true,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = this._bobberTransform,
            Event = (System.Action) (() =>
            {
              PlayerActor player = Singleton<Manager.Map>.Instance.Player;
              StuffItem equipedFishingItem = player.PlayerData.EquipedFishingItem;
              ItemIDKeyPair rodId = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.RodID;
              if (equipedFishingItem.CategoryID != rodId.categoryID && equipedFishingItem.ID != rodId.itemID)
              {
                MapUIContainer.PushWarningMessage(Popup.Warning.Type.NotSetFishingRod);
              }
              else
              {
                List<StuffItem> itemList = player.PlayerData.ItemList;
                if (player.PlayerData.InventorySlotMax <= itemList.Count)
                  MapUIContainer.PushWarningMessage(Popup.Warning.Type.PouchIsFull);
                else
                  this.StartFishing();
              }
            })
          };
        }
        return this._fishingLabel;
      }
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._isFishableState, (System.Action<M0>) (x => this.OnFishableStateChange()));
    }

    private void OnFishableStateChange()
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Player, (Object) null))
        return;
      this.RefreshCommands();
    }

    private void SetCollisionState(
      ICommandable element,
      CollisionState state,
      ref int changedCount)
    {
      int instanceId = element.InstanceID;
      CollisionState collisionState;
      this._collisionStateTable.TryGetValue(instanceId, out collisionState);
      if (collisionState == state)
        return;
      this._collisionStateTable[instanceId] = state;
      if (state != CollisionState.Enter && state != CollisionState.Exit)
        return;
      ++changedCount;
    }

    private void Update()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return;
      Vector3 convOffset = Vector3.get_zero();
      if (Object.op_Inequality((Object) this._baseTransform, (Object) null))
      {
        if (Singleton<Resources>.IsInstance())
        {
          if (player.PlayerData.EquipedFishingItem.ID == Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.RodID.itemID)
          {
            Vector3 vector3 = Vector3.op_Addition(this._baseTransform.get_position(), Quaternion.op_Multiply(this._baseTransform.get_rotation(), new Vector3(0.0f, 20f, 20f)));
            LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
            float waterCheckDistance = Singleton<Resources>.Instance.LocomotionProfile.FishingWaterCheckDistance;
            Vector3 forward = this._baseTransform.get_forward();
            bool flag = false;
            for (int index = 0; index < 3; ++index)
            {
              RaycastHit raycastHit;
              flag = Physics.Raycast(Vector3.op_Addition(vector3, Vector3.op_Multiply(Vector3.op_Multiply(forward, 7.5f), (float) index)), Vector3.get_down(), ref raycastHit, waterCheckDistance, LayerMask.op_Implicit(areaDetectionLayer));
              if (flag)
                flag &= ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_tag() == "Water";
              if (flag)
              {
                vector3.y = ((RaycastHit) ref raycastHit).get_point().y;
                this.BobberPosition = vector3;
                if (Object.op_Inequality((Object) this._bobberTransform, (Object) null))
                {
                  this._bobberTransform.set_position(vector3);
                  break;
                }
                break;
              }
            }
            this._isFishableState.set_Value(flag);
          }
          else
            this._isFishableState.set_Value(false);
        }
        Vector3 eulerAngles = this._baseTransform.get_eulerAngles();
        eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
        convOffset = Quaternion.op_Multiply(Quaternion.Euler(eulerAngles), this._offset);
      }
      this.UpdateCollision(player, convOffset);
    }

    public void UpdateCollision(PlayerActor player)
    {
      Vector3 convOffset = Vector3.get_zero();
      if (Object.op_Inequality((Object) this._baseTransform, (Object) null))
      {
        Vector3 eulerAngles = this._baseTransform.get_eulerAngles();
        eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
        convOffset = Quaternion.op_Multiply(Quaternion.Euler(eulerAngles), this._offset);
      }
      this.UpdateCollision(player, convOffset);
    }

    public void UpdateCollision(PlayerActor player, Vector3 convOffset)
    {
      if (this._commandableObjects.IsNullOrEmpty<ICommandable>())
        return;
      bool flag1 = player.Controller.State is Follow;
      NavMeshAgent navMeshAgent = player.NavMeshAgent;
      int changedCount = 0;
      foreach (ICommandable commandableObject in this._commandableObjects)
      {
        int instanceId = commandableObject.InstanceID;
        CollisionState collisionState1;
        if (!this._collisionStateTable.TryGetValue(instanceId, out collisionState1))
        {
          CollisionState collisionState2 = CollisionState.None;
          this._collisionStateTable[instanceId] = collisionState2;
          collisionState1 = collisionState2;
        }
        bool flag2 = this.WithinRange(navMeshAgent, commandableObject, convOffset);
        if (flag1)
          flag2 = false;
        if (flag2)
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              if (commandableObject.IsNeutralCommand && this.HasLabels(commandableObject))
              {
                this.SetCollisionState(commandableObject, CollisionState.Enter, ref changedCount);
                break;
              }
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(commandableObject, CollisionState.Stay, ref changedCount);
              break;
          }
          if (commandableObject.IsNeutralCommand && commandableObject.SetImpossible(true, (Actor) Singleton<Manager.Map>.Instance.Player) && !this._considerationObjects.Contains(commandableObject))
          {
            this._considerationObjects.Add(commandableObject);
            ++changedCount;
          }
        }
        else
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(commandableObject, CollisionState.None, ref changedCount);
              break;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(commandableObject, CollisionState.Exit, ref changedCount);
              break;
          }
          if (commandableObject.SetImpossible(false, (Actor) Singleton<Manager.Map>.Instance.Player) && this._considerationObjects.Contains(commandableObject))
            this._considerationObjects.Remove(commandableObject);
        }
      }
      if (changedCount <= 0)
        return;
      this.RefreshCommands();
    }

    private bool HasLabels(ICommandable elem)
    {
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      AgentActor agentActor = (AgentActor) null;
      if (Object.op_Inequality((Object) player, (Object) null))
        agentActor = player.AgentPartner;
      return Object.op_Equality((Object) agentActor, (Object) null) ? !elem.Labels.IsNullOrEmpty<CommandLabel.CommandInfo>() : !(!Object.op_Inequality((Object) player, (Object) null) ? (CommandLabel.CommandInfo[]) null : (player.Mode != Desire.ActionType.Date ? elem.Labels : elem.DateLabels)).IsNullOrEmpty<CommandLabel.CommandInfo>();
    }

    public void RefreshCommands()
    {
      List<CommandLabel.CommandInfo> toRelease = ListPool<CommandLabel.CommandInfo>.Get();
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      AgentActor agentActor = (AgentActor) null;
      if (Object.op_Inequality((Object) player, (Object) null))
        agentActor = player.AgentPartner;
      foreach (ICommandable considerationObject in this._considerationObjects)
      {
        if (Object.op_Equality((Object) agentActor, (Object) null))
        {
          if (!considerationObject.Labels.IsNullOrEmpty<CommandLabel.CommandInfo>())
          {
            foreach (CommandLabel.CommandInfo label in considerationObject.Labels)
            {
              Func<bool> displayCondition = label.DisplayCondition;
              bool? nullable1;
              bool? nullable2;
              if (displayCondition == null)
              {
                nullable1 = new bool?();
                nullable2 = nullable1;
              }
              else
                nullable2 = new bool?(displayCondition());
              nullable1 = nullable2;
              if ((!nullable1.HasValue ? 1 : (nullable1.Value ? 1 : 0)) != 0)
              {
                label.Position = considerationObject.CommandCenter;
                toRelease.Add(label);
              }
            }
          }
        }
        else
        {
          CommandLabel.CommandInfo[] source = !Object.op_Inequality((Object) player, (Object) null) ? (CommandLabel.CommandInfo[]) null : (player.Mode != Desire.ActionType.Date ? considerationObject.Labels : considerationObject.DateLabels);
          if (!source.IsNullOrEmpty<CommandLabel.CommandInfo>())
          {
            foreach (CommandLabel.CommandInfo commandInfo in source)
            {
              commandInfo.Position = considerationObject.CommandCenter;
              toRelease.Add(commandInfo);
            }
          }
        }
      }
      if (Object.op_Equality((Object) agentActor, (Object) null) && this._isFishableState.get_Value())
      {
        this.FishingLabel.Position = this.BobberPosition;
        toRelease.Add(this.FishingLabel);
      }
      if (Singleton<MapUIContainer>.IsInstance())
        MapUIContainer.CommandLabel.ObjectCommands = toRelease.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease);
    }

    public bool WithinRange(NavMeshAgent agent, ICommandable element, Vector3 offset)
    {
      bool flag1 = true;
      Vector3 commandCenter = element.CommandCenter;
      commandCenter.y = (__Null) 0.0;
      Vector3 basePosition = Vector3.op_Addition(this._baseTransform.get_position(), offset);
      basePosition.y = (__Null) 0.0;
      float distance = Vector3.Distance(commandCenter, basePosition);
      float num = Mathf.Abs((float) (element.CommandCenter.y - this._baseTransform.get_position().y + this._offset.y));
      bool flag2 = flag1 & (double) num < (double) this._height;
      if (flag2)
        flag2 &= element.Entered(basePosition, distance, this._radius, this._allaroundRadius, this._commandFovAngle, this._baseTransform.get_forward());
      if (flag2)
        flag2 &= this.HasLabels(element);
      if (flag2)
      {
        if (element is Actor)
          flag2 &= element.IsReachable(agent, this._agentRadius, this._allaroundRadius);
        else
          flag2 &= element.IsReachable(agent, this._radius, this._allaroundRadius);
      }
      return flag2;
    }

    public void SetCommandableObjects(ICommandable[] commandables)
    {
      this._commandableObjects.Clear();
      foreach (ICommandable commandable in commandables)
      {
        if (this._layerMask.Contains(commandable.Layer))
          this._commandableObjects.Add(commandable);
      }
    }

    public void AddCommandableObject(ICommandable commandable)
    {
      if (!this._layerMask.Contains(commandable.Layer) || this._commandableObjects.Contains(commandable))
        return;
      this._commandableObjects.Add(commandable);
    }

    public void RemoveCommandableObject(ICommandable commandable)
    {
      if (this._commandableObjects.Contains(commandable))
        this._commandableObjects.Remove(commandable);
      if (this._considerationObjects.Contains(commandable))
      {
        commandable.SetImpossible(false, (Actor) Singleton<Manager.Map>.Instance.Player);
        this._considerationObjects.Remove(commandable);
      }
      if (this._collisionStateTable.ContainsKey(commandable.InstanceID))
        this._collisionStateTable.Remove(commandable.InstanceID);
      this.RefreshCommands();
    }

    public void RemoveConsiderationObject(ICommandable commandable)
    {
      if (!this._considerationObjects.Contains(commandable))
        return;
      commandable.SetImpossible(false, (Actor) Singleton<Manager.Map>.Instance.Player);
      this._considerationObjects.Remove(commandable);
    }

    public void SequenceSetConsiderations(System.Action<ICommandable> action)
    {
      if (action == null)
        return;
      foreach (ICommandable commandableObject in this._commandableObjects)
      {
        if (action != null)
          action(commandableObject);
      }
    }

    public void InitCommandStates()
    {
      foreach (ICommandable commandableObject in this._commandableObjects)
        this._collisionStateTable[commandableObject.InstanceID] = CollisionState.None;
      foreach (ICommandable considerationObject in this._considerationObjects)
        considerationObject.SetImpossible(false, (Actor) Singleton<Manager.Map>.Instance.Player);
      this._considerationObjects.Clear();
    }

    public bool ContainsCommandableObject(ICommandable source)
    {
      return this._commandableObjects.Contains(source);
    }

    public bool ContainsConsiderationObject(ICommandable source)
    {
      return this._considerationObjects.Contains(source);
    }

    public bool CommandCondition(ICommandable element)
    {
      if (Object.op_Equality((Object) this._baseTransform, (Object) null))
        return false;
      bool flag1 = true;
      Vector3 vector3_1 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, (float) this._baseTransform.get_eulerAngles().y, 0.0f), this._offset);
      Vector3 position = element.Position;
      Vector3 vector3_2 = Vector3.op_Addition(this._baseTransform.get_position(), vector3_1);
      position.y = (__Null) (double) (vector3_2.y = (__Null) 0.0f);
      float num1 = Vector3.Distance(position, vector3_2);
      float num2 = Mathf.Abs((float) (element.CommandCenter.y - this._baseTransform.get_position().y + this._offset.y));
      bool flag2 = flag1 & (double) num2 < (double) this._height;
      bool flag3;
      if (element.CommandType == CommandType.Forward)
      {
        bool flag4 = flag2 & (double) num1 < (double) this._radius;
        float num3 = this._commandFovAngle / 2f;
        float num4 = Vector3.Angle(Vector3.op_Addition(Vector3.op_Subtraction(element.CommandCenter, this._baseTransform.get_position()), vector3_1), this._baseTransform.get_forward());
        flag3 = flag4 & (double) num4 < (double) num3;
      }
      else
        flag3 = flag2 & (double) num1 < (double) this._allaroundRadius;
      return flag3;
    }

    private void StartFishing()
    {
      Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("Fishing");
    }
  }
}
