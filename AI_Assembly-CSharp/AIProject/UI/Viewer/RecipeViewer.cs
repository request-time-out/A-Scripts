// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.RecipeViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject.UI.Viewer
{
  public class RecipeViewer : MonoBehaviour
  {
    [SerializeField]
    private CraftUI _craftUI;
    [SerializeField]
    private RecipeItemListUI _itemListUI;
    [SerializeField]
    private RectTransform _resetScrollTarget;
    private Vector2? _resetScrollTargetPos;

    public RecipeViewer()
    {
      base.\u002Ector();
    }

    [DebuggerHidden]
    public static IEnumerator Load(
      CraftUI craftUI,
      Transform viewerParent,
      Action<RecipeViewer> onComplete)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecipeViewer.\u003CLoad\u003Ec__Iterator0()
      {
        viewerParent = viewerParent,
        craftUI = craftUI,
        onComplete = onComplete
      };
    }

    public bool initialized { get; private set; }

    public RecipeItemListUI itemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    private void ResetScroll()
    {
      if (!this._resetScrollTargetPos.HasValue)
        return;
      this._resetScrollTarget.set_anchoredPosition(this._resetScrollTargetPos.Value);
    }

    private void Awake()
    {
      this._resetScrollTargetPos = this._resetScrollTarget != null ? new Vector2?(this._resetScrollTarget.get_anchoredPosition()) : new Vector2?();
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RecipeViewer.\u003CStart\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }
  }
}
