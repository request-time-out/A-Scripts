// Decompiled with JetBrains decompiler
// Type: AIProject.HasItemToDrink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;

namespace AIProject
{
  [TaskCategory("")]
  public class HasItemToDrink : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      List<StuffItem> itemList = this.Agent.AgentData.ItemList;
      Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>> drinkParameterTable = Singleton<Resources>.Instance.GameInfo.DrinkParameterTable;
      foreach (StuffItem stuffItem in itemList)
      {
        Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary;
        if (drinkParameterTable.TryGetValue(stuffItem.CategoryID, out dictionary) && dictionary.TryGetValue(stuffItem.ID, out Dictionary<int, FoodParameterPacket> _))
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
