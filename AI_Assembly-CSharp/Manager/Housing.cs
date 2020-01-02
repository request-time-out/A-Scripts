// Decompiled with JetBrains decompiler
// Type: Manager.Housing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Animal;
using AIProject.SaveData;
using Housing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;

namespace Manager
{
  public class Housing : Singleton<Manager.Housing>
  {
    public const string SavePath = "housing/";
    private Manager.Housing.WaitTime waitTime;
    private GameObject objScene;
    private CraftScene craftScene;
    private ItemComponent.ColInfo[] colInfos;

    public Dictionary<int, Manager.Housing.LoadInfo> dicLoadInfo { get; private set; } = new Dictionary<int, Manager.Housing.LoadInfo>();

    public Dictionary<int, Manager.Housing.CategoryInfo> dicCategoryInfo { get; private set; } = new Dictionary<int, Manager.Housing.CategoryInfo>();

    public Dictionary<int, Manager.Housing.AreaSizeInfo> dicAreaSizeInfo { get; private set; } = new Dictionary<int, Manager.Housing.AreaSizeInfo>();

    public Dictionary<int, Manager.Housing.AreaInfo> dicAreaInfo { get; private set; } = new Dictionary<int, Manager.Housing.AreaInfo>();

    public bool IsLoadList { get; private set; }

    public CraftInfo CraftInfo { get; private set; }

    public List<ObjectCtrl> ObjectCtrls
    {
      get
      {
        return this.CraftInfo == null ? (List<ObjectCtrl>) null : this.CraftInfo.ObjectCtrls.OrderBy<KeyValuePair<IObjectInfo, ObjectCtrl>, int>((Func<KeyValuePair<IObjectInfo, ObjectCtrl>, int>) (v => this.CraftInfo.ObjectInfos.FindIndex((Predicate<IObjectInfo>) (_i => _i == v.Key)))).Select<KeyValuePair<IObjectInfo, ObjectCtrl>, ObjectCtrl>((Func<KeyValuePair<IObjectInfo, ObjectCtrl>, ObjectCtrl>) (v => v.Value)).ToList<ObjectCtrl>();
      }
    }

    private Dictionary<int, GameObject> ObjRoots { get; set; } = new Dictionary<int, GameObject>();

    public bool IsCraft
    {
      get
      {
        return Object.op_Implicit((Object) this.objScene) && this.objScene.get_activeSelf();
      }
    }

    [DebuggerHidden]
    private IEnumerator LoadExcelDataCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Manager.Housing.\u003CLoadExcelDataCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void LoadCategoryInfo(ExcelData _ed, Dictionary<int, Manager.Housing.CategoryInfo> _dic)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList in _ed.list.Skip<ExcelData.Param>(2).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int result = -1;
        if (!int.TryParse(stringList.SafeGet<string>(0), out result))
          break;
        _dic[result] = new Manager.Housing.CategoryInfo(stringList);
      }
    }

    private void LoadLoadInfo(ExcelData _ed, Dictionary<int, Manager.Housing.LoadInfo> _dic, int _pack)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList in _ed.list.Skip<ExcelData.Param>(2).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int result = -1;
        if (!int.TryParse(stringList.SafeGet<string>(0), out result))
          break;
        _dic[result] = new Manager.Housing.LoadInfo(_pack, stringList);
      }
    }

    private void LoadAreaSizeInfo(ExcelData _ed)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList in _ed.list.Skip<ExcelData.Param>(2).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int result = -1;
        if (!int.TryParse(stringList.SafeGet<string>(0), out result))
          break;
        this.dicAreaSizeInfo[result] = new Manager.Housing.AreaSizeInfo(stringList);
      }
    }

    private void LoadAreaInfo(ExcelData _ed)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList in _ed.list.Skip<ExcelData.Param>(2).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int result = -1;
        if (!int.TryParse(stringList.SafeGet<string>(0), out result))
          break;
        this.dicAreaInfo[result] = new Manager.Housing.AreaInfo(stringList);
      }
    }

    public void SetCraftInfo(CraftInfo _craftInfo, bool _load = true)
    {
      this.CraftInfo = _craftInfo;
      if (!_load)
        return;
      this.LoadObject();
    }

    [DebuggerHidden]
    public IEnumerator LoadHousing(int _type = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Manager.Housing.\u003CLoadHousing\u003Ec__Iterator1()
      {
        _type = _type,
        \u0024this = this
      };
    }

    public void Release()
    {
      if (Singleton<Game>.Instance.Data.AutoData != null)
      {
        HousingData housingData = Singleton<Game>.Instance.Data.AutoData.HousingData;
        if (housingData != null)
        {
          foreach (KeyValuePair<int, CraftInfo> craftInfo in housingData.CraftInfos)
            craftInfo.Value.ObjectCtrls.Clear();
        }
      }
      foreach (KeyValuePair<int, WorldData> world in Singleton<Game>.Instance.Data.WorldList)
      {
        if (world.Value != null)
        {
          HousingData housingData = world.Value.HousingData;
          if (housingData != null)
          {
            foreach (KeyValuePair<int, CraftInfo> craftInfo in housingData.CraftInfos)
              craftInfo.Value.ObjectCtrls.Clear();
          }
        }
      }
      foreach (KeyValuePair<int, CraftInfo> craftInfo in Singleton<Game>.Instance.WorldData.HousingData.CraftInfos)
        craftInfo.Value.ObjectCtrls.Clear();
      this.ObjRoots.Clear();
    }

    public bool StartHousing()
    {
      this.objScene = CommonLib.LoadAsset<GameObject>("housing/base/06.unity3d", "CraftScene", true, string.Empty);
      if (Object.op_Equality((Object) this.objScene, (Object) null))
        return false;
      this.craftScene = (CraftScene) this.objScene.GetComponent<CraftScene>();
      int tutorialID = 13;
      this.craftScene.DisplayTutorial = Singleton<MapUIContainer>.IsInstance() && !MapUIContainer.GetTutorialOpenState(tutorialID);
      if (this.craftScene.DisplayTutorial)
        MapUIContainer.TutorialUI.BlockRaycastEnabled = true;
      ObservableExtensions.Subscribe<bool>(Observable.Take<bool>(Observable.Where<bool>((IObservable<M0>) ObserveExtensions.ObserveEveryValueChanged<CraftScene, bool>((M0) this.craftScene, (Func<M0, M1>) (cs => cs.IsInit), (FrameCountType) 0, false), (Func<M0, bool>) (b => b)), 1), (Action<M0>) (_ => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 1f, true), (Action<M0>) (__ => {}), (Action) (() =>
      {
        if (!this.craftScene.DisplayTutorial)
          return;
        MapUIContainer.TutorialUI.ClosedEvent = (Action) (() => this.craftScene.DisplayTutorial = false);
        MapUIContainer.OpenTutorialUI(tutorialID, false);
      }))));
      return true;
    }

    public void EndHousing()
    {
      this.CraftInfo?.SetOverlapColliders(false);
      if (Object.op_Implicit((Object) this.objScene))
        Object.DestroyImmediate((Object) this.objScene);
      this.objScene = (GameObject) null;
      this.craftScene = (CraftScene) null;
    }

    public ActionPoint[] ActionPoints
    {
      get
      {
        List<ActionPoint> _points = new List<ActionPoint>();
        foreach (KeyValuePair<int, CraftInfo> craftInfo in Singleton<Game>.Instance.WorldData.HousingData.CraftInfos)
          craftInfo.Value?.GetActionPoint(ref _points);
        return _points.ToArray();
      }
    }

    public FarmPoint[] FarmPoints
    {
      get
      {
        List<FarmPoint> _points = new List<FarmPoint>();
        foreach (KeyValuePair<int, CraftInfo> craftInfo in Singleton<Game>.Instance.WorldData.HousingData.CraftInfos)
          craftInfo.Value?.GetFarmPoint(ref _points);
        return _points.ToArray();
      }
    }

    public HPoint[] HPoints
    {
      get
      {
        List<HPoint> _points = new List<HPoint>();
        foreach (KeyValuePair<int, CraftInfo> craftInfo in Singleton<Game>.Instance.WorldData.HousingData.CraftInfos)
          craftInfo.Value?.GetHPoint(ref _points);
        return _points.ToArray();
      }
    }

    public HPoint[] GetHPoint(int _id)
    {
      List<HPoint> _points = new List<HPoint>();
      HousingData housingData = Singleton<Game>.Instance.WorldData.HousingData;
      CraftInfo craftInfo = (CraftInfo) null;
      if (housingData.CraftInfos.TryGetValue(_id, out craftInfo) && craftInfo != null)
        craftInfo.GetHPoint(ref _points);
      return _points.ToArray();
    }

    public ItemComponent.ColInfo[] GetColInfo(int _id)
    {
      List<ItemComponent.ColInfo> _infos = new List<ItemComponent.ColInfo>();
      HousingData housingData = Singleton<Game>.Instance.WorldData.HousingData;
      CraftInfo craftInfo = (CraftInfo) null;
      if (housingData.CraftInfos.TryGetValue(_id, out craftInfo) && craftInfo != null)
        craftInfo.GetColInfo(ref _infos);
      return _infos.ToArray();
    }

    public void StartShield(int _id)
    {
      List<ItemComponent.ColInfo> _infos = new List<ItemComponent.ColInfo>();
      HousingData housingData = Singleton<Game>.Instance.WorldData.HousingData;
      CraftInfo craftInfo = (CraftInfo) null;
      if (housingData.CraftInfos.TryGetValue(_id, out craftInfo) && craftInfo != null)
        craftInfo.GetColInfo(ref _infos);
      else
        Debug.LogWarningFormat("指定されたID[{0}]は対象外なので遮蔽情報の取得出来ません。", new object[1]
        {
          (object) _id
        });
      this.colInfos = _infos.ToArray();
    }

    public void ShieldProc(Collider _collider)
    {
      if (((IList<ItemComponent.ColInfo>) this.colInfos).IsNullOrEmpty<ItemComponent.ColInfo>())
        return;
      foreach (ItemComponent.ColInfo colInfo in this.colInfos)
        colInfo.CheckCollision(_collider);
    }

    public void EndShield()
    {
      this.VisibleShield();
      this.colInfos = (ItemComponent.ColInfo[]) null;
    }

    public void VisibleShield()
    {
      if (((IList<ItemComponent.ColInfo>) this.colInfos).IsNullOrEmpty<ItemComponent.ColInfo>())
        return;
      foreach (ItemComponent.ColInfo colInfo in this.colInfos)
        colInfo.Visible = true;
    }

    public PetHomePoint[] PetHomePoints
    {
      get
      {
        List<PetHomePoint> _points = new List<PetHomePoint>();
        foreach (KeyValuePair<int, CraftInfo> craftInfo in Singleton<Game>.Instance.WorldData.HousingData.CraftInfos)
          craftInfo.Value?.GetPetHomePoint(ref _points);
        return _points.ToArray();
      }
    }

    public JukePoint[] JukePoints
    {
      get
      {
        List<JukePoint> _points = new List<JukePoint>();
        foreach (KeyValuePair<int, CraftInfo> craftInfo in Singleton<Game>.Instance.WorldData.HousingData.CraftInfos)
          craftInfo.Value?.GetJukePoint(ref _points);
        return _points.ToArray();
      }
    }

    public CraftPoint[] CraftPoints
    {
      get
      {
        List<CraftPoint> _points = new List<CraftPoint>();
        foreach (KeyValuePair<int, CraftInfo> craftInfo in Singleton<Game>.Instance.WorldData.HousingData.CraftInfos)
          craftInfo.Value?.GetCraftPoint(ref _points);
        return _points.ToArray();
      }
    }

    public LightSwitchPoint[] LightSwitchPoints
    {
      get
      {
        List<LightSwitchPoint> _points = new List<LightSwitchPoint>();
        foreach (KeyValuePair<int, CraftInfo> craftInfo in Singleton<Game>.Instance.WorldData.HousingData.CraftInfos)
          craftInfo.Value?.GetLightSwitchPoint(ref _points);
        return _points.ToArray();
      }
    }

    public int GetSizeType(int _area)
    {
      Manager.Housing.AreaInfo areaInfo = (Manager.Housing.AreaInfo) null;
      return this.dicAreaInfo.TryGetValue(_area, out areaInfo) ? areaInfo.size : 0;
    }

    public GameObject GetRoot(int _idx)
    {
      GameObject gameObject = (GameObject) null;
      if (!this.ObjRoots.TryGetValue(_idx, out gameObject))
      {
        gameObject = new GameObject(string.Format("housing {0}", (object) _idx));
        if (Singleton<Map>.IsInstance() && Object.op_Implicit((Object) Singleton<Map>.Instance.NavMeshSurface))
          gameObject.get_transform().SetParent(((Component) Singleton<Map>.Instance.NavMeshSurface).get_transform());
        Transform transform = (Transform) null;
        if (Singleton<Map>.Instance.HousingPointTable.TryGetValue(_idx, out transform))
        {
          gameObject.get_transform().SetPositionAndRotation(transform.get_position(), transform.get_rotation());
        }
        else
        {
          gameObject.get_transform().set_localPosition(Vector3.get_zero());
          gameObject.get_transform().set_localRotation(Quaternion.get_identity());
        }
        this.ObjRoots.Add(_idx, gameObject);
      }
      return gameObject;
    }

    public void DeleteRoot()
    {
      using (Dictionary<int, GameObject>.Enumerator enumerator = this.ObjRoots.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, GameObject> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Value, (Object) null))
            Object.DestroyImmediate((Object) current.Value);
        }
      }
      this.ObjRoots.Clear();
    }

    public OCItem AddObject(int _id)
    {
      if (!this.dicLoadInfo.ContainsKey(_id))
        return (OCItem) null;
      return this.LoadObject(new OIItem() { ID = _id }, (ObjectCtrl) null, true, true);
    }

    public ObjectCtrl AddFolder()
    {
      return this.LoadFolder(new OIFolder(), (ObjectCtrl) null, false);
    }

    public ObjectCtrl DuplicateObject(ObjectCtrl _src)
    {
      if (_src == null)
        return (ObjectCtrl) null;
      ObjectCtrl objectCtrl = (ObjectCtrl) null;
      switch (_src.ObjectInfo.Kind)
      {
        case 0:
          if (!this.CheckLimitNum((_src.ObjectInfo as OIItem).ID))
            return (ObjectCtrl) null;
          objectCtrl = (ObjectCtrl) this.LoadObject(new OIItem(_src.ObjectInfo as OIItem), _src.Parent, false, true);
          break;
        case 1:
          objectCtrl = this.LoadFolder(new OIFolder(_src.ObjectInfo as OIFolder), _src.Parent, true);
          break;
      }
      return objectCtrl;
    }

    public bool RestoreObject(ObjectCtrl _src, ObjectCtrl _parent, int _insert, bool _info = true)
    {
      switch (_src.ObjectInfo.Kind)
      {
        case 0:
          this.RestoreItem(_src as OCItem, _parent, _insert, _info);
          return this.CheckOverlap((ObjectCtrl) (_src as OCItem));
        case 1:
          return this.RestoreFolder(_src as OCFolder, _parent, _insert, _info);
        default:
          return false;
      }
    }

    public bool Load(string _path, bool _keep = true, bool _create = false)
    {
      this.DeleteObject();
      Vector3 limitSize = this.CraftInfo.LimitSize;
      int areaNo = this.CraftInfo.AreaNo;
      if (!this.CraftInfo.Load(_path))
        return false;
      if (_keep)
      {
        this.CraftInfo.LimitSize = limitSize;
        this.CraftInfo.AreaNo = areaNo;
      }
      this.LoadObject();
      if (_create)
        this.CraftInfo.SetOverlapColliders(true);
      return true;
    }

    public bool LoadObject()
    {
      if (this.CraftInfo == null)
        return false;
      this.DeleteObject();
      for (int index = 0; index < this.CraftInfo.ObjectInfos.Count; ++index)
        this.CreateObjectCtrl(this.CraftInfo.ObjectInfos[index], (ObjectCtrl) null, false);
      return true;
    }

    public void ResetObject()
    {
      this.DeleteObject();
      this.CraftInfo?.ObjectCtrls.Clear();
      this.CraftInfo?.ObjectInfos.Clear();
    }

    public bool CheckLimitNum(int _no)
    {
      Manager.Housing.LoadInfo loadInfo = (Manager.Housing.LoadInfo) null;
      if (!this.dicLoadInfo.TryGetValue(_no, out loadInfo))
        return false;
      return loadInfo.limitNum < 0 || this.CraftInfo.GetUsedNum(_no) < loadInfo.limitNum;
    }

    public bool CheckOverlap(ObjectCtrl _oc)
    {
      bool flag = false;
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.CraftInfo.ObjectCtrls)
      {
        flag |= objectCtrl.Value.CheckOverlap(_oc, false);
        if (_oc is OCFolder ocFolder)
        {
          foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in ocFolder.Child)
            flag |= this.CheckOverlap(keyValuePair.Value);
        }
      }
      return flag;
    }

    public bool CheckOverlap()
    {
      bool flag = false;
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.CraftInfo.ObjectCtrls)
        flag |= this.CheckOverlap(objectCtrl.Value);
      return flag;
    }

    public bool CheckSize(Vector3 _size)
    {
      Vector3 limitSize = this.CraftInfo.LimitSize;
      return _size.x <= limitSize.x && _size.y <= limitSize.y && _size.z <= limitSize.z;
    }

    private OCItem LoadObject(OIItem _oiItem, ObjectCtrl _parent, bool _new = false, bool _create = false)
    {
      OCItem ocItem = this.CreateOCItem(_oiItem);
      if (ocItem == null)
        return (OCItem) null;
      if (_parent == null)
        this.AddInfoAndCtrl(this.CraftInfo.ObjectInfos, (IObjectInfo) _oiItem, this.CraftInfo.ObjectCtrls, (ObjectCtrl) ocItem, -1);
      else
        ocItem.OnAttach(_parent, -1);
      if (_new)
      {
        _oiItem.Pos = ocItem.ItemComponent.initPos;
        _oiItem.Color1 = ocItem.ItemComponent.defColor1;
        _oiItem.Color2 = ocItem.ItemComponent.defColor2;
        _oiItem.Color3 = ocItem.ItemComponent.defColor3;
        _oiItem.EmissionColor = ocItem.ItemComponent.defEmissionColor;
      }
      if (_create)
        ocItem.ItemComponent.SetOverlapColliders(true);
      ocItem.VisibleOption = ocItem.VisibleOption;
      ocItem.UpdateColor();
      ocItem.CalcTransform();
      ActionPoint[] actionPoints = ocItem.ActionPoints;
      if (!((IList<ActionPoint>) actionPoints).IsNullOrEmpty<ActionPoint>() && (((IList<int>) _oiItem.ActionPoints).IsNullOrEmpty<int>() || actionPoints.Length != _oiItem.ActionPoints.Length))
        _oiItem.ActionPoints = Enumerable.Repeat<int>(-1, actionPoints.Length).ToArray<int>();
      for (int index = 0; index < actionPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) actionPoints[index], (Object) null))
        {
          actionPoints[index].RegisterID = _oiItem.ActionPoints[index];
          _oiItem.ActionPoints[index] = Singleton<Map>.Instance.RegisterRuntimePoint((Point) actionPoints[index]);
        }
      }
      FarmPoint[] farmPoints = ocItem.FarmPoints;
      if (!((IList<FarmPoint>) farmPoints).IsNullOrEmpty<FarmPoint>() && (((IList<int>) _oiItem.FarmPoints).IsNullOrEmpty<int>() || farmPoints.Length != _oiItem.FarmPoints.Length))
        _oiItem.FarmPoints = Enumerable.Repeat<int>(-1, farmPoints.Length).ToArray<int>();
      for (int index = 0; index < farmPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) farmPoints[index], (Object) null))
        {
          farmPoints[index].RegisterID = _oiItem.FarmPoints[index];
          _oiItem.FarmPoints[index] = Singleton<Map>.Instance.RegisterRuntimePoint((Point) farmPoints[index]);
        }
      }
      PetHomePoint[] petHomePoints = ocItem.PetHomePoints;
      if (!((IList<PetHomePoint>) petHomePoints).IsNullOrEmpty<PetHomePoint>() && (((IList<int>) _oiItem.PetHomePoints).IsNullOrEmpty<int>() || petHomePoints.Length != _oiItem.PetHomePoints.Length))
        _oiItem.PetHomePoints = Enumerable.Repeat<int>(-1, petHomePoints.Length).ToArray<int>();
      for (int index = 0; index < petHomePoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) petHomePoints[index], (Object) null))
        {
          petHomePoints[index].RegisterID = _oiItem.PetHomePoints[index];
          _oiItem.PetHomePoints[index] = Singleton<Map>.Instance.RegisterRuntimePoint((Point) petHomePoints[index]);
        }
      }
      JukePoint[] jukePoints = ocItem.JukePoints;
      if (!((IList<JukePoint>) jukePoints).IsNullOrEmpty<JukePoint>() && (((IList<int>) _oiItem.JukePoints).IsNullOrEmpty<int>() || jukePoints.Length != _oiItem.JukePoints.Length))
        _oiItem.JukePoints = Enumerable.Repeat<int>(-1, jukePoints.Length).ToArray<int>();
      for (int index = 0; index < jukePoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) jukePoints[index], (Object) null))
        {
          jukePoints[index].RegisterID = _oiItem.JukePoints[index];
          _oiItem.JukePoints[index] = Singleton<Map>.Instance.RegisterRuntimePoint((Point) jukePoints[index]);
        }
      }
      CraftPoint[] craftPoints = ocItem.CraftPoints;
      if (!((IList<CraftPoint>) craftPoints).IsNullOrEmpty<CraftPoint>() && (((IList<int>) _oiItem.CraftPoints).IsNullOrEmpty<int>() || craftPoints.Length != _oiItem.CraftPoints.Length))
        _oiItem.CraftPoints = Enumerable.Repeat<int>(-1, craftPoints.Length).ToArray<int>();
      for (int index = 0; index < craftPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) craftPoints[index], (Object) null))
        {
          craftPoints[index].RegisterID = _oiItem.CraftPoints[index];
          _oiItem.CraftPoints[index] = Singleton<Map>.Instance.RegisterRuntimePoint((Point) craftPoints[index]);
        }
      }
      LightSwitchPoint[] lightSwitchPoints = ocItem.LightSwitchPoints;
      if (!((IList<LightSwitchPoint>) lightSwitchPoints).IsNullOrEmpty<LightSwitchPoint>() && (((IList<int>) _oiItem.LightSwitchPoints).IsNullOrEmpty<int>() || lightSwitchPoints.Length != _oiItem.LightSwitchPoints.Length))
        _oiItem.LightSwitchPoints = Enumerable.Repeat<int>(-1, lightSwitchPoints.Length).ToArray<int>();
      for (int index = 0; index < lightSwitchPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) lightSwitchPoints[index], (Object) null))
        {
          lightSwitchPoints[index].RegisterID = _oiItem.LightSwitchPoints[index];
          _oiItem.LightSwitchPoints[index] = Singleton<Map>.Instance.RegisterRuntimePoint((Point) lightSwitchPoints[index]);
        }
      }
      return ocItem;
    }

    private ObjectCtrl LoadFolder(OIFolder _oiFolder, ObjectCtrl _parent, bool _check)
    {
      ObjectCtrl ocFolder = this.CreateOCFolder(_oiFolder);
      if (ocFolder == null)
        return (ObjectCtrl) null;
      if (_parent == null)
        this.AddInfoAndCtrl(this.CraftInfo.ObjectInfos, (IObjectInfo) _oiFolder, this.CraftInfo.ObjectCtrls, ocFolder, -1);
      else
        ocFolder.OnAttach(_parent, -1);
      ocFolder.CalcTransform();
      List<IObjectInfo> self = new List<IObjectInfo>();
      foreach (IObjectInfo _objectInfo in _oiFolder.Child)
      {
        if (this.CreateObjectCtrl(_objectInfo, ocFolder, _check) == null)
          self.Add(_objectInfo);
      }
      if (!self.IsNullOrEmpty<IObjectInfo>())
      {
        foreach (IObjectInfo objectInfo in self)
          _oiFolder.Child.Remove(objectInfo);
      }
      return ocFolder;
    }

    private ObjectCtrl CreateObjectCtrl(
      IObjectInfo _objectInfo,
      ObjectCtrl _parent,
      bool _check = false)
    {
      ObjectCtrl objectCtrl = (ObjectCtrl) null;
      switch (_objectInfo.Kind)
      {
        case 0:
          OIItem oiItem = _objectInfo as OIItem;
          if (_check && !this.CheckLimitNum(oiItem.ID))
            return (ObjectCtrl) null;
          objectCtrl = (ObjectCtrl) this.LoadObject(_objectInfo as OIItem, _parent, false, _check);
          break;
        case 1:
          objectCtrl = this.LoadFolder(_objectInfo as OIFolder, _parent, _check);
          break;
      }
      return objectCtrl;
    }

    private OCItem CreateOCItem(OIItem _objectInfo)
    {
      Manager.Housing.LoadInfo _loadInfo = (Manager.Housing.LoadInfo) null;
      if (!this.dicLoadInfo.TryGetValue(_objectInfo.ID, out _loadInfo))
        return (OCItem) null;
      GameObject _gameObject = this.LoadObject(_objectInfo);
      return new OCItem(_objectInfo, _gameObject, this.CraftInfo, _loadInfo);
    }

    private GameObject LoadObject(OIItem _objectInfo)
    {
      Manager.Housing.LoadInfo loadInfo = (Manager.Housing.LoadInfo) null;
      if (!this.dicLoadInfo.TryGetValue(_objectInfo.ID, out loadInfo))
        return (GameObject) null;
      GameObject gameObject = CommonLib.LoadAsset<GameObject>(loadInfo.filePath.bundle, loadInfo.filePath.file, true, loadInfo.filePath.manifest);
      if (Object.op_Equality((Object) gameObject, (Object) null))
        return (GameObject) null;
      gameObject.get_transform().SetParent(this.CraftInfo.ObjRoot.get_transform());
      return gameObject;
    }

    private ObjectCtrl CreateOCFolder(OIFolder _oiFolder)
    {
      GameObject _gameObject = new GameObject(_oiFolder.Name);
      if (Object.op_Equality((Object) _gameObject, (Object) null))
        return (ObjectCtrl) null;
      _gameObject.get_transform().SetParent(this.CraftInfo.ObjRoot.get_transform());
      return (ObjectCtrl) new OCFolder(_oiFolder, _gameObject, this.CraftInfo);
    }

    public void DeleteObject()
    {
      this.CraftInfo?.DeleteObject();
    }

    public void DeleteObject(ObjectCtrl _objectCtrl)
    {
      _objectCtrl?.OnDelete();
    }

    private void RestoreItem(OCItem _ocItem, ObjectCtrl _parent, int _insert, bool _info)
    {
      _ocItem.RestoreObject(this.LoadObject(_ocItem.OIItem));
      if (_info)
      {
        if (_parent == null)
          this.AddInfoAndCtrl(this.CraftInfo.ObjectInfos, _ocItem.ObjectInfo, this.CraftInfo.ObjectCtrls, (ObjectCtrl) _ocItem, _insert);
        else
          _ocItem.OnAttach(_parent, _insert);
      }
      _ocItem.ItemComponent.SetOverlapColliders(true);
      _ocItem.VisibleOption = _ocItem.VisibleOption;
      _ocItem.UpdateColor();
      _ocItem.CalcTransform();
      if (!Singleton<Map>.IsInstance())
        return;
      OIItem oiItem = _ocItem.OIItem;
      ActionPoint[] actionPoints = _ocItem.ActionPoints;
      for (int index = 0; index < actionPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) actionPoints[index], (Object) null))
        {
          actionPoints[index].RegisterID = oiItem.ActionPoints[index];
          Singleton<Map>.Instance.RemoveRegIDCache(actionPoints[index].RegisterID);
        }
      }
      FarmPoint[] farmPoints = _ocItem.FarmPoints;
      for (int index = 0; index < farmPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) farmPoints[index], (Object) null))
        {
          farmPoints[index].RegisterID = oiItem.FarmPoints[index];
          foreach (FarmSection harvestSection in farmPoints[index].HarvestSections)
            harvestSection.HarvestID = farmPoints[index].RegisterID;
          Singleton<Map>.Instance.RemoveRegIDCache(farmPoints[index].RegisterID);
        }
      }
      PetHomePoint[] petHomePoints = _ocItem.PetHomePoints;
      for (int index = 0; index < petHomePoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) petHomePoints[index], (Object) null))
        {
          petHomePoints[index].RegisterID = oiItem.PetHomePoints[index];
          Singleton<Map>.Instance.RemoveRegIDCache(petHomePoints[index].RegisterID);
        }
      }
      JukePoint[] jukePoints = _ocItem.JukePoints;
      for (int index = 0; index < jukePoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) jukePoints[index], (Object) null))
        {
          jukePoints[index].RegisterID = oiItem.JukePoints[index];
          Singleton<Map>.Instance.RemoveRegIDCache(jukePoints[index].RegisterID);
        }
      }
      CraftPoint[] craftPoints = _ocItem.CraftPoints;
      for (int index = 0; index < craftPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) craftPoints[index], (Object) null))
        {
          craftPoints[index].RegisterID = oiItem.CraftPoints[index];
          Singleton<Map>.Instance.RemoveRegIDCache(craftPoints[index].RegisterID);
        }
      }
      LightSwitchPoint[] lightSwitchPoints = _ocItem.LightSwitchPoints;
      for (int index = 0; index < lightSwitchPoints.Length; ++index)
      {
        if (!Object.op_Equality((Object) lightSwitchPoints[index], (Object) null))
        {
          lightSwitchPoints[index].RegisterID = oiItem.LightSwitchPoints[index];
          Singleton<Map>.Instance.RemoveRegIDCache(lightSwitchPoints[index].RegisterID);
        }
      }
    }

    private bool RestoreFolder(OCFolder _ocFolder, ObjectCtrl _parent, int _insert, bool _info)
    {
      GameObject _gameObject = new GameObject(_ocFolder.Name);
      if (Object.op_Equality((Object) _gameObject, (Object) null))
        return false;
      _gameObject.get_transform().SetParent(this.CraftInfo.ObjRoot.get_transform());
      _ocFolder.RestoreObject(_gameObject);
      if (_info)
      {
        if (_parent == null)
          this.AddInfoAndCtrl(this.CraftInfo.ObjectInfos, _ocFolder.ObjectInfo, this.CraftInfo.ObjectCtrls, (ObjectCtrl) _ocFolder, _insert);
        else
          _ocFolder.OnAttach(_parent, _insert);
      }
      _ocFolder.CalcTransform();
      bool flag = false;
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> keyValuePair in _ocFolder.Child)
        flag |= this.RestoreObject(keyValuePair.Value, (ObjectCtrl) _ocFolder, -1, false);
      return flag;
    }

    private void AddInfoAndCtrl(
      List<IObjectInfo> _objectInfos,
      IObjectInfo _objectInfo,
      Dictionary<IObjectInfo, ObjectCtrl> _objectCtrls,
      ObjectCtrl _objectCtrl,
      int _insert = -1)
    {
      if (!_objectInfos.Contains(_objectInfo))
      {
        if (_insert != -1)
          _objectInfos.Insert(_insert, _objectInfo);
        else
          _objectInfos.Add(_objectInfo);
      }
      if (_objectCtrls.ContainsKey(_objectInfo))
        return;
      _objectCtrls.Add(_objectInfo, _objectCtrl);
    }

    private void AddCtrl(
      Dictionary<IObjectInfo, ObjectCtrl> _objectCtrls,
      IObjectInfo _objectInfo,
      ObjectCtrl _objectCtrl)
    {
      if (_objectCtrls.ContainsKey(_objectInfo))
        return;
      _objectCtrls.Add(_objectInfo, _objectCtrl);
    }

    public bool LoadAsync(string _path, Action<bool> _afterAction)
    {
      this.DeleteObject();
      Vector3 limitSize = this.CraftInfo.LimitSize;
      int areaNo = this.CraftInfo.AreaNo;
      if (!this.CraftInfo.Load(_path))
      {
        Action<bool> action = _afterAction;
        if (action != null)
          action(false);
        return false;
      }
      this.CraftInfo.LimitSize = limitSize;
      this.CraftInfo.AreaNo = areaNo;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine(new Func<IEnumerator>(this.LoadAsync), false), (Action<M0>) (_ => {}), (Action) (() =>
      {
        Action<bool> action = _afterAction;
        if (action == null)
          return;
        action(true);
      }));
      return true;
    }

    [DebuggerHidden]
    private IEnumerator LoadAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Manager.Housing.\u003CLoadAsync\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void GetOverlapTargets(ObjectCtrl _oc, HashSet<OCItem> _targets)
    {
      switch (_oc)
      {
        case OCItem _:
          _targets.Add(_oc as OCItem);
          break;
        case OCFolder ocFolder:
          using (Dictionary<IObjectInfo, ObjectCtrl>.Enumerator enumerator = ocFolder.Child.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.GetOverlapTargets(enumerator.Current.Value, _targets);
            break;
          }
      }
    }

    [DebuggerHidden]
    private IEnumerator LoadObjectAsync(CraftInfo _craftInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Manager.Housing.\u003CLoadObjectAsync\u003Ec__Iterator3()
      {
        _craftInfo = _craftInfo,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateObjectCtrlAsync(
      IObjectInfo _objectInfo,
      ObjectCtrl _parent)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Manager.Housing.\u003CCreateObjectCtrlAsync\u003Ec__Iterator4()
      {
        _objectInfo = _objectInfo,
        _parent = _parent,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator LoadFolderAsync(OIFolder _oiFolder, ObjectCtrl _parent)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Manager.Housing.\u003CLoadFolderAsync\u003Ec__Iterator5()
      {
        _oiFolder = _oiFolder,
        _parent = _parent,
        \u0024this = this
      };
    }

    public bool[] CheckMOD(CraftInfo _craftInfo, Dictionary<IObjectInfo, int> _modObjects)
    {
      bool[] flagArray = new bool[3];
      if (_craftInfo == null)
        return flagArray;
      if (!_craftInfo.ObjectInfos.IsNullOrEmpty<IObjectInfo>())
      {
        foreach (IObjectInfo objectInfo in _craftInfo.ObjectInfos)
          flagArray[0] |= this.CheckMOD(objectInfo, _modObjects);
      }
      Manager.Housing.AreaInfo areaInfo = (Manager.Housing.AreaInfo) null;
      if (this.dicAreaInfo.TryGetValue(_craftInfo.AreaNo, out areaInfo))
      {
        Manager.Housing.AreaSizeInfo areaSizeInfo = (Manager.Housing.AreaSizeInfo) null;
        flagArray[1] = !this.dicAreaSizeInfo.TryGetValue(areaInfo.size, out areaSizeInfo) || Vector3.op_Inequality(_craftInfo.LimitSize, Vector3Int.op_Implicit(areaSizeInfo.limitSize));
      }
      else
        flagArray[1] = true;
      flagArray[2] = !this.dicAreaInfo.ContainsKey(_craftInfo.AreaNo);
      return flagArray;
    }

    private bool CheckMOD(IObjectInfo _objectInfo, Dictionary<IObjectInfo, int> _modObjects)
    {
      bool flag = false;
      switch (_objectInfo.Kind)
      {
        case 0:
          OIItem oiItem = _objectInfo as OIItem;
          int num = 0;
          if (!this.dicCategoryInfo.ContainsKey(oiItem.Category))
            ++num;
          if (!this.dicLoadInfo.ContainsKey(oiItem.ID))
            num += 2;
          flag = num != 0;
          if (flag)
          {
            _modObjects.Add(_objectInfo, num);
            break;
          }
          break;
        case 1:
          OIFolder oiFolder = _objectInfo as OIFolder;
          if (!oiFolder.Child.IsNullOrEmpty<IObjectInfo>())
          {
            using (List<IObjectInfo>.Enumerator enumerator = oiFolder.Child.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IObjectInfo current = enumerator.Current;
                flag |= this.CheckMOD(current, _modObjects);
              }
              break;
            }
          }
          else
            break;
      }
      return flag;
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      this.StartCoroutine("LoadExcelDataCoroutine");
    }

    public class PathInfo
    {
      public string bundle = string.Empty;
      public string file = string.Empty;
      public string manifest = string.Empty;

      public bool IsNullOrEmpty
      {
        get
        {
          return this.bundle.IsNullOrEmpty() || this.file.IsNullOrEmpty();
        }
      }
    }

    public class RequiredMaterial
    {
      public int category = -1;
      public int no = -1;
      public int num = -1;

      public RequiredMaterial()
      {
      }

      public RequiredMaterial(string[] _s)
      {
        _s.SafeProc(0, (Action<string>) (s => int.TryParse(s, out this.category)));
        _s.SafeProc(1, (Action<string>) (s => int.TryParse(s, out this.no)));
        _s.SafeProc(2, (Action<string>) (s => int.TryParse(s, out this.num)));
      }
    }

    public class LoadInfo
    {
      public string name = string.Empty;
      public string text = string.Empty;
      public Manager.Housing.PathInfo filePath = new Manager.Housing.PathInfo();
      public Manager.Housing.PathInfo thumbnailPath = new Manager.Housing.PathInfo();
      public Vector3 size = Vector3.get_one();
      public int limitNum = -1;
      public int package;
      public int category;
      public Manager.Housing.RequiredMaterial[] requiredMaterials;
      public bool useOption;
      public bool isAccess;
      public bool isAction;
      public bool isHPoint;

      public LoadInfo(int _package, List<string> _lst)
      {
        int num1 = 1;
        this.package = _package;
        List<string> stringList1 = _lst;
        int index1 = num1;
        int num2 = index1 + 1;
        this.category = int.Parse(stringList1[index1]);
        List<string> stringList2 = _lst;
        int index2 = num2;
        int num3 = index2 + 1;
        this.name = stringList2[index2];
        List<string> stringList3 = _lst;
        int index3 = num3;
        int num4 = index3 + 1;
        this.text = stringList3[index3];
        Manager.Housing.PathInfo filePath1 = this.filePath;
        List<string> list1 = _lst;
        int index4 = num4;
        int num5 = index4 + 1;
        string str1 = list1.SafeGet<string>(index4);
        filePath1.bundle = str1;
        Manager.Housing.PathInfo filePath2 = this.filePath;
        List<string> list2 = _lst;
        int index5 = num5;
        int num6 = index5 + 1;
        string str2 = list2.SafeGet<string>(index5);
        filePath2.file = str2;
        Manager.Housing.PathInfo filePath3 = this.filePath;
        List<string> list3 = _lst;
        int index6 = num6;
        int num7 = index6 + 1;
        string str3 = list3.SafeGet<string>(index6);
        filePath3.manifest = str3;
        Manager.Housing.PathInfo thumbnailPath1 = this.thumbnailPath;
        List<string> list4 = _lst;
        int index7 = num7;
        int num8 = index7 + 1;
        string str4 = list4.SafeGet<string>(index7);
        thumbnailPath1.bundle = str4;
        Manager.Housing.PathInfo thumbnailPath2 = this.thumbnailPath;
        List<string> list5 = _lst;
        int index8 = num8;
        int num9 = index8 + 1;
        string str5 = list5.SafeGet<string>(index8);
        thumbnailPath2.file = str5;
        Manager.Housing.PathInfo thumbnailPath3 = this.thumbnailPath;
        List<string> list6 = _lst;
        int index9 = num9;
        int num10 = index9 + 1;
        string str6 = list6.SafeGet<string>(index9);
        thumbnailPath3.manifest = str6;
        List<string> list7 = _lst;
        int index10 = num10;
        int num11 = index10 + 1;
        double f1 = (double) this.TryParseF(list7.SafeGet<string>(index10));
        List<string> list8 = _lst;
        int index11 = num11;
        int num12 = index11 + 1;
        double f2 = (double) this.TryParseF(list8.SafeGet<string>(index11));
        List<string> list9 = _lst;
        int index12 = num12;
        int num13 = index12 + 1;
        double f3 = (double) this.TryParseF(list9.SafeGet<string>(index12));
        this.size = new Vector3((float) f1, (float) f2, (float) f3);
        List<Manager.Housing.RequiredMaterial> requiredMaterialList = new List<Manager.Housing.RequiredMaterial>();
        for (int index13 = 0; index13 < 8; ++index13)
        {
          string self = _lst.SafeGet<string>(num13++);
          if (!self.IsNullOrEmpty())
            requiredMaterialList.Add(new Manager.Housing.RequiredMaterial(self.Split('/')));
        }
        this.requiredMaterials = requiredMaterialList.ToArray();
        List<string> list10 = _lst;
        int index14 = num13;
        int num14 = index14 + 1;
        bool.TryParse(list10.SafeGet<string>(index14), out this.useOption);
        List<string> list11 = _lst;
        int index15 = num14;
        int num15 = index15 + 1;
        this.isAccess = this.TryParseIntToBool(list11.SafeGet<string>(index15));
        List<string> list12 = _lst;
        int index16 = num15;
        int num16 = index16 + 1;
        this.isAction = this.TryParseIntToBool(list12.SafeGet<string>(index16));
        List<string> list13 = _lst;
        int index17 = num16;
        int num17 = index17 + 1;
        this.isHPoint = this.TryParseIntToBool(list13.SafeGet<string>(index17));
        List<string> list14 = _lst;
        int index18 = num17;
        int num18 = index18 + 1;
        if (int.TryParse(list14.SafeGet<string>(index18), out this.limitNum))
          return;
        this.limitNum = -1;
      }

      public int Category
      {
        get
        {
          return 1 << this.category;
        }
      }

      private float TryParseF(string _str)
      {
        float result = 0.0f;
        float.TryParse(_str, out result);
        return result;
      }

      private bool TryParseIntToBool(string _str)
      {
        int result = 0;
        return int.TryParse(_str, out result) && result == 1;
      }
    }

    public class CategoryInfo
    {
      public string name = string.Empty;
      public Manager.Housing.PathInfo thumbnailPath = new Manager.Housing.PathInfo();

      public CategoryInfo(List<string> _lst)
      {
        int num1 = 1;
        List<string> list1 = _lst;
        int index1 = num1;
        int num2 = index1 + 1;
        this.name = list1.SafeGet<string>(index1);
        Manager.Housing.PathInfo thumbnailPath1 = this.thumbnailPath;
        List<string> list2 = _lst;
        int index2 = num2;
        int num3 = index2 + 1;
        string str1 = list2.SafeGet<string>(index2);
        thumbnailPath1.bundle = str1;
        Manager.Housing.PathInfo thumbnailPath2 = this.thumbnailPath;
        List<string> list3 = _lst;
        int index3 = num3;
        int num4 = index3 + 1;
        string str2 = list3.SafeGet<string>(index3);
        thumbnailPath2.file = str2;
        Manager.Housing.PathInfo thumbnailPath3 = this.thumbnailPath;
        List<string> list4 = _lst;
        int index4 = num4;
        int num5 = index4 + 1;
        string str3 = list4.SafeGet<string>(index4);
        thumbnailPath3.manifest = str3;
      }

      public Texture2D Thumbnail
      {
        get
        {
          return CommonLib.LoadAsset<Texture2D>(this.thumbnailPath.bundle, this.thumbnailPath.file, false, this.thumbnailPath.manifest);
        }
      }
    }

    public class AreaSizeInfo
    {
      public Vector3Int limitSize = new Vector3Int(100, 80, 100);
      public HashSet<int> compatibility = new HashSet<int>();
      public int no;

      public AreaSizeInfo(List<string> _lst)
      {
        int num1 = 0;
        List<string> list1 = _lst;
        int index1 = num1;
        int num2 = index1 + 1;
        this.no = int.Parse(list1.SafeGet<string>(index1));
        ref Vector3Int local = ref this.limitSize;
        List<string> list2 = _lst;
        int index2 = num2;
        int num3 = index2 + 1;
        int num4 = int.Parse(list2.SafeGet<string>(index2));
        List<string> list3 = _lst;
        int index3 = num3;
        int num5 = index3 + 1;
        int num6 = int.Parse(list3.SafeGet<string>(index3));
        List<string> list4 = _lst;
        int index4 = num5;
        int num7 = index4 + 1;
        int num8 = int.Parse(list4.SafeGet<string>(index4));
        ((Vector3Int) ref local).Set(num4, num6, num8);
        List<string> list5 = _lst;
        int index5 = num7;
        int num9 = index5 + 1;
        string[] strArray = list5.SafeGet<string>(index5).Split('/');
        if (!((IList<string>) strArray).IsNullOrEmpty<string>())
        {
          foreach (string s in strArray)
          {
            int result = 0;
            if (int.TryParse(s, out result))
              this.compatibility.Add(result);
          }
        }
        this.compatibility.Add(this.no);
      }
    }

    public class AreaInfo
    {
      public Manager.Housing.PathInfo presetPath = new Manager.Housing.PathInfo();
      public int no;
      public int size;

      public AreaInfo(List<string> _lst)
      {
        int num1 = 0;
        List<string> list1 = _lst;
        int index1 = num1;
        int num2 = index1 + 1;
        this.no = int.Parse(list1.SafeGet<string>(index1));
        List<string> list2 = _lst;
        int index2 = num2;
        int num3 = index2 + 1;
        this.size = int.Parse(list2.SafeGet<string>(index2));
        Manager.Housing.PathInfo presetPath1 = this.presetPath;
        List<string> list3 = _lst;
        int index3 = num3;
        int num4 = index3 + 1;
        string str1 = list3.SafeGet<string>(index3);
        presetPath1.bundle = str1;
        Manager.Housing.PathInfo presetPath2 = this.presetPath;
        List<string> list4 = _lst;
        int index4 = num4;
        int num5 = index4 + 1;
        string str2 = list4.SafeGet<string>(index4);
        presetPath2.file = str2;
        Manager.Housing.PathInfo presetPath3 = this.presetPath;
        List<string> list5 = _lst;
        int index5 = num5;
        int num6 = index5 + 1;
        string str3 = list5.SafeGet<string>(index5);
        presetPath3.manifest = str3;
      }
    }

    private class WaitTime
    {
      private const float intervalTime = 0.03f;
      private float nextFrameTime;

      public WaitTime()
      {
        this.Next();
      }

      public bool isOver
      {
        get
        {
          return (double) Time.get_realtimeSinceStartup() >= (double) this.nextFrameTime;
        }
      }

      public void Next()
      {
        this.nextFrameTime = Time.get_realtimeSinceStartup() + 0.03f;
      }
    }

    private class FileListInfo
    {
      public Dictionary<string, string[]> dicFile;

      public FileListInfo(List<string> _list)
      {
        this.dicFile = _list.ToDictionary<string, string, string[]>((Func<string, string>) (s => s), (Func<string, string[]>) (s => AssetBundleCheck.GetAllAssetName(s, false, (string) null, true)));
      }

      public bool Check(string _path, string _file)
      {
        string[] strArray = (string[]) null;
        if (!AssetBundleCheck.IsSimulation)
          _file = _file.ToLower();
        return this.dicFile.TryGetValue(_path, out strArray) && ((IEnumerable<string>) strArray).Contains<string>(_file);
      }

      public string[] FindRegex(string _path, string _regex)
      {
        string[] strArray = (string[]) null;
        if (!AssetBundleCheck.IsSimulation)
          _regex = _regex.ToLower();
        return this.dicFile.TryGetValue(_path, out strArray) ? ((IEnumerable<string>) strArray).Where<string>((Func<string, bool>) (s => Regex.Match(s, _regex, RegexOptions.IgnoreCase).Success)).ToArray<string>() : (string[]) null;
      }
    }
  }
}
