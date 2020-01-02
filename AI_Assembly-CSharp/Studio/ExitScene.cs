// Decompiled with JetBrains decompiler
// Type: Studio.ExitScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

namespace Studio
{
  public class ExitScene : MonoBehaviour
  {
    [SerializeField]
    private VoiceNode yes;
    [SerializeField]
    private VoiceNode no;
    private float timeScale;

    public ExitScene()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.timeScale = Time.get_timeScale();
      Time.set_timeScale(0.0f);
    }

    private void Start()
    {
      VoiceNode yes1 = this.yes;
      // ISSUE: reference to a compiler-generated field
      if (ExitScene.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ExitScene.\u003C\u003Ef__am\u0024cache0 = new UnityAction((object) null, __methodptr(\u003CStart\u003Em__0));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache0 = ExitScene.\u003C\u003Ef__am\u0024cache0;
      yes1.addOnClick = fAmCache0;
      VoiceNode yes2 = this.yes;
      // ISSUE: reference to a compiler-generated field
      if (ExitScene.\u003C\u003Ef__am\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ExitScene.\u003C\u003Ef__am\u0024cache1 = new UnityAction((object) null, __methodptr(\u003CStart\u003Em__1));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache1 = ExitScene.\u003C\u003Ef__am\u0024cache1;
      yes2.addOnClick = fAmCache1;
      VoiceNode no1 = this.no;
      // ISSUE: reference to a compiler-generated field
      if (ExitScene.\u003C\u003Ef__am\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ExitScene.\u003C\u003Ef__am\u0024cache2 = new UnityAction((object) null, __methodptr(\u003CStart\u003Em__2));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache2 = ExitScene.\u003C\u003Ef__am\u0024cache2;
      no1.addOnClick = fAmCache2;
      VoiceNode no2 = this.no;
      // ISSUE: reference to a compiler-generated field
      if (ExitScene.\u003C\u003Ef__am\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ExitScene.\u003C\u003Ef__am\u0024cache3 = new UnityAction((object) null, __methodptr(\u003CStart\u003Em__3));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache3 = ExitScene.\u003C\u003Ef__am\u0024cache3;
      no2.addOnClick = fAmCache3;
    }

    private void OnDestroy()
    {
      Time.set_timeScale(this.timeScale);
    }
  }
}
