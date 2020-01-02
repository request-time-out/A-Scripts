// Decompiled with JetBrains decompiler
// Type: Housing.ObjectCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Animal;
using System.Collections.Generic;
using UnityEngine;

namespace Housing
{
  public class ObjectCtrl
  {
    private Transform m_transform;

    public ObjectCtrl(IObjectInfo _objectInfo, GameObject _gameObject, CraftInfo _craftInfo)
    {
      this.ObjectInfo = _objectInfo;
      this.GameObject = _gameObject;
      this.CraftInfo = _craftInfo;
    }

    public IObjectInfo ObjectInfo { get; private set; }

    public GameObject GameObject { get; private set; }

    public CraftInfo CraftInfo { get; private set; }

    public ObjectCtrl Parent { get; private set; }

    public virtual string Name
    {
      get
      {
        return string.Empty;
      }
    }

    public Transform Transform
    {
      get
      {
        return this.m_transform ?? (this.m_transform = this.GameObject?.get_transform());
      }
    }

    public int InfoIndex
    {
      get
      {
        return this.Parent != null ? (!(this.Parent.ObjectInfo is OIFolder objectInfo) ? -1 : objectInfo.Child.IndexOf(this.ObjectInfo)) : this.CraftInfo.ObjectInfos.IndexOf(this.ObjectInfo);
      }
    }

    public int Kind
    {
      get
      {
        return this.ObjectInfo.Kind;
      }
    }

    public virtual bool IsOverlapNow
    {
      get
      {
        return false;
      }
    }

    public virtual void OnAttach(ObjectCtrl _parentCtrl, int _insert = -1)
    {
      if (this.Parent != null)
      {
        if (this.Parent.ObjectInfo is OIFolder objectInfo)
          objectInfo.Child.Remove(this.ObjectInfo);
        if (this.Parent is OCFolder parent)
          parent.Child.Remove(this.ObjectInfo);
        this.Parent = (ObjectCtrl) null;
        this.Transform?.SetParent(this.CraftInfo.ObjRoot.get_transform(), true);
      }
      else
      {
        this.CraftInfo.ObjectInfos.Remove(this.ObjectInfo);
        this.CraftInfo.ObjectCtrls.Remove(this.ObjectInfo);
      }
      if (!(_parentCtrl is OCFolder ocFolder))
      {
        this.ListAdd(this.CraftInfo.ObjectInfos, this.ObjectInfo, _insert);
        if (!this.CraftInfo.ObjectCtrls.ContainsKey(this.ObjectInfo))
          this.CraftInfo.ObjectCtrls.Add(this.ObjectInfo, this);
        this.Parent = (ObjectCtrl) null;
        this.Transform?.SetParent(this.CraftInfo.ObjRoot.get_transform(), true);
      }
      else
      {
        this.ListAdd(ocFolder.OIFolder.Child, this.ObjectInfo, _insert);
        if (!ocFolder.Child.ContainsKey(this.ObjectInfo))
          ocFolder.Child.Add(this.ObjectInfo, this);
        this.Parent = (ObjectCtrl) ocFolder;
        this.Transform?.SetParent(this.Parent.Transform, true);
      }
    }

    private void ListAdd(List<IObjectInfo> _lst, IObjectInfo _info, int _insert)
    {
      if (_insert != -1)
      {
        _lst.Remove(_info);
        if (MathfEx.RangeEqualOn<int>(0, _insert, _lst.Count - 1))
          _lst.Insert(_insert, _info);
        else
          _lst.Add(_info);
      }
      else
      {
        if (_lst.Contains(_info))
          return;
        _lst.Add(_info);
      }
    }

    public virtual bool OnRemoving()
    {
      return true;
    }

    public virtual void OnDelete()
    {
      this.OnAttach((ObjectCtrl) null, -1);
      this.CraftInfo.ObjectInfos.Remove(this.ObjectInfo);
      this.CraftInfo.ObjectCtrls.Remove(this.ObjectInfo);
      this.OnDestroy();
    }

    public virtual void OnDeleteChild()
    {
    }

    public virtual void OnDestroy()
    {
      Object.DestroyImmediate((Object) this.GameObject);
      this.GameObject = (GameObject) null;
      this.m_transform = (Transform) null;
    }

    public virtual void OnSelected()
    {
    }

    public virtual void OnDeselected()
    {
    }

    public virtual void RestoreObject(GameObject _gameObject)
    {
      this.GameObject = _gameObject;
    }

    public virtual void CalcTransform()
    {
      this.Transform.set_localPosition(this.LocalPosition);
      this.Transform.set_localRotation(this.LocalRotation);
    }

    public virtual Vector3 LocalPosition
    {
      get
      {
        return this.ObjectInfo.Pos;
      }
      set
      {
        this.ObjectInfo.Pos = value;
        this.CalcTransform();
      }
    }

    public virtual Vector3 LocalEulerAngles
    {
      get
      {
        return this.ObjectInfo.Rot;
      }
      set
      {
        this.ObjectInfo.Rot = value;
        this.CalcTransform();
      }
    }

    public virtual Quaternion LocalRotation
    {
      get
      {
        return Quaternion.Euler(this.LocalEulerAngles);
      }
      set
      {
        this.LocalEulerAngles = ((Quaternion) ref value).get_eulerAngles();
      }
    }

    public virtual Vector3 Position
    {
      get
      {
        return this.Transform.get_position();
      }
      set
      {
        this.Transform.set_position(value);
        this.ObjectInfo.Pos = this.Transform.get_localPosition();
        if (this.ObjectInfo.Pos.y >= 0.0)
          return;
        this.LocalPosition = new Vector3((float) this.ObjectInfo.Pos.x, 0.0f, (float) this.ObjectInfo.Pos.z);
      }
    }

    public virtual Vector3 EulerAngles
    {
      get
      {
        return this.Transform.get_eulerAngles();
      }
      set
      {
        this.Transform.set_eulerAngles(value);
        this.ObjectInfo.Rot = this.Transform.get_eulerAngles();
      }
    }

    public virtual Quaternion Rotation
    {
      get
      {
        return this.Transform.get_rotation();
      }
      set
      {
        this.EulerAngles = ((Quaternion) ref value).get_eulerAngles();
      }
    }

    public virtual void GetLocalMinMax(
      Vector3 _pos,
      Quaternion _rot,
      Transform _root,
      ref Vector3 _min,
      ref Vector3 _max)
    {
      _min = Vector3.get_one();
      _max = Vector3.get_one();
    }

    public virtual void GetActionPoint(ref List<ActionPoint> _points)
    {
    }

    public virtual void GetFarmPoint(ref List<FarmPoint> _points)
    {
    }

    public virtual void GetHPoint(ref List<HPoint> _points)
    {
    }

    public virtual void GetColInfo(ref List<ItemComponent.ColInfo> _infos)
    {
    }

    public virtual void GetPetHomePoint(ref List<PetHomePoint> _points)
    {
    }

    public virtual void GetJukePoint(ref List<JukePoint> _points)
    {
    }

    public virtual void GetCraftPoint(ref List<CraftPoint> _points)
    {
    }

    public virtual void GetLightSwitchPoint(ref List<LightSwitchPoint> _points)
    {
    }

    public virtual void GetUsedNum(int _no, ref int _num)
    {
    }

    public virtual bool CheckOverlap(ObjectCtrl _oc, bool _load = false)
    {
      return false;
    }

    public virtual void BeforeCheckOverlap()
    {
    }

    public virtual void AfterCheckOverlap()
    {
    }

    public virtual void SetOverlapColliders(bool _flag)
    {
    }

    public virtual void GetOverlapObject(ref List<ObjectCtrl> _lst)
    {
    }
  }
}
