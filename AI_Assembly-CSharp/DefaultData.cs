// Decompiled with JetBrains decompiler
// Type: DefaultData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

public static class DefaultData
{
  private static FileData fileDat = new FileData(nameof (DefaultData));

  public static string Create(string name)
  {
    return DefaultData.fileDat.Create(name);
  }

  public static string Path
  {
    get
    {
      return DefaultData.fileDat.Path;
    }
  }
}
