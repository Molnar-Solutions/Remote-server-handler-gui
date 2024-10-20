using GUI.model;
using GUI.service;
using GUI.utils;
using LibraryGUI.Lib;
using LogCommon;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace GUI
{
    public partial class MainForm : Form
    {
        private FileManager _fileManager;

        public MainForm()
        {
            InitializeComponent();
            loggedInDateLabel.Text = "";
            loggedInUserLabel.Text = "Anonymus";
            loggedInUserLabel.ForeColor = Color.Red;

            isConnectedLabel.Text = "Not connected!";
            isConnectedLabel.ForeColor = Color.Red;

            logOutButton.Enabled = false;
            fileManagerPanel.Hide();
            consolePanel.Hide();
            systemHealthPanel.Hide();

            apiUrlTextBox.Text = "http://localhost:3000/user/sign-in";
            apiUsernameTextBox.Text = "test@gmail.com";
            apiPasswordTextBox.Text = "asd123";
        }

        private void addFile_Click(object sender, EventArgs e)
        {
            /* User is logged in or not */
            if (ApplicationStateManager.getInstance().getState().userData is null)
            {
                return;
            }

            _fileManager.addFile(ApplicationStateManager.getInstance().getState().userData.name, filesDataTable);
        }

        private void fileManagerPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void fileManagerButton_Click(object sender, EventArgs e)
        {
            consolePanel.Hide();
            systemHealthPanel.Hide();
            fileManagerPanel.Show();

            /* Update file table contents */
            refreshFileContents_Click(sender, e);
        }

        private void systemHealthButton_Click(object sender, EventArgs e)
        {
            fileManagerPanel.Hide();
            consolePanel.Hide();
            systemHealthPanel.Show();
        }

        private void consoleButton_Click(object sender, EventArgs e)
        {
            fileManagerPanel.Hide();
            systemHealthPanel.Hide();
            consolePanel.Show();
        }

        private async void signInButton_Click(object sender, EventArgs e)
        {
            /* Sign in action */
            if (System.String.IsNullOrEmpty(apiUrlTextBox.Text))
            {
                MessageBox.Show("Hiba a(z) 'API' mező nem lehet üres!", "", MessageBoxButtons.OK);
                return;
            }

            if (System.String.IsNullOrEmpty(apiUsernameTextBox.Text))
            {
                MessageBox.Show("Hiba a(z) 'felhasználó név' mező nem lehet üres!", "", MessageBoxButtons.OK);
                return;
            }

            if (System.String.IsNullOrEmpty(apiPasswordTextBox.Text))
            {
                MessageBox.Show("Hiba a(z) 'jelszó' mező nem lehet üres!", "", MessageBoxButtons.OK);
                return;
            }

            /* Get application state */
            var applicationStateRef = ApplicationStateManager.getInstance();

            /* Authentication */
            using (NetworkClient client = new NetworkClient("http://localhost:3000/"))
            {
                APIResponse userResponse = await client.PostAsync<SignInDto>("user/sign-in", new()
                {
                    email = apiUsernameTextBox.Text,
                    password = apiPasswordTextBox.Text
                });

                StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(userResponse.Data), Encoding.UTF8, "application/json");
                UserDataResponse userData = System.Text.Json.JsonSerializer.Deserialize<UserDataResponse>(await jsonContent.ReadAsStringAsync());
                applicationStateRef.getState().userData = userData;

                if (userData is null)
                {
                    MessageBox.Show("Hiba! Nem sikerült betölteni a felhasználói adatokat!");
                    return;
                }

                /* Change dependent items */
                logOutButton.Enabled = true;
                loggedInDateLabel.Text = DateTime.Now.ToShortDateString();
                loggedInDateLabel.ForeColor = Color.Green;

                loggedInUserLabel.Text = userData.name;
                loggedInUserLabel.ForeColor = Color.Green;

                isConnectedLabel.Text = "Connected!";
                isConnectedLabel.ForeColor = Color.Green;
            }

            /* Make visible the file manager panel */
            fileManagerPanel.Show();

            /* Setup files section */
            _fileManager = new FileManager(ref filesDataTable, ApplicationStateManager.getInstance().getState().userData.name);
        }

        private void logOutButton_Click(object sender, EventArgs e)
        {
            /* Sign Out action */
            if (ApplicationStateManager.getInstance().getState().userData is null)
            {
                return;
            }

            /* Change dependent items */
            logOutButton.Enabled = false;
            loggedInDateLabel.Text = "";
            loggedInUserLabel.Text = "Anonymus";
            loggedInUserLabel.ForeColor = Color.Red;

            isConnectedLabel.Text = "Not connected!";
            isConnectedLabel.ForeColor = Color.Red;

            MessageBox.Show("Sikeres kijelentkezés!", "", MessageBoxButtons.OK);

            /* Make panels unvisible & remove refs */
            ApplicationStateManager.getInstance().getState().userData = null;
            consolePanel.Hide();
            systemHealthPanel.Hide();
            fileManagerPanel.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void refreshFileContents_Click(object sender, EventArgs e)
        {
            /* User is logged in or not */
            if (ApplicationStateManager.getInstance().getState().userData is null)
            {
                return;
            }

            /* I do not think it is neccessary but it looks good :) */
            filesDataTable.Hide();
            filesDataTable.Show();

            /* Setup files section */
            _fileManager = new FileManager(ref filesDataTable, ApplicationStateManager.getInstance().getState().userData.name);
        }

        private void removeSelectedFile_Click(object sender, EventArgs e)
        {
            /* User is logged in or not */
            if (ApplicationStateManager.getInstance().getState().userData is null)
            {
                return;
            }

            _fileManager.removeFile(ApplicationStateManager.getInstance().getState().userData.name);
        }

        private void downloadSelectedFile_Click(object sender, EventArgs e)
        {
            /* User is logged in or not */
            if (ApplicationStateManager.getInstance().getState().userData is null)
            {
                return;
            }

            _fileManager.downloadFile(ApplicationStateManager.getInstance().getState().userData.name);
        }
    }
}
