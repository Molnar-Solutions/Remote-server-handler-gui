using Data.model;
using GUI.model;
using GUI.utils;
using MD_Networking;
using MD_Store.Lib;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
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
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
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
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
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

            if (selectedItem is null)
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
                using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
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
                    model.TableContents.RemoveAll(x => x.fileName.ToLower() == selectedItem.fileName.ToLower());
                    MessageBox.Show($"File successfully deleted!");
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
                        var formData = new MultipartFormDataContent();

                        // Convert StreamContent to byte array
                        var fileBytes = await File.ReadAllBytesAsync(path);

                        // Add byte array to FormData
                        formData.Add(new ByteArrayContent(fileBytes), "fileContent", dialog.FileName);

                        var response = await client.PostAsync("http://localhost:3000/connector/upload-file", formData);

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

                        /* Refresh data table */
                        data.TableContents.Add(new()
                        {
                            date = DateTime.Now.ToLongDateString(),
                            fileName = dialog.FileName,
                            owner = _email,
                            privilege = $"{324552}",
                            sizeInBytes = fileBytes.Length
                        });
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
