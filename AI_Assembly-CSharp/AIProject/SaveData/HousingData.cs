// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.HousingData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Housing;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class HousingData : IDiffComparer
  {
    public HousingData()
    {
      this.UpdateDiff();
    }

    public HousingData(HousingData _src)
    {
      this.Unlock = _src.Unlock.ToDictionary<KeyValuePair<int, Dictionary<int, bool>>, int, Dictionary<int, bool>>((Func<KeyValuePair<int, Dictionary<int, bool>>, int>) (x => x.Key), (Func<KeyValuePair<int, Dictionary<int, bool>>, Dictionary<int, bool>>) (x =>
      {
        Dictionary<int, bool> source = x.Value;
        return source == null ? (Dictionary<int, bool>) null : source.ToDictionary<KeyValuePair<int, bool>, int, bool>((Func<KeyValuePair<int, bool>, int>) (y => y.Key), (Func<KeyValuePair<int, bool>, bool>) (y => y.Value));
      }));
      this.CraftInfos = _src.CraftInfos.ToDictionary<KeyValuePair<int, CraftInfo>, int, CraftInfo>((Func<KeyValuePair<int, CraftInfo>, int>) (x => x.Key), (Func<KeyValuePair<int, CraftInfo>, CraftInfo>) (x => x.Value != null ? new CraftInfo(x.Value, true) : (CraftInfo) null));
    }

    [Key(0)]
    public Dictionary<int, Dictionary<int, bool>> Unlock { get; set; } = new Dictionary<int, Dictionary<int, bool>>();

    [Key(1)]
    public Dictionary<int, CraftInfo> CraftInfos { get; set; } = new Dictionary<int, CraftInfo>();

    public void UpdateDiff()
    {
      if (Singleton<Manager.Housing>.IsInstance())
      {
        foreach (int num in new HashSet<int>(Singleton<Manager.Housing>.Instance.dicLoadInfo.Select<KeyValuePair<int, Manager.Housing.LoadInfo>, int>((Func<KeyValuePair<int, Manager.Housing.LoadInfo>, int>) (v => v.Value.category))))
        {
          int c = num;
          Dictionary<int, bool> dictionary = (Dictionary<int, bool>) null;
          if (!this.Unlock.TryGetValue(c, out dictionary))
          {
            dictionary = new Dictionary<int, bool>();
            this.Unlock.Add(c, dictionary);
          }
          foreach (KeyValuePair<int, Manager.Housing.LoadInfo> keyValuePair in Singleton<Manager.Housing>.Instance.dicLoadInfo.Where<KeyValuePair<int, Manager.Housing.LoadInfo>>((Func<KeyValuePair<int, Manager.Housing.LoadInfo>, bool>) (v => v.Value.category == c)))
          {
            if (!dictionary.ContainsKey(keyValuePair.Key))
              dictionary.Add(keyValuePair.Key, keyValuePair.Value.requiredMaterials.IsNullOrEmpty<Manager.Housing.RequiredMaterial>());
          }
        }
      }
      CraftInfo craftInfo1 = (CraftInfo) null;
      if (!Singleton<Manager.Housing>.IsInstance())
      {
        if (!this.CraftInfos.TryGetValue(0, out craftInfo1))
          this.CraftInfos.Add(0, new CraftInfo(new Vector3(100f, 80f, 100f), 0));
        if (!this.CraftInfos.TryGetValue(1, out craftInfo1))
          this.CraftInfos.Add(1, new CraftInfo(new Vector3(100f, 80f, 100f), 1));
        if (!this.CraftInfos.TryGetValue(2, out craftInfo1))
          this.CraftInfos.Add(2, new CraftInfo(new Vector3(200f, 100f, 200f), 2));
        if (this.CraftInfos.TryGetValue(3, out craftInfo1))
          return;
        this.CraftInfos.Add(3, new CraftInfo(new Vector3(500f, 150f, 500f), 3));
      }
      else
      {
        foreach (KeyValuePair<int, Manager.Housing.AreaInfo> keyValuePair in Singleton<Manager.Housing>.Instance.dicAreaInfo)
        {
          Manager.Housing.AreaSizeInfo areaSizeInfo = (Manager.Housing.AreaSizeInfo) null;
          if (!this.CraftInfos.ContainsKey(keyValuePair.Key))
          {
            CraftInfo craftInfo2 = new CraftInfo(Vector3Int.op_Implicit(!Singleton<Manager.Housing>.Instance.dicAreaSizeInfo.TryGetValue(keyValuePair.Value.size, out areaSizeInfo) ? new Vector3Int(100, 80, 100) : areaSizeInfo.limitSize), keyValuePair.Value.no);
            this.CraftInfos.Add(keyValuePair.Key, craftInfo2);
          }
        }
      }
    }

    public void CopyInstances(HousingData _src)
    {
      if (!Singleton<Manager.Housing>.IsInstance())
        return;
      foreach (KeyValuePair<int, CraftInfo> craftInfo1 in _src.CraftInfos)
      {
        CraftInfo craftInfo2 = this.CraftInfos[craftInfo1.Key];
        for (int index = 0; index < craftInfo1.Value.ObjectInfos.Count; ++index)
        {
          IObjectInfo objectInfo1 = craftInfo1.Value.ObjectInfos[index];
          IObjectInfo objectInfo2 = craftInfo2.ObjectInfos[index];
          ObjectCtrl _srcCtrl;
          if (craftInfo1.Value.ObjectCtrls.TryGetValue(objectInfo1, out _srcCtrl))
            craftInfo2.ObjectCtrls[objectInfo2] = this.CopyObjectCtrl(objectInfo2, objectInfo1, _srcCtrl, (ObjectCtrl) null, craftInfo2, craftInfo1.Value);
        }
        craftInfo2.ObjRoot = craftInfo1.Value.ObjRoot;
      }
    }

    private ObjectCtrl CopyObjectCtrl(
      IObjectInfo _dstObjectInfo,
      IObjectInfo _srcObjectInfo,
      ObjectCtrl _srcCtrl,
      ObjectCtrl _parent,
      CraftInfo _craftInfo,
      CraftInfo _srcCraftInfo)
    {
      if (_srcCtrl == null)
        return (ObjectCtrl) null;
      ObjectCtrl objectCtrl = (ObjectCtrl) null;
      switch (_dstObjectInfo.Kind)
      {
        case 0:
          objectCtrl = this.CopyObject(_dstObjectInfo as OIItem, _srcObjectInfo as OIItem, _srcCtrl.GameObject, _parent, _craftInfo, _srcCraftInfo);
          break;
        case 1:
          objectCtrl = this.CopyFolder(_dstObjectInfo as OIFolder, _srcObjectInfo as OIFolder, _srcCtrl as OCFolder, _parent, _craftInfo, _srcCraftInfo);
          break;
      }
      return objectCtrl;
    }

    private ObjectCtrl CopyFolder(
      OIFolder _oiFolder,
      OIFolder _srcOIFolder,
      OCFolder _srcOCFolder,
      ObjectCtrl _parent,
      CraftInfo craftInfo,
      CraftInfo _srcCraftInfo)
    {
      OCFolder ocFolder = new OCFolder(_oiFolder, _srcOCFolder.GameObject, craftInfo);
      if (_parent != null)
        ocFolder.OnAttach(_parent, -1);
      List<IObjectInfo> objectInfoList = new List<IObjectInfo>();
      for (int index = 0; index < _oiFolder.Child.Count; ++index)
      {
        IObjectInfo _dstObjectInfo = _oiFolder.Child[index];
        IObjectInfo objectInfo = _srcOIFolder.Child[index];
        ObjectCtrl _srcCtrl;
        if (_srcOCFolder.Child.TryGetValue(objectInfo, out _srcCtrl))
          ocFolder.Child[_dstObjectInfo] = this.CopyObjectCtrl(_dstObjectInfo, objectInfo, _srcCtrl, (ObjectCtrl) ocFolder, craftInfo, _srcCraftInfo);
      }
      return (ObjectCtrl) ocFolder;
    }

    private ObjectCtrl CopyObject(
      OIItem _oiItem,
      OIItem _srcOIFolder,
      GameObject _gameObject,
      ObjectCtrl _parent,
      CraftInfo craftInfo,
      CraftInfo _srcCraftInfo)
    {
      Manager.Housing.LoadInfo _loadInfo;
      if (!Singleton<Manager.Housing>.Instance.dicLoadInfo.TryGetValue(_oiItem.ID, out _loadInfo))
        return (ObjectCtrl) null;
      return Object.op_Equality((Object) _gameObject, (Object) null) ? (ObjectCtrl) null : (ObjectCtrl) new OCItem(_oiItem, _gameObject, craftInfo, _loadInfo);
    }
  }
}
