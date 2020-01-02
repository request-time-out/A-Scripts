// Decompiled with JetBrains decompiler
// Type: Housing.CraftInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Animal;
using Manager;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Housing
{
  [MessagePackObject(false)]
  public class CraftInfo
  {
    [IgnoreMember]
    private readonly Version _version = new Version(0, 0, 2);

    public CraftInfo()
    {
    }

    public CraftInfo(Vector3 _size, int _area)
    {
      this.LimitSize = _size;
      this.AreaNo = _area;
    }

    public CraftInfo(CraftInfo _src)
    {
      this.Copy(_src);
    }

    public CraftInfo(CraftInfo _src, bool _idCopy)
    {
      this.Copy(_src, _idCopy);
    }

    [Key(0)]
    public List<IObjectInfo> ObjectInfos { get; set; } = new List<IObjectInfo>();

    [Key(1)]
    public Vector3 LimitSize { get; set; }

    [Key(2)]
    public int AreaNo { get; set; }

    [IgnoreMember]
    public Dictionary<IObjectInfo, ObjectCtrl> ObjectCtrls { get; private set; } = new Dictionary<IObjectInfo, ObjectCtrl>();

    [IgnoreMember]
    public GameObject ObjRoot { get; set; }

    public void Copy(CraftInfo _src)
    {
      if (_src == null)
        return;
      this.ObjectInfos.Clear();
      foreach (IObjectInfo objectInfo in _src.ObjectInfos)
      {
        switch (objectInfo.Kind)
        {
          case 0:
            this.ObjectInfos.Add((IObjectInfo) new OIItem(objectInfo as OIItem));
            continue;
          case 1:
            this.ObjectInfos.Add((IObjectInfo) new OIFolder(objectInfo as OIFolder));
            continue;
          default:
            continue;
        }
      }
      this.LimitSize = _src.LimitSize;
      this.AreaNo = _src.AreaNo;
    }

    public void Copy(CraftInfo _src, bool _idCopy)
    {
      if (_src == null)
        return;
      this.ObjectInfos.Clear();
      foreach (IObjectInfo objectInfo in _src.ObjectInfos)
      {
        switch (objectInfo.Kind)
        {
          case 0:
            this.ObjectInfos.Add((IObjectInfo) new OIItem(objectInfo as OIItem, _idCopy));
            continue;
          case 1:
            this.ObjectInfos.Add((IObjectInfo) new OIFolder(objectInfo as OIFolder, _idCopy));
            continue;
          default:
            continue;
        }
      }
      this.LimitSize = _src.LimitSize;
      this.AreaNo = _src.AreaNo;
    }

    public void GetActionPoint(ref List<ActionPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetActionPoint(ref _points);
      }
    }

    public void GetFarmPoint(ref List<FarmPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetFarmPoint(ref _points);
      }
    }

    public void GetHPoint(ref List<HPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetHPoint(ref _points);
      }
    }

    public void GetColInfo(ref List<ItemComponent.ColInfo> _infos)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetColInfo(ref _infos);
      }
    }

    public void GetPetHomePoint(ref List<PetHomePoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetPetHomePoint(ref _points);
      }
    }

    public void GetJukePoint(ref List<JukePoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetJukePoint(ref _points);
      }
    }

    public void GetCraftPoint(ref List<CraftPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetCraftPoint(ref _points);
      }
    }

    public void GetLightSwitchPoint(ref List<LightSwitchPoint> _points)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetLightSwitchPoint(ref _points);
      }
    }

    public int GetUsedNum(int _no)
    {
      int _num = 0;
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetUsedNum(_no, ref _num);
      }
      return _num;
    }

    public void SetOverlapColliders(bool _flag)
    {
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.SetOverlapColliders(_flag);
      }
    }

    [IgnoreMember]
    public bool IsOverlapNow
    {
      get
      {
        return this.ObjectCtrls.Any<KeyValuePair<IObjectInfo, ObjectCtrl>>((Func<KeyValuePair<IObjectInfo, ObjectCtrl>, bool>) (v => v.Value.IsOverlapNow));
      }
    }

    public ObjectCtrl FindOverlapObject(ObjectCtrl _old)
    {
      List<ObjectCtrl> _lst = new List<ObjectCtrl>();
      foreach (KeyValuePair<IObjectInfo, ObjectCtrl> objectCtrl in this.ObjectCtrls)
      {
        if (objectCtrl.Value != null)
          objectCtrl.Value.GetOverlapObject(ref _lst);
      }
      if (_lst.IsNullOrEmpty<ObjectCtrl>())
        return (ObjectCtrl) null;
      int index = _lst.FindIndex((Predicate<ObjectCtrl>) (v => v == _old));
      return _lst.SafeGet<ObjectCtrl>(GlobalMethod.ValLoop(index + 1, _lst.Count));
    }

    public void DeleteObject()
    {
      foreach (ObjectCtrl objectCtrl in this.ObjectCtrls.Values.ToArray<ObjectCtrl>())
        objectCtrl?.OnDelete();
      this.ObjectCtrls.Clear();
    }

    public void Save(string _path, byte[] _pngData)
    {
      using (FileStream fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          binaryWriter.Write(_pngData);
          binaryWriter.Write(100);
          binaryWriter.Write("【AIS_Housing】");
          binaryWriter.Write(this._version.ToString());
          binaryWriter.Write(Singleton<GameSystem>.Instance.UserUUID);
          binaryWriter.Write(YS_Assist.CreateUUID());
          binaryWriter.Write(Singleton<Manager.Housing>.Instance.GetSizeType(this.AreaNo));
          binaryWriter.Write(Singleton<GameSystem>.Instance.languageInt);
          byte[] buffer = MessagePackSerializer.Serialize<CraftInfo>((M0) this);
          binaryWriter.Write(buffer);
        }
      }
    }

    public bool Load(string _path)
    {
      try
      {
        using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          if (fileStream.Length == 0L)
          {
            Debug.LogError((object) "空データ");
            return false;
          }
          using (BinaryReader br = new BinaryReader((Stream) fileStream))
          {
            PngFile.SkipPng(br);
            br.ReadInt32();
            br.ReadString();
            Version version = new Version(br.ReadString());
            br.ReadString();
            br.ReadString();
            br.ReadInt32();
            if (version.CompareTo(new Version("0.0.2")) >= 0)
              br.ReadInt32();
            return this.Load(br.ReadBytes((int) (br.BaseStream.Length - br.BaseStream.Position)));
          }
        }
      }
      catch (Exception ex)
      {
        if (ex is FileNotFoundException)
        {
          Debug.Log((object) ("指定されたデータは存在しません: " + _path));
          return false;
        }
        Debug.LogException(ex);
        return false;
      }
    }

    public bool Load(string _assetBundle, string _asset)
    {
      TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(_assetBundle, _asset, false, string.Empty);
      return !Object.op_Equality((Object) textAsset, (Object) null) && this.Load(textAsset.get_bytes());
    }

    public bool Load(byte[] _bytes)
    {
      try
      {
        if (((IList<byte>) _bytes).IsNullOrEmpty<byte>())
          return false;
        this.Copy((CraftInfo) MessagePackSerializer.Deserialize<CraftInfo>(_bytes));
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
        return false;
      }
      return true;
    }

    public static CraftInfo LoadStatic(string _path)
    {
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        if (fileStream.Length == 0L)
        {
          Debug.LogError((object) "空データ");
          return (CraftInfo) null;
        }
        using (BinaryReader br = new BinaryReader((Stream) fileStream))
        {
          PngFile.SkipPng(br);
          try
          {
            br.ReadInt32();
            br.ReadString();
            Version version = new Version(br.ReadString());
            br.ReadString();
            br.ReadString();
            br.ReadInt32();
            if (version.CompareTo(new Version("0.0.2")) >= 0)
              br.ReadInt32();
            byte[] numArray = br.ReadBytes((int) (br.BaseStream.Length - br.BaseStream.Position));
            if (!((IList<byte>) numArray).IsNullOrEmpty<byte>())
              return (CraftInfo) MessagePackSerializer.Deserialize<CraftInfo>(numArray);
            Debug.LogError((object) "画像データ");
            return (CraftInfo) null;
          }
          catch (Exception ex)
          {
            Debug.LogWarning((object) "画像データ");
            return (CraftInfo) null;
          }
        }
      }
    }

    public static CraftInfo.AboutInfo LoadAbout(string _path)
    {
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          PngFile.SkipPng(binaryReader);
          return new CraftInfo.AboutInfo(binaryReader);
        }
      }
    }

    public class AboutInfo
    {
      public AboutInfo(BinaryReader _reader)
      {
        this.ProductNo = _reader.ReadInt32();
        this.FileMark = _reader.ReadString();
        this.Version = new Version(_reader.ReadString());
        this.UUID = _reader.ReadString();
        this.HUID = _reader.ReadString();
        this.AreaType = _reader.ReadInt32();
        if (this.Version.CompareTo(new Version("0.0.2")) < 0)
          return;
        this.LanguageInt = _reader.ReadInt32();
      }

      public int ProductNo { get; private set; }

      public string FileMark { get; private set; }

      public Version Version { get; private set; }

      public string UUID { get; private set; }

      public string HUID { get; private set; }

      public int AreaType { get; private set; }

      public int LanguageInt { get; private set; }
    }
  }
}
