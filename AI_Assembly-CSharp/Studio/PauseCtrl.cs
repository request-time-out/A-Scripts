// Decompiled with JetBrains decompiler
// Type: Studio.PauseCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class PauseCtrl
  {
    public const string savePath = "studio/pose";
    public const string saveExtension = ".dat";
    public const string saveIdentifyingCode = "【pose】";
    public const int saveVersion = 101;

    public static void Save(OCIChar _ociChar, string _name)
    {
      string path = UserData.Create("studio/pose") + Utility.GetCurrentTime() + ".dat";
      PauseCtrl.FileInfo fileInfo = new PauseCtrl.FileInfo(_ociChar);
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter _writer = new BinaryWriter((Stream) fileStream))
        {
          _writer.Write("【pose】");
          _writer.Write(101);
          _writer.Write(_ociChar.oiCharInfo.sex);
          _writer.Write(_name);
          fileInfo.Save(_writer);
        }
      }
    }

    public static bool Load(OCIChar _ociChar, string _path)
    {
      PauseCtrl.FileInfo fileInfo = new PauseCtrl.FileInfo((OCIChar) null);
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BinaryReader _reader = new BinaryReader((Stream) fileStream))
        {
          if (string.Compare(_reader.ReadString(), "【pose】") != 0)
            return false;
          int _ver = _reader.ReadInt32();
          _reader.ReadInt32();
          _reader.ReadString();
          fileInfo.Load(_reader, _ver);
        }
      }
      fileInfo.Apply(_ociChar);
      return true;
    }

    public static bool CheckIdentifyingCode(string _path, int _sex)
    {
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          if (string.Compare(binaryReader.ReadString(), "【pose】") != 0)
            return false;
          binaryReader.ReadInt32();
          if (_sex != binaryReader.ReadInt32())
            return false;
        }
      }
      return true;
    }

    public static string LoadName(string _path)
    {
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          if (string.Compare(binaryReader.ReadString(), "【pose】") != 0)
            return string.Empty;
          binaryReader.ReadInt32();
          binaryReader.ReadInt32();
          return binaryReader.ReadString();
        }
      }
    }

    public class FileInfo
    {
      public int group = -1;
      public int category = -1;
      public int no = -1;
      public bool[] activeIK = new bool[5]
      {
        true,
        true,
        true,
        true,
        true
      };
      public Dictionary<int, ChangeAmount> dicIK = new Dictionary<int, ChangeAmount>();
      public bool[] activeFK = new bool[7]
      {
        false,
        true,
        false,
        true,
        false,
        false,
        false
      };
      public Dictionary<int, ChangeAmount> dicFK = new Dictionary<int, ChangeAmount>();
      public bool[] expression = new bool[4]
      {
        true,
        true,
        true,
        true
      };
      public float normalizedTime;
      public bool enableIK;
      public bool enableFK;

      public FileInfo(OCIChar _char = null)
      {
        if (_char == null)
          return;
        this.group = _char.oiCharInfo.animeInfo.group;
        this.category = _char.oiCharInfo.animeInfo.category;
        this.no = _char.oiCharInfo.animeInfo.no;
        this.normalizedTime = _char.charAnimeCtrl.normalizedTime;
        this.enableIK = _char.oiCharInfo.enableIK;
        for (int index = 0; index < this.activeIK.Length; ++index)
          this.activeIK[index] = _char.oiCharInfo.activeIK[index];
        foreach (KeyValuePair<int, OIIKTargetInfo> keyValuePair in _char.oiCharInfo.ikTarget)
          this.dicIK.Add(keyValuePair.Key, keyValuePair.Value.changeAmount.Clone());
        this.enableFK = _char.oiCharInfo.enableFK;
        for (int index = 0; index < this.activeFK.Length; ++index)
          this.activeFK[index] = _char.oiCharInfo.activeFK[index];
        OIBoneInfo.BoneGroup fkGroup = OIBoneInfo.BoneGroup.Body | OIBoneInfo.BoneGroup.RightLeg | OIBoneInfo.BoneGroup.LeftLeg | OIBoneInfo.BoneGroup.RightArm | OIBoneInfo.BoneGroup.LeftArm | OIBoneInfo.BoneGroup.RightHand | OIBoneInfo.BoneGroup.LeftHand | OIBoneInfo.BoneGroup.Neck | OIBoneInfo.BoneGroup.Breast;
        foreach (KeyValuePair<int, OIBoneInfo> keyValuePair in _char.oiCharInfo.bones.Where<KeyValuePair<int, OIBoneInfo>>((Func<KeyValuePair<int, OIBoneInfo>, bool>) (b => (fkGroup & b.Value.group) != (OIBoneInfo.BoneGroup) 0)))
          this.dicFK.Add(keyValuePair.Key, keyValuePair.Value.changeAmount.Clone());
        for (int index = 0; index < this.expression.Length; ++index)
          this.expression[index] = _char.oiCharInfo.expression[index];
      }

      public void Apply(OCIChar _char)
      {
        _char.LoadAnime(this.group, this.category, this.no, this.normalizedTime);
        for (int index = 0; index < this.activeIK.Length; ++index)
          _char.ActiveIK((OIBoneInfo.BoneGroup) (1 << index), this.activeIK[index], false);
        _char.ActiveKinematicMode(OICharInfo.KinematicMode.IK, this.enableIK, true);
        foreach (KeyValuePair<int, ChangeAmount> keyValuePair in this.dicIK)
          _char.oiCharInfo.ikTarget[keyValuePair.Key].changeAmount.Copy(keyValuePair.Value, true, true, true);
        for (int index = 0; index < this.activeFK.Length; ++index)
          _char.ActiveFK(FKCtrl.parts[index], this.activeFK[index], false);
        _char.ActiveKinematicMode(OICharInfo.KinematicMode.FK, this.enableFK, true);
        foreach (KeyValuePair<int, ChangeAmount> keyValuePair in this.dicFK)
          _char.oiCharInfo.bones[keyValuePair.Key].changeAmount.Copy(keyValuePair.Value, true, true, true);
        for (int _category = 0; _category < this.expression.Length; ++_category)
          _char.EnableExpressionCategory(_category, this.expression[_category]);
      }

      public void Save(BinaryWriter _writer)
      {
        _writer.Write(this.group);
        _writer.Write(this.category);
        _writer.Write(this.no);
        _writer.Write(this.normalizedTime);
        _writer.Write(this.enableIK);
        for (int index = 0; index < this.activeIK.Length; ++index)
          _writer.Write(this.activeIK[index]);
        _writer.Write(this.dicIK.Count);
        foreach (KeyValuePair<int, ChangeAmount> keyValuePair in this.dicIK)
        {
          _writer.Write(keyValuePair.Key);
          keyValuePair.Value.Save(_writer);
        }
        _writer.Write(this.enableFK);
        for (int index = 0; index < this.activeFK.Length; ++index)
          _writer.Write(this.activeFK[index]);
        _writer.Write(this.dicFK.Count);
        foreach (KeyValuePair<int, ChangeAmount> keyValuePair in this.dicFK)
        {
          _writer.Write(keyValuePair.Key);
          keyValuePair.Value.Save(_writer);
        }
        for (int index = 0; index < this.expression.Length; ++index)
          _writer.Write(this.expression[index]);
      }

      public void Load(BinaryReader _reader, int _ver)
      {
        this.group = _reader.ReadInt32();
        this.category = _reader.ReadInt32();
        this.no = _reader.ReadInt32();
        if (_ver >= 101)
          this.normalizedTime = _reader.ReadSingle();
        this.enableIK = _reader.ReadBoolean();
        for (int index = 0; index < this.activeIK.Length; ++index)
          this.activeIK[index] = _reader.ReadBoolean();
        int num1 = _reader.ReadInt32();
        for (int index = 0; index < num1; ++index)
        {
          int key = _reader.ReadInt32();
          ChangeAmount changeAmount = new ChangeAmount();
          changeAmount.Load(_reader);
          try
          {
            this.dicIK.Add(key, changeAmount);
          }
          catch (Exception ex)
          {
            Debug.Log((object) key);
          }
        }
        this.enableFK = _reader.ReadBoolean();
        for (int index = 0; index < this.activeFK.Length; ++index)
          this.activeFK[index] = _reader.ReadBoolean();
        int num2 = _reader.ReadInt32();
        for (int index = 0; index < num2; ++index)
        {
          int key = _reader.ReadInt32();
          ChangeAmount changeAmount = new ChangeAmount();
          changeAmount.Load(_reader);
          this.dicFK.Add(key, changeAmount);
        }
        for (int index = 0; index < this.expression.Length; ++index)
          this.expression[index] = _reader.ReadBoolean();
      }
    }
  }
}
