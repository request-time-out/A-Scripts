// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.WildAnimalData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class WildAnimalData
  {
    public WildAnimalData()
    {
    }

    public WildAnimalData(WildAnimalData data)
    {
      this.Copy(data);
    }

    [Key(0)]
    public float CoolTime { get; set; }

    [Key(1)]
    public bool IsAdded { get; set; }

    public void Copy(WildAnimalData data)
    {
      if (data == null)
        return;
      this.CoolTime = data.CoolTime;
      this.IsAdded = data.IsAdded;
    }
  }
}
