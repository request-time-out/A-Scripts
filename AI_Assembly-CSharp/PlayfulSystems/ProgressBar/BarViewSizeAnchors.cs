// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.BarViewSizeAnchors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace PlayfulSystems.ProgressBar
{
  [RequireComponent(typeof (RectTransform))]
  public class BarViewSizeAnchors : ProgressBarProView
  {
    [SerializeField]
    private bool hideOnEmpty = true;
    [SerializeField]
    private int numSteps = 10;
    [SerializeField]
    protected RectTransform rectTrans;
    [SerializeField]
    protected BarViewSizeAnchors.FillType fillType;
    [SerializeField]
    private bool useDiscreteSteps;
    protected DrivenRectTransformTracker m_Tracker;
    protected bool isDisplaySizeZero;

    public override bool CanUpdateView(float currentValue, float targetValue)
    {
      return ((Behaviour) this).get_isActiveAndEnabled() || this.isDisplaySizeZero;
    }

    public override void UpdateView(float currentValue, float targetValue)
    {
      if (this.hideOnEmpty && (double) currentValue <= 0.0)
      {
        ((Component) this.rectTrans).get_gameObject().SetActive(false);
        this.isDisplaySizeZero = true;
      }
      else
      {
        this.isDisplaySizeZero = false;
        ((Component) this.rectTrans).get_gameObject().SetActive(true);
        this.SetPivot(0.0f, currentValue);
      }
    }

    protected void SetPivot(float startEdge, float endEdge)
    {
      if (this.useDiscreteSteps)
      {
        startEdge = Mathf.Round(startEdge * (float) this.numSteps) / (float) this.numSteps;
        endEdge = Mathf.Round(endEdge * (float) this.numSteps) / (float) this.numSteps;
      }
      this.UpdateTracker();
      switch (this.fillType)
      {
        case BarViewSizeAnchors.FillType.LeftToRight:
          this.rectTrans.set_anchorMin(new Vector2(startEdge, (float) this.rectTrans.get_anchorMin().y));
          this.rectTrans.set_anchorMax(new Vector2(endEdge, (float) this.rectTrans.get_anchorMax().y));
          break;
        case BarViewSizeAnchors.FillType.RightToLeft:
          this.rectTrans.set_anchorMin(new Vector2(1f - endEdge, (float) this.rectTrans.get_anchorMin().y));
          this.rectTrans.set_anchorMax(new Vector2(1f - startEdge, (float) this.rectTrans.get_anchorMax().y));
          break;
        case BarViewSizeAnchors.FillType.TopToBottom:
          this.rectTrans.set_anchorMin(new Vector2((float) this.rectTrans.get_anchorMin().x, 1f - endEdge));
          this.rectTrans.set_anchorMax(new Vector2((float) this.rectTrans.get_anchorMax().x, 1f - startEdge));
          break;
        case BarViewSizeAnchors.FillType.BottomToTop:
          this.rectTrans.set_anchorMin(new Vector2((float) this.rectTrans.get_anchorMin().x, startEdge));
          this.rectTrans.set_anchorMax(new Vector2((float) this.rectTrans.get_anchorMax().x, endEdge));
          break;
      }
    }

    private void UpdateTracker()
    {
      if (this.fillType == BarViewSizeAnchors.FillType.LeftToRight || this.fillType == BarViewSizeAnchors.FillType.RightToLeft)
        ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, this.rectTrans, (DrivenTransformProperties) 1280);
      else
        ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, this.rectTrans, (DrivenTransformProperties) 2560);
    }

    private void OnDisable()
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
    }

    public enum FillType
    {
      LeftToRight,
      RightToLeft,
      TopToBottom,
      BottomToTop,
    }
  }
}
