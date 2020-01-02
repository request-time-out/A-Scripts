// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.ExtraPadding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;

namespace AIProject.UI.Recycling
{
  public class ExtraPadding : ItemNodeUI.ExtraData
  {
    public ExtraPadding(StuffItem item, ItemListController source)
    {
      this.item = item;
      this.source = source;
    }

    public StuffItem item { get; }

    public ItemListController source { get; }
  }
}
