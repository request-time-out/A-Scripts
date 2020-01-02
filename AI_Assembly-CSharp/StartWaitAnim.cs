// Decompiled with JetBrains decompiler
// Type: StartWaitAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

public class StartWaitAnim
{
  public StartWaitAnim.Info[] Player = new StartWaitAnim.Info[2];
  public StartWaitAnim.Info[] Agent = new StartWaitAnim.Info[2];
  public int ID;
  public string CameraFile;
  public int VisibleMode;

  public struct Info
  {
    public string abName;
    public string assetName;
    public string State;
  }
}
