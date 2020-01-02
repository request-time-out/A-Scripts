// Decompiled with JetBrains decompiler
// Type: CraftInfomationUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftInfomationUI : MonoBehaviour
{
  [SerializeField]
  private Text costText;
  [SerializeField]
  private GameObject gageBG;
  private RectTransform gageRectBG;
  [SerializeField]
  private GameObject gage;
  private RectTransform gageRect;
  private float fGageAmountvalue;
  [SerializeField]
  private GameObject warningPanel;
  private Image warningBG;
  private Text warningTex;
  public float fWarningExist;
  [SerializeField]
  private GameObject opelatePanel;
  public Button moveSelect;
  public Button deleteSelect;
  [SerializeField]
  private CraftControler craftControler;
  private string[] warningMassage;
  private IntReactiveProperty cost;
  private int prevCost;
  private BoolReactiveProperty fade;
  private float alphaVel;
  private Color[] WarningdefColor;
  public const int nMaxCost = 500;

  public CraftInfomationUI()
  {
    base.\u002Ector();
  }

  public int nCost
  {
    get
    {
      return ((ReactiveProperty<int>) this.cost).get_Value();
    }
    set
    {
      ((ReactiveProperty<int>) this.cost).set_Value(value);
    }
  }

  public bool bFade
  {
    get
    {
      return ((ReactiveProperty<bool>) this.fade).get_Value();
    }
    set
    {
      ((ReactiveProperty<bool>) this.fade).set_Value(value);
    }
  }

  private void Start()
  {
    this.nCost = 0;
    this.prevCost = 0;
    ObservableExtensions.Subscribe<int>(Observable.Where<int>((IObservable<M0>) this.cost, (Func<M0, bool>) (x => x != this.prevCost)), (Action<M0>) (x => this.ChangeCostTex(x)));
    this.gageRectBG = (RectTransform) this.gageBG.GetComponent<RectTransform>();
    this.gageRect = (RectTransform) this.gage.GetComponent<RectTransform>();
    this.fGageAmountvalue = (float) (this.gageRectBG.get_sizeDelta().x / 500.0);
    this.warningBG = (Image) this.warningPanel.GetComponentInChildren<Image>();
    this.warningTex = (Text) this.warningPanel.GetComponentInChildren<Text>();
    this.WarningdefColor = new Color[2]
    {
      ((Graphic) this.warningBG).get_color(),
      ((Graphic) this.warningTex).get_color()
    };
    ObservableExtensions.Subscribe<long>(Observable.Where<long>((IObservable<M0>) Observable.EveryUpdate(), (Func<M0, bool>) (_ => ((ReactiveProperty<bool>) this.fade).get_Value())), (Action<M0>) (_ => this.fadeMassage()));
    this.fWarningExist = 0.0f;
    if (this.moveSelect != null)
    {
      // ISSUE: method pointer
      ((UnityEvent) this.moveSelect.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(BuildPartsSelect)));
    }
    if (this.deleteSelect == null)
      return;
    // ISSUE: method pointer
    ((UnityEvent) this.deleteSelect.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
  }

  private void ChangeCostTex(int newCost)
  {
    this.prevCost = newCost;
    this.costText.set_text(string.Format("{0}/{1}", (object) this.nCost, (object) 500));
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector(this.fGageAmountvalue * (float) this.nCost, (float) this.gageRectBG.get_sizeDelta().y);
    vector2.x = (__Null) (double) Mathf.Clamp((float) vector2.x, 0.0f, (float) this.gageRectBG.get_sizeDelta().x);
    this.gageRect.set_sizeDelta(vector2);
  }

  public void SetWarningMessage(int Pattern = 0)
  {
    if (this.warningPanel.get_activeSelf())
      return;
    this.warningPanel.SetActive(true);
    this.warningTex.set_text(this.warningMassage[Pattern]);
    this.fWarningExist = 0.0f;
    ((Graphic) this.warningBG).set_color(this.WarningdefColor[0]);
    ((Graphic) this.warningTex).set_color(this.WarningdefColor[1]);
  }

  private void fadeMassage()
  {
    Color[] colorArray = new Color[2]
    {
      ((Graphic) this.warningBG).get_color(),
      ((Graphic) this.warningTex).get_color()
    };
    colorArray[0].a = (__Null) (double) Mathf.SmoothDamp((float) colorArray[0].a, 0.0f, ref this.alphaVel, 0.8f);
    colorArray[1].a = (__Null) (double) Mathf.SmoothDamp((float) colorArray[1].a, 0.0f, ref this.alphaVel, 0.8f);
    if (colorArray[0].a <= 0.0)
    {
      this.warningPanel.SetActive(false);
      this.bFade = false;
      this.fWarningExist = 0.0f;
      colorArray[0].a = (__Null) 0.0;
      colorArray[1].a = (__Null) 0.0;
    }
    ((Graphic) this.warningBG).set_color(colorArray[0]);
    ((Graphic) this.warningTex).set_color(colorArray[1]);
  }

  public bool GetWarningActive()
  {
    return this.warningPanel.get_activeSelf();
  }

  public void SetOpeLatePanel()
  {
    this.opelatePanel.SetActive(true);
  }

  private void BuildPartsSelect()
  {
    this.opelatePanel.SetActive(false);
    this.craftControler.SelectBuldPart();
  }
}
