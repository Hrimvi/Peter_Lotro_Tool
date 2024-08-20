namespace EssenceValueCalculator
{
    partial class Form2
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
            backToMainWindow = new Button();
            SuspendLayout();
            // 
            // subEssencesCheckbox
            // 
            subEssencesCheckbox.AutoSize = true;
            subEssencesCheckbox.Location = new Point(37, 80);
            subEssencesCheckbox.Name = "subEssencesCheckbox";
            subEssencesCheckbox.Size = new Size(251, 19);
            subEssencesCheckbox.TabIndex = 0;
            subEssencesCheckbox.Text = "Use Supplemental Essences for Calculation";
            subEssencesCheckbox.UseVisualStyleBackColor = true;
            subEssencesCheckbox.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // itemLevelDropBox
            // 
            itemLevelDropBox.FormattingEnabled = true;
            itemLevelDropBox.Location = new Point(167, 105);
            itemLevelDropBox.Name = "itemLevelDropBox";
            itemLevelDropBox.Size = new Size(121, 23);
            itemLevelDropBox.TabIndex = 1;
            // 
            // EssenceItemlevelLabel
            // 
            EssenceItemlevelLabel.AutoSize = true;
            EssenceItemlevelLabel.Location = new Point(37, 108);
            EssenceItemlevelLabel.Name = "EssenceItemlevelLabel";
            EssenceItemlevelLabel.Size = new Size(104, 15);
            EssenceItemlevelLabel.TabIndex = 2;
            EssenceItemlevelLabel.Text = "Essence-Itemlevel:";
            // 
            // backToMainWindow
            // 
            backToMainWindow.Location = new Point(12, 12);
            backToMainWindow.Name = "backToMainWindow";
            backToMainWindow.Size = new Size(141, 23);
            backToMainWindow.TabIndex = 3;
            backToMainWindow.Text = "Back To Main Window";
            backToMainWindow.UseVisualStyleBackColor = true;
            backToMainWindow.Click += backToMainWindow_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(backToMainWindow);
            Controls.Add(EssenceItemlevelLabel);
            Controls.Add(itemLevelDropBox);
            Controls.Add(subEssencesCheckbox);
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox subEssencesCheckbox;
        private ComboBox itemLevelDropBox;
        private Label EssenceItemlevelLabel;
        private Button backToMainWindow;
    }
}