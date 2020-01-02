// Decompiled with JetBrains decompiler
// Type: ADV.Commands.EventCG.Common
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV.EventCG;
using UnityEngine;

namespace ADV.Commands.EventCG
{
  internal static class Common
  {
    public static bool Release(TextScenario scenario)
    {
      bool flag = scenario.commandController.EventCGRoot.get_childCount() > 0;
      if (flag)
      {
        Transform child = scenario.commandController.EventCGRoot.GetChild(0);
        Data component = (Data) ((Component) child).GetComponent<Data>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          component.ItemClear();
          component.Restore();
        }
        Object.Destroy((Object) ((Component) child).get_gameObject());
        Transform transform = child;
        ((Object) transform).set_name(((Object) transform).get_name() + "(Destroyed)");
        child.set_parent((Transform) null);
      }
      return flag;
    }
  }
}
