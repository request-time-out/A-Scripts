// Decompiled with JetBrains decompiler
// Type: AIProject.ShipPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using IllusionUtility.GetUtility;
using Manager;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class ShipPoint : Point, ICommandable
  {
    [SerializeField]
    private bool _enableRangeCheck = true;
    [SerializeField]
    private float _radius = 1f;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    private Transform _startPointFromMigrate;
    private int? _hashCode;
    private NavMeshPath _pathForCalc;
    private CommandLabel.CommandInfo[] _labels;

    public Transform StartPointFromMigrate
    {
      get
      {
        return this._startPointFromMigrate;
      }
    }

    public int InstanceID
    {
      get
      {
        if (!this._hashCode.HasValue)
          this._hashCode = new int?(((Object) this).GetInstanceID());
        return this._hashCode.Value;
      }
    }

    public bool IsImpossible { get; private set; }

    public bool SetImpossible(bool value, Actor actor)
    {
      return true;
    }

    public bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      if (this.TutorialHideMode() || !Singleton<Game>.Instance.WorldData.Cleared || (double) distance > (double) radiusA)
        return false;
      Vector3 position = this.Position;
      position.y = (__Null) 0.0;
      float num = angle / 2f;
      return (double) Vector3.Angle(Vector3.op_Subtraction(position, basePosition), forward) <= (double) num;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      bool flag2;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        nmAgent.CalculatePath(this.Position, this._pathForCalc);
        flag2 = flag1 & this._pathForCalc.get_status() == 0;
        float num1 = 0.0f;
        Vector3[] corners = this._pathForCalc.get_corners();
        for (int index = 0; index < corners.Length - 1; ++index)
        {
          float num2 = Vector3.Distance(corners[index], corners[index + 1]);
          num1 += num2;
          float num3 = this.CommandType != CommandType.Forward ? radiusB : radiusA;
          if ((double) num1 > (double) num3)
          {
            flag2 = false;
            break;
          }
        }
      }
      else
        flag2 = false;
      return flag2;
    }

    public bool IsNeutralCommand
    {
      get
      {
        return !this.TutorialHideMode() && Singleton<Game>.Instance.WorldData.Cleared;
      }
    }

    public bool TutorialHideMode()
    {
      return Manager.Map.TutorialMode;
    }

    public Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
    }

    public Vector3 CommandCenter
    {
      get
      {
        return Object.op_Inequality((Object) this._commandBasePoint, (Object) null) ? this._commandBasePoint.get_position() : ((Component) this).get_transform().get_position();
      }
    }

    public CommandLabel.CommandInfo[] Labels
    {
      get
      {
        PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
        return Object.op_Inequality((Object) playerActor, (Object) null) && playerActor.PlayerController.State is Onbu ? (CommandLabel.CommandInfo[]) null : this._labels;
      }
    }

    public CommandLabel.CommandInfo[] DateLabels { get; private set; }

    public ObjectLayer Layer { get; } = ObjectLayer.Command;

    public CommandType CommandType { get; }

    protected override void Start()
    {
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
        this._commandBasePoint = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName)?.get_transform() ?? ((Component) this).get_transform();
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      DefinePack.MapGroup mapDefines = Singleton<Resources>.Instance.DefinePack.MapDefines;
      Sprite sprite;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.CommonDefine.Icon.ShipIconID, out sprite);
      Transform transform = ((Component) this).get_transform().FindLoop(mapDefines.ShipPointLabelTargetName)?.get_transform() ?? ((Component) this).get_transform();
      this._labels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = "船",
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = transform,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() =>
          {
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
            Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("ShipMenu");
          })
        }
      };
    }
  }
}
