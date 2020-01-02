// Decompiled with JetBrains decompiler
// Type: Housing.OCFolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Animal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Housing
{
  public class OCFolder : ObjectCtrl
  {
    public OCFolder(OIFolder _oiFolder, GameObject _gameObject, CraftInfo _craftInfo)
      : base((IObjectInfo) _oiFolder, _gameObject, _craftInfo)
    {
    }

    public OIFolder OIFolder
    {
      get
      {
        return this.ObjectInfo as OIFolder;
      }
    }

    public Dictionary<IObjectInfo, ObjectCtrl> Child { get; private set; } = new Dictionary<IObjectInfo, ObjectCtrl>();

    public override string Name
    {
      get
      {
        return this.OIFolder.Name;
      }
    }

    public override bool IsOverlapNow
    {
      get
      {
        return this.Child.Any<KeyValuePair<IObjectInfo, ObjectCtrl>>((Func<KeyValuePair<IObjectInfo, ObjectCtrl>, bool>) (v => v.Value.IsOverlapNow));
      }
    }

    public List<ObjectCtrl> ChildObjectCtrls
    {
      get
      {
        return this.Child.OrderBy<KeyValuePair<IObjectInfo, ObjectCtrl>, int>((Func<KeyValuePair<IObjectInfo, ObjectCtrl>, int>) (v => this.OIFolder.Child.FindIndex((Predicate<IObjectInfo>) (_i => _i == v.Key)))).Select<KeyValuePair<IObjectInfo, ObjectCtrl>, ObjectCtrl>((Func<KeyValuePair<IObjectInfo, ObjectCtrl>, ObjectCtrl>) (v => v.Value)).ToList<ObjectCtrl>();
      }
    }

    public override bool OnRemoving()
    {
      bool flag = true;
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        flag &= keyValuePair.Value.OnRemoving();
      return flag;
    }

    public override void OnDelete()
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.OnDeleteChild();
      base.OnDelete();
    }

    public override void OnDeleteChild()
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.OnDeleteChild();
    }

    public override void OnDestroy()
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.OnDestroy();
      base.OnDestroy();
    }

    public override void GetActionPoint(ref List<ActionPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetActionPoint(ref _points);
    }

    public override void GetFarmPoint(ref List<FarmPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetFarmPoint(ref _points);
    }

    public override void GetHPoint(ref List<HPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetHPoint(ref _points);
    }

    public override void GetColInfo(ref List<ItemComponent.ColInfo> _infos)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetColInfo(ref _infos);
    }

    public override void GetPetHomePoint(ref List<PetHomePoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetPetHomePoint(ref _points);
    }

    public override void GetJukePoint(ref List<JukePoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetJukePoint(ref _points);
    }

    public override void GetCraftPoint(ref List<CraftPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetCraftPoint(ref _points);
    }

    public override void GetLightSwitchPoint(ref List<LightSwitchPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetLightSwitchPoint(ref _points);
    }

    public override void GetUsedNum(int _no, ref int _num)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetUsedNum(_no, ref _num);
    }

    public override bool CheckOverlap(ObjectCtrl _oc, bool _load = false)
    {
      bool flag = false;
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        flag |= keyValuePair.Value.CheckOverlap(_oc, _load);
      return flag;
    }

    public override void BeforeCheckOverlap()
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.BeforeCheckOverlap();
    }

    public override void AfterCheckOverlap()
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.AfterCheckOverlap();
    }

    public override void SetOverlapColliders(bool _flag)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.SetOverlapColliders(_flag);
    }

    public override void GetOverlapObject(ref List<ObjectCtrl> _lst)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in this.Child)
        keyValuePair.Value.GetOverlapObject(ref _lst);
    }
  }
}
