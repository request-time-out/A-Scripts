// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomClothesPatternSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomClothesPatternSelect : MonoBehaviour
  {
    [SerializeField]
    private CustomSelectScrollController sscPattern;
    [SerializeField]
    private Button btnClose;
    private CanvasGroup canvasGroup;
    public Action<int, int> onSelect;

    public CustomClothesPatternSelect()
    {
      base.\u002Ector();
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    private ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    private ChaFileClothes nowClothes
    {
      get
      {
        return this.chaCtrl.nowCoordinate.clothes;
      }
    }

    private ChaFileClothes orgClothes
    {
      get
      {
        return this.chaCtrl.chaFile.coordinate.clothes;
      }
    }

    public int parts { get; set; }

    public int idx { get; set; }

    public void ChangeLink(int _parts, int _idx)
    {
      this.parts = _parts;
      this.idx = _idx;
      if (this.parts == -1 || this.idx == -1)
        return;
      this.sscPattern.SetToggleID(this.nowClothes.parts[this.parts].colorInfo[this.idx].pattern);
      this.sscPattern.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.nowClothes.parts[this.parts].colorInfo[this.idx].pattern == info.id)
          return;
        this.nowClothes.parts[this.parts].colorInfo[this.idx].pattern = info.id;
        this.orgClothes.parts[this.parts].colorInfo[this.idx].pattern = info.id;
        this.chaCtrl.ChangeCustomClothes(this.parts, false, 0 == this.idx, 1 == this.idx, 2 == this.idx);
        if (this.onSelect == null)
          return;
        this.onSelect(this.parts, this.idx);
      });
    }

    private void Awake()
    {
      this.canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
    }

    private void Start()
    {
      this.sscPattern.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_pattern, ChaListDefine.KeyType.Unknown));
      if (!Object.op_Implicit((Object) this.btnClose))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnClose), (Action<M0>) (_ => this.customBase.customCtrl.showPattern = false));
    }
  }
}
