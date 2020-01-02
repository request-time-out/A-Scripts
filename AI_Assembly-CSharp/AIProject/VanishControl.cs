// Decompiled with JetBrains decompiler
// Type: AIProject.VanishControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class VanishControl : MonoBehaviour
  {
    [SerializeField]
    private CapsuleCollider _viewCollider;
    protected List<VirtualCameraController.VisibleObjectH> _mapVanishList;
    protected List<Collider> _colliderList;
    private bool _isConfigVanish;

    public VanishControl()
    {
      base.\u002Ector();
    }

    public Vector3 LookAtPosition { get; set; }

    public bool ConfigVanish
    {
      get
      {
        return this._isConfigVanish;
      }
      set
      {
        if (this._isConfigVanish == value)
          return;
        this._isConfigVanish = value;
        this.VisibleForceVanish(true);
      }
    }

    public void Load()
    {
      this._mapVanishList.Clear();
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      List<Manager.Map.VisibleObject> lstMapVanish = Singleton<Manager.Map>.Instance.LstMapVanish;
      for (int index1 = 0; index1 < lstMapVanish.Count; ++index1)
      {
        int index2 = index1;
        if (!Object.op_Equality((Object) lstMapVanish[index2].collider, (Object) null) && ((Component) lstMapVanish[index2].collider).get_gameObject().get_activeSelf())
        {
          VirtualCameraController.VisibleObjectH visibleObjectH = new VirtualCameraController.VisibleObjectH();
          visibleObjectH.nameCollider = lstMapVanish[index2].nameCollider;
          visibleObjectH.collider = lstMapVanish[index2].collider;
          visibleObjectH.vanishObj = lstMapVanish[index2].vanishObj;
          visibleObjectH.initEnable = Object.op_Implicit((Object) lstMapVanish[index2].collider);
          this._mapVanishList.Add(visibleObjectH);
          visibleObjectH.collider.set_enabled(true);
        }
      }
    }

    public void LoadHousingVanish(PlayerActor player)
    {
      if (!Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.PointAgent, (Object) null))
        return;
      BasePoint[] basePoints = Singleton<Manager.Map>.Instance.PointAgent.BasePoints;
      if (basePoints == null)
        return;
      int mapId = Singleton<Manager.Map>.Instance.MapID;
      int index1 = -1;
      Dictionary<int, Dictionary<int, List<int>>> housingAreaGroup = Singleton<Resources>.Instance.Map.VanishHousingAreaGroup;
      if (housingAreaGroup == null || !housingAreaGroup.ContainsKey(mapId))
        return;
      foreach (KeyValuePair<int, List<int>> keyValuePair in housingAreaGroup[mapId])
      {
        if (keyValuePair.Value.Contains(player.AreaID))
        {
          index1 = keyValuePair.Key;
          break;
        }
      }
      if (index1 < 0)
        return;
      for (int index2 = 0; index2 < basePoints.Length; ++index2)
      {
        if (!Object.op_Equality((Object) basePoints[index2].OwnerArea, (Object) null) && housingAreaGroup[mapId][index1].Contains(basePoints[index2].OwnerArea.AreaID) && basePoints[index2].ID >= 0)
        {
          Singleton<Housing>.Instance.StartShield(basePoints[index2].ID);
          break;
        }
      }
    }

    public void ResetVanish()
    {
      for (int index = 0; index < this._mapVanishList.Count; ++index)
      {
        if (!Object.op_Equality((Object) this._mapVanishList[index].collider, (Object) null))
          this._mapVanishList[index].collider.set_enabled(this._mapVanishList[index].initEnable);
      }
    }

    public void VisibleForceVanish(bool v)
    {
      foreach (VirtualCameraController.VisibleObjectH mapVanish in this._mapVanishList)
      {
        if (Object.op_Inequality((Object) mapVanish.vanishObj, (Object) null))
          mapVanish.vanishObj.SetActiveIfDifferent(v);
        mapVanish.isVisible = v;
        mapVanish.delay = !v ? 0.0f : 0.3f;
      }
    }

    private void VisibleForceVanish(VirtualCameraController.VisibleObjectH obj, bool v)
    {
      if (obj == null || Object.op_Equality((Object) obj.vanishObj, (Object) null))
        return;
      obj.vanishObj.SetActiveIfDifferent(v);
      obj.delay = !v ? 0.0f : 0.3f;
      obj.isVisible = v;
    }

    private bool VanishProc()
    {
      if (!this._isConfigVanish)
        return false;
      for (int i = 0; i < this._mapVanishList.Count; ++i)
      {
        List<Collider> all = this._colliderList.FindAll((Predicate<Collider>) (x => this._mapVanishList[i].nameCollider == ((Object) x).get_name()));
        if (all == null || all.Count == 0)
          this.VanishDelayVisible(this._mapVanishList[i]);
        else if (this._mapVanishList[i].isVisible)
          this.VisibleForceVanish(this._mapVanishList[i], false);
      }
      if (Object.op_Inequality((Object) this._viewCollider, (Object) null) && Singleton<Housing>.IsInstance())
        Singleton<Housing>.Instance.ShieldProc((Collider) this._viewCollider);
      return true;
    }

    private bool VanishDelayVisible(VirtualCameraController.VisibleObjectH obj)
    {
      if (obj.isVisible)
        return false;
      obj.delay += Time.get_deltaTime();
      if ((double) obj.delay >= 0.300000011920929)
        this.VisibleForceVanish(obj, true);
      return true;
    }

    private void Start()
    {
      this._viewCollider = (CapsuleCollider) ((Component) this).get_gameObject().AddComponent<CapsuleCollider>();
      this._viewCollider.set_radius(0.05f);
      ((Collider) this._viewCollider).set_isTrigger(true);
      this._viewCollider.set_direction(2);
      Rigidbody rigidbody = (Rigidbody) ((Component) this).get_gameObject().AddComponent<Rigidbody>();
      rigidbody.set_useGravity(false);
      rigidbody.set_isKinematic(true);
    }

    private void OnDisable()
    {
      this.VisibleForceVanish(true);
    }

    private void Update()
    {
      this.ConfigVanish = Manager.Config.GraphicData.Shield;
    }

    private void LateUpdate()
    {
      if (Object.op_Inequality((Object) this._viewCollider, (Object) null))
      {
        this._viewCollider.set_height(Vector3.Distance(((Component) this).get_transform().get_position(), this.LookAtPosition));
        this._viewCollider.set_center(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_forward(), this._viewCollider.get_height()), 0.5f));
      }
      this.VanishProc();
    }

    private void OnTriggerEnter(Collider other)
    {
      if (this._colliderList.FindAll((Predicate<Collider>) (x => ((Object) x).get_name() == ((Object) other).get_name())) != null)
        return;
      this._colliderList.Add(other);
    }

    private void OnTriggerStay(Collider other)
    {
      if (!Object.op_Equality((Object) this._colliderList.Find((Predicate<Collider>) (x => ((Object) x).get_name() == ((Object) other).get_name())), (Object) null))
        return;
      this._colliderList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
      this._colliderList.Clear();
    }
  }
}
