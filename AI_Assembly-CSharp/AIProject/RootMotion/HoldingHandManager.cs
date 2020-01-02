// Decompiled with JetBrains decompiler
// Type: AIProject.RootMotion.HoldingHandManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.RootMotion
{
  public class HoldingHandManager : MonoBehaviour
  {
    [SerializeField]
    private HandsHolder _leftHandHolder;

    public HoldingHandManager()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      ((Behaviour) this._leftHandHolder).set_enabled(true);
      this._leftHandHolder.EnabledHolding = true;
    }

    private void OnDisable()
    {
      ((Behaviour) this._leftHandHolder).set_enabled(false);
      this._leftHandHolder.EnabledHolding = false;
    }
  }
}
