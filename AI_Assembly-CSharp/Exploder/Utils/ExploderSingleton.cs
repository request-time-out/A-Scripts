// Decompiled with JetBrains decompiler
// Type: Exploder.Utils.ExploderSingleton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Exploder.Utils
{
  public class ExploderSingleton : MonoBehaviour
  {
    [Obsolete("ExploderInstance is obsolete, please use Instance instead.")]
    public static ExploderObject ExploderInstance;
    public static ExploderObject Instance;

    public ExploderSingleton()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      ExploderSingleton.Instance = (ExploderObject) ((Component) this).get_gameObject().GetComponent<ExploderObject>();
      ExploderSingleton.ExploderInstance = ExploderSingleton.Instance;
    }
  }
}
