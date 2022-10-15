using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

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
            _clearButt.onClick.AddListener(ClearMessage);
            _sendButt.onClick.AddListener(Send);
            ClearMessage();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            int available = BluetoothManager.Available();
            if(available > 0)
            {
                string line = BluetoothManager.ReadLine();
                PushMessage($"<color=lime>REMOTE</color>: {line}\n");
            }
            else if(available < 0)
            {
                // Error!
                PushMessage("<color=red>CONNECTION LOST</color>\n");
            }
        }

        void Stop()
        {
            enabled = false;
            BluetoothManager.Stop();
            FindObjectOfType<PanelController>().ShowPanel(PanelController.PanelType.Search);
        }        

        void Send()
        {            
            PushMessage($"<color=yellow>ME</color>: {_sendInput.text}");            
            if(BluetoothManager.Send(_sendInput.text + "\r\n"))
            {
                // send ok
                _sendInput.text = "";
            }
            else
            {
                BluetoothManager.Toast("Failed to send message!");
            }
        }

        void PushMessage(string msg)
        {
            _messageText.text += $"{DateTime.Now.ToString("HH:mm:ss")} {msg}\n";
        }
        void ClearMessage()
        {
            _messageText.text = "";
        }
    }
}