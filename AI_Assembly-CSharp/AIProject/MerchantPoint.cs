// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using IllusionUtility.GetUtility;
using Manager;
using ReMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class MerchantPoint : Point
  {
    private List<MerchantPointInfo> _pointInfos = new List<MerchantPointInfo>();
    [SerializeField]
    [Tooltip("行動ID")]
    private int _actionID;
    [SerializeField]
    [Tooltip("開放エリア等に使用")]
    [DisableInPlayMode]
    private int _areaID;
    [SerializeField]
    [Tooltip("エリア内でのグループ分け")]
    [DisableInPlayMode]
    private int _groupID;
    [SerializeField]
    [Tooltip("このポイントで起こせる行動")]
    [HideInEditorMode]
    private Merchant.EventType _eventType;
    [SerializeField]
    [Tooltip("商人登場イベント時のスタートポイント")]
    private bool _isStartPoint;
    [SerializeField]
    [Tooltip("島から出ていく時に使うポイント")]
    private bool _isExitPoint;
    [SerializeField]
    [Tooltip("Actorがこのポイントを目指す到達地点")]
    private Transform _destination;
    private int? instanceID;

    public int ActionID
    {
      get
      {
        return this._actionID;
      }
    }

    public int AreaID
    {
      get
      {
        return this._areaID;
      }
    }

    [ShowInInlineEditors]
    [HideInEditorMode]
    [ReadOnly]
    public int AreaIDOnChunk { get; set; }

    public int GroupID
    {
      get
      {
        return this._groupID;
      }
    }

    public Merchant.EventType EventType
    {
      get
      {
        return this._eventType;
      }
    }

    public bool IsStartPoint
    {
      get
      {
        return this._isStartPoint;
      }
    }

    public bool IsExitPoint
    {
      get
      {
        return this._isExitPoint;
      }
    }

    public int PointID { get; set; }

    public Vector3 Destination
    {
      get
      {
        return Object.op_Inequality((Object) this._destination, (Object) null) ? this._destination.get_position() : ((Component) this).get_transform().get_position();
      }
    }

    public List<ForcedHideObject> ItemObjects { get; set; }

    public int InstanceID
    {
      get
      {
        return this.instanceID.HasValue ? this.instanceID.Value : (this.instanceID = new int?(((Object) this).GetInstanceID())).Value;
      }
    }

    protected override void OnEnable()
    {
      base.OnEnable();
    }

    protected override void Start()
    {
      this.Refresh();
      base.Start();
    }

    public void SetItemObjectsActive(bool active)
    {
      Debug.LogWarning((object) string.Format("MerchantPoint.SetItemObjectsActive({0})", (object) active));
      if (this.ItemObjects.IsNullOrEmpty<ForcedHideObject>())
        return;
      foreach (ForcedHideObject itemObject in this.ItemObjects)
      {
        if (!Object.op_Equality((Object) itemObject, (Object) null))
        {
          if (active)
            itemObject.Show();
          else
            itemObject.Hide();
        }
      }
    }

    public void ShowItemObjects()
    {
      this.SetItemObjectsActive(true);
    }

    public void HideItemObjects()
    {
      this.SetItemObjectsActive(false);
    }

    public bool AnyActiveItemObjects()
    {
      if (this.ItemObjects.IsNullOrEmpty<ForcedHideObject>())
        return false;
      foreach (ForcedHideObject itemObject in this.ItemObjects)
      {
        if (Object.op_Inequality((Object) itemObject, (Object) null) && itemObject.Active)
          return true;
      }
      return false;
    }

    public void Refresh()
    {
      if (Object.op_Equality((Object) this._destination, (Object) null))
        this._destination = ((Component) this).get_transform();
      if (this._pointInfos != null)
        this._pointInfos.Clear();
      if (Object.op_Equality((Object) this.OwnerArea, (Object) null))
        return;
      this.AreaIDOnChunk = this.OwnerArea.AreaID;
      Dictionary<int, Dictionary<int, List<MerchantPointInfo>>> merchantPointInfoTable = Singleton<Resources>.Instance.Map.MerchantPointInfoTable;
      int key = !Singleton<Manager.Map>.IsInstance() ? 0 : Singleton<Manager.Map>.Instance.MapID;
      if (merchantPointInfoTable.ContainsKey(key) && merchantPointInfoTable[key].ContainsKey(this._actionID))
        this._pointInfos = merchantPointInfoTable[key][this._actionID];
      this._eventType = (Merchant.EventType) 0;
      foreach (MerchantPointInfo pointInfo in this._pointInfos)
        this._eventType |= pointInfo.eventTypeMask;
    }

    public void GetPointInfoList(
      Merchant.EventType eventType,
      ref List<MerchantPointInfo> outInfoList)
    {
      if (eventType == (Merchant.EventType) 0)
        return;
      outInfoList.Clear();
      foreach (MerchantPointInfo pointInfo in this._pointInfos)
      {
        if (pointInfo.eventTypeMask == eventType)
        {
          if (outInfoList == null)
            outInfoList = new List<MerchantPointInfo>();
          outInfoList.Add(pointInfo);
        }
      }
    }

    public bool TryGetPointInfo(Merchant.EventType eventType, out MerchantPointInfo pointInfo)
    {
      List<MerchantPointInfo> outInfoList = ListPool<MerchantPointInfo>.Get();
      this.GetPointInfoList(eventType, ref outInfoList);
      if (outInfoList.IsNullOrEmpty<MerchantPointInfo>())
      {
        ListPool<MerchantPointInfo>.Release(outInfoList);
        pointInfo = new MerchantPointInfo();
        return false;
      }
      pointInfo = outInfoList[Random.Range(0, outInfoList.Count)];
      ListPool<MerchantPointInfo>.Release(outInfoList);
      return true;
    }

    public Transform GetBasePoint(string baseNullName)
    {
      GameObject loop = ((Component) this).get_transform().FindLoop(baseNullName);
      return Object.op_Equality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
    }

    public Transform GetBasePoint(Merchant.EventType eventType)
    {
      MerchantPointInfo pointInfo;
      return !this.TryGetPointInfo(eventType, out pointInfo) ? ((Component) this).get_transform() : this.GetBasePoint(pointInfo.baseNullName);
    }

    public Transform GetRecoveryPoint(string recoveryNullName)
    {
      return ((Component) this).get_transform().FindLoop(recoveryNullName)?.get_transform();
    }

    public Transform GetRecoveryPoint(Merchant.EventType eventType)
    {
      MerchantPointInfo pointInfo;
      return !this.TryGetPointInfo(eventType, out pointInfo) ? ((Component) this).get_transform() : this.GetRecoveryPoint(pointInfo.recoveryNullName);
    }

    public Tuple<Transform, Transform> GetEventPoint(
      Merchant.EventType eventType)
    {
      MerchantPointInfo pointInfo;
      return !this.TryGetPointInfo(eventType, out pointInfo) ? new Tuple<Transform, Transform>(((Component) this).get_transform(), ((Component) this).get_transform()) : new Tuple<Transform, Transform>(this.GetBasePoint(pointInfo.baseNullName), this.GetRecoveryPoint(pointInfo.recoveryNullName));
    }

    public Tuple<MerchantPointInfo, Transform, Transform> GetEventInfo(
      Merchant.EventType eventType)
    {
      MerchantPointInfo pointInfo;
      this.TryGetPointInfo(eventType, out pointInfo);
      Transform basePoint = this.GetBasePoint(pointInfo.baseNullName);
      Transform recoveryPoint = this.GetRecoveryPoint(pointInfo.recoveryNullName);
      return new Tuple<MerchantPointInfo, Transform, Transform>(pointInfo, basePoint, recoveryPoint);
    }

    public bool CheckDistance(Vector3 position, float checkDistance)
    {
      if ((double) Vector3.Distance(this.Destination, position) <= (double) checkDistance)
        return true;
      if (!this.ItemObjects.IsNullOrEmpty<ForcedHideObject>())
      {
        foreach (ForcedHideObject itemObject in this.ItemObjects)
        {
          if (!Object.op_Equality((Object) itemObject, (Object) null) && (double) Vector3.Distance(((Component) itemObject).get_transform().get_position(), position) <= (double) checkDistance)
            return true;
        }
      }
      return false;
    }

    public void SetStand(
      MerchantActor merchant,
      Transform t,
      bool enableFade,
      float fadeTime,
      int dirc,
      System.Action onComplete = null)
    {
      if (Object.op_Equality((Object) merchant, (Object) null) || Object.op_Equality((Object) t, (Object) null))
      {
        System.Action action = onComplete;
        if (action == null)
          return;
        action();
      }
      else
      {
        IConnectableObservable<TimeInterval<float>> iconnectableObservable = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(fadeTime, false), false));
        IDisposable disposable1 = iconnectableObservable.Connect();
        merchant.DisposeSequenceAction();
        Vector3 position = merchant.Position;
        Quaternion rotation = merchant.Rotation;
        switch (dirc)
        {
          case 0:
            if (enableFade)
            {
              merchant.AddSequenceActionDisposable(disposable1);
              IDisposable disposable2 = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (System.Action<M0>) (x =>
              {
                merchant.Position = Vector3.Lerp(position, t.get_position(), ((TimeInterval<float>) ref x).get_Value());
                merchant.Rotation = Quaternion.Lerp(rotation, t.get_rotation(), ((TimeInterval<float>) ref x).get_Value());
              })), (Component) merchant);
              merchant.AddSequenceActionDisposable(disposable2);
              IDisposable disposable3 = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<TimeInterval<float>[]>((IObservable<M0>) Observable.WhenAll<TimeInterval<float>>((IObservable<M0>[]) new IObservable<TimeInterval<float>>[1]
              {
                (IObservable<TimeInterval<float>>) iconnectableObservable
              }), (System.Action<M0>) (_ =>
              {
                System.Action action = onComplete;
                if (action != null)
                  action();
                merchant.ClearSequenceAction();
              })), (Component) merchant);
              merchant.AddSequenceActionDisposable(disposable3);
              merchant.AddSequenceActionOnComplete(onComplete);
              break;
            }
            merchant.Position = t.get_position();
            merchant.Rotation = t.get_rotation();
            System.Action action1 = onComplete;
            if (action1 == null)
              break;
            action1();
            break;
          case 1:
            Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), merchant.Position);
            vector3.y = (__Null) 0.0;
            Quaternion lookRotation = Quaternion.LookRotation(((Vector3) ref vector3).get_normalized(), Vector3.get_up());
            if (enableFade)
            {
              IDisposable disposable2 = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (System.Action<M0>) (x => merchant.Rotation = Quaternion.Lerp(rotation, lookRotation, ((TimeInterval<float>) ref x).get_Value())));
              merchant.AddSequenceActionDisposable(disposable2);
              IDisposable disposable3 = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<TimeInterval<float>[]>((IObservable<M0>) Observable.WhenAll<TimeInterval<float>>((IObservable<M0>[]) new IObservable<TimeInterval<float>>[1]
              {
                (IObservable<TimeInterval<float>>) iconnectableObservable
              }), (System.Action<M0>) (_ =>
              {
                System.Action action = onComplete;
                if (action == null)
                  return;
                action();
              })), (Component) merchant);
              merchant.AddSequenceActionDisposable(disposable3);
              merchant.AddSequenceActionOnComplete(onComplete);
              break;
            }
            merchant.Rotation = lookRotation;
            System.Action action3 = onComplete;
            if (action3 == null)
              break;
            action3();
            break;
          default:
            System.Action action4 = onComplete;
            if (action4 == null)
              break;
            action4();
            break;
        }
      }
    }

    public void SetStand(
      Transform root,
      Transform t,
      bool enableFade,
      float fadeTime,
      int dirc,
      System.Action onComplete = null)
    {
      if (Object.op_Equality((Object) root, (Object) null) || Object.op_Equality((Object) t, (Object) null))
      {
        System.Action action = onComplete;
        if (action == null)
          return;
        action();
      }
      else
      {
        IConnectableObservable<TimeInterval<float>> iconnectableObservable = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(fadeTime, false), false));
        iconnectableObservable.Connect();
        Vector3 position = root.get_position();
        Quaternion rotation = root.get_rotation();
        switch (dirc)
        {
          case 0:
            if (enableFade)
            {
              ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (System.Action<M0>) (x =>
              {
                root.set_position(Vector3.Lerp(position, t.get_position(), ((TimeInterval<float>) ref x).get_Value()));
                root.set_rotation(Quaternion.Lerp(rotation, t.get_rotation(), ((TimeInterval<float>) ref x).get_Value()));
              }));
              ObservableExtensions.Subscribe<TimeInterval<float>[]>((IObservable<M0>) Observable.WhenAll<TimeInterval<float>>((IObservable<M0>[]) new IObservable<TimeInterval<float>>[1]
              {
                (IObservable<TimeInterval<float>>) iconnectableObservable
              }), (System.Action<M0>) (_ =>
              {
                System.Action action = onComplete;
                if (action == null)
                  return;
                action();
              }));
              break;
            }
            root.set_position(t.get_position());
            root.set_rotation(t.get_rotation());
            System.Action action1 = onComplete;
            if (action1 == null)
              break;
            action1();
            break;
          case 1:
            Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), root.get_position());
            vector3.y = (__Null) 0.0;
            Quaternion lookRotation = Quaternion.LookRotation(((Vector3) ref vector3).get_normalized(), Vector3.get_up());
            if (enableFade)
            {
              ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (System.Action<M0>) (x => root.set_rotation(Quaternion.Lerp(rotation, lookRotation, ((TimeInterval<float>) ref x).get_Value()))));
              ObservableExtensions.Subscribe<TimeInterval<float>[]>((IObservable<M0>) Observable.WhenAll<TimeInterval<float>>((IObservable<M0>[]) new IObservable<TimeInterval<float>>[1]
              {
                (IObservable<TimeInterval<float>>) iconnectableObservable
              }), (System.Action<M0>) (_ =>
              {
                System.Action action = onComplete;
                if (action == null)
                  return;
                action();
              }));
              break;
            }
            root.set_rotation(lookRotation);
            System.Action action3 = onComplete;
            if (action3 == null)
              break;
            action3();
            break;
          default:
            System.Action action4 = onComplete;
            if (action4 == null)
              break;
            action4();
            break;
        }
      }
    }
  }
}
