using GUI.model;
using GUI.utils;
using MD_Networking;
using System.Text;
using Newtonsoft.Json;

namespace GUI.service
{
    public class FileManager
    {
        private DataGridView? _dataGridViewRef;
        private string? _email;

        public FileManager(ref DataGridView dataGridView, string email)
        {
            dataGridView.Rows.Clear();

            this._dataGridViewRef = dataGridView;
            this._email = email;

            this._loadFileContents(dataGridView);
        }

        private async Task _loadFileContents(DataGridView dataGridView)
        {
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
                {
                    APIResponse response = await client.PostAsync<GetFilesDto>("connector/list-files", new()
                    {
                        email = _email
                    });

                    /* Convert the embedded data object to json serialized string */
                    StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(response.Data), Encoding.UTF8,
                        "application/json");

                    IEnumerable<FileTableDataModel>? files = System.Text.Json.JsonSerializer
                        .Deserialize<IEnumerable<FileTableDataModel>>(await jsonContent.ReadAsStringAsync());

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
                MessageBox.Show("Something went wrong during loading file contents, more info: " + ex.Message);
            }
        }

        public async Task downloadFile()
        {
            var currentlySelectedRows = _dataGridViewRef.SelectedRows;

            if (currentlySelectedRows is null || currentlySelectedRows.Count == 0)
            {
                MessageBox.Show("Whooops! You have to select a line before download a file :)");
                return;
            }
            var firstRowData = currentlySelectedRows[0];

            var fileName = firstRowData.Cells[0].Value ?? "-1";

            /* Send download request to the API */
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
                {
                    APIResponse response = await client.GetAsync<APIResponse>($"connector/download-file?fileName={fileName}&email={_email}");

                    if (!(response.StatusCode >= 200 && response.StatusCode < 300))
                    {
                        MessageBox.Show("Something went wrong during processing your request, more info: " + response.Message);
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
                            MessageBox.Show("The file successfully downloaded!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("File upload cancelled!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong during downloading your file, more info: " + ex.Message);
            }
        }

        public async Task removeFile()
        {
            var currentlySelectedRows = _dataGridViewRef.SelectedRows;

            if (currentlySelectedRows is null || currentlySelectedRows.Count == 0)
            {
                MessageBox.Show("Whooops! You have to select a line before remove a file :)");
                return;
            }
            var firstRowData = currentlySelectedRows[0];

            var fileName = firstRowData.Cells[0].Value ?? "-1";

            /* Send delete request to the API */
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
                {
                    APIResponse response = await client.PostAsync<RemoveFileDto>("connector/remove-file", new()
                    {
                        email = _email,
                        fileName = fileName.ToString() ?? "-1"
                    });

                    if (!(response.StatusCode >= 200 && response.StatusCode < 300))
                    {
                        MessageBox.Show("Something went wrong during processing your request, more info: " + response.Message);
                        return;
                    }

                    /* Refresh data table */
                    await _loadFileContents(_dataGridViewRef);
                    MessageBox.Show($"File successfully deleted!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong during deleting your file, more info: " + ex.Message);
            }
        }

        public async Task addFile(DataGridView dataGridView)
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
                        email = _email,
                        fileName = dialog.FileName,
                    };

                    using (HttpClient client = new HttpClient())
                    {
                        var formData = new MultipartFormDataContent();

                        // Convert StreamContent to byte array
                        var fileBytes = await File.ReadAllBytesAsync(path);

                        // Add byte array to FormData
                        formData.Add(new ByteArrayContent(fileBytes), "fileContent", dialog.FileName);
                        formData.Add(new StringContent(dialog.FileName), "fileName");

                        var response = await client.PostAsync("http://localhost:9998/connector/upload-file", formData);

                        /* Response is okay */
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("File successfully uploaded!");
                            return;
                        }

                        /* Response is not okay */
                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Something went wrong during uploading your file, more info: {response.ReasonPhrase}");
                            return;
                        }
                    }


                    /* Refresh data table */
                    await _loadFileContents(dataGridView);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong during uploading your file, more info: " + ex.Message);
            }
        }
    }
}
