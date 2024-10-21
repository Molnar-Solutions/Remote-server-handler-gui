using GUI.model;
using GUI.utils;
using LibraryGUI.Lib;
using MD_Networking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.service
{
    public enum LabelType
    {
        OS_TYPE,
        ARCHITECTURE,
        CPU_USAGE,
        OS_AVAILABLE_MEMORY,
        OS_TOTAL_MEMORY,
        OS_AVAILABLE_STORAGE
    }

    public class SystemHealth
    {
        private DataGridView _dataGridViewRef;
        private string _email;

        private TextBox _systemOsTypeRef;
        private TextBox _systemOsArchitectureRef;
        private TextBox _systemCPUUsageRef;
        private TextBox _systemOsAvailableMemoryRef;
        private TextBox _systemOsTotalMemoryRef;
        private TextBox _systemOsAvailableStorageRef;

        public SystemHealth(ref DataGridView dataGridView, string email)
        {
            this._dataGridViewRef = dataGridView;
            this._email = email;
        }

        public void SetTextBoxRef(ref TextBox label, LabelType type)
        {
            switch (type)
            {
                case LabelType.OS_TYPE:
                    _systemOsTypeRef = label;
                    break;

                case LabelType.ARCHITECTURE:
                    _systemOsArchitectureRef = label;
                    break;

                case LabelType.CPU_USAGE:
                    _systemCPUUsageRef = label;
                    break;

                case LabelType.OS_AVAILABLE_MEMORY:
                    _systemOsAvailableMemoryRef = label;
                    break;

                case LabelType.OS_TOTAL_MEMORY:
                    _systemOsTotalMemoryRef = label;
                    break;

                case LabelType.OS_AVAILABLE_STORAGE:
                    _systemOsAvailableStorageRef = label;
                    break;
            }
        }

        public TextBox GetTextBoxRef(LabelType type)
        {
            switch (type)
            {
                case LabelType.OS_TYPE:
                    return _systemOsArchitectureRef;

                case LabelType.ARCHITECTURE:
                    return _systemOsArchitectureRef;

                case LabelType.CPU_USAGE:
                    return _systemCPUUsageRef;

                case LabelType.OS_AVAILABLE_MEMORY:
                    return _systemOsAvailableMemoryRef;

                case LabelType.OS_TOTAL_MEMORY:
                    return _systemOsTotalMemoryRef;

                case LabelType.OS_AVAILABLE_STORAGE:
                    return _systemOsAvailableStorageRef;

                default: return null;
            }
        }

        public async void _loadSystemInformations()
        {
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
                {
                    APIResponse response = await client.PostAsync<SystemHealthRequest>("connector/system-health", new()
                    {
                        email = _email
                    });

                    /* Convert response data to SysteamHealthResponse object */
                    SysteamHealthResponse responseData = JsonConvert.DeserializeObject<SysteamHealthResponse>(response.Data.ToString());

                    /* Clear previous items from the table */
                    _dataGridViewRef.Rows.Clear();

                    /* Setup fields */
                    _systemOsTypeRef.Text = responseData.osType;
                    _systemOsTotalMemoryRef.Text = responseData.totalMemory + "";
                    _systemOsAvailableStorageRef.Text = responseData.availableStorage + "";
                    _systemOsAvailableMemoryRef.Text = responseData.availableMemory + "";
                    _systemCPUUsageRef.Text = (responseData.cpuUsage * 100 ) + "";

                    /* Load file contents into the table */
                    foreach (var log in responseData.logs)
                    {
                        _dataGridViewRef.Rows.Add(log);
                    }

                    if (_dataGridViewRef.Rows.Count < 45)
                    {
                        for (short i = 0; i < (45 - (short)_dataGridViewRef.Rows.Count); i++)
                        {
                            _dataGridViewRef.Rows.Add("");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
