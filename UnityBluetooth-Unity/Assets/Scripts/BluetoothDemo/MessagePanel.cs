using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace JC.BluetoothUnity.Demo
{
    public class MessagePanel : MonoBehaviour
    {
        [SerializeField] Text _messageText;
        [SerializeField] Button _stopButt;
        [SerializeField] Button _clearButt;
        [SerializeField] InputField _sendInput;
        [SerializeField] Button _sendButt;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _stopButt.onClick.AddListener(Stop);
            _clearButt.onClick.AddListener(Clear);
            _sendButt.onClick.AddListener(Send);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            string line = BluetoothManager.ReadLine();
            _messageText.text += line;
        }

        void Stop()
        {
            enabled = false;
            BluetoothManager.Stop();
            FindObjectOfType<PanelController>().ShowPanel(PanelController.PanelType.Search);
        }

        void Clear()
        {
            _messageText.text = "";
        }

        void Send()
        {
            if(BluetoothManager.Send(_sendInput.text))
            {
                // send ok
                _sendInput.text = "";
            }
        }
    }
}