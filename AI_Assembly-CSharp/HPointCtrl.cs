// Decompiled with JetBrains decompiler
// Type: HPointCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEx;

public class HPointCtrl : Singleton<HPointCtrl>
{
  public List<ValueTuple<GameObject, HPoint>> lstMarker = new List<ValueTuple<GameObject, HPoint>>();
  public int HEnterCategory = -1;
  private Dictionary<int, HPointList> hPointLists = new Dictionary<int, HPointList>();
  private RaycastHit[] HitInfo = new RaycastHit[50];
  private RaycastHit[] HitInfo2 = new RaycastHit[100];
  public List<HScene.AnimationListInfo>[] lstAnimInfo = new List<HScene.AnimationListInfo>[6];
  [SerializeField]
  private int gpID = -1;
  private ShuffleRand rand = new ShuffleRand(-1);
  private List<ForcedHideObject> CheckHitObjs = new List<ForcedHideObject>();
  private int[] LesPlaceID = new int[8]
  {
    0,
    1,
    2,
    3,
    4,
    5,
    6,
    13
  };
  private int[] multiFemalePlaceID = new int[1];
  private int dildoPlaceID = 21;
  private int NotMerchantPlaceID = 5;
  private int kadOnaID = 20;
  private Dictionary<int, Dictionary<int, HPointCtrl.AreaGroupDefine>> Areagp = new Dictionary<int, Dictionary<int, HPointCtrl.AreaGroupDefine>>()
  {
    {
      0,
      new Dictionary<int, HPointCtrl.AreaGroupDefine>()
      {
        {
          0,
          new HPointCtrl.AreaGroupDefine()
          {
            IDs = new List<int>() { 0, 11, 14 }
          }
        },
        {
          1,
          new HPointCtrl.AreaGroupDefine()
          {
            IDs = new List<int>() { 4, 12, 15, 16 }
          }
        },
        {
          2,
          new HPointCtrl.AreaGroupDefine()
          {
            IDs = new List<int>() { 6, 13, 17 }
          }
        }
      }
    },
    {
      1,
      new Dictionary<int, HPointCtrl.AreaGroupDefine>()
      {
        {
          3,
          new HPointCtrl.AreaGroupDefine()
          {
            IDs = new List<int>() { 0, 1 }
          }
        }
      }
    }
  };
  private float OffsetDownHeight = 5f;
  private float OffsetUpHeight = 20f;
  [SerializeField]
  private GameObject initNullObj;
  private HScene hScene;
  private HSceneManager hSceneManager;
  private HSceneSprite hSceneSprite;
  private Camera Cam;
  private ValueTuple<Vector3, Quaternion, int> _InitNull;
  private HPoint[] housingHpoints;
  public bool IsMarker;
  private HSceneFlagCtrl ctrlFlag;
  public int playerSex;
  public bool ExistSecondfemale;
  private List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> startList;
  private List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> startListM;
  public bool InitUsePoint;
  private BasePoint[] BasePoints;
  private CrossFade fade;

  public ValueTuple<Vector3, Quaternion, int> InitNull
  {
    get
    {
      return this._InitNull;
    }
  }

  public Dictionary<int, HPointList> HPointLists
  {
    get
    {
      return this.hPointLists;
    }
  }

  public HPoint[] HousingHpoints
  {
    get
    {
      return this.housingHpoints;
    }
  }

  public void InitHPoint()
  {
    this.hSceneManager = Singleton<HSceneManager>.Instance;
    this.startList = Singleton<Resources>.Instance.HSceneTable.lstStartAnimInfo;
    this.startListM = Singleton<Resources>.Instance.HSceneTable.lstStartAnimInfoM;
    this.lstAnimInfo = Singleton<Resources>.Instance.HSceneTable.lstAnimInfo;
    this.initNullObj = Singleton<Resources>.Instance.HSceneTable.HPointObj;
    this.AreagpInit();
    Dictionary<int, Dictionary<bool, ForcedHideObject[]>> areaOpenObjectTable = Singleton<Manager.Map>.Instance.AreaOpenObjectTable;
    this.CheckHitObjs.Clear();
    if (areaOpenObjectTable != null && areaOpenObjectTable.Count > 0)
    {
      foreach (KeyValuePair<int, Dictionary<bool, ForcedHideObject[]>> keyValuePair in areaOpenObjectTable)
      {
        if (keyValuePair.Value != null && keyValuePair.Value.Count != 0)
        {
          int key = keyValuePair.Key;
          ForcedHideObject[] forcedHideObjectArray;
          if (keyValuePair.Value.TryGetValue(false, out forcedHideObjectArray))
            this.CheckHitObjs.AddRange((IEnumerable<ForcedHideObject>) forcedHideObjectArray);
        }
      }
    }
    if (!Object.op_Equality((Object) this.fade, (Object) null))
      return;
    this.fade = this.hSceneManager.Player.CameraControl.CrossFade;
  }

  public void MarkerObjSet(Vector3 PlayerPos, int mapID = 0, int AreaID = 0)
  {
    if (!this.InitUsePoint)
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.initNullObj.get_transform().get_position(), PlayerPos);
      if ((double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) this.hSceneSprite.HpointSearchRange * (double) this.hSceneSprite.HpointSearchRange)
      {
        ((HPoint) this.lstMarker[0].Item2).SetEffectActive(true);
        Collider collider = ((HPoint) this.lstMarker[0].Item2).GetCollider();
        if (Object.op_Inequality((Object) collider, (Object) null))
          collider.set_enabled(true);
      }
    }
    if (this.gpID == -1)
    {
      if (this.hPointLists.ContainsKey(mapID) && this.hPointLists[mapID].lst.ContainsKey(AreaID))
      {
        for (int index1 = 1; index1 < this.lstMarker.Count; ++index1)
        {
          int index2 = index1;
          HPoint point = (HPoint) this.lstMarker[index2].Item2;
          if (this.hPointLists[mapID].lst[AreaID].Contains(point))
          {
            Vector3 vector3 = Vector3.op_Subtraction(((Component) point).get_transform().get_position(), PlayerPos);
            if ((double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) this.hSceneSprite.HpointSearchRange * (double) this.hSceneSprite.HpointSearchRange)
            {
              if (this.playerSex == 1)
              {
                if (!this.CheckPlace(point._nPlace, 0))
                  continue;
              }
              else if (this.ExistSecondfemale && !this.CheckPlace(point._nPlace, 1))
                continue;
              bool flag = false;
              using (Dictionary<int, ValueTuple<int, int>>.ValueCollection.Enumerator enumerator = point._nPlace.Values.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  if (enumerator.Current.Item1 == this.dildoPlaceID)
                  {
                    flag = true;
                    break;
                  }
                }
              }
              if (!flag && this.CanInfo(point._nPlace) && (!this.HitObstacle(PlayerPos, point) && this.HpointAreaOpen(point)))
              {
                ((HPoint) this.lstMarker[index2].Item2).SetEffectActive(true);
                Collider collider = ((HPoint) this.lstMarker[index2].Item2).GetCollider();
                if (Object.op_Inequality((Object) collider, (Object) null))
                  collider.set_enabled(true);
              }
            }
          }
        }
      }
    }
    else if (this.hPointLists.ContainsKey(mapID))
    {
      Dictionary<int, HPointCtrl.AreaGroupDefine> dictionary;
      if (!this.Areagp.TryGetValue(mapID, out dictionary))
        return;
      for (int index1 = 0; index1 < dictionary[this.gpID].IDs.Count; ++index1)
      {
        if (this.hPointLists[mapID].lst.ContainsKey(dictionary[this.gpID].IDs[index1]))
        {
          for (int index2 = 1; index2 < this.lstMarker.Count; ++index2)
          {
            int index3 = index2;
            HPoint point = (HPoint) this.lstMarker[index3].Item2;
            if (this.hPointLists[mapID].lst[dictionary[this.gpID].IDs[index1]].Contains(point))
            {
              Vector3 vector3 = Vector3.op_Subtraction(((Component) point).get_transform().get_position(), PlayerPos);
              if ((double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) this.hSceneSprite.HpointSearchRange * (double) this.hSceneSprite.HpointSearchRange)
              {
                if (this.playerSex == 1)
                {
                  if (!this.CheckPlace(point._nPlace, 0))
                    continue;
                }
                else if (this.ExistSecondfemale && !this.CheckPlace(point._nPlace, 1))
                  continue;
                bool flag = false;
                using (Dictionary<int, ValueTuple<int, int>>.ValueCollection.Enumerator enumerator = point._nPlace.Values.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    if (enumerator.Current.Item1 == this.dildoPlaceID)
                    {
                      flag = true;
                      break;
                    }
                  }
                }
                if (!flag && this.CanInfo(point._nPlace) && (!this.HitObstacle(PlayerPos, point) && this.HpointAreaOpen(point)))
                {
                  ((HPoint) this.lstMarker[index3].Item2).SetEffectActive(true);
                  Collider collider = ((HPoint) this.lstMarker[index3].Item2).GetCollider();
                  if (Object.op_Inequality((Object) collider, (Object) null))
                    collider.set_enabled(true);
                }
              }
            }
          }
        }
      }
    }
    if (this.housingHpoints != null && this.housingHpoints.Length != 0)
    {
      for (int index1 = 1; index1 < this.lstMarker.Count; ++index1)
      {
        int index2 = index1;
        HPoint hpoint = (HPoint) this.lstMarker[index2].Item2;
        if (this.HousingListContain(hpoint))
        {
          Vector3 vector3 = Vector3.op_Subtraction(((Component) hpoint).get_transform().get_position(), PlayerPos);
          if ((double) ((Vector3) ref vector3).get_sqrMagnitude() <= (double) this.hSceneSprite.HpointSearchRange * (double) this.hSceneSprite.HpointSearchRange)
          {
            if (this.playerSex == 1)
            {
              if (!this.CheckPlace(hpoint._nPlace, 0))
                continue;
            }
            else if (this.ExistSecondfemale && !this.CheckPlace(hpoint._nPlace, 1))
              continue;
            bool flag = false;
            using (Dictionary<int, ValueTuple<int, int>>.ValueCollection.Enumerator enumerator = hpoint._nPlace.Values.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current.Item1 == this.dildoPlaceID)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (!flag && this.CanInfo(hpoint._nPlace) && (!this.HitObstacle(PlayerPos, hpoint) && this.HpointAreaOpen(hpoint)))
            {
              ((HPoint) this.lstMarker[index2].Item2).SetEffectActive(true);
              Collider collider = ((HPoint) this.lstMarker[index2].Item2).GetCollider();
              if (Object.op_Inequality((Object) collider, (Object) null))
                collider.set_enabled(true);
            }
          }
        }
      }
    }
    this.IsMarker = true;
  }

  public void MarkerObjDel()
  {
    for (int index = 0; index < this.lstMarker.Count; ++index)
    {
      if (((HPoint) this.lstMarker[index].Item2).EffectActive())
      {
        ((HPoint) this.lstMarker[index].Item2).SetEffectActive(false);
        Collider collider = ((HPoint) this.lstMarker[index].Item2).GetCollider();
        if (Object.op_Inequality((Object) collider, (Object) null))
          collider.set_enabled(false);
      }
    }
    this.IsMarker = false;
  }

  private bool HousingListContain(HPoint target)
  {
    if (this.housingHpoints == null)
      return false;
    for (int index = 0; index < this.housingHpoints.Length; ++index)
    {
      if (Object.op_Equality((Object) this.housingHpoints[index], (Object) target))
        return true;
    }
    return false;
  }

  public void Init(int mapID = 0, int AreaID = 0)
  {
    this.lstMarker.Clear();
    this.housingHpoints = (HPoint[]) null;
    this.lstMarker.Add(new ValueTuple<GameObject, HPoint>((GameObject) null, (HPoint) this.initNullObj.GetComponent<HPoint>()));
    this.hPointLists = Singleton<Resources>.Instance.HSceneTable.hPointLists;
    this.gpID = -1;
    HPointList hPointList = this.hPointLists[mapID];
    Dictionary<int, HPointCtrl.AreaGroupDefine> dictionary;
    if (!this.Areagp.TryGetValue(mapID, out dictionary))
      return;
    foreach (KeyValuePair<int, HPointCtrl.AreaGroupDefine> keyValuePair in dictionary)
    {
      if (keyValuePair.Value.IDs.Contains(AreaID))
      {
        this.gpID = keyValuePair.Key;
        for (int index1 = 0; index1 < keyValuePair.Value.IDs.Count; ++index1)
        {
          int id = keyValuePair.Value.IDs[index1];
          if (hPointList.lst.ContainsKey(id))
          {
            for (int index2 = 0; index2 < hPointList.lst[id].Count; ++index2)
              this.lstMarker.Add(new ValueTuple<GameObject, HPoint>(((Component) hPointList.lst[id][index2].markerPos).get_gameObject(), hPointList.lst[id][index2]));
          }
        }
        break;
      }
    }
    if (this.gpID == -1 && hPointList.lst.ContainsKey(AreaID))
    {
      for (int index = 0; index < hPointList.lst[AreaID].Count; ++index)
        this.lstMarker.Add(new ValueTuple<GameObject, HPoint>(((Component) hPointList.lst[AreaID][index].markerPos).get_gameObject(), hPointList.lst[AreaID][index]));
    }
    if (!Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.PointAgent, (Object) null) || Singleton<Manager.Map>.Instance.PointAgent.BasePoints == null)
      return;
    this.BasePoints = Singleton<Manager.Map>.Instance.PointAgent.BasePoints;
    for (int index = 0; index < this.BasePoints.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.BasePoints[index].OwnerArea, (Object) null) && this.gpID != -1 && dictionary[this.gpID].IDs.Contains(this.BasePoints[index].OwnerArea.AreaID))
        this.housingHpoints = Singleton<Housing>.Instance.GetHPoint(this.BasePoints[index].ID);
    }
    if (this.housingHpoints == null)
      return;
    for (int index = 0; index < this.housingHpoints.Length; ++index)
      this.lstMarker.Add(new ValueTuple<GameObject, HPoint>(((Component) this.housingHpoints[index].markerPos).get_gameObject(), this.housingHpoints[index]));
  }

  public void SetStartPos(Transform trans, int basho)
  {
    this._InitNull = new ValueTuple<Vector3, Quaternion, int>(trans.get_position(), trans.get_rotation(), basho);
    HPoint component = (HPoint) this.initNullObj.GetComponent<HPoint>();
    component.id = -1;
    component.Init();
    if (!component._nPlace.ContainsKey(0))
      component._nPlace.Add(0, new ValueTuple<int, int>((int) this._InitNull.Item3, this.HEnterCategory));
    else
      component._nPlace[0] = new ValueTuple<int, int>((int) this._InitNull.Item3, this.HEnterCategory);
    this.lstMarker[0] = new ValueTuple<GameObject, HPoint>(((Component) component.markerPos).get_gameObject(), component);
    if (Object.op_Inequality((Object) this.initNullObj.get_transform().get_parent(), (Object) ((Component) this).get_transform()))
      this.initNullObj.get_transform().SetParent(((Component) this).get_transform());
    this.initNullObj.get_transform().set_position((Vector3) this._InitNull.Item1);
    this.initNullObj.get_transform().set_rotation((Quaternion) this._InitNull.Item2);
    if (this.InitUsePoint)
      return;
    this.ctrlFlag.nowHPoint = component;
  }

  private void Update()
  {
    if (!this.IsMarker)
      return;
    int hitID = -1;
    if (!this.hSceneSprite.IsSpriteOver())
    {
      int num = Physics.RaycastNonAlloc(this.Cam.ScreenPointToRay(Input.get_mousePosition()), this.HitInfo, 1000f, 1);
      if (num > 0)
      {
        Array.Sort<RaycastHit>(this.HitInfo, 0, num, (IComparer<RaycastHit>) new HPointCtrl.RayDistanceCompare());
        hitID = this.HitObjectFind(num);
      }
    }
    if (hitID == -1 || this.ctrlFlag.nowOrgasm || (this.hScene.NowChangeAnim || !Singleton<Input>.Instance.IsPressedKey(ActionID.MouseLeft)))
      return;
    this.ctrlFlag.pointMoveAnimChange = true;
    HScene.AnimationListInfo nowAnimationInfo = this.ctrlFlag.nowAnimationInfo;
    this.fade.FadeStart(1.5f);
    this.OffSetMove(hitID, nowAnimationInfo);
    if (((HPoint) this.lstMarker[this.ctrlFlag.HPointID].Item2).id != ((HPoint) this.lstMarker[hitID].Item2).id)
      this.ChangeAnim(hitID);
    else
      this.ctrlFlag.pointMoveAnimChange = false;
    this.ctrlFlag.HPointID = hitID;
    ChangeHItem componentInChildren1 = (ChangeHItem) ((Component) this.ctrlFlag.nowHPoint).GetComponentInChildren<ChangeHItem>();
    if (Object.op_Inequality((Object) componentInChildren1, (Object) null))
      componentInChildren1.ChangeActive(true);
    ((HPoint) this.lstMarker[this.ctrlFlag.HPointID].Item2).SetEffectActive(false);
    ChangeHItem componentInChildren2 = (ChangeHItem) ((Component) this.lstMarker[this.ctrlFlag.HPointID].Item2).GetComponentInChildren<ChangeHItem>();
    if (Object.op_Inequality((Object) componentInChildren2, (Object) null))
      componentInChildren2.ChangeActive(false);
    this.ctrlFlag.nowHPoint = (HPoint) this.lstMarker[this.ctrlFlag.HPointID].Item2;
    this.hSceneSprite.RefleshAutoButtom();
    this.hSceneSprite.usePoint = true;
    this.hSceneSprite.SetMotionListDraw(false, -1);
    this.hSceneSprite.LoadMotionList(this.ctrlFlag.categoryMotionList);
    ((Component) this.hSceneSprite.categoryMain).get_gameObject().SetActive(false);
    this.hSceneSprite.MarkerObjSet();
  }

  private void OffSetMove(int hitID, HScene.AnimationListInfo info)
  {
    Vector3 pos = Vector3.get_zero();
    Vector3 rot = Vector3.get_zero();
    int index1 = -1;
    for (int index2 = 0; index2 < info.lstOffset.Count; ++index2)
    {
      if (((HPoint) this.lstMarker[hitID].Item2)._nPlace[0].Item1 == info.nPositons[index2])
      {
        index1 = index2;
        break;
      }
    }
    if (index1 >= 0 && info.lstOffset[index1] != null && info.lstOffset[index1] != string.Empty)
      this.hScene.LoadMoveOffset(info.lstOffset[index1], out pos, out rot);
    this.hScene.SetMovePositionPoint(((HPoint) this.lstMarker[hitID].Item2).pivot, pos, rot);
  }

  private void ChangeAnim(int hitID)
  {
    List<HScene.AnimationListInfo> infos = new List<HScene.AnimationListInfo>();
    int num = (int) ((HPoint) this.lstMarker[hitID].Item2)._nPlace[0].Item1;
    if (this.hSceneManager.bMerchant && num >= this.NotMerchantPlaceID)
    {
      for (int index = 0; index < ((HPoint) this.lstMarker[hitID].Item2)._nPlace.Count; ++index)
      {
        if (((HPoint) this.lstMarker[hitID].Item2)._nPlace[index].Item1 < this.NotMerchantPlaceID)
        {
          num = (int) ((HPoint) this.lstMarker[hitID].Item2)._nPlace[index].Item1;
          break;
        }
      }
    }
    if (this.ctrlFlag.nowAnimationInfo.nPositons.Contains(num))
    {
      this.ctrlFlag.pointMoveAnimChange = false;
      this.ctrlFlag.nPlace = num;
    }
    else
    {
      List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> valueTupleList = this.startList;
      if (this.hSceneManager.bMerchant)
        valueTupleList = this.startListM;
      if (this.playerSex == 0)
      {
        for (int index1 = 0; index1 < valueTupleList.Count; ++index1)
        {
          if (valueTupleList[index1].Item2 == num && (!this.ExistSecondfemale || ((HScene.StartMotion) valueTupleList[index1].Item3).mode == 5) && (this.ExistSecondfemale || ((HScene.StartMotion) valueTupleList[index1].Item3).mode != 5))
          {
            this.ctrlFlag.nPlace = num;
            int mode = ((HScene.StartMotion) valueTupleList[index1].Item3).mode;
            int id = ((HScene.StartMotion) valueTupleList[index1].Item3).id;
            if (mode != 4 && (!this.hSceneManager.bMerchant || this.hSceneManager.MerchantLimit != 1 || mode == 1) && (!((HPoint) this.lstMarker[hitID].Item2).notMotion[mode].motionID.Contains(id) && this.lstAnimInfo.Length >= mode))
            {
              int index2 = -1;
              for (int index3 = 0; index3 < this.lstAnimInfo[mode].Count; ++index3)
              {
                if (this.lstAnimInfo[mode][index3].id == id)
                  index2 = index3;
              }
              if (index2 != -1)
              {
                if (this.hSceneManager.bMerchant)
                {
                  if (!this.lstAnimInfo[mode][index2].bMerchantMotion || this.lstAnimInfo[mode][index2].nIyaAction == 2)
                    continue;
                }
                else if (this.hSceneManager.EventKind != HSceneManager.HEvent.Yobai)
                {
                  if (this.hSceneManager.isForce)
                  {
                    if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                      continue;
                  }
                  else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                    continue;
                }
                else if (!this.lstAnimInfo[mode][index2].bSleep)
                  continue;
                if (!this.lstAnimInfo[mode][index2].isNeedItem || this.hSceneManager.CheckHadItem((int) this.lstAnimInfo[mode][index2].ActionCtrl.Item1, this.lstAnimInfo[mode][index2].id))
                {
                  if (this.ctrlFlag.isFaintness)
                  {
                    if (this.lstAnimInfo[mode][index2].nDownPtn == 0)
                      continue;
                  }
                  else if (this.lstAnimInfo[mode][index2].nFaintnessLimit == 1)
                    continue;
                  if (this.lstAnimInfo[mode][index2].nInitiativeFemale == 0)
                    infos.Add(this.lstAnimInfo[mode][index2]);
                }
              }
            }
          }
        }
        if (infos.Count == 0)
          return;
        int randID = 0;
        this.rand.Init(infos.Count);
        randID = this.rand.Get();
        ObservableExtensions.Subscribe<Unit>(Observable.Finally<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.hScene.ChangeAnimation(infos[randID], false, false, false)), false), (Action) (() =>
        {
          this.ctrlFlag.selectAnimationListInfo = (HScene.AnimationListInfo) null;
          this.ctrlFlag.isAutoActionChange = false;
          if (this.ctrlFlag.pointMoveAnimChange)
            this.ctrlFlag.pointMoveAnimChange = false;
          GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, true);
          this.hSceneSprite.ChangeStart = false;
        })));
      }
      else
      {
        if (this.playerSex != 1)
          return;
        for (int index1 = 0; index1 < valueTupleList.Count; ++index1)
        {
          if (valueTupleList[index1].Item2 == num && ((HScene.StartMotion) valueTupleList[index1].Item3).mode == 4)
          {
            this.ctrlFlag.nPlace = num;
            int mode = ((HScene.StartMotion) valueTupleList[index1].Item3).mode;
            int id = ((HScene.StartMotion) valueTupleList[index1].Item3).id;
            if (!((HPoint) this.lstMarker[hitID].Item2).notMotion[mode].motionID.Contains(id) && this.lstAnimInfo.Length >= mode)
            {
              int index2 = -1;
              for (int index3 = 0; index3 < this.lstAnimInfo[mode].Count; ++index3)
              {
                if (this.lstAnimInfo[mode][index3].id == id)
                  index2 = index3;
              }
              if (index2 != -1 && (!this.hSceneManager.bMerchant || this.lstAnimInfo[mode][index2].bMerchantMotion) && (!this.lstAnimInfo[mode][index2].isNeedItem || this.hSceneManager.CheckHadItem((int) this.lstAnimInfo[mode][index2].ActionCtrl.Item1, this.lstAnimInfo[mode][index2].id)))
              {
                if (this.ctrlFlag.isFaintness)
                {
                  if (this.lstAnimInfo[mode][index2].nDownPtn == 0)
                    continue;
                }
                else if (this.lstAnimInfo[mode][index2].nFaintnessLimit == 1)
                  continue;
                if (this.lstAnimInfo[mode][index2].nInitiativeFemale == 0)
                  infos.Add(this.lstAnimInfo[mode][index2]);
              }
            }
          }
        }
        if (infos.Count == 0)
          return;
        int randID = 0;
        this.rand.Init(infos.Count);
        randID = this.rand.Get();
        ObservableExtensions.Subscribe<Unit>(Observable.Finally<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.hScene.ChangeAnimation(infos[randID], false, false, false)), false), (Action) (() =>
        {
          this.ctrlFlag.selectAnimationListInfo = (HScene.AnimationListInfo) null;
          this.ctrlFlag.isAutoActionChange = false;
          if (this.ctrlFlag.pointMoveAnimChange)
            this.ctrlFlag.pointMoveAnimChange = false;
          GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, true);
          this.hSceneSprite.ChangeStart = false;
        })));
      }
    }
  }

  public static bool DicTupleContainsValue(
    Dictionary<int, ValueTuple<int, int>> dic,
    int target,
    int item)
  {
    using (Dictionary<int, ValueTuple<int, int>>.Enumerator enumerator = dic.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<int, ValueTuple<int, int>> current = enumerator.Current;
        if (item == 0 && current.Value.Item1 == target || item == 1 && current.Value.Item2 == target)
          return true;
      }
    }
    return false;
  }

  public static bool DicTupleContainsValue(
    Dictionary<int, ValueTuple<int, int>> dic,
    List<int> target,
    int item)
  {
    using (Dictionary<int, ValueTuple<int, int>>.Enumerator enumerator = dic.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<int, ValueTuple<int, int>> current = enumerator.Current;
        if (item == 0 && target.Contains((int) current.Value.Item1) || item == 1 && target.Contains((int) current.Value.Item2))
          return true;
      }
    }
    return false;
  }

  public void SetHSceneSprite(HSceneSprite sprite, HScene HScene)
  {
    this.ctrlFlag = Singleton<HSceneFlagCtrl>.Instance;
    this.hSceneSprite = sprite;
    this.hScene = HScene;
  }

  public void CameraSet()
  {
    this.Cam = this.ctrlFlag.cameraCtrl.thisCamera;
  }

  private int HitObjectFind(int hitNum)
  {
    int num = -1;
    for (int index1 = 0; index1 < hitNum; ++index1)
    {
      for (int index2 = 0; index2 < this.lstMarker.Count; ++index2)
      {
        Collider collider = ((HPoint) this.lstMarker[index2].Item2).GetCollider();
        if (!Object.op_Equality((Object) collider, (Object) null) && !Object.op_Inequality((Object) collider, (Object) ((RaycastHit) ref this.HitInfo[index1]).get_collider()) && index2 != this.ctrlFlag.HPointID)
          return index2;
      }
    }
    return num;
  }

  public bool CheckStartPoint(ref Transform res, int place = -2)
  {
    int mapId = Singleton<Manager.Map>.Instance.MapID;
    int areaId = this.hSceneManager.Player.AreaID;
    HPointCtrl.MinHpoint minHpoint1;
    minHpoint1.AreaID = -1;
    minHpoint1.PointID = -1;
    int index1 = -1;
    List<HPointCtrl.MinHpoint> minHpointList = new List<HPointCtrl.MinHpoint>();
    this.gpID = -1;
    Dictionary<int, HPointCtrl.AreaGroupDefine> dictionary;
    if (!this.Areagp.TryGetValue(mapId, out dictionary))
      return false;
    foreach (KeyValuePair<int, HPointCtrl.AreaGroupDefine> keyValuePair in dictionary)
    {
      if (keyValuePair.Value.IDs.Contains(areaId))
      {
        this.gpID = keyValuePair.Key;
        break;
      }
    }
    List<bool> boolList = new List<bool>();
    if (this.gpID == -1)
    {
      boolList.Add(this.hPointLists.Count == 0 || !this.hPointLists.ContainsKey(mapId) || !this.hPointLists[mapId].lst.ContainsKey(areaId) || this.hPointLists[mapId].lst[areaId].Count == 0);
    }
    else
    {
      for (int index2 = 0; index2 < dictionary[this.gpID].IDs.Count; ++index2)
      {
        int id = dictionary[this.gpID].IDs[index2];
        boolList.Add(this.hPointLists.Count == 0 || !this.hPointLists.ContainsKey(mapId) || !this.hPointLists[mapId].lst.ContainsKey(id) || this.hPointLists[mapId].lst[id].Count == 0);
      }
    }
    bool flag1 = this.housingHpoints == null || this.housingHpoints.Length == 0;
    bool[] flagArray = new bool[2];
    if (!boolList.Contains(false) && flag1)
      return false;
    if (boolList.Contains(false))
    {
      if (this.gpID == -1)
      {
        minHpoint1.AreaID = areaId;
        minHpoint1.PointID = this.PlayerClosePointID(res, this.hPointLists[mapId].lst[areaId], place);
      }
      else
      {
        for (int index2 = 0; index2 < dictionary[this.gpID].IDs.Count; ++index2)
        {
          if (!boolList[index2])
            minHpointList.Add(new HPointCtrl.MinHpoint()
            {
              AreaID = dictionary[this.gpID].IDs[index2],
              PointID = this.PlayerClosePointID(res, this.hPointLists[mapId].lst[dictionary[this.gpID].IDs[index2]], place)
            });
        }
        foreach (HPointCtrl.MinHpoint minHpoint2 in minHpointList)
        {
          if (minHpoint2.PointID != -1)
          {
            if (minHpoint1.AreaID == -1 || minHpoint1.PointID == -1)
            {
              minHpoint1 = minHpoint2;
            }
            else
            {
              Vector3 vector3_1 = Vector3.op_Subtraction(((Component) this.hPointLists[mapId].lst[minHpoint2.AreaID][minHpoint2.PointID]).get_transform().get_position(), res.get_position());
              float sqrMagnitude1 = ((Vector3) ref vector3_1).get_sqrMagnitude();
              Vector3 vector3_2 = Vector3.op_Subtraction(((Component) this.hPointLists[mapId].lst[minHpoint1.AreaID][minHpoint1.PointID]).get_transform().get_position(), res.get_position());
              float sqrMagnitude2 = ((Vector3) ref vector3_2).get_sqrMagnitude();
              if ((double) sqrMagnitude1 < (double) sqrMagnitude2)
              {
                minHpoint1.AreaID = minHpoint2.AreaID;
                minHpoint1.PointID = minHpoint2.PointID;
              }
            }
          }
        }
      }
    }
    if (minHpoint1.AreaID != -1 && minHpoint1.PointID != -1)
      flagArray[0] = true;
    if (!flag1)
      index1 = this.PlayerClosePointID(res, this.housingHpoints, place);
    if (index1 != -1)
      flagArray[1] = true;
    if (!flagArray[0] && !flagArray[1])
      return false;
    bool flag2 = true;
    if (flagArray[0] && flagArray[1])
    {
      Vector3 vector3_1 = Vector3.op_Subtraction(((Component) this.hPointLists[mapId].lst[minHpoint1.AreaID][minHpoint1.PointID]).get_transform().get_position(), res.get_position());
      double sqrMagnitude1 = (double) ((Vector3) ref vector3_1).get_sqrMagnitude();
      Vector3 vector3_2 = Vector3.op_Subtraction(((Component) this.HousingHpoints[index1]).get_transform().get_position(), res.get_position());
      double sqrMagnitude2 = (double) ((Vector3) ref vector3_2).get_sqrMagnitude();
      if (sqrMagnitude1 - sqrMagnitude2 >= 0.0)
      {
        res.set_position(this.HousingHpoints[index1].pivot.get_position());
        res.set_rotation(this.HousingHpoints[index1].pivot.get_rotation());
        this.hSceneManager.height = Object.op_Equality((Object) this.hSceneManager.females[1], (Object) null) || Object.op_Inequality((Object) ((Component) this.hSceneManager.females[1]).GetComponent<PlayerActor>(), (Object) null) ? (place != -2 ? place : (int) this.housingHpoints[index1]._nPlace[0].Item1) : 0;
        flag2 = false;
      }
      else
      {
        res.set_position(this.hPointLists[mapId].lst[minHpoint1.AreaID][minHpoint1.PointID].pivot.get_position());
        res.set_rotation(this.hPointLists[mapId].lst[minHpoint1.AreaID][minHpoint1.PointID].pivot.get_rotation());
        this.hSceneManager.height = Object.op_Equality((Object) this.hSceneManager.females[1], (Object) null) || Object.op_Inequality((Object) ((Component) this.hSceneManager.females[1]).GetComponent<PlayerActor>(), (Object) null) ? (place != -2 ? place : (int) this.housingHpoints[index1]._nPlace[0].Item1) : 0;
        flag2 = true;
      }
    }
    else if (flagArray[0])
    {
      res.set_position(this.hPointLists[mapId].lst[minHpoint1.AreaID][minHpoint1.PointID].pivot.get_position());
      res.set_rotation(this.hPointLists[mapId].lst[minHpoint1.AreaID][minHpoint1.PointID].pivot.get_rotation());
      this.hSceneManager.height = Object.op_Equality((Object) this.hSceneManager.females[1], (Object) null) || Object.op_Inequality((Object) ((Component) this.hSceneManager.females[1]).GetComponent<PlayerActor>(), (Object) null) ? (place != -2 ? place : (int) this.housingHpoints[index1]._nPlace[0].Item1) : 0;
      flag2 = true;
    }
    else if (flagArray[1])
    {
      res.set_position(this.HousingHpoints[index1].pivot.get_position());
      res.set_rotation(this.HousingHpoints[index1].pivot.get_rotation());
      this.hSceneManager.height = Object.op_Equality((Object) this.hSceneManager.females[1], (Object) null) || Object.op_Inequality((Object) ((Component) this.hSceneManager.females[1]).GetComponent<PlayerActor>(), (Object) null) ? (place != -2 ? place : (int) this.housingHpoints[index1]._nPlace[0].Item1) : 0;
      flag2 = false;
    }
    if (flag2)
    {
      int num = 0;
      if (this.gpID != -1)
      {
        foreach (KeyValuePair<int, List<HPoint>> keyValuePair in this.hPointLists[mapId].lst)
        {
          if (dictionary.ContainsKey(this.gpID) && dictionary[this.gpID].IDs.Contains(keyValuePair.Key) && keyValuePair.Key < minHpoint1.AreaID)
            num += keyValuePair.Value.Count;
        }
      }
      this.ctrlFlag.HPointID = num + minHpoint1.PointID + 1;
      this.ctrlFlag.nowHPoint = this.hPointLists[mapId].lst[minHpoint1.AreaID][minHpoint1.PointID];
    }
    else
    {
      if (!boolList.Contains(false))
        this.ctrlFlag.HPointID = index1 + 1;
      else if (this.gpID == -1)
      {
        this.ctrlFlag.HPointID = index1 + 1 + this.hPointLists[mapId].lst[areaId].Count;
      }
      else
      {
        int num = 0;
        for (int index2 = 0; index2 < dictionary[this.gpID].IDs.Count; ++index2)
        {
          if (!boolList[index2])
            num += this.hPointLists[mapId].lst[dictionary[this.gpID].IDs[index2]].Count;
        }
        this.ctrlFlag.HPointID = index1 + 1 + num;
      }
      this.ctrlFlag.nowHPoint = this.housingHpoints[index1];
    }
    ChangeHItem componentInChildren = (ChangeHItem) ((Component) this.ctrlFlag.nowHPoint).GetComponentInChildren<ChangeHItem>();
    if (Object.op_Inequality((Object) componentInChildren, (Object) null))
      componentInChildren.ChangeActive(false);
    return true;
  }

  public void HousingHStart(HPoint hPoint)
  {
    int mapId = Singleton<Manager.Map>.Instance.MapID;
    int areaId = this.hSceneManager.Player.AreaID;
    this.gpID = -1;
    Dictionary<int, HPointCtrl.AreaGroupDefine> dictionary;
    if (!this.Areagp.TryGetValue(mapId, out dictionary))
      return;
    foreach (KeyValuePair<int, HPointCtrl.AreaGroupDefine> keyValuePair in dictionary)
    {
      if (keyValuePair.Value.IDs.Contains(areaId))
      {
        this.gpID = keyValuePair.Key;
        break;
      }
    }
    List<bool> boolList = new List<bool>();
    if (this.gpID == -1)
    {
      boolList.Add(this.hPointLists.Count == 0 || !this.hPointLists.ContainsKey(mapId) || !this.hPointLists[mapId].lst.ContainsKey(areaId) || this.hPointLists[mapId].lst[areaId].Count == 0);
    }
    else
    {
      for (int index = 0; index < dictionary[this.gpID].IDs.Count; ++index)
      {
        int id = dictionary[this.gpID].IDs[index];
        boolList.Add(this.hPointLists.Count == 0 || !this.hPointLists.ContainsKey(mapId) || !this.hPointLists[mapId].lst.ContainsKey(id) || this.hPointLists[mapId].lst[id].Count == 0);
      }
    }
    int index1 = this.PlayerClosePointID(hPoint, this.housingHpoints, -2);
    if (this.gpID == -1)
    {
      this.ctrlFlag.HPointID = index1 + 1 + this.hPointLists[mapId].lst[areaId].Count;
    }
    else
    {
      int num = 0;
      for (int index2 = 0; index2 < dictionary[this.gpID].IDs.Count; ++index2)
      {
        if (!boolList[index2])
          num += this.hPointLists[mapId].lst[dictionary[this.gpID].IDs[index2]].Count;
      }
      this.ctrlFlag.HPointID = index1 + 1 + num;
    }
    this.ctrlFlag.nowHPoint = this.housingHpoints[index1];
    if (this.ctrlFlag.HPointID >= this.lstMarker.Count)
      return;
    ChangeHItem componentInChildren = (ChangeHItem) ((Component) this.lstMarker[this.ctrlFlag.HPointID].Item2).GetComponentInChildren<ChangeHItem>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.ChangeActive(false);
  }

  private int PlayerClosePointID(Transform nowPos, List<HPoint> hPoints, int nPlace = -2)
  {
    int num1 = -1;
    float num2 = this.hSceneSprite.HpointSearchRange * this.hSceneSprite.HpointSearchRange;
    float num3 = float.MaxValue;
    float y = (float) nowPos.get_position().y;
    int num4 = 0;
    if (this.hSceneManager.EventKind != HSceneManager.HEvent.GyakuYobai && this.hSceneManager.EventKind != HSceneManager.HEvent.Yobai && (this.hSceneManager.EventKind != HSceneManager.HEvent.FromFemale && Object.op_Equality((Object) this.hSceneManager.Agent[1], (Object) null)))
      num4 = 1;
    for (int index1 = 0; index1 < hPoints.Count; ++index1)
    {
      int index2 = index1;
      switch (nPlace)
      {
        case -2:
          if (this.CheckEvent(hPoints[index2]._nPlace, this.hSceneManager.EventKind))
          {
            if (this.hSceneManager.EventKind != HSceneManager.HEvent.TsukueBare)
            {
              if (hPoints[index2].id == this.kadOnaID)
                break;
            }
            else if (hPoints[index2].id != this.kadOnaID)
              break;
            if (this.CanInfo(hPoints[index2]._nPlace))
            {
              switch (num4)
              {
                case 0:
                  if (((Component) hPoints[index2]).get_transform().get_position().y >= (double) y - (double) this.OffsetDownHeight && ((Component) hPoints[index2]).get_transform().get_position().y <= (double) y + (double) this.OffsetUpHeight)
                  {
                    Vector2 vector2 = (Vector2) null;
                    vector2.x = ((Component) hPoints[index2]).get_transform().get_position().x - nowPos.get_position().x;
                    vector2.y = ((Component) hPoints[index2]).get_transform().get_position().z - nowPos.get_position().z;
                    num3 = ((Vector2) ref vector2).get_sqrMagnitude();
                    break;
                  }
                  continue;
                case 1:
                  Vector3 vector3 = Vector3.op_Subtraction(((Component) hPoints[index2]).get_transform().get_position(), nowPos.get_position());
                  num3 = ((Vector3) ref vector3).get_sqrMagnitude();
                  break;
              }
              if ((double) num2 > (double) num3)
              {
                num2 = num3;
                num1 = index2;
                break;
              }
              break;
            }
            break;
          }
          break;
        case -1:
          if (HPointCtrl.DicTupleContainsValue(hPoints[index2]._nPlace, 0, 0) || HPointCtrl.DicTupleContainsValue(hPoints[index2]._nPlace, 1, 0))
            goto case -2;
          else
            break;
        default:
          if (HPointCtrl.DicTupleContainsValue(hPoints[index2]._nPlace, nPlace, 0))
            goto case -2;
          else
            break;
      }
    }
    return num1;
  }

  private int PlayerClosePointID(HPoint now, HPoint[] hPoints, int nPlace = -2)
  {
    int num = -1;
    for (int index = 0; index < hPoints.Length; ++index)
    {
      if (!Object.op_Inequality((Object) now, (Object) hPoints[index]))
      {
        num = index;
        break;
      }
    }
    return num;
  }

  private int PlayerClosePointID(Transform nowPos, HPoint[] hPoints, int nPlace = -2)
  {
    int num1 = -1;
    float num2 = this.hSceneSprite.HpointSearchRange * this.hSceneSprite.HpointSearchRange;
    float num3 = float.MaxValue;
    float y = (float) nowPos.get_position().y;
    int num4 = 0;
    if (this.hSceneManager.EventKind != HSceneManager.HEvent.GyakuYobai && this.hSceneManager.EventKind != HSceneManager.HEvent.Yobai && (this.hSceneManager.EventKind != HSceneManager.HEvent.FromFemale && Object.op_Equality((Object) this.hSceneManager.Agent[1], (Object) null)))
      num4 = 1;
    for (int index1 = 0; index1 < hPoints.Length; ++index1)
    {
      int index2 = index1;
      switch (nPlace)
      {
        case -2:
          if (this.CheckEvent(hPoints[index2]._nPlace, this.hSceneManager.EventKind) && this.CanInfo(hPoints[index2]._nPlace))
          {
            if (this.hSceneManager.EventKind != HSceneManager.HEvent.TsukueBare)
            {
              if (hPoints[index2].id == this.kadOnaID)
                break;
            }
            else if (hPoints[index2].id != this.kadOnaID)
              break;
            switch (num4)
            {
              case 0:
                if (((Component) hPoints[index2]).get_transform().get_position().y >= (double) y - (double) this.OffsetDownHeight && ((Component) hPoints[index2]).get_transform().get_position().y <= (double) y + (double) this.OffsetUpHeight)
                {
                  Vector2 vector2 = (Vector2) null;
                  vector2.x = ((Component) hPoints[index2]).get_transform().get_position().x - nowPos.get_position().x;
                  vector2.y = ((Component) hPoints[index2]).get_transform().get_position().z - nowPos.get_position().z;
                  num3 = ((Vector2) ref vector2).get_sqrMagnitude();
                  break;
                }
                continue;
              case 1:
                Vector3 vector3 = Vector3.op_Subtraction(((Component) hPoints[index2]).get_transform().get_position(), nowPos.get_position());
                num3 = ((Vector3) ref vector3).get_sqrMagnitude();
                break;
            }
            if ((double) num2 > (double) num3)
            {
              num2 = num3;
              num1 = index2;
              break;
            }
            break;
          }
          break;
        case -1:
          if (HPointCtrl.DicTupleContainsValue(hPoints[index2]._nPlace, 0, 0) || HPointCtrl.DicTupleContainsValue(hPoints[index2]._nPlace, 1, 0))
            goto case -2;
          else
            break;
        default:
          if (HPointCtrl.DicTupleContainsValue(hPoints[index2]._nPlace, nPlace, 0))
            goto case -2;
          else
            break;
      }
    }
    return num1;
  }

  private bool CheckEvent(Dictionary<int, ValueTuple<int, int>> dic, HSceneManager.HEvent Event)
  {
    switch (Event)
    {
      case HSceneManager.HEvent.Bath:
        return HPointCtrl.DicTupleContainsValue(dic, 11, 0);
      case HSceneManager.HEvent.Toilet1:
        return HPointCtrl.DicTupleContainsValue(dic, 13, 0);
      case HSceneManager.HEvent.Kitchen:
        return HPointCtrl.DicTupleContainsValue(dic, 9, 0);
      case HSceneManager.HEvent.Tachi:
      case HSceneManager.HEvent.MapBath:
        return HPointCtrl.DicTupleContainsValue(dic, 1, 0);
      case HSceneManager.HEvent.Stairs:
      case HSceneManager.HEvent.StairsBare:
        return HPointCtrl.DicTupleContainsValue(dic, 10, 0);
      case HSceneManager.HEvent.KabeanaBack:
      case HSceneManager.HEvent.KabeanaFront:
        return HPointCtrl.DicTupleContainsValue(dic, 15, 0);
      case HSceneManager.HEvent.Neonani:
        return HPointCtrl.DicTupleContainsValue(dic, 0, 0);
      case HSceneManager.HEvent.TsukueBare:
        return HPointCtrl.DicTupleContainsValue(dic, 4, 0);
      default:
        return true;
    }
  }

  public bool CheckPlace(Dictionary<int, ValueTuple<int, int>> place, int mode)
  {
    int[] numArray = (int[]) null;
    switch (mode)
    {
      case 0:
        numArray = this.LesPlaceID;
        break;
      case 1:
        numArray = this.multiFemalePlaceID;
        break;
    }
    if (numArray == null)
      return false;
    for (int index = 0; index < numArray.Length; ++index)
    {
      using (Dictionary<int, ValueTuple<int, int>>.ValueCollection.Enumerator enumerator = place.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.Item1 == numArray[index])
            return true;
        }
      }
    }
    return false;
  }

  private bool CanInfo(Dictionary<int, ValueTuple<int, int>> place)
  {
    using (Dictionary<int, ValueTuple<int, int>>.ValueCollection.Enumerator enumerator = place.Values.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        if (this.CheckMotionLimit((int) enumerator.Current.Item1))
          return true;
      }
    }
    return false;
  }

  private bool CheckMotionLimit(int place)
  {
    if (this.hSceneManager.bMerchant && this.hSceneManager.MerchantLimit == 1)
    {
      if (this.playerSex == 0)
      {
        for (int index = 0; index < this.lstAnimInfo[1].Count; ++index)
        {
          if (this.CheckMotionLimit(place, this.lstAnimInfo[1][index]))
            return true;
        }
      }
      else if (this.playerSex == 1)
      {
        for (int index = 0; index < this.lstAnimInfo[4].Count; ++index)
        {
          if (this.CheckMotionLimit(place, this.lstAnimInfo[4][index]))
            return true;
        }
      }
    }
    else
    {
      for (int index1 = 0; index1 < this.lstAnimInfo.Length; ++index1)
      {
        for (int index2 = 0; index2 < this.lstAnimInfo[index1].Count; ++index2)
        {
          if (this.CheckMotionLimit(place, this.lstAnimInfo[index1][index2]))
            return true;
        }
      }
    }
    return false;
  }

  private bool CheckMotionLimit(int place, HScene.AnimationListInfo lstAnimInfo)
  {
    if (this.playerSex == 0 || this.playerSex == 1 && this.ctrlFlag.bFutanari)
    {
      if (Object.op_Equality((Object) this.hSceneManager.females[1], (Object) null))
      {
        if (lstAnimInfo.nPromiscuity == 1)
          return false;
      }
      else if (lstAnimInfo.nPromiscuity != 1)
        return false;
      if (lstAnimInfo.nPromiscuity == 2 || lstAnimInfo.ActionCtrl.Item1 == 3 && !this.hSceneSprite.NonTokushuCheckIDs.Contains(lstAnimInfo.id) && lstAnimInfo.fileMale == string.Empty)
        return false;
    }
    else if (this.playerSex == 1 && lstAnimInfo.nPromiscuity < 2)
      return false;
    if (!this.hSceneManager.bMerchant && (lstAnimInfo.nHentai == 1 && this.hSceneManager.GetFlaverSkillLevel(2) < 100 || lstAnimInfo.nHentai == 2 && this.hSceneManager.GetFlaverSkillLevel(2) < 170))
      return false;
    if (this.hSceneManager.bMerchant)
    {
      if (!lstAnimInfo.bMerchantMotion || lstAnimInfo.nIyaAction == 2)
        return false;
    }
    else if (this.hSceneManager.EventKind != HSceneManager.HEvent.Yobai)
    {
      if (this.hSceneManager.isForce)
      {
        if (lstAnimInfo.nIyaAction < 1)
          return false;
      }
      else if (lstAnimInfo.nIyaAction == 2)
        return false;
    }
    else if (!lstAnimInfo.bSleep)
      return false;
    if (!this.hSceneManager.bMerchant)
    {
      if (!lstAnimInfo.nPositons.Contains(place))
        return false;
    }
    else if (place >= this.NotMerchantPlaceID)
      return false;
    if (lstAnimInfo.isNeedItem && !this.hSceneManager.CheckHadItem((int) lstAnimInfo.ActionCtrl.Item1, lstAnimInfo.id) || lstAnimInfo.nDownPtn == 0 && this.ctrlFlag.isFaintness || lstAnimInfo.nFaintnessLimit == 1 && !this.ctrlFlag.isFaintness)
      return false;
    switch (this.ctrlFlag.initiative)
    {
      case 0:
        if (lstAnimInfo.nInitiativeFemale != 0)
          return false;
        break;
      case 1:
        if (lstAnimInfo.nInitiativeFemale == 0)
          return false;
        break;
      case 2:
        if (lstAnimInfo.nInitiativeFemale != 2)
          return false;
        break;
    }
    return true;
  }

  public void endHScene()
  {
    for (int index = 0; index < this.lstMarker.Count; ++index)
    {
      ((HPoint) this.lstMarker[index].Item2).SetEffectActive(false);
      Collider collider = ((HPoint) this.lstMarker[index].Item2).GetCollider();
      if (Object.op_Inequality((Object) collider, (Object) null))
        collider.set_enabled(false);
    }
    this.IsMarker = false;
    this.lstMarker.Clear();
    this.hPointLists = (Dictionary<int, HPointList>) null;
    this.InitUsePoint = false;
  }

  private bool HitObstacle(Vector3 pos, HPoint point)
  {
    ref Vector3 local1 = ref pos;
    local1.y = (__Null) (local1.y + 20.0);
    Vector3 position = ((Component) point).get_transform().get_position();
    Ray ray;
    ref Ray local2 = ref ray;
    Vector3 vector3_1 = pos;
    Vector3 vector3_2 = Vector3.op_Subtraction(position, pos);
    Vector3 normalized = ((Vector3) ref vector3_2).get_normalized();
    ((Ray) ref local2).\u002Ector(vector3_1, normalized);
    int num1 = 2048;
    int num2 = Physics.RaycastNonAlloc(ray, this.HitInfo2, this.hSceneSprite.HpointSearchRange, num1);
    if (num2 > 0)
    {
      for (int index = 0; index < num2; ++index)
      {
        double distance = (double) ((RaycastHit) ref this.HitInfo2[index]).get_distance();
        Vector3 vector3_3 = Vector3.op_Subtraction(position, pos);
        double magnitude = (double) ((Vector3) ref vector3_3).get_magnitude();
        if (distance <= magnitude)
        {
          foreach (Component checkHitObj in this.CheckHitObjs)
          {
            foreach (Collider componentsInChild in (Collider[]) checkHitObj.GetComponentsInChildren<Collider>())
            {
              if (Object.op_Equality((Object) ((RaycastHit) ref this.HitInfo2[index]).get_collider(), (Object) componentsInChild))
                return true;
            }
          }
        }
      }
    }
    return false;
  }

  private bool HpointAreaOpen(HPoint point)
  {
    if (point.OpenID == null || point.OpenID.Count == 0)
      return true;
    bool flag = true;
    for (int index1 = 0; index1 < point.OpenID.Count; ++index1)
    {
      int index2 = point.OpenID[index1];
      flag &= Singleton<Game>.Instance.Environment.AreaOpenState[index2];
    }
    return flag;
  }

  public void EndProc()
  {
    this.housingHpoints = (HPoint[]) null;
    this.ExistSecondfemale = false;
  }

  private void AreagpInit()
  {
    Dictionary<int, Dictionary<int, List<int>>> housingAreaGroup = Singleton<Resources>.Instance.Map.VanishHousingAreaGroup;
    if (housingAreaGroup == null)
      return;
    foreach (KeyValuePair<int, Dictionary<int, List<int>>> keyValuePair1 in housingAreaGroup)
    {
      if (!this.Areagp.ContainsKey(keyValuePair1.Key))
        this.Areagp.Add(keyValuePair1.Key, new Dictionary<int, HPointCtrl.AreaGroupDefine>());
      foreach (KeyValuePair<int, List<int>> keyValuePair2 in keyValuePair1.Value)
      {
        if (!this.Areagp[keyValuePair1.Key].ContainsKey(keyValuePair2.Key))
          this.Areagp[keyValuePair1.Key].Add(keyValuePair2.Key, new HPointCtrl.AreaGroupDefine());
        for (int index = 0; index < keyValuePair2.Value.Count; ++index)
        {
          if (!this.Areagp[keyValuePair1.Key][keyValuePair2.Key].IDs.Contains(keyValuePair2.Value[index]))
            this.Areagp[keyValuePair1.Key][keyValuePair2.Key].IDs.Add(keyValuePair2.Value[index]);
        }
      }
    }
  }

  public class RayDistanceCompare : IComparer<RaycastHit>
  {
    public int Compare(RaycastHit x, RaycastHit y)
    {
      if ((double) ((RaycastHit) ref x).get_distance() < (double) ((RaycastHit) ref y).get_distance())
        return -1;
      return (double) ((RaycastHit) ref x).get_distance() > (double) ((RaycastHit) ref y).get_distance() ? 1 : 0;
    }
  }

  [Serializable]
  public struct MinHpoint
  {
    public int AreaID;
    public int PointID;
  }

  [Serializable]
  private class AreaGroupDefine
  {
    public List<int> IDs = new List<int>();
  }
}
