using UnityEngine;
using UnityEngine.Rendering;
using Insect_VFX;

namespace Insect_VFX
{
    [RequireComponent(typeof(InsectEmitter))]
    public class InsectAnimation : MonoBehaviour
    {
        public string key;

        InsectEmitter emitter;

        private void Awake()
        {
            emitter = GetComponent<InsectEmitter>();
            emitter.OnEmittedObjectPaused += SetKeyPaused;
            emitter.OnEmittedObjectResume += SetKeyResume;
        }

        void SetKeyPaused(int index)
        {
            emitter.GetEntityByIndex(index).GetComponent<Animator>().SetBool(key, false);
        }

        void SetKeyResume(int index)
        {
            emitter.GetEntityByIndex(index).GetComponent<Animator>().SetBool(key, true);
        }

    }
}