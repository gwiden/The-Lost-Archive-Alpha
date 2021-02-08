// (c) Copyright HutongGames. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Audio)]
    [Tooltip("Plays a Random Audio Clip at a position defined by a Game Object or a Vector3. If a position is defined, it takes priority over the game object. You can set the relative weight of the clips to control how often they are selected.")]
    public class AudioPlayRandom : FsmStateAction
    {
        [Tooltip("The GameObject to play the sound.")]
        public FsmOwnerDefault gameObject;

        [CompoundArray("Audio Clips", "Audio Clip", "Weight")]
        [ObjectType(typeof(AudioClip))]
        public FsmObject[] audioClips;
        [HasFloatSlider(0, 1)]
        public FsmFloat[] weights;
        [HasFloatSlider(0, 1)]
        public FsmFloat volume = 1f;

        [Tooltip("Event to send when the action finishes.")]
        public FsmEvent finishedEvent;

        [Tooltip("Wait until the end of the clip to send the Finish Event. Set to false to send the finish event immediately.")]
        public FsmBool WaitForEndOfClip;

        private int randomIndex;
        private int lastIndex = -1;

        private AudioSource audio;

        public override void Reset()
        {
            gameObject = null;
            audioClips = new FsmObject[3];
            weights = new FsmFloat[] { 1, 1, 1 };
            volume = 1;
            finishedEvent = null;
            WaitForEndOfClip = true;
        }

        public override void OnEnter()
        {
            DoPlayRandomClip();
        }

        void DoPlayRandomClip()
        {
            if (audioClips.Length == 0) return;

            if (weights.Length == 1)
            {
                randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);
            }
            else
            {
                do
                {
                    randomIndex = ActionHelpers.GetRandomWeightedIndex(weights);
                } while (randomIndex == lastIndex && randomIndex != -1);

                lastIndex = randomIndex;
            }

            if (randomIndex != -1)
            {
                var clip = audioClips[randomIndex].Value as AudioClip;
                if (clip != null)
                {
                    var go = Fsm.GetOwnerDefaultTarget(gameObject);
                    if (go != null)
                    {
                        // cache the AudioSource component

                        audio = go.GetComponent<AudioSource>();
                        if (audio != null)
                        {

                            if (clip == null)
                            {
                                audio.Play();

                                if (!volume.IsNone)
                                {
                                    audio.volume = volume.Value;
                                }

                                return;
                            }

                            if (!volume.IsNone)
                            {
                                audio.PlayOneShot(clip, volume.Value);
                            }
                            else
                            {
                                audio.PlayOneShot(clip);
                            }
                            if (WaitForEndOfClip.Value == false)
                            {
                                Fsm.Event(finishedEvent);
                                Finish();
                            }

                            return;
                        }
                    }
                }
            }
        }
        public override void OnUpdate()
        {
            if (audio == null)
            {
                Finish();
            }
            else
            {
                if (!audio.isPlaying)
                {
                    Fsm.Event(finishedEvent);
                    Finish();
                }
                else if (!volume.IsNone && volume.Value != audio.volume)
                {
                    audio.volume = volume.Value;
                }
            }
        }

#if UNITY_EDITOR
        public override string AutoName()
        {
            if (audioClips[randomIndex].Value != null && !audioClips[randomIndex].IsNone)
            {
                return ActionHelpers.AutoName(this, audioClips[randomIndex]);
            }

            return null;
        }
#endif
    }
}