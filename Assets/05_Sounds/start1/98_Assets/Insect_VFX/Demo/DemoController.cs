using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Insect_VFX;

namespace Insect_VFX_DEMO
{

    public class DemoController : MonoBehaviour
    {

        [SerializeField] Transform camTransform;
        [SerializeField] Transform cameraPositionsParent;
        [SerializeField] InsectEmitter[] emmiters;

        [SerializeField] Button button_Next;
        [SerializeField] TextMeshProUGUI text_numEntities;
        [SerializeField] Slider slider_numEntities;

        int currentSectionIndex = 0;
        int[] loopIndex = { 1 };

        void Awake()
        {
            button_Next.onClick.AddListener(NextSection);
            slider_numEntities.onValueChanged.AddListener((float v) => { SetNumberOfEntities((int)v); });
        }

        private void Start()
        {
            SetSection(currentSectionIndex);
        }

        #region PUBLIC METHODS
        public void StartSim()
        {
            if (loopIndex.Contains(currentSectionIndex))
                emmiters[currentSectionIndex].StartLoopSimulation();
            else
                emmiters[currentSectionIndex].StartSimulation();
        }

        public void EndSim()
        {
            emmiters[currentSectionIndex].EndSimulation();
        }

        public void SetNumberOfEntities(int value)
        {
            emmiters[currentSectionIndex].SetNumberOfEmissions(value);
            text_numEntities.text = value.ToString();

        }
        #endregion

        public void NextSection()
        {
            emmiters[currentSectionIndex].EndSimulation();

            currentSectionIndex++;
            currentSectionIndex %= 3;

            SetSection(currentSectionIndex);
        }

        void SetSection(int index)
        {
            SetCamPos(index);
            slider_numEntities.value = emmiters[currentSectionIndex].numberOfEmissions;
        }

        void SetCamPos(int index)
        {
            camTransform.position = cameraPositionsParent.GetChild(index).position;
            camTransform.rotation = cameraPositionsParent.GetChild(index).rotation;
        }

        private void OnGUI()
        {
            // Show FPS
            float fps = 1.0f / Time.deltaTime;
            string text = $"FPS: {fps:0.}";
            GUI.Label(new Rect(10, 10, 10, 10), text);
        }

    }
}