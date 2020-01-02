// Decompiled with JetBrains decompiler
// Type: Housing.OCItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Animal;
using Housing.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Housing
{
  public class OCItem : ObjectCtrl
  {
    private static Vector3 correctionSize = new Vector3(0.2f, 0.2f, 0.2f);
    private Subject<bool> subjectColor1 = new Subject<bool>();
    private Subject<bool> subjectColor2 = new Subject<bool>();
    private Subject<bool> subjectColor3 = new Subject<bool>();
    private Subject<bool> subjectEmissionColor = new Subject<bool>();
    private ItemComponent m_itemComponent;

    public OCItem(
      OIItem _oiItem,
      GameObject _gameObject,
      CraftInfo _craftInfo,
      Manager.Housing.LoadInfo _loadInfo)
      : base((IObjectInfo) _oiItem, _gameObject, _craftInfo)
    {
      this.LoadInfo = _loadInfo;
      this.m_itemComponent = !Object.op_Inequality((Object) this.GameObject, (Object) null) ? (ItemComponent) null : (ItemComponent) this.GameObject.GetComponent<ItemComponent>();
      if (Object.op_Equality((Object) this.m_itemComponent, (Object) null))
      {
        this.m_itemComponent = (ItemComponent) this.GameObject.AddComponent<ItemComponent>();
        this.m_itemComponent.Setup(false);
      }
      if (this.m_itemComponent != null)
        this.m_itemComponent.SetHPoint();
      ObservableExtensions.Subscribe<IList<bool>>((IObservable<M0>) Observable.BatchFrame<bool>(Observable.Merge<bool>((IObservable<M0>[]) new IObservable<bool>[4]
      {
        (IObservable<bool>) this.subjectColor1,
        (IObservable<bool>) this.subjectColor2,
        (IObservable<bool>) this.subjectColor3,
        (IObservable<bool>) this.subjectEmissionColor
      }), 0, (FrameCountType) 0), (Action<M0>) (_ => this.UpdateColor()));
    }

    public OIItem OIItem
    {
      get
      {
        return this.ObjectInfo as OIItem;
      }
    }

    public Manager.Housing.LoadInfo LoadInfo { get; private set; }

    public ItemComponent ItemComponent
    {
      get
      {
        return this.m_itemComponent ?? (this.m_itemComponent = (ItemComponent) this.GameObject?.GetComponent<ItemComponent>());
      }
    }

    public HashSet<OCItem> HashOverlap { get; private set; } = new HashSet<OCItem>();

    public HashSet<OCItem> CheckedOverlap { get; private set; } = new HashSet<OCItem>();

    public override string Name
    {
      get
      {
        return this.LoadInfo.name;
      }
    }

    public int Category
    {
      get
      {
        return this.LoadInfo.category;
      }
    }

    public ActionPoint[] ActionPoints
    {
      get
      {
        return this.ItemComponent?.actionPoints;
      }
    }

    public FarmPoint[] FarmPoints
    {
      get
      {
        return this.ItemComponent?.farmPoints;
      }
    }

    public HPoint[] HPoints
    {
      get
      {
        return this.ItemComponent?.hPoints;
      }
    }

    public ItemComponent.ColInfo[] ColInfos
    {
      get
      {
        return this.ItemComponent?.colInfos;
      }
    }

    public PetHomePoint[] PetHomePoints
    {
      get
      {
        return this.ItemComponent?.petHomePoints;
      }
    }

    public JukePoint[] JukePoints
    {
      get
      {
        return this.ItemComponent?.jukePoints;
      }
    }

    public CraftPoint[] CraftPoints
    {
      get
      {
        return this.ItemComponent?.craftPoints;
      }
    }

    public LightSwitchPoint[] LightSwitchPoints
    {
      get
      {
        return this.ItemComponent?.lightSwitchPoints;
      }
    }

    public bool IsColor
    {
      get
      {
        return Object.op_Implicit((Object) this.ItemComponent) && this.ItemComponent.IsColor;
      }
    }

    public bool IsColor1
    {
      get
      {
        return Object.op_Implicit((Object) this.ItemComponent) && this.ItemComponent.IsColor1;
      }
    }

    public bool IsColor2
    {
      get
      {
        return Object.op_Implicit((Object) this.ItemComponent) && this.ItemComponent.IsColor2;
      }
    }

    public bool IsColor3
    {
      get
      {
        return Object.op_Implicit((Object) this.ItemComponent) && this.ItemComponent.IsColor3;
      }
    }

    public bool IsEmissionColor
    {
      get
      {
        return Object.op_Implicit((Object) this.ItemComponent) && this.ItemComponent.IsEmissionColor;
      }
    }

    public Color Color1
    {
      get
      {
        return this.OIItem.Color1;
      }
      set
      {
        this.OIItem.Color1 = value;
        this.subjectColor1.OnNext(true);
      }
    }

    public Color Color2
    {
      get
      {
        return this.OIItem.Color2;
      }
      set
      {
        this.OIItem.Color2 = value;
        this.subjectColor2.OnNext(true);
      }
    }

    public Color Color3
    {
      get
      {
        return this.OIItem.Color1;
      }
      set
      {
        this.OIItem.Color3 = value;
        this.subjectColor3.OnNext(true);
      }
    }

    public Color EmissionColor
    {
      get
      {
        return this.OIItem.EmissionColor;
      }
      set
      {
        this.OIItem.EmissionColor = value;
        this.subjectEmissionColor.OnNext(true);
      }
    }

    public bool IsOption
    {
      get
      {
        return ((this.LoadInfo.useOption ? 1 : 0) & (!Object.op_Implicit((Object) this.ItemComponent) ? 0 : (this.ItemComponent.IsOption ? 1 : 0))) != 0;
      }
    }

    public bool VisibleOption
    {
      get
      {
        return this.OIItem.VisibleOption;
      }
      set
      {
        this.OIItem.VisibleOption = value;
        this.ItemComponent?.SetVisibleOption(value);
      }
    }

    public override bool IsOverlapNow
    {
      get
      {
        return this.HashOverlap.Count<OCItem>() != 0;
      }
    }

    public IEnumerable<Collider> OverlapColliders
    {
      get
      {
        if (!Object.op_Implicit((Object) this.ItemComponent))
          return (IEnumerable<Collider>) null;
        return ((IList<Collider>) this.ItemComponent.overlapColliders).IsNullOrEmpty<Collider>() ? (IEnumerable<Collider>) null : ((IEnumerable<Collider>) this.ItemComponent.overlapColliders).Where<Collider>((Func<Collider, bool>) (v => Object.op_Inequality((Object) v, (Object) null)));
      }
    }

    public override bool OnRemoving()
    {
      bool flag = true;
      if (Object.op_Inequality((Object) this.ItemComponent, (Object) null))
      {
        if (!((IList<PetHomePoint>) this.ItemComponent.petHomePoints).IsNullOrEmpty<PetHomePoint>())
        {
          foreach (PetHomePoint petHomePoint in this.ItemComponent.petHomePoints)
            flag &= petHomePoint.CanDelete();
        }
        if (!((IList<CraftPoint>) this.ItemComponent.craftPoints).IsNullOrEmpty<CraftPoint>())
        {
          foreach (CraftPoint craftPoint in this.ItemComponent.craftPoints)
            flag &= craftPoint.CanDelete();
        }
      }
      return flag;
    }

    public override void OnDelete()
    {
      foreach (OCItem ocItem in this.HashOverlap)
        ocItem.HashOverlap.Remove(this);
      this.HashOverlap.Clear();
      base.OnDelete();
    }

    public override void OnDeleteChild()
    {
      foreach (OCItem ocItem in this.HashOverlap)
        ocItem.HashOverlap.Remove(this);
      this.HashOverlap.Clear();
    }

    public override void OnDestroy()
    {
      foreach (ActionPoint actionPoint in ((IEnumerable<ActionPoint>) this.ActionPoints).Where<ActionPoint>((Func<ActionPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))))
      {
        Singleton<Manager.Map>.Instance.RemoveAppendActionPoint(actionPoint);
        Singleton<Manager.Map>.Instance.RemoveAppendCommandable((ICommandable) actionPoint);
        Singleton<Manager.Map>.Instance.RemoveAgentSearchActionPoint(actionPoint);
        Singleton<Manager.Map>.Instance.UnregisterRuntimePoint((Point) actionPoint, false);
      }
      foreach (FarmPoint point in ((IEnumerable<FarmPoint>) this.FarmPoints).Where<FarmPoint>((Func<FarmPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))))
      {
        Singleton<Manager.Map>.Instance.RemoveRuntimeFarmPoint(point);
        Singleton<Manager.Map>.Instance.RemoveAppendCommandable((ICommandable) point);
        Singleton<Manager.Map>.Instance.UnregisterRuntimePoint((Point) point, false);
      }
      foreach (PetHomePoint petHomePoint in ((IEnumerable<PetHomePoint>) this.PetHomePoints).Where<PetHomePoint>((Func<PetHomePoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))))
      {
        Singleton<Manager.Map>.Instance.RemovePetHomePoint(petHomePoint);
        Singleton<Manager.Map>.Instance.RemoveAppendCommandable((ICommandable) petHomePoint);
        Singleton<Manager.Map>.Instance.UnregisterRuntimePoint((Point) petHomePoint, false);
      }
      foreach (JukePoint point in ((IEnumerable<JukePoint>) this.JukePoints).Where<JukePoint>((Func<JukePoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))))
      {
        Singleton<Manager.Map>.Instance.RemoveJukePoint(point);
        Singleton<Manager.Map>.Instance.RemoveAppendCommandable((ICommandable) point);
        Singleton<Manager.Map>.Instance.UnregisterRuntimePoint((Point) point, false);
      }
      foreach (CraftPoint point in ((IEnumerable<CraftPoint>) this.CraftPoints).Where<CraftPoint>((Func<CraftPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))))
      {
        Singleton<Manager.Map>.Instance.RemoveCraftPoint(point);
        Singleton<Manager.Map>.Instance.RemoveAppendCommandable((ICommandable) point);
        Singleton<Manager.Map>.Instance.UnregisterRuntimePoint((Point) point, false);
      }
      foreach (LightSwitchPoint point in ((IEnumerable<LightSwitchPoint>) this.LightSwitchPoints).Where<LightSwitchPoint>((Func<LightSwitchPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))))
      {
        Singleton<Manager.Map>.Instance.RemoveRuntimeLightSwitchPoint(point);
        Singleton<Manager.Map>.Instance.RemoveAppendCommandable((ICommandable) point);
        Singleton<Manager.Map>.Instance.UnregisterRuntimePoint((Point) point, false);
      }
      this.m_itemComponent = (ItemComponent) null;
      base.OnDestroy();
    }

    public override void RestoreObject(GameObject _gameObject)
    {
      base.RestoreObject(_gameObject);
      this.m_itemComponent = (ItemComponent) this.GameObject.GetComponent<ItemComponent>();
      if (Object.op_Equality((Object) this.m_itemComponent, (Object) null))
      {
        this.m_itemComponent = (ItemComponent) this.GameObject.AddComponent<ItemComponent>();
        this.m_itemComponent.Setup(false);
      }
      if (this.m_itemComponent == null)
        return;
      this.m_itemComponent.SetHPoint();
    }

    public override void GetLocalMinMax(
      Vector3 _pos,
      Quaternion _rot,
      Transform _root,
      ref Vector3 _min,
      ref Vector3 _max)
    {
      Matrix4x4 matrix4x4_1 = Matrix4x4.TRS(_root.get_position(), _root.get_rotation(), Vector3.get_one());
      Matrix4x4 matrix4x4_2 = Matrix4x4.TRS(_pos, _rot, Vector3.get_one());
      Matrix4x4 matrix4x4_3 = Matrix4x4.op_Multiply(((Matrix4x4) ref matrix4x4_1).get_inverse(), matrix4x4_2);
      Vector3 min = this.ItemComponent.min;
      Vector3 max = this.ItemComponent.max;
      Vector3[] vector3Array = new Vector3[8]
      {
        ((Matrix4x4) ref matrix4x4_3).MultiplyPoint3x4(new Vector3((float) min.x, (float) min.y, (float) min.z)),
        ((Matrix4x4) ref matrix4x4_3).MultiplyPoint3x4(new Vector3((float) min.x, (float) min.y, (float) max.z)),
        ((Matrix4x4) ref matrix4x4_3).MultiplyPoint3x4(new Vector3((float) max.x, (float) min.y, (float) min.z)),
        ((Matrix4x4) ref matrix4x4_3).MultiplyPoint3x4(new Vector3((float) max.x, (float) min.y, (float) max.z)),
        ((Matrix4x4) ref matrix4x4_3).MultiplyPoint3x4(new Vector3((float) min.x, (float) max.y, (float) min.z)),
        ((Matrix4x4) ref matrix4x4_3).MultiplyPoint3x4(new Vector3((float) min.x, (float) max.y, (float) max.z)),
        ((Matrix4x4) ref matrix4x4_3).MultiplyPoint3x4(new Vector3((float) max.x, (float) max.y, (float) min.z)),
        ((Matrix4x4) ref matrix4x4_3).MultiplyPoint3x4(new Vector3((float) max.x, (float) max.y, (float) max.z))
      };
      ((Vector3) ref _min).\u002Ector(((IEnumerable<Vector3>) vector3Array).Min<Vector3>((Func<Vector3, float>) (_v => (float) _v.x)), ((IEnumerable<Vector3>) vector3Array).Min<Vector3>((Func<Vector3, float>) (_v => (float) _v.y)), ((IEnumerable<Vector3>) vector3Array).Min<Vector3>((Func<Vector3, float>) (_v => (float) _v.z)));
      ((Vector3) ref _max).\u002Ector(((IEnumerable<Vector3>) vector3Array).Max<Vector3>((Func<Vector3, float>) (_v => (float) _v.x)), ((IEnumerable<Vector3>) vector3Array).Max<Vector3>((Func<Vector3, float>) (_v => (float) _v.y)), ((IEnumerable<Vector3>) vector3Array).Max<Vector3>((Func<Vector3, float>) (_v => (float) _v.z)));
    }

    public override void GetActionPoint(ref List<ActionPoint> _points)
    {
      ActionPoint[] actionPoints = this.ActionPoints;
      if (((IList<ActionPoint>) actionPoints).IsNullOrEmpty<ActionPoint>())
        return;
      _points.AddRange(((IEnumerable<ActionPoint>) actionPoints).Where<ActionPoint>((Func<ActionPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))));
    }

    public override void GetFarmPoint(ref List<FarmPoint> _points)
    {
      FarmPoint[] farmPoints = this.FarmPoints;
      if (((IList<FarmPoint>) farmPoints).IsNullOrEmpty<FarmPoint>())
        return;
      _points.AddRange(((IEnumerable<FarmPoint>) farmPoints).Where<FarmPoint>((Func<FarmPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))));
    }

    public override void GetHPoint(ref List<HPoint> _points)
    {
      HPoint[] hpoints = this.HPoints;
      if (((IList<HPoint>) hpoints).IsNullOrEmpty<HPoint>())
        return;
      _points.AddRange(((IEnumerable<HPoint>) hpoints).Where<HPoint>((Func<HPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))));
    }

    public override void GetColInfo(ref List<ItemComponent.ColInfo> _infos)
    {
      ItemComponent.ColInfo[] colInfos = this.ColInfos;
      if (((IList<ItemComponent.ColInfo>) colInfos).IsNullOrEmpty<ItemComponent.ColInfo>())
        return;
      _infos.AddRange(((IEnumerable<ItemComponent.ColInfo>) colInfos).Where<ItemComponent.ColInfo>((Func<ItemComponent.ColInfo, bool>) (v => v != null)));
    }

    public override void GetPetHomePoint(ref List<PetHomePoint> _points)
    {
      PetHomePoint[] petHomePoints = this.PetHomePoints;
      if (((IList<PetHomePoint>) petHomePoints).IsNullOrEmpty<PetHomePoint>())
        return;
      _points.AddRange(((IEnumerable<PetHomePoint>) petHomePoints).Where<PetHomePoint>((Func<PetHomePoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))));
    }

    public override void GetJukePoint(ref List<JukePoint> _points)
    {
      JukePoint[] jukePoints = this.JukePoints;
      if (((IList<JukePoint>) jukePoints).IsNullOrEmpty<JukePoint>())
        return;
      _points.AddRange(((IEnumerable<JukePoint>) jukePoints).Where<JukePoint>((Func<JukePoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))));
    }

    public override void GetCraftPoint(ref List<CraftPoint> _points)
    {
      CraftPoint[] craftPoints = this.CraftPoints;
      if (((IList<CraftPoint>) craftPoints).IsNullOrEmpty<CraftPoint>())
        return;
      _points.AddRange(((IEnumerable<CraftPoint>) craftPoints).Where<CraftPoint>((Func<CraftPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))));
    }

    public override void GetLightSwitchPoint(ref List<LightSwitchPoint> _points)
    {
      LightSwitchPoint[] lightSwitchPoints = this.LightSwitchPoints;
      if (((IList<LightSwitchPoint>) lightSwitchPoints).IsNullOrEmpty<LightSwitchPoint>())
        return;
      _points.AddRange(((IEnumerable<LightSwitchPoint>) lightSwitchPoints).Where<LightSwitchPoint>((Func<LightSwitchPoint, bool>) (v => Object.op_Inequality((Object) v, (Object) null))));
    }

    public override void GetUsedNum(int _no, ref int _num)
    {
      if (this.OIItem.ID != _no)
        return;
      ++_num;
    }

    public void ResetColor()
    {
      if (this.IsColor1)
        this.Color1 = this.ItemComponent.defColor1;
      if (this.IsColor2)
        this.Color2 = this.ItemComponent.defColor2;
      if (this.IsColor3)
        this.Color3 = this.ItemComponent.defColor3;
      if (!this.IsEmissionColor)
        return;
      this.EmissionColor = this.ItemComponent.defEmissionColor;
    }

    public void UpdateColor()
    {
      if (Object.op_Equality((Object) this.ItemComponent, (Object) null) || !this.ItemComponent.IsColor)
        return;
      OIItem info = this.OIItem;
      foreach (ItemComponent.Info renderer in this.ItemComponent.renderers)
      {
        Material[] materials = ((Renderer) renderer.meshRenderer).get_materials();
        for (int index = 0; index < materials.Length; ++index)
        {
          Material m = materials[index];
          if (!Object.op_Equality((Object) m, (Object) null))
            renderer.materialInfos.SafeProc<ItemComponent.MaterialInfo>(index, (Action<ItemComponent.MaterialInfo>) (_info =>
            {
              if (_info.isColor1)
                m.SetColor(ItemShader._Color, info.Color1);
              if (_info.isColor2)
                m.SetColor(ItemShader._Color2, info.Color2);
              if (_info.isColor3)
                m.SetColor(ItemShader._Color3, info.Color3);
              if (!_info.isEmission)
                return;
              m.SetColor(ItemShader._EmissionColor, info.EmissionColor);
            }));
        }
        ((Renderer) renderer.meshRenderer).set_materials(materials);
      }
    }

    public override bool CheckOverlap(ObjectCtrl _oc, bool _load = false)
    {
      if (Object.op_Equality((Object) this.ItemComponent, (Object) null) || !(_oc is OCItem _oc1) || this == _oc1)
        return false;
      if (_load && this.CheckedOverlap.Contains(_oc1))
        return this.HashOverlap.Contains(_oc1);
      if (!_oc1.ItemComponent.overlap || !this.ItemComponent.overlap)
      {
        if (_load)
        {
          this.CheckedOverlap.Add(_oc1);
          _oc1.CheckedOverlap.Add(this);
        }
        return false;
      }
      bool flag = false;
      IEnumerable<Collider> overlapColliders1 = _oc1.OverlapColliders;
      if (!overlapColliders1.IsNullOrEmpty<Collider>())
      {
        Vector3 zero = Vector3.get_zero();
        float num = 0.0f;
        IEnumerable<Collider> overlapColliders2 = this.OverlapColliders;
        if (overlapColliders2.IsNullOrEmpty<Collider>())
        {
          Bounds bounds = (Bounds) null;
          ((Bounds) ref bounds).SetMinMax(this.ItemComponent.min, this.ItemComponent.max);
          BoxCollider boxCollider = Singleton<CraftScene>.Instance.TestColliders.SafeGet<BoxCollider>(0);
          boxCollider.set_center(((Bounds) ref bounds).get_center());
          boxCollider.set_size(Vector3.op_Subtraction(((Bounds) ref bounds).get_size(), OCItem.correctionSize));
          using (IEnumerator<Collider> enumerator = overlapColliders1.GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              Collider current = enumerator.Current;
              flag |= Physics.ComputePenetration((Collider) boxCollider, this.Position, this.Rotation, current, ((Component) current).get_transform().get_position(), ((Component) current).get_transform().get_rotation(), ref zero, ref num);
            }
          }
        }
        else
        {
          using (IEnumerator<Collider> enumerator1 = overlapColliders1.GetEnumerator())
          {
            while (((IEnumerator) enumerator1).MoveNext())
            {
              Collider current1 = enumerator1.Current;
              using (IEnumerator<Collider> enumerator2 = overlapColliders2.GetEnumerator())
              {
                while (((IEnumerator) enumerator2).MoveNext())
                {
                  Collider current2 = enumerator2.Current;
                  flag |= Physics.ComputePenetration(current1, ((Component) current1).get_transform().get_position(), ((Component) current1).get_transform().get_rotation(), current2, ((Component) current2).get_transform().get_position(), ((Component) current2).get_transform().get_rotation(), ref zero, ref num);
                }
              }
            }
          }
        }
      }
      else
        flag = this.CheckOverlapSize(_oc1);
      if (flag)
      {
        this.HashOverlap.Add(_oc1);
        _oc1.HashOverlap.Add(this);
      }
      else
      {
        this.HashOverlap.Remove(_oc1);
        _oc1.HashOverlap.Remove(this);
      }
      if (_load)
      {
        this.CheckedOverlap.Add(_oc1);
        _oc1.CheckedOverlap.Add(this);
      }
      return flag;
    }

    private bool CheckOverlapSize(OCItem _oc)
    {
      bool flag = false;
      IEnumerable<Collider> overlapColliders = this.OverlapColliders;
      if (overlapColliders.IsNullOrEmpty<Collider>())
      {
        Vector3 zero1 = Vector3.get_zero();
        Vector3 zero2 = Vector3.get_zero();
        _oc.GetLocalMinMax(_oc.Position, _oc.Rotation, this.CraftInfo.ObjRoot.get_transform(), ref zero1, ref zero2);
        Bounds bounds1 = (Bounds) null;
        ((Bounds) ref bounds1).SetMinMax(zero1, zero2);
        ref Bounds local1 = ref bounds1;
        ((Bounds) ref local1).set_size(Vector3.op_Subtraction(((Bounds) ref local1).get_size(), OCItem.correctionSize));
        Vector3 zero3 = Vector3.get_zero();
        Vector3 zero4 = Vector3.get_zero();
        this.GetLocalMinMax(this.Position, this.Rotation, this.CraftInfo.ObjRoot.get_transform(), ref zero3, ref zero4);
        Bounds bounds2 = (Bounds) null;
        ((Bounds) ref bounds2).SetMinMax(zero3, zero4);
        ref Bounds local2 = ref bounds2;
        ((Bounds) ref local2).set_size(Vector3.op_Subtraction(((Bounds) ref local2).get_size(), OCItem.correctionSize));
        return ((Bounds) ref bounds1).Intersects(bounds2);
      }
      Bounds bounds = (Bounds) null;
      ((Bounds) ref bounds).SetMinMax(_oc.ItemComponent.min, _oc.ItemComponent.max);
      BoxCollider boxCollider = Singleton<CraftScene>.Instance.TestColliders.SafeGet<BoxCollider>(0);
      boxCollider.set_center(((Bounds) ref bounds).get_center());
      boxCollider.set_size(Vector3.op_Subtraction(((Bounds) ref bounds).get_size(), OCItem.correctionSize));
      Vector3 zero = Vector3.get_zero();
      float num = 0.0f;
      using (IEnumerator<Collider> enumerator = overlapColliders.GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Collider current = enumerator.Current;
          flag |= Physics.ComputePenetration((Collider) boxCollider, _oc.Position, _oc.Rotation, current, ((Component) current).get_transform().get_position(), ((Component) current).get_transform().get_rotation(), ref zero, ref num);
        }
      }
      return flag;
    }

    public override void BeforeCheckOverlap()
    {
      this.CheckedOverlap.Clear();
      this.SetOverlapColliders(true);
    }

    public override void AfterCheckOverlap()
    {
      this.CheckedOverlap.Clear();
    }

    public override void SetOverlapColliders(bool _flag)
    {
      this.ItemComponent?.SetOverlapColliders(_flag);
    }

    public override void GetOverlapObject(ref List<ObjectCtrl> _lst)
    {
      if (!this.IsOverlapNow)
        return;
      _lst.Add((ObjectCtrl) this);
    }
  }
}
