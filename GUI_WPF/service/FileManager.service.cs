using Data.model;
using GUI.model;
using GUI.utils;
using MD_Networking;
using MD_Store.Lib;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace GUI_WPF.service
{
    public class FileManagerService
    {
        public FileManagerService() {}

        /** Bind it to file manager button click */
        public async Task _loadFileContents(FileManagerModel model)
        {
            try
            {
                string? registryValue = RegistryConfig.Init().GetConfigValue("userEmail");
                if (string.IsNullOrEmpty(registryValue))
                {
                    throw new Exception("Whooops! The userEmail property is not found!");
                }

                string _email = registryValue;

                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
                {
                    APIResponse response = await client.PostAsync<GetFilesDto>("connector/list-files", new()
                    {
                        email = _email
                    });

                    /* Convert the embedded data object to json serialized string */
                    System.Net.Http.StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(response.Data), Encoding.UTF8,
                        "application/json");

                    IEnumerable<FileTableDataModel>? files = System.Text.Json.JsonSerializer
                        .Deserialize<IEnumerable<FileTableDataModel>>(await jsonContent.ReadAsStringAsync());

                    model.TableContents.Clear();

                    /* Load file contents into the table */
                    foreach (var file in files)
                    {
                        model.TableContents.Add(new()
                        {
                            fileName = file.fileName,
                            owner = file.owner,
                            sizeInBytes = file.sizeInBytes,
                            date = file.date,
                            privilege = file.privilege
                        });
                    }

                    for (int i = 0; i < (model.TableContents.Count < 30 ? (30-model.TableContents.Count) : 0); i++)
                    {
                        model.TableContents.Add(new()
                        {
                            fileName = "",
                            owner = "",
                            sizeInBytes = 00,
                            date = "",
                            privilege = ""
                        });
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Something went wrong during loading file contents, more info: " + ex.Message);
            }
        }

        public async Task downloadFile(FileManagerModel model)
        {
            var selectedItem = model.SelectedElement;

            if (selectedItem is null)
            {
                MessageBox.Show("Whooops! You have to select a line before remove a file :)");
                return;
            }

            /* Send download request to the API */
            try
            {
                string? registryValue = RegistryConfig.Init().GetConfigValue("userEmail");
                if (string.IsNullOrEmpty(registryValue))
                {
                    throw new Exception("Whooops! The userEmail property is not found!");
                }

                string _email = registryValue;


                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
                {
                    APIResponse response = await client.GetAsync<APIResponse>($"connector/download-file?fileName={selectedItem.fileName}&email={_email}");

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
                    saveFileDialog.FileName = selectedItem.fileName.ToString();

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        /* Save the file to the specified path */
                        using (var fileStream = File.Create(saveFileDialog.FileName))
                        {

                            await fileStream.WriteAsync(dataAsByteArray.data, 0, dataAsByteArray.data.Length);
                            MessageBox.Show("The file successfully downloaded!");

                            /* Refresh data table */
                            await this._loadFileContents(model);
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

        public async Task removeFile(FileManagerModel model)
        {
            var selectedItem = model.SelectedElement;

            if (selectedItem is null || selectedItem.isNull())
            {
                MessageBox.Show("Whooops! You have to select a line before remove a file :)");
                return;
            }

            /* Send delete request to the API */
            try
            {
                string? registryValue = RegistryConfig.Init().GetConfigValue("userEmail");
                if (string.IsNullOrEmpty(registryValue))
                {
                    throw new Exception("Whooops! The userEmail property is not found!");
                }

                string _email = registryValue;

                /* Get files from the API */
                using (NetworkClient client = new NetworkClient("http://localhost:9998/"))
                {
                    APIResponse response = await client.PostAsync<RemoveFileDto>("connector/remove-file", new()
                    {
                        email = _email,
                        fileName = selectedItem.fileName ?? "-1"
                    });

                    if (!(response.StatusCode >= 200 && response.StatusCode < 300))
                    {
                        MessageBox.Show("Something went wrong during processing your request, more info: " + response.Message);
                        return;
                    }

                    /* Refresh data table */
                    Int32 elementIndex = -1;
                    Int32 i = 0;
                    foreach (var element in model.TableContents)
                    {
                        if (element.fileName.ToLower().Equals(selectedItem.fileName.ToLower()))
                        {
                            elementIndex = i;
                            break;
                        }
                        i++;
                    }

                    MessageBox.Show($"File successfully deleted!");

                    /* Refresh data table */
                    await this._loadFileContents(model);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong during deleting your file, more info: " + ex.Message);
            }
        }

        public async Task addFile(FileManagerModel data)
        {
            try
            {
                string? registryValue = RegistryConfig.Init().GetConfigValue("userEmail");
                if (string.IsNullOrEmpty(registryValue))
                {
                    throw new Exception("Whooops! The userEmail property is not found!");
                }

                string _email = registryValue;

                /* Choose a file and process it */
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.Multiselect = false;

                if (dialog.ShowDialog() == true)
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
                        var content = new MultipartFormDataContent("----WebKitFormBoundary7MA4YWxkTrZu0gW");

                        /* Text fields */
                        content.Add(new StringContent(_email), "email");
                        content.Add(new StringContent(dialog.FileName), "fileName");

                        // Convert StreamContent to byte array
                        var fileBytes = await File.ReadAllBytesAsync(path);

                        // Add byte array to FormData
                        var fileContent = new ByteArrayContent(fileBytes);
                        content.Add(fileContent, "fileContent", dialog.FileName);
                        var response = await client.PostAsync("http://localhost:9998/connector/upload-file",
                            content);

                        /* Response is okay */
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("File successfully uploaded!");

                            /* Refresh data table */
                            await this._loadFileContents(data);
                            return;
                        }

                        /* Response is not okay */
                        MessageBox.Show($"Something went wrong during uploading your file, more info: {response.ReasonPhrase}");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong during uploading your file, more info: " + ex.Message);
            }
        }
    }
}
