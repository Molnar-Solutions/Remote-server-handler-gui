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

namespace GUI.service
{
    public class FileManager
    {
        public FileManager(ref System.Data.DataTable table, string userName)
        {
            this._loadFileContents(table, userName);
        }

        private async void _loadFileContents(System.Data.DataTable dataTable, string userName)
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

                    dataTable.Columns.Add("privilege", typeof(string));
                    dataTable.Columns.Add("owner", typeof(string));
                    dataTable.Columns.Add("group", typeof(string));
                    dataTable.Columns.Add("sizeInBytes", typeof(int));
                    dataTable.Columns.Add("date", typeof(string));
                    dataTable.Columns.Add("fileName", typeof(string));

                    /* Load file contents into the table */
                    foreach (var file in files)
                    {
                        dataTable.Rows.Add(file.privilege, file.owner, file.group, file.sizeInBytes, file.date, file.fileName);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void addFile(string userName, DataTable dataTable)
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


                    /* Refresh data tabel */
                    _loadFileContents(dataTable, ApplicationStateManager.getInstance().getState().userData.name);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
