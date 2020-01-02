// Decompiled with JetBrains decompiler
// Type: TestSwitchBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class TestSwitchBar : MonoBehaviour
{
  [SerializeField]
  private ProgressBarPro[] progressBarPrefabs;
  [SerializeField]
  private Transform prefabParent;
  [SerializeField]
  private int currentPrefab;
  [SerializeField]
  private Text prefabName;
  private ProgressBarPro bar;
  private float currentValue;

  public TestSwitchBar()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.InstantiateBar(this.currentPrefab);
  }

  public void SetRandomBar()
  {
    this.InstantiateBar(Random.Range(0, this.progressBarPrefabs.Length));
  }

  public void ShiftBar(int shift)
  {
    int num = this.currentPrefab + shift;
    if (num >= this.progressBarPrefabs.Length)
      this.InstantiateBar(0);
    if (num < 0)
      this.InstantiateBar(this.progressBarPrefabs.Length - 1);
    else
      this.InstantiateBar(num);
  }

  private void InstantiateBar(int num)
  {
    if (num < 0 || num >= this.progressBarPrefabs.Length)
      return;
    this.currentPrefab = num;
    if (Object.op_Inequality((Object) this.bar, (Object) null))
      Object.Destroy((Object) ((Component) this.bar).get_gameObject());
    this.bar = (ProgressBarPro) Object.Instantiate<ProgressBarPro>((M0) this.progressBarPrefabs[num], this.prefabParent);
    ((Component) this.bar).get_gameObject().SetActive(false);
    ((Component) this.bar).get_transform().set_localScale(Vector3.get_one());
    this.bar.SetValue(this.currentValue, false);
    ((Component) this.bar).get_gameObject().SetActive(true);
    this.prefabName.set_text(((Object) ((Component) this.progressBarPrefabs[this.currentPrefab]).get_gameObject()).get_name());
    this.Invoke("EnableBar", 0.01f);
  }

  private void EnableBar()
  {
    if (!Object.op_Inequality((Object) this.bar, (Object) null))
      return;
    ((Component) this.bar).get_gameObject().SetActive(true);
  }

  public void SetValue(float value)
  {
    this.currentValue = value;
    if (!Object.op_Inequality((Object) this.bar, (Object) null))
      return;
    this.bar.SetValue(value, false);
  }

  public void SetBarColor(Color color)
  {
    if (!Object.op_Inequality((Object) this.bar, (Object) null))
      return;
    this.bar.SetBarColor(color);
  }
}
