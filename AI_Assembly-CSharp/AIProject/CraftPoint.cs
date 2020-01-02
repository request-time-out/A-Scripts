// Decompiled with JetBrains decompiler
// Type: AIProject.CraftPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Player;
using AIProject.SaveData;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  public class CraftPoint : Point, ICommandable
  {
    [SerializeField]
    private bool _enabledRangeCheck = true;
    [SerializeField]
    private float _radius = 1f;
    [SerializeField]
    private CraftPoint.CraftKind _kind;
    [SerializeField]
    private int _id;
    [SerializeField]
    private Transform _commandBasePoint;
    private int? _hashCode;
    private NavMeshPath _pathForCalc;
    private CommandLabel.CommandInfo[] _labels;

    public CraftPoint.CraftKind Kind
    {
      get
      {
        return this._kind;
      }
    }

    public int ID
    {
      get
      {
        return this._id;
      }
    }

    public float Radius
    {
      get
      {
        return this._radius;
      }
    }

    private List<Transform> NavMeshPoints { get; } = new List<Transform>();

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
      if (this.TutorialHideMode() || (double) distance > (double) radiusA)
        return false;
      Vector3 commandCenter = this.CommandCenter;
      commandCenter.y = (__Null) 0.0;
      float num = angle / 2f;
      if ((double) Vector3.Angle(Vector3.op_Subtraction(commandCenter, basePosition), forward) > (double) num)
        return false;
      PlayerActor player = Manager.Map.GetPlayer();
      return !Object.op_Inequality((Object) player, (Object) null) || !(player.PlayerController.State is Onbu);
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      bool flag2;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        if (!this.NavMeshPoints.IsNullOrEmpty<Transform>())
        {
          bool flag3 = false;
          using (List<Transform>.Enumerator enumerator = this.NavMeshPoints.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Transform current = enumerator.Current;
              if (!Object.op_Equality((Object) current, (Object) null))
              {
                nmAgent.CalculatePath(current.get_position(), this._pathForCalc);
                if (this._pathForCalc.get_status() == null)
                {
                  float num1 = 0.0f;
                  Vector3[] corners = this._pathForCalc.get_corners();
                  float num2 = (this.CommandType != CommandType.Forward ? radiusB : radiusA) + this._radius;
                  for (int index = 0; index < corners.Length - 1; ++index)
                  {
                    float num3 = Vector3.Distance(corners[index], corners[index + 1]);
                    num1 += num3;
                  }
                  if ((double) num1 < (double) num2)
                  {
                    flag3 = true;
                    break;
                  }
                }
              }
            }
          }
          flag2 = flag3;
        }
        else
        {
          nmAgent.CalculatePath(this.Position, this._pathForCalc);
          flag2 = flag1 & this._pathForCalc.get_status() == 0;
          float num1 = 0.0f;
          Vector3[] corners = this._pathForCalc.get_corners();
          for (int index = 0; index < corners.Length - 1; ++index)
          {
            float num2 = Vector3.Distance(corners[index], corners[index + 1]);
            num1 += num2;
            float num3 = (this.CommandType != CommandType.Forward ? radiusB : radiusA) + this._radius;
            if ((double) num1 > (double) num3)
            {
              flag2 = false;
              break;
            }
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
        return !this.TutorialHideMode();
      }
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
        return this._labels;
      }
    }

    public CommandLabel.CommandInfo[] DateLabels { get; private set; }

    public ObjectLayer Layer { get; } = ObjectLayer.Command;

    public CommandType CommandType { get; }

    protected override void Start()
    {
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
        this._commandBasePoint = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName)?.get_transform() ?? ((Component) this).get_transform();
      base.Start();
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      DefinePack.MapGroup mapDefines = Singleton<Resources>.Instance.DefinePack.MapDefines;
      string empty = string.Empty;
      int key = -1;
      string str;
      switch (this._kind)
      {
        case CraftPoint.CraftKind.Medicine:
          key = Singleton<Resources>.Instance.CommonDefine.Icon.MedicineCraftIconID;
          str = "薬台";
          break;
        case CraftPoint.CraftKind.Pet:
          key = Singleton<Resources>.Instance.CommonDefine.Icon.PetCraftIcon;
          str = "ペット合成装置";
          break;
        case CraftPoint.CraftKind.Recycling:
          key = Singleton<Resources>.Instance.CommonDefine.Icon.RecyclingCraftIcon;
          str = "リサイクル装置";
          break;
        default:
          str = "？？？？";
          break;
      }
      Sprite sprite;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(key, out sprite);
      Transform transform = ((Component) this).get_transform().FindLoop(mapDefines.CraftPointLabelTargetName)?.get_transform() ?? ((Component) this).get_transform();
      this._labels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = str,
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = transform,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() =>
          {
            PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
            if (!Object.op_Inequality((Object) playerActor, (Object) null))
              return;
            playerActor.CurrentCraftPoint = this;
            playerActor.PlayerController.ChangeState("Craft");
          })
        }
      };
      this.NavMeshPoints.Add(((Component) this).get_transform());
      List<GameObject> gameObjectList = ListPool<GameObject>.Get();
      ((Component) this).get_transform().FindLoopPrefix(gameObjectList, mapDefines.NavMeshTargetName);
      if (gameObjectList.IsNullOrEmpty<GameObject>())
        return;
      using (List<GameObject>.Enumerator enumerator = gameObjectList.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.NavMeshPoints.Add(enumerator.Current.get_transform());
      }
    }

    public bool TutorialHideMode()
    {
      return Manager.Map.TutorialMode;
    }

    private static List<StuffItem> ItemStock { get; } = new List<StuffItem>();

    private static StuffItem GetItemInstance()
    {
      CraftPoint.ItemStock.RemoveAll((Predicate<StuffItem>) (x => x == null));
      if (CraftPoint.ItemStock.IsNullOrEmpty<StuffItem>())
        return new StuffItem();
      return CraftPoint.ItemStock.IsNullOrEmpty<StuffItem>() ? new StuffItem() : CraftPoint.ItemStock.Pop<StuffItem>();
    }

    private static void ReturnItemInstance(StuffItem item)
    {
      if (item == null || CraftPoint.ItemStock.Contains(item))
        return;
      CraftPoint.ItemStock.Add(item);
    }

    private static void CopyItem(StuffItem from, StuffItem to)
    {
      if (from == null || to == null)
        return;
      to.CategoryID = from.CategoryID;
      to.ID = from.ID;
      to.Count = from.Count;
      to.LatestDateTime = from.LatestDateTime;
    }

    public bool CanDelete()
    {
      if (this._kind != CraftPoint.CraftKind.Recycling || !Singleton<Manager.Map>.IsInstance() || !Singleton<Game>.IsInstance())
        return true;
      WorldData worldData = Singleton<Game>.Instance.WorldData;
      AIProject.SaveData.Environment environment = worldData == null ? (AIProject.SaveData.Environment) null : worldData.Environment;
      if (environment == null)
        return true;
      RecyclingData recyclingData = (RecyclingData) null;
      if (!environment.RecyclingDataTable.TryGetValue(this.RegisterID, out recyclingData) || recyclingData == null)
        return true;
      recyclingData.DecidedItemList.RemoveAll((Predicate<StuffItem>) (x => x == null || x.Count <= 0));
      recyclingData.CreatedItemList.RemoveAll((Predicate<StuffItem>) (x => x == null || x.Count <= 0));
      if (recyclingData.DecidedItemList.IsNullOrEmpty<StuffItem>() && recyclingData.CreatedItemList.IsNullOrEmpty<StuffItem>())
        return true;
      List<StuffItem> stuffItemList1 = ListPool<StuffItem>.Get();
      foreach (StuffItem decidedItem in recyclingData.DecidedItemList)
      {
        StuffItem itemInstance = CraftPoint.GetItemInstance();
        CraftPoint.CopyItem(decidedItem, itemInstance);
        stuffItemList1.AddItem(itemInstance);
      }
      foreach (StuffItem createdItem in recyclingData.CreatedItemList)
      {
        StuffItem itemInstance = CraftPoint.GetItemInstance();
        CraftPoint.CopyItem(createdItem, itemInstance);
        stuffItemList1.AddItem(itemInstance);
      }
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      List<ValueTuple<int, List<StuffItem>>> inventoryList = instance.GetInventoryList();
      List<ValueTuple<int, List<StuffItem>>> valueTupleList = ListPool<ValueTuple<int, List<StuffItem>>>.Get();
      using (List<ValueTuple<int, List<StuffItem>>>.Enumerator enumerator = inventoryList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, List<StuffItem>> current = enumerator.Current;
          int num = (int) current.Item1;
          List<StuffItem> source = (List<StuffItem>) current.Item2;
          List<StuffItem> stuffItemList2 = ListPool<StuffItem>.Get();
          valueTupleList.Add(new ValueTuple<int, List<StuffItem>>(num, stuffItemList2));
          if (!source.IsNullOrEmpty<StuffItem>())
          {
            foreach (StuffItem from in source)
            {
              StuffItem itemInstance = CraftPoint.GetItemInstance();
              CraftPoint.CopyItem(from, itemInstance);
              stuffItemList2.Add(itemInstance);
            }
          }
        }
      }
      using (List<ValueTuple<int, List<StuffItem>>>.Enumerator enumerator = valueTupleList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, List<StuffItem>> current = enumerator.Current;
          int num = (int) current.Item1;
          List<StuffItem> self = (List<StuffItem>) current.Item2;
          for (int index = 0; index < stuffItemList1.Count; ++index)
          {
            StuffItem element = stuffItemList1.GetElement<StuffItem>(index);
            if (element == null || element.Count <= 0)
            {
              stuffItemList1.RemoveAt(index);
              --index;
            }
            else
            {
              StuffItem itemInstance = CraftPoint.GetItemInstance();
              CraftPoint.CopyItem(element, itemInstance);
              int possible = 0;
              StuffItemExtensions.CanAddItem((IReadOnlyCollection<StuffItem>) self, num, itemInstance, out possible);
              if (0 < possible)
              {
                possible = Mathf.Min(possible, itemInstance.Count);
                self.AddItem(itemInstance, possible, num);
              }
              element.Count -= possible;
              if (element.Count <= 0)
              {
                stuffItemList1.RemoveAt(index);
                --index;
              }
            }
          }
        }
      }
      stuffItemList1.RemoveAll((Predicate<StuffItem>) (x => x == null || x.Count <= 0));
      bool flag = stuffItemList1.IsNullOrEmpty<StuffItem>();
      if (flag)
      {
        using (List<ValueTuple<int, List<StuffItem>>>.Enumerator enumerator = inventoryList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ValueTuple<int, List<StuffItem>> current = enumerator.Current;
            int slotMax = (int) current.Item1;
            List<StuffItem> receiver = (List<StuffItem>) current.Item2;
            instance.SendItemListToList(slotMax, recyclingData.DecidedItemList, receiver);
            instance.SendItemListToList(slotMax, recyclingData.CreatedItemList, receiver);
          }
        }
      }
      using (List<ValueTuple<int, List<StuffItem>>>.Enumerator enumerator = valueTupleList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, List<StuffItem>> current = enumerator.Current;
          if (current.Item2 != null)
          {
            foreach (StuffItem stuffItem in (List<StuffItem>) current.Item2)
              CraftPoint.ReturnItemInstance(stuffItem);
            ListPool<StuffItem>.Release((List<StuffItem>) current.Item2);
          }
        }
      }
      foreach (StuffItem stuffItem in stuffItemList1)
        CraftPoint.ReturnItemInstance(stuffItem);
      ListPool<StuffItem>.Release(stuffItemList1);
      instance.ReturnInventoryList(inventoryList);
      return flag;
    }

    public enum CraftKind
    {
      Medicine,
      Pet,
      Recycling,
    }
  }
}
