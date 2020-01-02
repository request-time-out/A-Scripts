// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.ShopRateViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public class ShopRateViewer : MonoBehaviour
  {
    [SerializeField]
    private Text _infoText;
    [SerializeField]
    private ItemListUI _itemListUI;
    [SerializeField]
    private ConditionalTextXtoYViewer _rateCounter;

    public ShopRateViewer()
    {
      base.\u002Ector();
    }

    public Text infoText
    {
      get
      {
        return this._infoText;
      }
    }

    public ItemListUI itemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    public ConditionalTextXtoYViewer rateCounter
    {
      get
      {
        return this._rateCounter;
      }
    }

    public bool isTrade { get; private set; }

    public ShopViewer.ItemListController controller { get; }

    public bool initialized { get; private set; }

    private void Awake()
    {
      this.controller.Bind(this._itemListUI);
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopRateViewer.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
