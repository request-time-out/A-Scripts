// Decompiled with JetBrains decompiler
// Type: HouseName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HouseName : MonoBehaviour
{
  [SerializeField]
  private GameObject nameTextObj;
  [SerializeField]
  private Text prevNameTextObj;
  [SerializeField]
  private GameObject nameInputForm;
  [SerializeField]
  private Text Name;
  [SerializeField]
  private Button decide;
  [SerializeField]
  private Button cancel;

  public HouseName()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) this.nameTextObj, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) ((Button) this.nameTextObj.GetComponent<Button>()).get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickNameTex)));
    }
    if (Object.op_Inequality((Object) this.decide, (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent) this.decide.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(ChangeTex)));
    }
    if (!Object.op_Inequality((Object) this.cancel, (Object) null))
      return;
    // ISSUE: method pointer
    ((UnityEvent) this.cancel.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(CloseInputForm)));
  }

  private void ChangeTex()
  {
    ((Text) this.nameTextObj.GetComponentInChildren<Text>()).set_text(this.Name.get_text());
    this.nameInputForm.SetActive(false);
  }

  private void CloseInputForm()
  {
    this.nameInputForm.SetActive(false);
  }

  private void OnClickNameTex()
  {
    if (Object.op_Inequality((Object) this.nameInputForm, (Object) null))
      this.nameInputForm.SetActive(true);
    this.prevNameTextObj.set_text(((Text) this.nameTextObj.GetComponentInChildren<Text>()).get_text());
  }
}
