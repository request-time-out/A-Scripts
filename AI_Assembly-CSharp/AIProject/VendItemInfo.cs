// Decompiled with JetBrains decompiler
// Type: AIProject.VendItemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject
{
  public class VendItemInfo
  {
    private VendItemInfo(StuffItemInfo info)
    {
      this.info = info;
    }

    public VendItemInfo(StuffItemInfo info, VendData.Param param)
      : this(info)
    {
      this.Rate = param.Rate;
      this.Stocks = param.Stocks;
      this.Percent = param.Percent;
    }

    public VendItemInfo(StuffItemInfo info, VendSpecialData.Param param)
      : this(info)
    {
      this.Rate = param.Rate;
      this.Stocks = new int[1]{ param.Stock };
    }

    private StuffItemInfo info { get; }

    public int CategoryID
    {
      get
      {
        return this.info.CategoryID;
      }
    }

    public int ID
    {
      get
      {
        return this.info.ID;
      }
    }

    public string Name
    {
      get
      {
        return this.info.Name;
      }
    }

    public int[] Stocks { get; } = new int[1]{ 1 };

    public int Rate { get; }

    public int Percent { get; } = 100;
  }
}
