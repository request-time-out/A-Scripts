// Decompiled with JetBrains decompiler
// Type: AIProject.JukePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using Housing;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class JukePoint : Point, ICommandable
  {
    [SerializeField]
    private float _rangeRadius = 1f;
    [SerializeField]
    private float _checkHeight = 1f;
    [SerializeField]
    private int _id;
    [SerializeField]
    private bool _isCylinderCheck;
    [SerializeField]
    private bool _enableRangeCheck;
    [SerializeField]
    private float _checkRadius;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    private Transform _labelPoint;
    [SerializeField]
    private Transform _soundPlayPoint;
    private int? _instanceID;
    private CommandLabel.CommandInfo[] _labels;
    private NavMeshPath _pathForCalc;

    public override int RegisterID
    {
      get
      {
        return this._id;
      }
      set
      {
        this._id = value;
      }
    }

    public bool IsCylinderCheck
    {
      get
      {
        return this._isCylinderCheck;
      }
    }

    public bool EnableRangeCheck
    {
      get
      {
        return this._enableRangeCheck;
      }
    }

    public float RangeRadius
    {
      get
      {
        return this._rangeRadius;
      }
    }

    public Transform CommandBasePoint
    {
      get
      {
        return Object.op_Inequality((Object) this._commandBasePoint, (Object) null) ? this._commandBasePoint : ((Component) this).get_transform();
      }
    }

    public Transform LabelPoint
    {
      get
      {
        return Object.op_Inequality((Object) this._labelPoint, (Object) null) ? this._labelPoint : ((Component) this).get_transform();
      }
    }

    public Transform SoundPlayPoint
    {
      get
      {
        return Object.op_Inequality((Object) this._soundPlayPoint, (Object) null) ? this._soundPlayPoint : ((Component) this).get_transform();
      }
    }

    public int AreaID { get; private set; } = -1;

    public Vector3 CommandCenter
    {
      get
      {
        Vector3 position = this.CommandBasePoint.get_position();
        if (!this._isCylinderCheck)
          return position;
        CommandArea commandArea = Manager.Map.GetCommandArea();
        if (Object.op_Inequality((Object) commandArea, (Object) null) && Object.op_Inequality((Object) commandArea.BaseTransform, (Object) null))
          position.y = commandArea.BaseTransform.get_position().y;
        return position;
      }
    }

    public int InstanceID
    {
      get
      {
        return (!this._instanceID.HasValue ? (this._instanceID = new int?(((Object) this).GetInstanceID())) : this._instanceID).Value;
      }
    }

    public bool IsImpossible { get; private set; }

    public bool SetImpossible(bool value, Actor actor)
    {
      return true;
    }

    public bool IsNeutralCommand
    {
      get
      {
        return !this.TutorialHideMode();
      }
    }

    private bool TutorialHideMode()
    {
      return Manager.Map.TutorialMode;
    }

    public CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return this._labels;
      }
    }

    public CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        return (CommandLabel.CommandInfo[]) null;
      }
    }

    public Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
    }

    public ObjectLayer Layer
    {
      get
      {
        return ObjectLayer.Command;
      }
    }

    public CommandType CommandType
    {
      get
      {
        return CommandType.Forward;
      }
    }

    private void InitializeCommandLabels()
    {
      if (this._labels != null)
        return;
      Resources instance = Singleton<Resources>.Instance;
      CommonDefine.CommonIconGroup icon = instance.CommonDefine.Icon;
      int jukeBoxIconId = instance.CommonDefine.Icon.JukeBoxIconID;
      Sprite sprite;
      instance.itemIconTables.ActionIconTable.TryGetValue(jukeBoxIconId, out sprite);
      int index = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
      List<string> source;
      instance.Map.EventPointCommandLabelTextTable.TryGetValue(17, out source);
      this._labels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = source.GetElement<string>(index),
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = this.LabelPoint,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() =>
          {
            PlayerActor player = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
            if (!Object.op_Inequality((Object) player, (Object) null))
              return;
            player.CurrentjukePoint = this;
            MapUIContainer.SetVisibleHUD(false);
            player.PlayerController.ChangeState("Idle");
            MapUIContainer.JukeBoxUI.ClosedAction = (Action) (() =>
            {
              MapUIContainer.SetVisibleHUD(true);
              player.PlayerController.ChangeState("Normal");
            });
            MapUIContainer.SetActiveJukeBoxUI(true);
          })
        }
      };
    }

    public bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      if (this.TutorialHideMode())
        return false;
      bool flag;
      if (this.CommandType == CommandType.Forward)
      {
        float num1 = (!this._isCylinderCheck ? 0.0f : this._checkRadius) + radiusA;
        if (!this._isCylinderCheck && this._enableRangeCheck)
          num1 += this._rangeRadius;
        if ((double) num1 < (double) distance)
          return false;
        Vector3 position = this.CommandBasePoint.get_position();
        position.y = (__Null) 0.0;
        float num2 = angle / 2f;
        flag = (double) Vector3.Angle(Vector3.op_Subtraction(position, basePosition), forward) <= (double) num2;
      }
      else
      {
        float num = (!this._isCylinderCheck ? 0.0f : this._checkRadius) + radiusB;
        if (!this._isCylinderCheck && this._enableRangeCheck)
          num += this._rangeRadius;
        flag = (double) distance <= (double) num;
      }
      if (!flag)
        return false;
      PlayerActor player = Manager.Map.GetPlayer();
      return (!Object.op_Inequality((Object) player, (Object) null) || !(player.PlayerController.State is Onbu)) && flag;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      bool flag2;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        if (!this._isCylinderCheck)
        {
          flag2 = flag1 & nmAgent.CalculatePath(this.Position, this._pathForCalc);
          if (flag2)
            flag2 = this._pathForCalc.get_status() == 0;
          if (!flag2)
            return false;
          float num1 = 0.0f;
          Vector3[] corners = this._pathForCalc.get_corners();
          float num2 = this.CommandType != CommandType.Forward ? radiusB : radiusA;
          for (int index = 0; index < corners.Length - 1; ++index)
          {
            float num3 = Vector3.Distance(corners[index], corners[index + 1]);
            num1 += num3;
            if ((double) num2 < (double) num1)
            {
              flag2 = false;
              break;
            }
          }
        }
        else
          flag2 = (double) Mathf.Abs((float) (((Component) nmAgent).get_transform().get_position().y - this.CommandBasePoint.get_position().y)) <= (double) this._checkHeight / 2.0;
      }
      else
        flag2 = false;
      return flag2;
    }

    public void SetAreaID()
    {
      this.AreaID = -1;
      ItemComponent componentInParent1 = (ItemComponent) ((Component) this).GetComponentInParent<ItemComponent>();
      if (Object.op_Equality((Object) componentInParent1, (Object) null))
        return;
      Vector3 vector3 = Vector3.op_Addition(componentInParent1.position, Vector3.op_Multiply(Vector3.get_up(), 5f));
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      if (0 >= Mathf.Min(Physics.RaycastNonAlloc(vector3, Vector3.get_down(), Point._raycastHits, 1000f, LayerMask.op_Implicit(areaDetectionLayer)), Point._raycastHits.Length))
        return;
      foreach (RaycastHit raycastHit in Point._raycastHits)
      {
        MapArea componentInParent2 = (MapArea) ((Component) ((RaycastHit) ref raycastHit).get_transform())?.GetComponentInParent<MapArea>();
        if (Object.op_Inequality((Object) componentInParent2, (Object) null))
        {
          this.AreaID = componentInParent2.AreaID;
          break;
        }
      }
    }

    protected override void Start()
    {
      base.Start();
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName);
        this._commandBasePoint = !Object.op_Inequality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
      }
      if (Object.op_Equality((Object) this._labelPoint, (Object) null))
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.JukeBoxPointLabelTargetName);
        this._labelPoint = !Object.op_Inequality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
      }
      if (Object.op_Equality((Object) this._soundPlayPoint, (Object) null))
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.JukeBoxSoundRootTargetName);
        this._soundPlayPoint = !Object.op_Inequality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
      }
      this.InitializeCommandLabels();
    }
  }
}
