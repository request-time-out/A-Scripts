// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Object.ObjectEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.Object
{
  internal static class ObjectEx
  {
    public static Transform FindRoot(string findType, CommandController commandController)
    {
      Transform transform = (Transform) null;
      if (!findType.IsNullOrEmpty())
      {
        int result;
        transform = !int.TryParse(findType, out result) ? commandController.Objects[findType].get_transform() : commandController.CharaRoot.GetChild(result);
      }
      return transform;
    }

    public static Transform FindChild(Transform root, string name)
    {
      return ((IEnumerable<Transform>) ((Component) root).GetComponentsInChildren<Transform>(true)).FirstOrDefault<Transform>((Func<Transform, bool>) (t => ((UnityEngine.Object) t).get_name() == name));
    }

    public static Transform FindGet(
      string findType,
      string childName,
      string otherRootName,
      CommandController commandController)
    {
      Transform root = ObjectEx.FindRoot(findType, commandController);
      if (UnityEngine.Object.op_Equality((UnityEngine.Object) root, (UnityEngine.Object) null))
        root = GameObject.Find(otherRootName).get_transform();
      if (!childName.IsNullOrEmpty())
        root = ObjectEx.FindChild(root, childName);
      return root;
    }
  }
}
