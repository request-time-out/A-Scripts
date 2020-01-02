// Decompiled with JetBrains decompiler
// Type: FolderAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;

public class FolderAssist
{
  public List<FolderAssist.FileInfo> lstFile = new List<FolderAssist.FileInfo>();

  public int GetFileCount()
  {
    return this.lstFile.Count;
  }

  public bool CreateFolderInfo(string folder, string searchPattern, bool getFiles = true, bool clear = true)
  {
    if (clear)
      this.lstFile.Clear();
    if (!Directory.Exists(folder))
      return false;
    string[] strArray = !getFiles ? Directory.GetDirectories(folder) : Directory.GetFiles(folder, searchPattern);
    if (strArray.Length == 0)
      return false;
    foreach (string path in strArray)
    {
      FolderAssist.FileInfo fileInfo = new FolderAssist.FileInfo();
      fileInfo.FullPath = path;
      if (getFiles)
        fileInfo.FileName = Path.GetFileNameWithoutExtension(path);
      fileInfo.time = File.GetLastWriteTime(path);
      this.lstFile.Add(fileInfo);
    }
    return true;
  }

  public bool CreateFolderInfoEx(string folder, string[] searchPattern, bool clear = true)
  {
    if (clear)
      this.lstFile.Clear();
    if (!Directory.Exists(folder))
      return false;
    List<string> stringList = new List<string>();
    foreach (string searchPattern1 in searchPattern)
      stringList.AddRange((IEnumerable<string>) Directory.GetFiles(folder, searchPattern1));
    string[] array = stringList.ToArray();
    if (array.Length == 0)
      return false;
    foreach (string path in array)
      this.lstFile.Add(new FolderAssist.FileInfo()
      {
        FullPath = path,
        FileName = Path.GetFileNameWithoutExtension(path),
        time = File.GetLastWriteTime(path)
      });
    return true;
  }

  public int GetIndexFromFileName(string filename)
  {
    int num = 0;
    foreach (FolderAssist.FileInfo fileInfo in this.lstFile)
    {
      if (fileInfo.FileName == filename)
        return num;
      ++num;
    }
    return -1;
  }

  public void SortName(bool ascend = true)
  {
    if (this.lstFile.Count == 0)
      return;
    if (ascend)
      this.lstFile.Sort((Comparison<FolderAssist.FileInfo>) ((a, b) => a.FileName.CompareTo(b.FileName)));
    else
      this.lstFile.Sort((Comparison<FolderAssist.FileInfo>) ((a, b) => b.FileName.CompareTo(a.FileName)));
  }

  public void SortDate(bool ascend = true)
  {
    if (this.lstFile.Count == 0)
      return;
    if (ascend)
      this.lstFile.Sort((Comparison<FolderAssist.FileInfo>) ((a, b) => a.time.CompareTo(b.time)));
    else
      this.lstFile.Sort((Comparison<FolderAssist.FileInfo>) ((a, b) => b.time.CompareTo(a.time)));
  }

  public class FileInfo
  {
    public string FullPath = string.Empty;
    public string FileName = string.Empty;
    public DateTime time;

    public FileInfo()
    {
    }

    public FileInfo(FolderAssist.FileInfo src)
    {
      this.FullPath = src.FullPath;
      this.FileName = src.FileName;
      this.time = src.time;
    }

    public void Copy(FolderAssist.FileInfo src)
    {
      this.FullPath = src.FullPath;
      this.FileName = src.FileName;
      this.time = src.time;
    }
  }
}
