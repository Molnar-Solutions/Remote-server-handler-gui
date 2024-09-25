namespace GUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            apiUrlTextBox = new TextBox();
            apiUsernameTextBox = new TextBox();
            apiPasswordTextBox = new TextBox();
            fileManagerButton = new Button();
            filesDataTable = new DataGridView();
            addFile = new Button();
            removeSelectedFile = new Button();
            downloadSelectedFile = new Button();
            fileManagerPanel = new Panel();
            systemHealthButton = new Button();
            systemHealthPanel = new Panel();
            availableStorageTextBox = new TextBox();
            totalMemoryTextBox = new TextBox();
            availableMemoryTextBox = new TextBox();
            cpuUsageTextBox4 = new TextBox();
            architectureTextBox = new TextBox();
            systemOsTypeTextBox = new TextBox();
            systemWarningErrorTable = new DataGridView();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            signInButton = new Button();
            label12 = new Label();
            isConnectedLabel = new Label();
            label13 = new Label();
            loggedInUserLabel = new Label();
            label14 = new Label();
            loggedInDateLabel = new Label();
            applicationLogo = new Button();
            logOutButton = new Button();
            consoleButton = new Button();
            consolePanel = new Panel();
            submitConsoleCommand = new Button();
            consoleCommandTextBox = new TextBox();
            consoleTextBox = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)filesDataTable).BeginInit();
            fileManagerPanel.SuspendLayout();
            systemHealthPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)systemWarningErrorTable).BeginInit();
            consolePanel.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 0;
            label1.Text = "API URL:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(259, 23);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 1;
            label2.Text = "Username:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(517, 26);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 1;
            label3.Text = "Password:";
            // 
            // apiUrlTextBox
            // 
            apiUrlTextBox.Location = new Point(70, 20);
            apiUrlTextBox.Name = "apiUrlTextBox";
            apiUrlTextBox.Size = new Size(183, 23);
            apiUrlTextBox.TabIndex = 2;
            // 
            // apiUsernameTextBox
            // 
            apiUsernameTextBox.Location = new Point(328, 23);
            apiUsernameTextBox.Name = "apiUsernameTextBox";
            apiUsernameTextBox.Size = new Size(183, 23);
            apiUsernameTextBox.TabIndex = 2;
            // 
            // apiPasswordTextBox
            // 
            apiPasswordTextBox.Location = new Point(583, 26);
            apiPasswordTextBox.Name = "apiPasswordTextBox";
            apiPasswordTextBox.PasswordChar = '*';
            apiPasswordTextBox.Size = new Size(183, 23);
            apiPasswordTextBox.TabIndex = 2;
            // 
            // fileManagerButton
            // 
            fileManagerButton.Location = new Point(12, 91);
            fileManagerButton.Name = "fileManagerButton";
            fileManagerButton.Size = new Size(95, 23);
            fileManagerButton.TabIndex = 4;
            fileManagerButton.Text = "File manager";
            fileManagerButton.UseVisualStyleBackColor = true;
            fileManagerButton.Click += fileManagerButton_Click;
            // 
            // filesDataTable
            // 
            filesDataTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            filesDataTable.Location = new Point(137, 3);
            filesDataTable.Name = "filesDataTable";
            filesDataTable.Size = new Size(712, 517);
            filesDataTable.TabIndex = 0;
            // 
            // addFile
            // 
            addFile.Location = new Point(3, 3);
            addFile.Name = "addFile";
            addFile.Size = new Size(128, 23);
            addFile.TabIndex = 5;
            addFile.Text = "[+] Add file";
            addFile.UseVisualStyleBackColor = true;
            addFile.Click += addFile_Click;
            // 
            // removeSelectedFile
            // 
            removeSelectedFile.Location = new Point(3, 32);
            removeSelectedFile.Name = "removeSelectedFile";
            removeSelectedFile.Size = new Size(128, 23);
            removeSelectedFile.TabIndex = 5;
            removeSelectedFile.Text = "[-] Remove file";
            removeSelectedFile.UseVisualStyleBackColor = true;
            // 
            // downloadSelectedFile
            // 
            downloadSelectedFile.Location = new Point(3, 61);
            downloadSelectedFile.Name = "downloadSelectedFile";
            downloadSelectedFile.Size = new Size(128, 23);
            downloadSelectedFile.TabIndex = 5;
            downloadSelectedFile.Text = "[↓] Download file";
            downloadSelectedFile.UseVisualStyleBackColor = true;
            // 
            // fileManagerPanel
            // 
            fileManagerPanel.Controls.Add(addFile);
            fileManagerPanel.Controls.Add(filesDataTable);
            fileManagerPanel.Controls.Add(downloadSelectedFile);
            fileManagerPanel.Controls.Add(removeSelectedFile);
            fileManagerPanel.Location = new Point(122, 91);
            fileManagerPanel.Name = "fileManagerPanel";
            fileManagerPanel.Size = new Size(849, 517);
            fileManagerPanel.TabIndex = 6;
            fileManagerPanel.Paint += fileManagerPanel_Paint;
            // 
            // systemHealthButton
            // 
            systemHealthButton.Location = new Point(12, 123);
            systemHealthButton.Name = "systemHealthButton";
            systemHealthButton.Size = new Size(95, 23);
            systemHealthButton.TabIndex = 4;
            systemHealthButton.Text = "System health";
            systemHealthButton.UseVisualStyleBackColor = true;
            systemHealthButton.Click += systemHealthButton_Click;
            // 
            // systemHealthPanel
            // 
            systemHealthPanel.Controls.Add(availableStorageTextBox);
            systemHealthPanel.Controls.Add(totalMemoryTextBox);
            systemHealthPanel.Controls.Add(availableMemoryTextBox);
            systemHealthPanel.Controls.Add(cpuUsageTextBox4);
            systemHealthPanel.Controls.Add(architectureTextBox);
            systemHealthPanel.Controls.Add(systemOsTypeTextBox);
            systemHealthPanel.Controls.Add(systemWarningErrorTable);
            systemHealthPanel.Controls.Add(label11);
            systemHealthPanel.Controls.Add(label10);
            systemHealthPanel.Controls.Add(label9);
            systemHealthPanel.Controls.Add(label8);
            systemHealthPanel.Controls.Add(label7);
            systemHealthPanel.Controls.Add(label6);
            systemHealthPanel.Controls.Add(label5);
            systemHealthPanel.Controls.Add(label4);
            systemHealthPanel.Location = new Point(122, 91);
            systemHealthPanel.Name = "systemHealthPanel";
            systemHealthPanel.Size = new Size(849, 520);
            systemHealthPanel.TabIndex = 7;
            systemHealthPanel.Visible = false;
            // 
            // availableStorageTextBox
            // 
            availableStorageTextBox.Enabled = false;
            availableStorageTextBox.Location = new Point(239, 157);
            availableStorageTextBox.Name = "availableStorageTextBox";
            availableStorageTextBox.Size = new Size(259, 23);
            availableStorageTextBox.TabIndex = 12;
            // 
            // totalMemoryTextBox
            // 
            totalMemoryTextBox.Enabled = false;
            totalMemoryTextBox.Location = new Point(238, 128);
            totalMemoryTextBox.Name = "totalMemoryTextBox";
            totalMemoryTextBox.Size = new Size(260, 23);
            totalMemoryTextBox.TabIndex = 11;
            // 
            // availableMemoryTextBox
            // 
            availableMemoryTextBox.Enabled = false;
            availableMemoryTextBox.Location = new Point(239, 96);
            availableMemoryTextBox.Name = "availableMemoryTextBox";
            availableMemoryTextBox.Size = new Size(259, 23);
            availableMemoryTextBox.TabIndex = 10;
            // 
            // cpuUsageTextBox4
            // 
            cpuUsageTextBox4.Enabled = false;
            cpuUsageTextBox4.Location = new Point(239, 66);
            cpuUsageTextBox4.Name = "cpuUsageTextBox4";
            cpuUsageTextBox4.Size = new Size(259, 23);
            cpuUsageTextBox4.TabIndex = 9;
            // 
            // architectureTextBox
            // 
            architectureTextBox.Enabled = false;
            architectureTextBox.Location = new Point(239, 37);
            architectureTextBox.Name = "architectureTextBox";
            architectureTextBox.Size = new Size(259, 23);
            architectureTextBox.TabIndex = 8;
            // 
            // systemOsTypeTextBox
            // 
            systemOsTypeTextBox.Enabled = false;
            systemOsTypeTextBox.Location = new Point(239, 8);
            systemOsTypeTextBox.Name = "systemOsTypeTextBox";
            systemOsTypeTextBox.Size = new Size(259, 23);
            systemOsTypeTextBox.TabIndex = 7;
            // 
            // systemWarningErrorTable
            // 
            systemWarningErrorTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            systemWarningErrorTable.Enabled = false;
            systemWarningErrorTable.Location = new Point(17, 231);
            systemWarningErrorTable.Name = "systemWarningErrorTable";
            systemWarningErrorTable.Size = new Size(813, 283);
            systemWarningErrorTable.TabIndex = 6;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(13, 202);
            label11.Name = "label11";
            label11.Size = new Size(152, 15);
            label11.TabIndex = 5;
            label11.Text = "System warnings and errors";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(17, 178);
            label10.Name = "label10";
            label10.Size = new Size(0, 15);
            label10.TabIndex = 4;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(13, 151);
            label9.Name = "label9";
            label9.Size = new Size(208, 15);
            label9.TabIndex = 3;
            label9.Text = "Available storage(sum of all partition):";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(13, 124);
            label8.Name = "label8";
            label8.Size = new Size(83, 15);
            label8.TabIndex = 2;
            label8.Text = "Total memory:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(13, 96);
            label7.Name = "label7";
            label7.Size = new Size(106, 15);
            label7.TabIndex = 1;
            label7.Text = "Available memory:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(13, 65);
            label6.Name = "label6";
            label6.Size = new Size(68, 15);
            label6.TabIndex = 0;
            label6.Text = "CPU Usage:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(13, 36);
            label5.Name = "label5";
            label5.Size = new Size(75, 15);
            label5.TabIndex = 0;
            label5.Text = "Architecture:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 8);
            label4.Name = "label4";
            label4.Size = new Size(52, 15);
            label4.TabIndex = 0;
            label4.Text = "OS Type:";
            // 
            // signInButton
            // 
            signInButton.Location = new Point(901, 26);
            signInButton.Name = "signInButton";
            signInButton.Size = new Size(70, 23);
            signInButton.TabIndex = 8;
            signInButton.Text = "Sign In";
            signInButton.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(10, 555);
            label12.Name = "label12";
            label12.Size = new Size(106, 15);
            label12.TabIndex = 9;
            label12.Text = "Connection status:";
            // 
            // isConnectedLabel
            // 
            isConnectedLabel.AutoSize = true;
            isConnectedLabel.ForeColor = Color.SeaGreen;
            isConnectedLabel.Location = new Point(12, 570);
            isConnectedLabel.Name = "isConnectedLabel";
            isConnectedLabel.Size = new Size(65, 15);
            isConnectedLabel.TabIndex = 10;
            isConnectedLabel.Text = "Connected";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(13, 502);
            label13.Name = "label13";
            label13.Size = new Size(93, 15);
            label13.TabIndex = 11;
            label13.Text = "Connected user:";
            // 
            // loggedInUserLabel
            // 
            loggedInUserLabel.AutoSize = true;
            loggedInUserLabel.ForeColor = Color.SeaGreen;
            loggedInUserLabel.Location = new Point(17, 519);
            loggedInUserLabel.Name = "loggedInUserLabel";
            loggedInUserLabel.Size = new Size(72, 15);
            loggedInUserLabel.TabIndex = 12;
            loggedInUserLabel.Text = "Sample User";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(12, 445);
            label14.Name = "label14";
            label14.Size = new Size(89, 15);
            label14.TabIndex = 13;
            label14.Text = "Logged in date:";
            // 
            // loggedInDateLabel
            // 
            loggedInDateLabel.AutoSize = true;
            loggedInDateLabel.ForeColor = Color.SeaGreen;
            loggedInDateLabel.Location = new Point(17, 460);
            loggedInDateLabel.Name = "loggedInDateLabel";
            loggedInDateLabel.Size = new Size(91, 15);
            loggedInDateLabel.TabIndex = 14;
            loggedInDateLabel.Text = "2024.09.25 15:56";
            loggedInDateLabel.Click += label15_Click;
            // 
            // applicationLogo
            // 
            applicationLogo.BackgroundImage = (Image)resources.GetObject("applicationLogo.BackgroundImage");
            applicationLogo.BackgroundImageLayout = ImageLayout.Stretch;
            applicationLogo.Location = new Point(17, 322);
            applicationLogo.Name = "applicationLogo";
            applicationLogo.Size = new Size(84, 110);
            applicationLogo.TabIndex = 15;
            applicationLogo.UseVisualStyleBackColor = true;
            // 
            // logOutButton
            // 
            logOutButton.Location = new Point(820, 26);
            logOutButton.Name = "logOutButton";
            logOutButton.Size = new Size(75, 23);
            logOutButton.TabIndex = 16;
            logOutButton.Text = "Sign Out";
            logOutButton.UseVisualStyleBackColor = true;
            // 
            // consoleButton
            // 
            consoleButton.Location = new Point(12, 156);
            consoleButton.Name = "consoleButton";
            consoleButton.Size = new Size(94, 23);
            consoleButton.TabIndex = 17;
            consoleButton.Text = "Console";
            consoleButton.UseVisualStyleBackColor = true;
            consoleButton.Click += consoleButton_Click;
            // 
            // consolePanel
            // 
            consolePanel.Controls.Add(submitConsoleCommand);
            consolePanel.Controls.Add(consoleCommandTextBox);
            consolePanel.Controls.Add(consoleTextBox);
            consolePanel.Location = new Point(122, 91);
            consolePanel.Name = "consolePanel";
            consolePanel.Size = new Size(849, 520);
            consolePanel.TabIndex = 13;
            consolePanel.Visible = false;
            // 
            // submitConsoleCommand
            // 
            submitConsoleCommand.Location = new Point(726, 479);
            submitConsoleCommand.Name = "submitConsoleCommand";
            submitConsoleCommand.Size = new Size(120, 23);
            submitConsoleCommand.TabIndex = 2;
            submitConsoleCommand.Text = "Submit";
            submitConsoleCommand.UseVisualStyleBackColor = true;
            // 
            // consoleCommandTextBox
            // 
            consoleCommandTextBox.Location = new Point(13, 479);
            consoleCommandTextBox.Name = "consoleCommandTextBox";
            consoleCommandTextBox.Size = new Size(707, 23);
            consoleCommandTextBox.TabIndex = 1;
            // 
            // consoleTextBox
            // 
            consoleTextBox.Enabled = false;
            consoleTextBox.Location = new Point(13, 8);
            consoleTextBox.Name = "consoleTextBox";
            consoleTextBox.Size = new Size(833, 459);
            consoleTextBox.TabIndex = 0;
            consoleTextBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(983, 620);
            Controls.Add(consoleButton);
            Controls.Add(logOutButton);
            Controls.Add(applicationLogo);
            Controls.Add(loggedInDateLabel);
            Controls.Add(label14);
            Controls.Add(loggedInUserLabel);
            Controls.Add(label13);
            Controls.Add(isConnectedLabel);
            Controls.Add(label12);
            Controls.Add(signInButton);
            Controls.Add(systemHealthButton);
            Controls.Add(fileManagerButton);
            Controls.Add(apiPasswordTextBox);
            Controls.Add(apiUsernameTextBox);
            Controls.Add(apiUrlTextBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(fileManagerPanel);
            Controls.Add(consolePanel);
            Controls.Add(systemHealthPanel);
            Cursor = Cursors.Hand;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Remote server handler";
            ((System.ComponentModel.ISupportInitialize)filesDataTable).EndInit();
            fileManagerPanel.ResumeLayout(false);
            systemHealthPanel.ResumeLayout(false);
            systemHealthPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)systemWarningErrorTable).EndInit();
            consolePanel.ResumeLayout(false);
            consolePanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox apiUrlTextBox;
        private TextBox apiUsernameTextBox;
        private TextBox apiPasswordTextBox;
        private Button fileManagerButton;
        private DataGridView filesDataTable;
        private Button addFile;
        private Button removeSelectedFile;
        private Button downloadSelectedFile;
        private Panel fileManagerPanel;
        private Button systemHealthButton;
        private Panel systemHealthPanel;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private TextBox cpuUsageTextBox4;
        private TextBox architectureTextBox;
        private TextBox systemOsTypeTextBox;
        private DataGridView systemWarningErrorTable;
        private Label label11;
        private Label label10;
        private TextBox availableMemoryTextBox;
        private TextBox availableStorageTextBox;
        private TextBox totalMemoryTextBox;
        private Button signInButton;
        private Label label12;
        private Label isConnectedLabel;
        private Label label13;
        private Label loggedInUserLabel;
        private Label label14;
        private Label loggedInDateLabel;
        private Button applicationLogo;
        private Button logOutButton;
        private Button consoleButton;
        private Panel consolePanel;
        private Button submitConsoleCommand;
        private TextBox consoleCommandTextBox;
        private RichTextBox consoleTextBox;
    }
}
