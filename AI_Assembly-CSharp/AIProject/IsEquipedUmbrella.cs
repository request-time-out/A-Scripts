// Decompiled with JetBrains decompiler
// Type: AIProject.IsEquipedUmbrella
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class IsEquipedUmbrella : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.EquipedItem == null)
        return (TaskStatus) 1;
      ItemIDKeyPair umbrellaId = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.UmbrellaID;
      return Singleton<Resources>.Instance.GameInfo.CommonEquipEventItemTable[umbrellaId.categoryID][umbrellaId.itemID].EventItemID == this.Agent.EquipedItem.EventItemID ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
