// Decompiled with JetBrains decompiler
// Type: Studio.ShortcutMenuScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace Studio
{
  public class ShortcutMenuScene : MonoBehaviour
  {
    private float timeScale;

    public ShortcutMenuScene()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.timeScale = Time.get_timeScale();
      Time.set_timeScale(0.0f);
    }

    private void Update()
    {
      if (!Input.GetMouseButtonDown(1) && !Input.GetKeyDown((KeyCode) 283))
        return;
      Singleton<Scene>.Instance.UnLoad();
    }

    private void OnDestroy()
    {
      Time.set_timeScale(this.timeScale);
    }
  }
}
