// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomCharaFileInfoAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.SaveData;
using Manager;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CharaCustom
{
  public class CustomCharaFileInfoAssist
  {
    private static void AddList(
      List<CustomCharaFileInfo> _list,
      string path,
      byte sex,
      bool useMyData,
      bool useDownload,
      bool preset,
      bool _isFindSaveData,
      ref int idx)
    {
      string[] searchPattern = new string[1]{ "*.png" };
      List<string> stringList = new List<string>();
      if (_isFindSaveData && Singleton<Game>.Instance.Data != null)
      {
        WorldData autoData = Singleton<Game>.Instance.Data.AutoData;
        if (autoData != null)
        {
          stringList.Add(autoData.PlayerData.CharaFileName);
          foreach (KeyValuePair<int, AgentData> keyValuePair in autoData.AgentTable)
            stringList.Add(keyValuePair.Value.CharaFileName);
        }
        foreach (KeyValuePair<int, WorldData> world in Singleton<Game>.Instance.Data.WorldList)
        {
          stringList.Add(world.Value.PlayerData.CharaFileName);
          foreach (KeyValuePair<int, AgentData> keyValuePair in world.Value.AgentTable)
            stringList.Add(keyValuePair.Value.CharaFileName);
        }
      }
      string userUuid = Singleton<GameSystem>.Instance.UserUUID;
      CharaCategoryKind charaCategoryKind1 = sex != (byte) 0 ? CharaCategoryKind.Female : CharaCategoryKind.Male;
      if (preset)
        charaCategoryKind1 |= CharaCategoryKind.Preset;
      FolderAssist folderAssist = new FolderAssist();
      folderAssist.CreateFolderInfoEx(path, searchPattern, true);
      int fileCount = folderAssist.GetFileCount();
      for (int index = 0; index < fileCount; ++index)
      {
        ChaFileControl chaFileControl = new ChaFileControl();
        if (!chaFileControl.LoadCharaFile(folderAssist.lstFile[index].FullPath, byte.MaxValue, false, true))
          Debug.LogFormat("キャラファイル読込みエラー：Code {0}", new object[1]
          {
            (object) chaFileControl.GetLastErrorCode()
          });
        else if ((int) chaFileControl.parameter.sex == (int) sex)
        {
          CharaCategoryKind charaCategoryKind2 = (CharaCategoryKind) 0;
          if (!preset)
          {
            if (userUuid == chaFileControl.userID)
            {
              if (useMyData)
                charaCategoryKind2 = CharaCategoryKind.MyData;
              else
                continue;
            }
            else if (useDownload)
              charaCategoryKind2 = CharaCategoryKind.Download;
            else
              continue;
          }
          string empty = string.Empty;
          VoiceInfo.Param obj;
          string str = sex == (byte) 0 ? string.Empty : (Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(chaFileControl.parameter.personality, out obj) ? obj.Personality : "不明");
          List<CustomCharaFileInfo> customCharaFileInfoList = _list;
          CustomCharaFileInfo customCharaFileInfo1 = new CustomCharaFileInfo();
          CustomCharaFileInfo customCharaFileInfo2 = customCharaFileInfo1;
          int num1;
          idx = (num1 = idx) + 1;
          int num2 = num1;
          customCharaFileInfo2.index = num2;
          customCharaFileInfo1.name = chaFileControl.parameter.fullname;
          customCharaFileInfo1.personality = str;
          customCharaFileInfo1.type = chaFileControl.parameter.personality;
          customCharaFileInfo1.height = chaFileControl.custom.GetHeightKind();
          customCharaFileInfo1.bustSize = chaFileControl.custom.GetBustSizeKind();
          customCharaFileInfo1.hair = chaFileControl.custom.hair.kind;
          customCharaFileInfo1.birthMonth = (int) chaFileControl.parameter.birthMonth;
          customCharaFileInfo1.birthDay = (int) chaFileControl.parameter.birthDay;
          customCharaFileInfo1.strBirthDay = chaFileControl.parameter.strBirthDay;
          customCharaFileInfo1.lifestyle = chaFileControl.gameinfo.lifestyle;
          customCharaFileInfo1.pheromone = chaFileControl.gameinfo.flavorState[0];
          customCharaFileInfo1.reliability = chaFileControl.gameinfo.flavorState[1];
          customCharaFileInfo1.reason = chaFileControl.gameinfo.flavorState[2];
          customCharaFileInfo1.instinct = chaFileControl.gameinfo.flavorState[3];
          customCharaFileInfo1.dirty = chaFileControl.gameinfo.flavorState[4];
          customCharaFileInfo1.wariness = chaFileControl.gameinfo.flavorState[5];
          customCharaFileInfo1.darkness = chaFileControl.gameinfo.flavorState[6];
          customCharaFileInfo1.sociability = chaFileControl.gameinfo.flavorState[7];
          customCharaFileInfo1.skill_n01 = chaFileControl.gameinfo.normalSkill[0];
          customCharaFileInfo1.skill_n02 = chaFileControl.gameinfo.normalSkill[1];
          customCharaFileInfo1.skill_n03 = chaFileControl.gameinfo.normalSkill[2];
          customCharaFileInfo1.skill_n04 = chaFileControl.gameinfo.normalSkill[3];
          customCharaFileInfo1.skill_n05 = chaFileControl.gameinfo.normalSkill[4];
          customCharaFileInfo1.skill_h01 = chaFileControl.gameinfo.hSkill[0];
          customCharaFileInfo1.skill_h02 = chaFileControl.gameinfo.hSkill[1];
          customCharaFileInfo1.skill_h03 = chaFileControl.gameinfo.hSkill[2];
          customCharaFileInfo1.skill_h04 = chaFileControl.gameinfo.hSkill[3];
          customCharaFileInfo1.skill_h05 = chaFileControl.gameinfo.hSkill[4];
          customCharaFileInfo1.wish_01 = chaFileControl.parameter.wish01;
          customCharaFileInfo1.wish_02 = chaFileControl.parameter.wish02;
          customCharaFileInfo1.wish_03 = chaFileControl.parameter.wish03;
          customCharaFileInfo1.sex = (int) chaFileControl.parameter.sex;
          customCharaFileInfo1.FullPath = folderAssist.lstFile[index].FullPath;
          customCharaFileInfo1.FileName = folderAssist.lstFile[index].FileName;
          customCharaFileInfo1.time = folderAssist.lstFile[index].time;
          customCharaFileInfo1.gameRegistration = chaFileControl.gameinfo.gameRegistration;
          customCharaFileInfo1.flavorState = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.flavorState);
          customCharaFileInfo1.phase = chaFileControl.gameinfo.phase;
          customCharaFileInfo1.normalSkill = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.normalSkill);
          customCharaFileInfo1.hSkill = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.hSkill);
          customCharaFileInfo1.favoritePlace = chaFileControl.gameinfo.favoritePlace;
          customCharaFileInfo1.futanari = chaFileControl.parameter.futanari;
          customCharaFileInfo1.cateKind = charaCategoryKind1 | charaCategoryKind2;
          customCharaFileInfo1.data_uuid = chaFileControl.dataID;
          customCharaFileInfo1.isInSaveData = stringList.Contains(Path.GetFileNameWithoutExtension(chaFileControl.charaFileName));
          CustomCharaFileInfo customCharaFileInfo3 = customCharaFileInfo1;
          customCharaFileInfoList.Add(customCharaFileInfo3);
        }
      }
    }

    public static List<CustomCharaFileInfo> CreateCharaFileInfoList(
      bool useMale,
      bool useFemale,
      bool useMyData = true,
      bool useDownload = true,
      bool usePreset = true,
      bool _isFindSaveData = true)
    {
      List<CustomCharaFileInfo> _list = new List<CustomCharaFileInfo>();
      int idx = 0;
      if (usePreset)
      {
        if (useMale)
        {
          string path = DefaultData.Path + "chara/male/";
          CustomCharaFileInfoAssist.AddList(_list, path, (byte) 0, false, false, true, false, ref idx);
        }
        if (useFemale)
        {
          string path = DefaultData.Path + "chara/female/";
          CustomCharaFileInfoAssist.AddList(_list, path, (byte) 1, false, false, true, false, ref idx);
        }
      }
      if (useMyData || useDownload)
      {
        if (useMale)
        {
          string path = UserData.Path + "chara/male/";
          CustomCharaFileInfoAssist.AddList(_list, path, (byte) 0, useMyData, useDownload, false, _isFindSaveData, ref idx);
        }
        if (useFemale)
        {
          string path = UserData.Path + "chara/female/";
          CustomCharaFileInfoAssist.AddList(_list, path, (byte) 1, useMyData, useDownload, false, _isFindSaveData, ref idx);
        }
      }
      return _list;
    }
  }
}
