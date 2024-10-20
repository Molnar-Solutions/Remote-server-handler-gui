using GUI.model;
using GUI.utils;
using LibraryGUI.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace GUI.service
{
    public class FileManager
    {
        private DataGridView _dataGridViewRef;

        public FileManager(ref DataGridView dataGridView, string userName)
        {
            dataGridView.Rows.Clear();
            this._loadFileContents(dataGridView, userName);
            this._dataGridViewRef = dataGridView;
        }

        private async void _loadFileContents(DataGridView dataGridView, string userName)
        {
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
                {
                    APIResponse response = await client.PostAsync<GetFilesDto>("connector/list-files", new()
                    {
                        userName = userName
                    });

                    /* Convert the embedded data object to json serialized string */
                    StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(response.Data), Encoding.UTF8,
                        "application/json");

                    IEnumerable<FileTableDataModel> files = System.Text.Json.JsonSerializer
                        .Deserialize<IEnumerable<FileTableDataModel>>(await jsonContent.ReadAsStringAsync());

                    /* dataTable.Columns.Add("fileName", typeof(string));
                    dataTable.Columns.Add("owner", typeof(string));
                    dataTable.Columns.Add("sizeInBytes", typeof(int));
                    dataTable.Columns.Add("date", typeof(string));
                    dataTable.Columns.Add("privilege", typeof(string)); */

                    /* Clear previous items from the table */

                    /* Load file contents into the table */
                    foreach (var file in files)
                    {
                        dataGridView.Rows.Add(file.fileName, file.owner,
                            file.sizeInBytes, file.date, file.privilege);
                    }

                    if (dataGridView.Rows.Count < 45)
                    {
                        for (short i = 0; i < (45 - (short)dataGridView.Rows.Count); i++)
                        {
                            dataGridView.Rows.Add("", "", "", "", "");
                        }
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void downloadFile(string userName)
        {
            var currentlySelectedRows = _dataGridViewRef.SelectedRows;

            if (currentlySelectedRows is null || currentlySelectedRows.Count == 0)
            {
                MessageBox.Show("Hiba! Egy sort sem jelöltél ki! Fontos, hogy több sor kijelölése esetén az elsőt értelmezi a program!");
                return;
            }
            var firstRowData = currentlySelectedRows[0];

            var fileName = firstRowData.Cells[0].Value ?? "-1";
            var owner = firstRowData.Cells[1].Value ?? "-1";
            var sizeInBytes = firstRowData.Cells[2].Value ?? -1;
            var date = firstRowData.Cells[3].Value ?? "-1";
            var privilege = firstRowData.Cells[4].Value ?? "-1";

            /* Send download request to the API */
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
                {
                    APIResponse response = await client.GetAsync<APIResponse>($"connector/download-file?fileName={fileName}");

                    if (!(response.StatusCode >= 200 && response.StatusCode < 300))
                    {
                        MessageBox.Show($"Hiba a kérés feldolgozása közben! {response.Message}");
                        return;
                    }

                    FileDownloadResponse dataAsByteArray = JsonConvert.DeserializeObject<FileDownloadResponse>(response.Data.ToString());

                    /* Create a save file dialog */
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "All Files (*.*)|*.*";
                    saveFileDialog.Title = "Save File";
                    saveFileDialog.FileName = fileName.ToString();

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        /* Save the file to the specified path */
                        using (var fileStream = File.Create(saveFileDialog.FileName))
                        {

                            await fileStream.WriteAsync(dataAsByteArray.data, 0, dataAsByteArray.data.Length);
                            MessageBox.Show("Sikeres fájl letöltés!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Fájl letöltés megszakítva");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void removeFile(string userName)
        {
            var currentlySelectedRows = _dataGridViewRef.SelectedRows;

            if (currentlySelectedRows is null || currentlySelectedRows.Count == 0)
            {
                MessageBox.Show("Hiba! Egy sort sem jelöltél ki! Fontos, hogy több sor kijelölése esetén az elsőt értelmezi a program!");
                return;
            }
            var firstRowData = currentlySelectedRows[0];

            var fileName = firstRowData.Cells[0].Value ?? "-1";
            var owner = firstRowData.Cells[1].Value ?? "-1";
            var sizeInBytes = firstRowData.Cells[2].Value ?? -1;
            var date = firstRowData.Cells[3].Value ?? "-1";
            var privilege = firstRowData.Cells[4].Value ?? "-1";

            /* Send delete request to the API */
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
                {
                    APIResponse response = await client.PostAsync<RemoveFileDto>("connector/remove-file", new()
                    {
                        userName = userName,
                        fileName = fileName.ToString() ?? "-1"
                    });

                    if (!(response.StatusCode >= 200 && response.StatusCode < 300))
                    {
                        MessageBox.Show($"Hiba a kérés feldolgozása közben! {response.Message}");
                        return;
                    }

                    /* Refresh data table */
                    _loadFileContents(_dataGridViewRef, ApplicationStateManager.getInstance().getState().userData.name);
                    MessageBox.Show($"Sikeres törlés!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void addFile(string userName, DataGridView dataGridView)
        {
            try
            {
                /* Choose a file and process it */
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.Multiselect = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    String path = dialog.FileName;

                    /* Send post request to upload the file */
                    CreateFileDto createFileDto = new CreateFileDto()
                    {
                        userName = userName,
                        fileName = dialog.FileName,
                    };

                    using (HttpClient client = new HttpClient())
                    {
                        var formData = new MultipartFormDataContent();

                        // Convert StreamContent to byte array
                        var fileBytes = await File.ReadAllBytesAsync(path);

                        // Add byte array to FormData
                        formData.Add(new ByteArrayContent(fileBytes), "fileContent", dialog.FileName);

                        var response = await client.PostAsync("http://localhost:3000/connector/upload-file", formData);

                        /* Response is okay */
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Sikeres fájl feltöltés!");
                            return;
                        }

                        /* Response is not okay */
                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Hiba a fájl feltöltése közben! {response.ReasonPhrase}");
                            return;
                        }
                    }


                    /* Refresh data table */
                    _loadFileContents(dataGridView, ApplicationStateManager.getInstance().getState().userData.name);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
