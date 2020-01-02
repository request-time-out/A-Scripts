// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomClothesFileInfoAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System.Collections.Generic;
using UnityEngine;

namespace CharaCustom
{
  public class CustomClothesFileInfoAssist
  {
    private static void AddList(
      List<CustomClothesFileInfo> _list,
      string path,
      byte sex,
      bool preset,
      ref int idx)
    {
      string[] searchPattern = new string[1]{ "*.png" };
      CoordinateCategoryKind coordinateCategoryKind = sex != (byte) 0 ? CoordinateCategoryKind.Female : CoordinateCategoryKind.Male;
      if (preset)
        coordinateCategoryKind |= CoordinateCategoryKind.Preset;
      FolderAssist folderAssist = new FolderAssist();
      folderAssist.CreateFolderInfoEx(path, searchPattern, true);
      int fileCount = folderAssist.GetFileCount();
      for (int index = 0; index < fileCount; ++index)
      {
        ChaFileCoordinate chaFileCoordinate = new ChaFileCoordinate();
        if (!chaFileCoordinate.LoadFile(folderAssist.lstFile[index].FullPath))
        {
          Debug.LogFormat("コーディネートファイル読込みエラー：Code {0}", new object[1]
          {
            (object) chaFileCoordinate.GetLastErrorCode()
          });
        }
        else
        {
          List<CustomClothesFileInfo> customClothesFileInfoList = _list;
          CustomClothesFileInfo customClothesFileInfo1 = new CustomClothesFileInfo();
          CustomClothesFileInfo customClothesFileInfo2 = customClothesFileInfo1;
          int num1;
          idx = (num1 = idx) + 1;
          int num2 = num1;
          customClothesFileInfo2.index = num2;
          customClothesFileInfo1.name = chaFileCoordinate.coordinateName;
          customClothesFileInfo1.FullPath = folderAssist.lstFile[index].FullPath;
          customClothesFileInfo1.FileName = folderAssist.lstFile[index].FileName;
          customClothesFileInfo1.time = folderAssist.lstFile[index].time;
          customClothesFileInfo1.cateKind = coordinateCategoryKind;
          CustomClothesFileInfo customClothesFileInfo3 = customClothesFileInfo1;
          customClothesFileInfoList.Add(customClothesFileInfo3);
        }
      }
    }

    public static List<CustomClothesFileInfo> CreateClothesFileInfoList(
      bool useMale,
      bool useFemale,
      bool useMyData = true,
      bool usePreset = true)
    {
      List<CustomClothesFileInfo> _list = new List<CustomClothesFileInfo>();
      int idx = 0;
      if (usePreset)
      {
        if (useMale)
        {
          string path = DefaultData.Path + "coordinate/male/";
          CustomClothesFileInfoAssist.AddList(_list, path, (byte) 0, true, ref idx);
        }
        if (useFemale)
        {
          string path = DefaultData.Path + "coordinate/female/";
          CustomClothesFileInfoAssist.AddList(_list, path, (byte) 1, true, ref idx);
        }
      }
      if (useMyData)
      {
        if (useMale)
        {
          string path = UserData.Path + "coordinate/male/";
          CustomClothesFileInfoAssist.AddList(_list, path, (byte) 0, false, ref idx);
        }
        if (useFemale)
        {
          string path = UserData.Path + "coordinate/female/";
          CustomClothesFileInfoAssist.AddList(_list, path, (byte) 1, false, ref idx);
        }
      }
      return _list;
    }
  }
}
