// Decompiled with JetBrains decompiler
// Type: DetectUnknownControllerMappings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class DetectUnknownControllerMappings : MonoBehaviour
{
  public Text axis1Value;
  public Text axis2Value;
  public Text axis3Value;
  public Text axis4Value;
  public Text axis5Value;
  public Text axis6Value;
  public Text axis7Value;
  public Text axis8Value;
  public Text axis9Value;
  public Text axis10Value;
  public Text axis11Value;
  public Text axis12Value;
  public Text axis13Value;
  public Text axis14Value;
  public Text axis15Value;
  public Text axis16Value;
  public Text axis17Value;
  public Text axis18Value;
  public Text axis19Value;
  public Text axis20Value;
  public Text axis21Value;
  public Text axis22Value;
  public Text axis23Value;
  public Text axis24Value;
  public Text axis25Value;
  public Text axis26Value;
  public Text axis27Value;
  public Text axis28Value;
  public Text button0Value;
  public Text button1Value;
  public Text button2Value;
  public Text button3Value;
  public Text button4Value;
  public Text button5Value;
  public Text button6Value;
  public Text button7Value;
  public Text button8Value;
  public Text button9Value;
  public Text button10Value;
  public Text button11Value;
  public Text button12Value;
  public Text button13Value;
  public Text button14Value;
  public Text button15Value;
  public Text button16Value;
  public Text button17Value;
  public Text button18Value;
  public Text button19Value;

  public DetectUnknownControllerMappings()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if ((double) Input.GetAxis("Axis 1") > 0.0)
      this.axis1Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 1") < 0.0)
      this.axis1Value.set_text("negative");
    else
      this.axis1Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 2") > 0.0)
      this.axis2Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 2") < 0.0)
      this.axis2Value.set_text("negative");
    else
      this.axis2Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 3") > 0.0)
      this.axis3Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 3") < 0.0)
      this.axis3Value.set_text("negative");
    else
      this.axis3Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 4") > 0.0)
      this.axis4Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 4") < 0.0)
      this.axis4Value.set_text("negative");
    else
      this.axis4Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 5") > 0.0)
      this.axis5Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 5") < 0.0)
      this.axis5Value.set_text("negative");
    else
      this.axis5Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 6") > 0.0)
      this.axis6Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 6") < 0.0)
      this.axis6Value.set_text("negative");
    else
      this.axis6Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 7") > 0.0)
      this.axis7Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 7") < 0.0)
      this.axis7Value.set_text("negative");
    else
      this.axis7Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 8") > 0.0)
      this.axis8Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 8") < 0.0)
      this.axis8Value.set_text("negative");
    else
      this.axis8Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 9") > 0.0)
      this.axis9Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 9") < 0.0)
      this.axis9Value.set_text("negative");
    else
      this.axis9Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 10") > 0.0)
      this.axis10Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 10") < 0.0)
      this.axis10Value.set_text("negative");
    else
      this.axis10Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 11") > 0.0)
      this.axis11Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 11") < 0.0)
      this.axis11Value.set_text("negative");
    else
      this.axis11Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 12") > 0.0)
      this.axis12Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 12") < 0.0)
      this.axis12Value.set_text("negative");
    else
      this.axis12Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 13") > 0.0)
      this.axis13Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 13") < 0.0)
      this.axis13Value.set_text("negative");
    else
      this.axis13Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 14") > 0.0)
      this.axis14Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 14") < 0.0)
      this.axis14Value.set_text("negative");
    else
      this.axis14Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 15") > 0.0)
      this.axis15Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 15") < 0.0)
      this.axis15Value.set_text("negative");
    else
      this.axis15Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 16") > 0.0)
      this.axis16Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 16") < 0.0)
      this.axis16Value.set_text("negative");
    else
      this.axis16Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 17") > 0.0)
      this.axis17Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 17") < 0.0)
      this.axis17Value.set_text("negative");
    else
      this.axis17Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 18") > 0.0)
      this.axis18Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 18") < 0.0)
      this.axis18Value.set_text("negative");
    else
      this.axis18Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 19") > 0.0)
      this.axis19Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 19") < 0.0)
      this.axis19Value.set_text("negative");
    else
      this.axis19Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 20") > 0.0)
      this.axis20Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 20") < 0.0)
      this.axis20Value.set_text("negative");
    else
      this.axis20Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 21") > 0.0)
      this.axis21Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 21") < 0.0)
      this.axis21Value.set_text("negative");
    else
      this.axis21Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 22") > 0.0)
      this.axis22Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 22") < 0.0)
      this.axis22Value.set_text("negative");
    else
      this.axis22Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 23") > 0.0)
      this.axis23Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 23") < 0.0)
      this.axis23Value.set_text("negative");
    else
      this.axis23Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 24") > 0.0)
      this.axis24Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 24") < 0.0)
      this.axis24Value.set_text("negative");
    else
      this.axis24Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 25") > 0.0)
      this.axis25Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 25") < 0.0)
      this.axis25Value.set_text("negative");
    else
      this.axis25Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 26") > 0.0)
      this.axis26Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 26") < 0.0)
      this.axis26Value.set_text("negative");
    else
      this.axis26Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 27") > 0.0)
      this.axis27Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 27") < 0.0)
      this.axis27Value.set_text("negative");
    else
      this.axis27Value.set_text(string.Empty);
    if ((double) Input.GetAxis("Axis 28") > 0.0)
      this.axis28Value.set_text("positive");
    else if ((double) Input.GetAxis("Axis 28") < 0.0)
      this.axis28Value.set_text("negative");
    else
      this.axis28Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 330))
      this.button0Value.set_text("pressed");
    else
      this.button0Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 331))
      this.button1Value.set_text("pressed");
    else
      this.button1Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 332))
      this.button2Value.set_text("pressed");
    else
      this.button2Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 333))
      this.button3Value.set_text("pressed");
    else
      this.button3Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 334))
      this.button4Value.set_text("pressed");
    else
      this.button4Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 335))
      this.button5Value.set_text("pressed");
    else
      this.button5Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 336))
      this.button6Value.set_text("pressed");
    else
      this.button6Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 337))
      this.button7Value.set_text("pressed");
    else
      this.button7Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 338))
      this.button8Value.set_text("pressed");
    else
      this.button8Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 339))
      this.button9Value.set_text("pressed");
    else
      this.button9Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 340))
      this.button10Value.set_text("pressed");
    else
      this.button10Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 341))
      this.button11Value.set_text("pressed");
    else
      this.button11Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 342))
      this.button12Value.set_text("pressed");
    else
      this.button12Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 343))
      this.button13Value.set_text("pressed");
    else
      this.button13Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 344))
      this.button14Value.set_text("pressed");
    else
      this.button14Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 345))
      this.button15Value.set_text("pressed");
    else
      this.button15Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 346))
      this.button16Value.set_text("pressed");
    else
      this.button16Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 347))
      this.button17Value.set_text("pressed");
    else
      this.button17Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 348))
      this.button18Value.set_text("pressed");
    else
      this.button18Value.set_text(string.Empty);
    if (Input.GetKey((KeyCode) 349))
      this.button19Value.set_text("pressed");
    else
      this.button19Value.set_text(string.Empty);
  }
}
