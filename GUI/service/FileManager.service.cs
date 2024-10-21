using GUI.model;
using GUI.utils;
using MD_Networking;
using System.Text;
using Newtonsoft.Json;

namespace GUI.service
{
    public class FileManager
    {
        private DataGridView _dataGridViewRef;
        private string _email;

        public FileManager(ref DataGridView dataGridView, string email)
        {
            dataGridView.Rows.Clear();

            this._dataGridViewRef = dataGridView;
            this._email = email;

            this._loadFileContents(dataGridView);
        }

        private async void _loadFileContents(DataGridView dataGridView)
        {
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
                {
                    APIResponse response = await client.PostAsync<GetFilesDto>("connector/list-files", new()
                    {
                        email = _email
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

        public async void downloadFile()
        {
            var currentlySelectedRows = _dataGridViewRef.SelectedRows;

            if (currentlySelectedRows is null || currentlySelectedRows.Count == 0)
            {
                MessageBox.Show("Hiba! Egy sort sem jelöltél ki! Fontos, hogy több sor kijelölése esetén az elsőt értelmezi a program!");
                return;
            }
            var firstRowData = currentlySelectedRows[0];

            var fileName = firstRowData.Cells[0].Value ?? "-1";

            /* Send download request to the API */
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
                {
                    APIResponse response = await client.GetAsync<APIResponse>($"connector/download-file?fileName={fileName}&email={_email}");

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

        public async void removeFile()
        {
            var currentlySelectedRows = _dataGridViewRef.SelectedRows;

            if (currentlySelectedRows is null || currentlySelectedRows.Count == 0)
            {
                MessageBox.Show("Hiba! Egy sort sem jelöltél ki! Fontos, hogy több sor kijelölése esetén az elsőt értelmezi a program!");
                return;
            }
            var firstRowData = currentlySelectedRows[0];

            var fileName = firstRowData.Cells[0].Value ?? "-1";

            /* Send delete request to the API */
            try
            {
                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
                {
                    APIResponse response = await client.PostAsync<RemoveFileDto>("connector/remove-file", new()
                    {
                        email = _email,
                        fileName = fileName.ToString() ?? "-1"
                    });

                    if (!(response.StatusCode >= 200 && response.StatusCode < 300))
                    {
                        MessageBox.Show($"Hiba a kérés feldolgozása közben! {response.Message}");
                        return;
                    }

                    /* Refresh data table */
                    _loadFileContents(_dataGridViewRef);
                    MessageBox.Show($"Sikeres törlés!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void addFile(DataGridView dataGridView)
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
                    _loadFileContents(dataGridView);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
