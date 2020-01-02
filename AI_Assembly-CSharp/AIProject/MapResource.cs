// Decompiled with JetBrains decompiler
// Type: AIProject.MapResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject
{
  public class MapResource : MonoBehaviour
  {
    [SerializeField]
    private string _assetBundleName;
    [SerializeField]
    private string _manifestName;
    [SerializeField]
    private string[] _prefabs;

    public MapResource()
    {
      base.\u002Ector();
    }

    [DebuggerHidden]
    public IEnumerator Load()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MapResource.\u003CLoad\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
