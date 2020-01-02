// Decompiled with JetBrains decompiler
// Type: AIProject.UI.GameCoordinateFileInfoAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AIProject.UI
{
  public class GameCoordinateFileInfoAssist
  {
    private static readonly string[] _searchPatterns = new string[1]
    {
      "*.png"
    };

    private static void AddList(
      List<GameCoordinateFileInfo> list,
      List<string> coordList,
      string path,
      ref int idx)
    {
      List<string> toRelease = ListPool<string>.Get();
      if (!coordList.IsNullOrEmpty<string>())
      {
        foreach (string coord in coordList)
        {
          if (!toRelease.Contains(coord))
            toRelease.Add(coord);
        }
      }
      FolderAssist folderAssist = new FolderAssist();
      folderAssist.CreateFolderInfoEx(path, GameCoordinateFileInfoAssist._searchPatterns, true);
      int fileCount = folderAssist.GetFileCount();
      for (int index = 0; index < fileCount; ++index)
      {
        ChaFileCoordinate chaFileCoordinate = new ChaFileCoordinate();
        FolderAssist.FileInfo fileInfo = folderAssist.lstFile[index];
        if (!chaFileCoordinate.LoadFile(fileInfo.FullPath))
          Debug.Log((object) string.Format("衣装カードファイル読み込みエラー：Code {0}", (object) chaFileCoordinate.GetLastErrorCode()));
        else if (!toRelease.Contains(Path.GetFileNameWithoutExtension(chaFileCoordinate.coordinateFileName)))
        {
          List<GameCoordinateFileInfo> coordinateFileInfoList = list;
          GameCoordinateFileInfo coordinateFileInfo1 = new GameCoordinateFileInfo();
          GameCoordinateFileInfo coordinateFileInfo2 = coordinateFileInfo1;
          int num1;
          idx = (num1 = idx) + 1;
          int num2 = num1;
          coordinateFileInfo2.Index = num2;
          coordinateFileInfo1.FullPath = fileInfo.FullPath;
          coordinateFileInfo1.FileName = fileInfo.FileName;
          coordinateFileInfo1.Time = fileInfo.time;
          GameCoordinateFileInfo coordinateFileInfo3 = coordinateFileInfo1;
          coordinateFileInfoList.Add(coordinateFileInfo3);
        }
      }
      ListPool<string>.Release(toRelease);
    }

    private static void AddFileNameList(
      List<GameCoordinateFileInfo> list,
      List<string> fileNameList,
      string path,
      ref int idx)
    {
      if (fileNameList.IsNullOrEmpty<string>())
        return;
      int count = fileNameList.Count;
      for (int index = 0; index < count; ++index)
      {
        ChaFileCoordinate chaFileCoordinate = new ChaFileCoordinate();
        string fileName = fileNameList[index];
        string path1 = string.Format("{0}{1}.png", (object) path, (object) fileName);
        if (!chaFileCoordinate.LoadFile(path1))
        {
          Debug.Log((object) string.Format("衣装カードファイル読み込みエラー：Code {0}", (object) chaFileCoordinate.GetLastErrorCode()));
        }
        else
        {
          List<GameCoordinateFileInfo> coordinateFileInfoList = list;
          GameCoordinateFileInfo coordinateFileInfo1 = new GameCoordinateFileInfo();
          GameCoordinateFileInfo coordinateFileInfo2 = coordinateFileInfo1;
          int num1;
          idx = (num1 = idx) + 1;
          int num2 = num1;
          coordinateFileInfo2.Index = num2;
          coordinateFileInfo1.FullPath = path1;
          coordinateFileInfo1.FileName = fileName;
          coordinateFileInfo1.Time = File.GetLastWriteTime(path1);
          GameCoordinateFileInfo coordinateFileInfo3 = coordinateFileInfo1;
          coordinateFileInfoList.Add(coordinateFileInfo3);
        }
      }
    }

    public static List<GameCoordinateFileInfo> CreateCoordinateFileInfoList(
      bool useMale,
      bool useFemale,
      List<string> filterList = null)
    {
      List<GameCoordinateFileInfo> list = new List<GameCoordinateFileInfo>();
      int idx = 0;
      if (useMale)
      {
        string path = string.Format("{0}{1}", (object) UserData.Path, (object) "coordinate/male/");
        GameCoordinateFileInfoAssist.AddList(list, filterList, path, ref idx);
      }
      if (useFemale)
      {
        string path = string.Format("{0}{1}", (object) UserData.Path, (object) "coordinate/female/");
        GameCoordinateFileInfoAssist.AddList(list, filterList, path, ref idx);
      }
      return list;
    }

    public static List<GameCoordinateFileInfo> CreateCoordinateFileInfoQueryList(
      List<string> fileNames)
    {
      List<GameCoordinateFileInfo> list = new List<GameCoordinateFileInfo>();
      int idx = 0;
      string path = string.Format("{0}{1}", (object) UserData.Path, (object) "coordinate/female/");
      GameCoordinateFileInfoAssist.AddFileNameList(list, fileNames, path, ref idx);
      return list;
    }
  }
}
