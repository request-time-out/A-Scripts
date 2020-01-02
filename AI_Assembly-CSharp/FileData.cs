// Decompiled with JetBrains decompiler
// Type: FileData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.IO;
using UnityEngine;

public class FileData
{
  private string rootName = string.Empty;

  public FileData(string rootName = "")
  {
    this.rootName = rootName;
  }

  public string Create(string name)
  {
    string path = this.Path + name;
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    return path + (object) '/';
  }

  public string Path
  {
    get
    {
      string str = Application.get_isEditor() || Application.get_platform() == 2 ? Application.get_dataPath() + "/../" : Application.get_persistentDataPath() + "/";
      if (this.rootName != string.Empty)
        str = str + this.rootName + (object) '/';
      return str;
    }
  }
}
