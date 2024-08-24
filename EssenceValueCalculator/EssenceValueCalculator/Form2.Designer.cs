namespace EssenceValueCalculator
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            subEssencesCheckbox = new CheckBox();
            itemLevelDropBox = new ComboBox();
            EssenceItemlevelLabel = new Label();
            activeStatConfigLabel = new Label();
            activeStatConfigSelection = new ComboBox();
            configPanel = new Panel();
            configEditorSelection = new ComboBox();
            createConfig = new Button();
            deleteConfig = new Button();
            SuspendLayout();
            // 
            // subEssencesCheckbox
            // 
            subEssencesCheckbox.AutoSize = true;
            subEssencesCheckbox.Font = new Font("Arial Rounded MT Bold", 9.75F);
            subEssencesCheckbox.Location = new Point(37, 80);
            subEssencesCheckbox.Name = "subEssencesCheckbox";
            subEssencesCheckbox.Size = new Size(308, 19);
            subEssencesCheckbox.TabIndex = 0;
            subEssencesCheckbox.Text = "Use Supplemental Essences for Calculation";
            subEssencesCheckbox.UseVisualStyleBackColor = true;
            subEssencesCheckbox.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // itemLevelDropBox
            // 
            itemLevelDropBox.Font = new Font("Arial Rounded MT Bold", 9.75F);
            itemLevelDropBox.FormattingEnabled = true;
            itemLevelDropBox.Location = new Point(167, 105);
            itemLevelDropBox.Name = "itemLevelDropBox";
            itemLevelDropBox.Size = new Size(121, 23);
            itemLevelDropBox.TabIndex = 1;
            itemLevelDropBox.SelectedIndexChanged += itemLevelDropBox_SelectedIndexChanged;
            // 
            // EssenceItemlevelLabel
            // 
            EssenceItemlevelLabel.AutoSize = true;
            EssenceItemlevelLabel.Font = new Font("Arial Rounded MT Bold", 9.75F);
            EssenceItemlevelLabel.Location = new Point(37, 108);
            EssenceItemlevelLabel.Name = "EssenceItemlevelLabel";
            EssenceItemlevelLabel.Size = new Size(130, 15);
            EssenceItemlevelLabel.TabIndex = 2;
            EssenceItemlevelLabel.Text = "Essence-Itemlevel:";
            // 
            // activeStatConfigLabel
            // 
            activeStatConfigLabel.AutoSize = true;
            activeStatConfigLabel.Font = new Font("Arial Rounded MT Bold", 9.75F);
            activeStatConfigLabel.Location = new Point(37, 143);
            activeStatConfigLabel.Name = "activeStatConfigLabel";
            activeStatConfigLabel.Size = new Size(128, 15);
            activeStatConfigLabel.TabIndex = 4;
            activeStatConfigLabel.Text = "Active Stat-Config:";
            // 
            // activeStatConfigSelection
            // 
            activeStatConfigSelection.Font = new Font("Arial Rounded MT Bold", 9.75F);
            activeStatConfigSelection.FormattingEnabled = true;
            activeStatConfigSelection.Location = new Point(167, 140);
            activeStatConfigSelection.Name = "activeStatConfigSelection";
            activeStatConfigSelection.Size = new Size(121, 23);
            activeStatConfigSelection.TabIndex = 5;
            activeStatConfigSelection.SelectedIndexChanged += activeStatConfigSelection_SelectedIndexChanged;
            // 
            // configPanel
            // 
            configPanel.AutoScroll = true;
            configPanel.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            configPanel.Location = new Point(418, 80);
            configPanel.Name = "configPanel";
            configPanel.Size = new Size(370, 292);
            configPanel.TabIndex = 6;
            configPanel.Paint += configPanel_Paint;
            // 
            // configEditorSelection
            // 
            configEditorSelection.Font = new Font("Arial Rounded MT Bold", 9.75F);
            configEditorSelection.FormattingEnabled = true;
            configEditorSelection.Location = new Point(418, 35);
            configEditorSelection.Name = "configEditorSelection";
            configEditorSelection.Size = new Size(198, 23);
            configEditorSelection.TabIndex = 7;
            // 
            // createConfig
            // 
            createConfig.Font = new Font("Arial Rounded MT Bold", 9.75F);
            createConfig.Location = new Point(622, 35);
            createConfig.Name = "createConfig";
            createConfig.Size = new Size(75, 23);
            createConfig.TabIndex = 8;
            createConfig.Text = "+";
            createConfig.UseVisualStyleBackColor = true;
            createConfig.Click += createConfig_Click;
            // 
            // deleteConfig
            // 
            deleteConfig.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            deleteConfig.Location = new Point(703, 35);
            deleteConfig.Name = "deleteConfig";
            deleteConfig.Size = new Size(85, 23);
            deleteConfig.TabIndex = 9;
            deleteConfig.Text = "-";
            deleteConfig.UseVisualStyleBackColor = true;
            deleteConfig.Click += deleteConfig_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(deleteConfig);
            Controls.Add(createConfig);
            Controls.Add(configEditorSelection);
            Controls.Add(configPanel);
            Controls.Add(activeStatConfigSelection);
            Controls.Add(activeStatConfigLabel);
            Controls.Add(EssenceItemlevelLabel);
            Controls.Add(itemLevelDropBox);
            Controls.Add(subEssencesCheckbox);
            Name = "SettingsForm";
            Text = "Form2";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox subEssencesCheckbox;
        private ComboBox itemLevelDropBox;
        private Label EssenceItemlevelLabel;
        private Label activeStatConfigLabel;
        private ComboBox activeStatConfigSelection;
        private Panel configPanel;
        private ComboBox configEditorSelection;
        private Button createConfig;
        private Button deleteConfig;
    }
}