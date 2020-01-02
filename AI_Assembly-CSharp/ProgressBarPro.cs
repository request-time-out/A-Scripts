// Decompiled with JetBrains decompiler
// Type: ProgressBarPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using PlayfulSystems.ProgressBar;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
public class ProgressBarPro : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  private float m_value;
  private float displayValue;
  [Space(10f)]
  [Tooltip("Smoothes out the animation of the bar.")]
  [SerializeField]
  private bool animateBar;
  [SerializeField]
  private ProgressBarPro.AnimationType animationType;
  [SerializeField]
  private float animTime;
  [Space(10f)]
  [SerializeField]
  private ProgressBarProView[] views;
  private Coroutine sizeAnim;

  public ProgressBarPro()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    if (this.views != null && this.views.Length != 0)
      return;
    this.views = (ProgressBarProView[]) ((Component) this).GetComponentsInChildren<ProgressBarProView>();
  }

  private void OnEnable()
  {
    this.SetDisplayValue(this.m_value, true);
  }

  public float Value
  {
    get
    {
      return this.m_value;
    }
    set
    {
      if ((double) value == (double) this.m_value)
        return;
      this.SetValue(value, false);
    }
  }

  public void SetValue(float value, float maxValue)
  {
    if ((double) maxValue != 0.0)
      this.SetValue(value / maxValue, false);
    else
      this.SetValue(0.0f, false);
  }

  public void SetValue(int value, int maxValue)
  {
    if (maxValue != 0)
      this.SetValue((float) value / (float) maxValue, false);
    else
      this.SetValue(0.0f, false);
  }

  public void SetValue(float percentage, bool forceUpdate = false)
  {
    if (!forceUpdate && Mathf.Approximately(this.m_value, percentage))
      return;
    this.m_value = Mathf.Clamp01(percentage);
    for (int index = 0; index < this.views.Length; ++index)
      this.views[index].NewChangeStarted(this.displayValue, this.m_value);
    if (this.animateBar && Application.get_isPlaying() && ((Component) this).get_gameObject().get_activeInHierarchy())
      this.StartSizeAnim(percentage);
    else
      this.SetDisplayValue(percentage, false);
  }

  public bool IsAnimating()
  {
    return this.animateBar && !Mathf.Approximately(this.displayValue, this.m_value);
  }

  public void SetBarColor(Color color)
  {
    for (int index = 0; index < this.views.Length; ++index)
      this.views[index].SetBarColor(color);
  }

  private void StartSizeAnim(float percentage)
  {
    if (this.sizeAnim != null)
      this.StopCoroutine(this.sizeAnim);
    this.sizeAnim = this.StartCoroutine(this.DoBarSizeAnim());
  }

  [DebuggerHidden]
  private IEnumerator DoBarSizeAnim()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ProgressBarPro.\u003CDoBarSizeAnim\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void SetDisplayValue(float value, bool forceUpdate = false)
  {
    if (!forceUpdate && (double) this.displayValue >= 0.0 && Mathf.Approximately(this.displayValue, value))
      return;
    this.displayValue = value;
    this.UpdateBarViews(this.displayValue, this.m_value, forceUpdate);
  }

  private void UpdateBarViews(float currentValue, float targetValue, bool forceUpdate = false)
  {
    if (this.views == null)
      return;
    for (int index = 0; index < this.views.Length; ++index)
    {
      if (Object.op_Inequality((Object) this.views[index], (Object) null) && (forceUpdate || this.views[index].CanUpdateView(currentValue, targetValue)))
        this.views[index].UpdateView(currentValue, targetValue);
    }
  }

  private void OnDidApplyAnimationProperties()
  {
    this.SetValue(this.m_value, true);
  }

  public enum AnimationType
  {
    FixedTimeForChange,
    ChangeSpeed,
  }
}
