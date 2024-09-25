namespace GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            loggedInDateLabel.Text = "";
            loggedInUserLabel.Text = "Anonymus";
            loggedInUserLabel.ForeColor = Color.Red;

            isConnectedLabel.Text = "Not connected!";
            isConnectedLabel.ForeColor = Color.Red;

            logOutButton.Enabled = false;


        }

        private void addFile_Click(object sender, EventArgs e)
        {

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
    }
}
