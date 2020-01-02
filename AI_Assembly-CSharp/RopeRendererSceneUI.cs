// Decompiled with JetBrains decompiler
// Type: RopeRendererSceneUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using PicoGames.QuickRopes;
using PicoGames.Utilities;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RopeRendererSceneUI : MonoBehaviour
{
  public Color stoppedColor;
  public Color playingColor;
  public Slider radiusSlider;
  public Slider edgeCountSlider;
  public Slider edgeIndentSlider;
  public Slider edgeDetailSlider;
  public Slider strandCountSlider;
  public Slider strandOffsetSlider;
  public Slider strandTwistSlider;
  public Button playButton;
  public Text radiusText;
  public Text edgeCountText;
  public Text edgeIndentText;
  public Text edgeDetailText;
  public Text strandCountText;
  public Text strandOffsetText;
  public Text strandTwistText;
  public Text buttonText;
  public Material[] availMaterials;
  public QuickRope rope;
  public RopeRenderer rRender;

  public RopeRendererSceneUI()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Equality((Object) this.rRender, (Object) null))
      return;
    this.radiusSlider.set_value(this.rRender.Radius);
    this.edgeCountSlider.set_value((float) this.rRender.EdgeCount);
    this.edgeIndentSlider.set_value(this.rRender.EdgeIndent);
    this.edgeDetailSlider.set_value((float) this.rRender.EdgeDetail);
    this.strandCountSlider.set_value((float) this.rRender.StrandCount);
    this.strandOffsetSlider.set_value(this.rRender.StrandOffset);
    this.strandTwistSlider.set_value(this.rRender.StrandTwist);
    this.buttonText.set_text("Press To " + (!this.rope.usePhysics ? "Play Physics" : "Stop Physics"));
    Button playButton = this.playButton;
    ColorBlock colorBlock1 = (ColorBlock) null;
    ((ColorBlock) ref colorBlock1).set_normalColor(!this.rope.usePhysics ? this.stoppedColor : this.playingColor);
    ((ColorBlock) ref colorBlock1).set_highlightedColor(!this.rope.usePhysics ? this.stoppedColor : this.playingColor);
    ((ColorBlock) ref colorBlock1).set_pressedColor(!this.rope.usePhysics ? this.stoppedColor : this.playingColor);
    ((ColorBlock) ref colorBlock1).set_colorMultiplier(1f);
    ((ColorBlock) ref colorBlock1).set_fadeDuration(0.2f);
    ColorBlock colorBlock2 = colorBlock1;
    ((Selectable) playButton).set_colors(colorBlock2);
    this.UpdateLabels();
    // ISSUE: method pointer
    ((UnityEvent<float>) this.radiusSlider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(UpdateRenderer)));
    // ISSUE: method pointer
    ((UnityEvent<float>) this.edgeCountSlider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(UpdateRenderer)));
    // ISSUE: method pointer
    ((UnityEvent<float>) this.edgeIndentSlider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(UpdateRenderer)));
    // ISSUE: method pointer
    ((UnityEvent<float>) this.edgeDetailSlider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(UpdateRenderer)));
    // ISSUE: method pointer
    ((UnityEvent<float>) this.strandCountSlider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(UpdateRenderer)));
    // ISSUE: method pointer
    ((UnityEvent<float>) this.strandOffsetSlider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(UpdateRenderer)));
    // ISSUE: method pointer
    ((UnityEvent<float>) this.strandTwistSlider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(UpdateRenderer)));
  }

  public void TogglePhysics()
  {
    this.rope.usePhysics = !this.rope.usePhysics;
    this.buttonText.set_text("Press To " + (!this.rope.usePhysics ? "Play Physics" : "Stop Physics"));
    Button playButton = this.playButton;
    ColorBlock colorBlock1 = (ColorBlock) null;
    ((ColorBlock) ref colorBlock1).set_normalColor(!this.rope.usePhysics ? this.stoppedColor : this.playingColor);
    ((ColorBlock) ref colorBlock1).set_highlightedColor(!this.rope.usePhysics ? this.stoppedColor : this.playingColor);
    ((ColorBlock) ref colorBlock1).set_pressedColor(!this.rope.usePhysics ? this.stoppedColor : this.playingColor);
    ((ColorBlock) ref colorBlock1).set_colorMultiplier(1f);
    ((ColorBlock) ref colorBlock1).set_fadeDuration(0.2f);
    ColorBlock colorBlock2 = colorBlock1;
    ((Selectable) playButton).set_colors(colorBlock2);
    this.rope.defaultColliderSettings.radius = this.rRender.Radius * 0.5f;
    this.rope.Generate();
  }

  private void UpdateRenderer(float value)
  {
    this.rRender.Radius = this.radiusSlider.get_value();
    this.rRender.EdgeCount = (int) this.edgeCountSlider.get_value();
    this.rRender.EdgeIndent = this.edgeIndentSlider.get_value();
    this.rRender.EdgeDetail = (int) this.edgeDetailSlider.get_value();
    this.rRender.StrandCount = (int) this.strandCountSlider.get_value();
    this.rRender.StrandOffset = this.strandOffsetSlider.get_value();
    this.rRender.StrandTwist = this.strandTwistSlider.get_value();
    this.UpdateLabels();
  }

  private void UpdateLabels()
  {
    this.radiusText.set_text(this.rRender.Radius.ToString("0.00"));
    this.edgeCountText.set_text(this.rRender.EdgeCount.ToString());
    this.edgeIndentText.set_text(this.rRender.EdgeIndent.ToString("0.00"));
    this.edgeDetailText.set_text(this.rRender.EdgeDetail.ToString());
    this.strandCountText.set_text(this.rRender.StrandCount.ToString());
    this.strandOffsetText.set_text(this.rRender.StrandOffset.ToString("0.00"));
    this.strandTwistText.set_text(this.rRender.StrandTwist.ToString("0.00"));
  }

  public void VisitAssetStore()
  {
  }

  public void FadeToSingleStrand()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.SmoothSetRope(0.3f, 8, 1, 5f, 1, 0.75f, 0.0f));
  }

  public void FadeToHighPolyBraidedRope()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.SmoothSetRope(0.2f, 12, 1, 5f, 6, 0.9f, 35f));
  }

  public void FadeToLowPolyBraidedRope()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.SmoothSetRope(0.3f, 6, 1, 5f, 3, 0.5f, 50f));
  }

  public void FadeToStarRope()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.SmoothSetRope(0.5f, 5, 2, 2.5f, 1, 0.0f, 15f));
  }

  public void FadeToFlowerRope()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.SmoothSetRope(0.5f, 7, 10, 2f, 1, 0.0f, 15f));
  }

  public void FadeToSmallCable()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.SmoothSetRope(0.15f, 8, 1, 5f, 4, 0.5f, 50f));
  }

  [DebuggerHidden]
  private IEnumerator SmoothSetRope(
    float _radius,
    int _edgeCount,
    int _edgeDetail,
    float _edgeIndent,
    int _strandCount,
    float _strandOffset,
    float _strandTwist)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RopeRendererSceneUI.\u003CSmoothSetRope\u003Ec__Iterator0()
    {
      _radius = _radius,
      _edgeCount = _edgeCount,
      _edgeDetail = _edgeDetail,
      _edgeIndent = _edgeIndent,
      _strandCount = _strandCount,
      _strandOffset = _strandOffset,
      _strandTwist = _strandTwist,
      \u0024this = this
    };
  }
}
