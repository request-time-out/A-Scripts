// Decompiled with JetBrains decompiler
// Type: AIProject.StuffItemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject
{
  public class StuffItemInfo
  {
    public StuffItemInfo(int category, ItemData.Param param, bool isNone)
    {
      this.CategoryID = category;
      this.param = param;
      this.isNone = isNone;
      if (!isNone)
        return;
      this.Rarelity = (Rarelity) param.Grade;
    }

    private ItemData.Param param { get; set; }

    public int ID
    {
      get
      {
        return this.param.ID;
      }
    }

    public string Name
    {
      get
      {
        return this.param.Name;
      }
    }

    public int IconID
    {
      get
      {
        return this.param.IconID;
      }
    }

    public int CategoryID { get; private set; }

    public Rarelity Rarelity { get; set; }

    public int ReactionType { get; set; }

    public bool IsAvailableHeroine { get; set; }

    public ThresholdInt EnnuiAddition { get; set; }

    public ThresholdInt TasteAdditionNormal { get; set; }

    public ThresholdInt TasteAdditionEnnui { get; set; }

    public ItemEquipableState EquipableState { get; set; }

    public int Rate
    {
      get
      {
        return this.param.Rate;
      }
    }

    public Grade Grade
    {
      get
      {
        return (Grade) this.param.Grade;
      }
    }

    public string Explanation
    {
      get
      {
        return this.param.Explanation;
      }
    }

    public bool isTrash
    {
      get
      {
        return this.param.Rate >= 0;
      }
    }

    public int nameHash
    {
      get
      {
        return this.param.nameHash;
      }
    }

    public bool isNone { get; }
  }
}
