// Decompiled with JetBrains decompiler
// Type: GameLoadCharaFileSystem.GameCharaFileInfoAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.SaveData;
using AIProject.Scene;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameLoadCharaFileSystem
{
  public class GameCharaFileInfoAssist
  {
    private static void AddList(
      List<GameCharaFileInfo> _list,
      string path,
      byte sex,
      bool useMyData,
      bool useDownload,
      bool preset,
      bool _isFindSaveData,
      bool firstEmpty,
      ref int idx)
    {
      string[] searchPattern = new string[1]{ "*.png" };
      HashSet<string> stringSet = new HashSet<string>();
      if (_isFindSaveData)
      {
        WorldData autoData = Singleton<Game>.Instance.Data.AutoData;
        if (autoData != null)
        {
          stringSet.Add(autoData.PlayerData.CharaFileName);
          foreach (KeyValuePair<int, AgentData> keyValuePair in autoData.AgentTable)
            stringSet.Add(keyValuePair.Value.CharaFileName);
        }
        foreach (KeyValuePair<int, WorldData> world in Singleton<Game>.Instance.Data.WorldList)
        {
          if (Singleton<Game>.Instance.WorldData == null || !Singleton<MapScene>.IsInstance() || world.Value.WorldID != Singleton<Game>.Instance.WorldData.WorldID)
          {
            stringSet.Add(world.Value.PlayerData.CharaFileName);
            foreach (KeyValuePair<int, AgentData> keyValuePair in world.Value.AgentTable)
              stringSet.Add(keyValuePair.Value.CharaFileName);
          }
        }
        if (Singleton<Game>.Instance.WorldData != null)
        {
          stringSet.Add(Singleton<Game>.Instance.WorldData.PlayerData.CharaFileName);
          foreach (KeyValuePair<int, AgentData> keyValuePair in Singleton<Game>.Instance.WorldData.AgentTable)
            stringSet.Add(keyValuePair.Value.CharaFileName);
        }
      }
      string userUuid = Singleton<GameSystem>.Instance.UserUUID;
      CategoryKind categoryKind1 = sex != (byte) 0 ? CategoryKind.Female : CategoryKind.Male;
      if (preset)
        categoryKind1 |= CategoryKind.Preset;
      FolderAssist folderAssist = new FolderAssist();
      folderAssist.CreateFolderInfoEx(path, searchPattern, true);
      int fileCount = folderAssist.GetFileCount();
      if (firstEmpty)
      {
        List<GameCharaFileInfo> gameCharaFileInfoList = _list;
        GameCharaFileInfo gameCharaFileInfo1 = new GameCharaFileInfo();
        GameCharaFileInfo gameCharaFileInfo2 = gameCharaFileInfo1;
        int num1;
        idx = (num1 = idx) + 1;
        int num2 = num1;
        gameCharaFileInfo2.index = num2;
        gameCharaFileInfo1.name = (string) null;
        gameCharaFileInfo1.personality = "不明";
        gameCharaFileInfo1.voice = 0;
        gameCharaFileInfo1.hair = 0;
        gameCharaFileInfo1.birthMonth = 0;
        gameCharaFileInfo1.birthDay = 0;
        gameCharaFileInfo1.strBirthDay = string.Empty;
        gameCharaFileInfo1.sex = 1;
        gameCharaFileInfo1.FullPath = (string) null;
        gameCharaFileInfo1.FileName = (string) null;
        gameCharaFileInfo1.time = DateTime.MinValue;
        gameCharaFileInfo1.gameRegistration = false;
        gameCharaFileInfo1.flavorState = (Dictionary<int, int>) null;
        gameCharaFileInfo1.phase = 0;
        gameCharaFileInfo1.normalSkill = (Dictionary<int, int>) null;
        gameCharaFileInfo1.hSkill = (Dictionary<int, int>) null;
        gameCharaFileInfo1.favoritePlace = 0;
        gameCharaFileInfo1.futanari = false;
        gameCharaFileInfo1.lifeStyle = -1;
        gameCharaFileInfo1.cateKind = categoryKind1;
        gameCharaFileInfo1.data_uuid = string.Empty;
        gameCharaFileInfo1.isInSaveData = false;
        GameCharaFileInfo gameCharaFileInfo3 = gameCharaFileInfo1;
        gameCharaFileInfoList.Add(gameCharaFileInfo3);
      }
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
          CategoryKind categoryKind2 = (CategoryKind) 0;
          if (!preset)
          {
            if (userUuid == chaFileControl.userID || chaFileControl.userID == "illusion-2019-1025-xxxx-aisyoujyocha")
            {
              if (useMyData)
                categoryKind2 = CategoryKind.MyData;
              else
                continue;
            }
            else if (useDownload)
              categoryKind2 = CategoryKind.Download;
            else
              continue;
          }
          string empty = string.Empty;
          VoiceInfo.Param obj;
          string str = sex == (byte) 0 ? string.Empty : (Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(chaFileControl.parameter.personality, out obj) ? obj.Personality : "不明");
          List<GameCharaFileInfo> gameCharaFileInfoList = _list;
          GameCharaFileInfo gameCharaFileInfo1 = new GameCharaFileInfo();
          GameCharaFileInfo gameCharaFileInfo2 = gameCharaFileInfo1;
          int num1;
          idx = (num1 = idx) + 1;
          int num2 = num1;
          gameCharaFileInfo2.index = num2;
          gameCharaFileInfo1.name = chaFileControl.parameter.fullname;
          gameCharaFileInfo1.personality = str;
          gameCharaFileInfo1.voice = chaFileControl.parameter.personality;
          gameCharaFileInfo1.hair = chaFileControl.custom.hair.kind;
          gameCharaFileInfo1.birthMonth = (int) chaFileControl.parameter.birthMonth;
          gameCharaFileInfo1.birthDay = (int) chaFileControl.parameter.birthDay;
          gameCharaFileInfo1.strBirthDay = chaFileControl.parameter.strBirthDay;
          gameCharaFileInfo1.sex = (int) chaFileControl.parameter.sex;
          gameCharaFileInfo1.FullPath = folderAssist.lstFile[index].FullPath;
          gameCharaFileInfo1.FileName = folderAssist.lstFile[index].FileName;
          gameCharaFileInfo1.time = folderAssist.lstFile[index].time;
          gameCharaFileInfo1.gameRegistration = chaFileControl.gameinfo.gameRegistration;
          gameCharaFileInfo1.flavorState = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.flavorState);
          gameCharaFileInfo1.phase = chaFileControl.gameinfo.phase;
          gameCharaFileInfo1.normalSkill = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.normalSkill);
          gameCharaFileInfo1.hSkill = new Dictionary<int, int>((IDictionary<int, int>) chaFileControl.gameinfo.hSkill);
          gameCharaFileInfo1.favoritePlace = chaFileControl.gameinfo.favoritePlace;
          gameCharaFileInfo1.futanari = chaFileControl.parameter.futanari;
          gameCharaFileInfo1.lifeStyle = chaFileControl.gameinfo.lifestyle;
          gameCharaFileInfo1.cateKind = categoryKind1 | categoryKind2;
          gameCharaFileInfo1.data_uuid = chaFileControl.dataID;
          gameCharaFileInfo1.isInSaveData = stringSet.Contains(Path.GetFileNameWithoutExtension(chaFileControl.charaFileName));
          GameCharaFileInfo gameCharaFileInfo3 = gameCharaFileInfo1;
          gameCharaFileInfoList.Add(gameCharaFileInfo3);
        }
      }
    }

    public static List<GameCharaFileInfo> CreateCharaFileInfoList(
      bool useMale,
      bool useFemale,
      bool useMyData = true,
      bool useDownload = true,
      bool firstEmpty = false)
    {
      List<GameCharaFileInfo> _list = new List<GameCharaFileInfo>();
      int idx = 0;
      if (useMale)
      {
        string path = UserData.Path + "chara/male/";
        GameCharaFileInfoAssist.AddList(_list, path, (byte) 0, useMyData, useDownload, false, false, firstEmpty, ref idx);
      }
      if (useFemale)
      {
        string path = UserData.Path + "chara/female/";
        GameCharaFileInfoAssist.AddList(_list, path, (byte) 1, useMyData, useDownload, false, true, firstEmpty, ref idx);
      }
      return _list;
    }
  }
}
