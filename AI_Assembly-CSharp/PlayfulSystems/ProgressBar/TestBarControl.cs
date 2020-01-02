// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.TestBarControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayfulSystems.ProgressBar
{
  public class TestBarControl : MonoBehaviour
  {
    [SerializeField]
    private Transform barParent;
    [SerializeField]
    private Transform sizeButtonParent;
    [SerializeField]
    private TestSwitchBar[] barSelectors;
    private ProgressBarPro[] bars;
    private Button[] buttons;
    private Slider slider;

    public TestBarControl()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (Object.op_Inequality((Object) this.barParent, (Object) null))
        this.bars = (ProgressBarPro[]) ((Component) this.barParent).GetComponentsInChildren<ProgressBarPro>(true);
      if (!Object.op_Inequality((Object) this.sizeButtonParent, (Object) null))
        return;
      this.buttons = (Button[]) ((Component) this.sizeButtonParent).GetComponentsInChildren<Button>();
      this.slider = (Slider) ((Component) this).GetComponentInChildren<Slider>();
      this.SetupButtons();
    }

    private void SetupButtons()
    {
      for (int index = 0; index < this.buttons.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TestBarControl.\u003CSetupButtons\u003Ec__AnonStorey0 buttonsCAnonStorey0 = new TestBarControl.\u003CSetupButtons\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        buttonsCAnonStorey0.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        buttonsCAnonStorey0.currentValue = (float) index / (float) (this.buttons.Length - 1);
        Button button = this.buttons[index];
        // ISSUE: reference to a compiler-generated field
        ((Object) button).set_name("Button_" + (object) buttonsCAnonStorey0.currentValue);
        // ISSUE: reference to a compiler-generated field
        ((Text) ((Component) button).GetComponentInChildren<Text>()).set_text(buttonsCAnonStorey0.currentValue.ToString());
        // ISSUE: method pointer
        ((UnityEvent) button.get_onClick()).AddListener(new UnityAction((object) buttonsCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      }
    }

    private void SetSlider(float value)
    {
      if (!Object.op_Inequality((Object) this.slider, (Object) null))
        return;
      this.slider.set_value(value);
    }

    public void SetBars(float value)
    {
      if (this.bars != null)
      {
        for (int index = 0; index < this.bars.Length; ++index)
          this.bars[index].SetValue(value, false);
      }
      if (this.barSelectors == null)
        return;
      for (int index = 0; index < this.barSelectors.Length; ++index)
        this.barSelectors[index].SetValue(value);
    }

    public void SetRandomColor()
    {
      this.SetColor(new Color(Random.get_value(), Random.get_value(), Random.get_value()));
    }

    public void SetColor(Color color)
    {
      if (this.bars != null)
      {
        for (int index = 0; index < this.bars.Length; ++index)
          this.bars[index].SetBarColor(color);
      }
      if (this.barSelectors == null)
        return;
      for (int index = 0; index < this.barSelectors.Length; ++index)
        this.barSelectors[index].SetBarColor(color);
    }
  }
}
