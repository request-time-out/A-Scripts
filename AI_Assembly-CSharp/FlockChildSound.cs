// Decompiled with JetBrains decompiler
// Type: FlockChildSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class FlockChildSound : MonoBehaviour
{
  public AudioClip[] _idleSounds;
  public float _idleSoundRandomChance;
  public AudioClip[] _flightSounds;
  public float _flightSoundRandomChance;
  public AudioClip[] _scareSounds;
  public float _pitchMin;
  public float _pitchMax;
  public float _volumeMin;
  public float _volumeMax;
  private FlockChild _flockChild;
  private AudioSource _audio;
  private bool _hasLanded;

  public FlockChildSound()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    this._flockChild = (FlockChild) ((Component) this).GetComponent<FlockChild>();
    this._audio = (AudioSource) ((Component) this).GetComponent<AudioSource>();
    this.InvokeRepeating("PlayRandomSound", Random.get_value() + 1f, 1f);
    if (this._scareSounds.Length <= 0)
      return;
    this.InvokeRepeating("ScareSound", 1f, 0.01f);
  }

  public void PlayRandomSound()
  {
    if (!((Component) this).get_gameObject().get_activeInHierarchy())
      return;
    if (!this._audio.get_isPlaying() && this._flightSounds.Length > 0 && ((double) this._flightSoundRandomChance > (double) Random.get_value() && !this._flockChild._landing))
    {
      this._audio.set_clip(this._flightSounds[Random.Range(0, this._flightSounds.Length)]);
      this._audio.set_pitch(Random.Range(this._pitchMin, this._pitchMax));
      this._audio.set_volume(Random.Range(this._volumeMin, this._volumeMax));
      this._audio.Play();
    }
    else
    {
      if (this._audio.get_isPlaying() || this._idleSounds.Length <= 0 || ((double) this._idleSoundRandomChance <= (double) Random.get_value() || !this._flockChild._landing))
        return;
      this._audio.set_clip(this._idleSounds[Random.Range(0, this._idleSounds.Length)]);
      this._audio.set_pitch(Random.Range(this._pitchMin, this._pitchMax));
      this._audio.set_volume(Random.Range(this._volumeMin, this._volumeMax));
      this._audio.Play();
      this._hasLanded = true;
    }
  }

  public void ScareSound()
  {
    if (!((Component) this).get_gameObject().get_activeInHierarchy() || !this._hasLanded || (this._flockChild._landing || (double) this._idleSoundRandomChance * 2.0 <= (double) Random.get_value()))
      return;
    this._audio.set_clip(this._scareSounds[Random.Range(0, this._scareSounds.Length)]);
    this._audio.set_volume(Random.Range(this._volumeMin, this._volumeMax));
    this._audio.PlayDelayed(Random.get_value() * 0.2f);
    this._hasLanded = false;
  }
}
