﻿namespace EssenceValueCalculator
{
    partial class Peter_Lotro_Tool
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
            comboBoxStats = new ComboBox();
            inputField = new TextBox();
            addButton = new Button();
            removeButton = new Button();
            resetButton = new Button();
            essenceValueText = new Label();
            classBox = new ComboBox();
            primaryBox1 = new ComboBox();
            primaryBox2 = new ComboBox();
            primaryBox3 = new ComboBox();
            vitalBox1 = new ComboBox();
            vitalBox2 = new ComboBox();
            vitalBox3 = new ComboBox();
            primaryEssenceLabel = new Label();
            vitalEssenceLabel = new Label();
            SuspendLayout();
            // 
            // comboBoxStats
            // 
            comboBoxStats.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBoxStats.FormattingEnabled = true;
            comboBoxStats.Location = new System.Drawing.Point(69, 123);
            comboBoxStats.Name = "comboBoxStats";
            comboBoxStats.Size = new Size(121, 23);
            comboBoxStats.TabIndex = 0;
            // 
            // inputField
            // 
            inputField.Font = new Font("Arial Rounded MT Bold", 9.75F);
            inputField.Location = new System.Drawing.Point(217, 123);
            inputField.Name = "inputField";
            inputField.Size = new Size(100, 23);
            inputField.TabIndex = 1;
            // 
            // addButton
            // 
            addButton.Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addButton.Location = new System.Drawing.Point(69, 339);
            addButton.Name = "addButton";
            addButton.Size = new Size(121, 23);
            addButton.TabIndex = 2;
            addButton.Text = "+";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // removeButton
            // 
            removeButton.Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            removeButton.Location = new System.Drawing.Point(201, 339);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(121, 23);
            removeButton.TabIndex = 3;
            removeButton.Text = "-";
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += removeButton_Click;
            // 
            // resetButton
            // 
            resetButton.Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            resetButton.Location = new System.Drawing.Point(328, 339);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(121, 23);
            resetButton.TabIndex = 4;
            resetButton.Text = "Reset all";
            resetButton.UseVisualStyleBackColor = true;
            resetButton.Click += resetButton_Click;
            // 
            // essenceValueText
            // 
            essenceValueText.AutoSize = true;
            essenceValueText.Font = new Font("Arial Rounded MT Bold", 11F);
            essenceValueText.Location = new System.Drawing.Point(471, 70);
            essenceValueText.Name = "essenceValueText";
            essenceValueText.Size = new Size(121, 17);
            essenceValueText.TabIndex = 5;
            essenceValueText.Text = "Essence-Value:";
            // 
            // classBox
            // 
            classBox.Font = new Font("Arial Rounded MT Bold", 9.75F);
            classBox.FormattingEnabled = true;
            classBox.Location = new System.Drawing.Point(69, 32);
            classBox.Name = "classBox";
            classBox.Size = new Size(121, 23);
            classBox.TabIndex = 7;
            // 
            // primaryBox1
            // 
            primaryBox1.Font = new Font("Arial Rounded MT Bold", 9.75F);
            primaryBox1.FormattingEnabled = true;
            primaryBox1.Location = new System.Drawing.Point(471, 281);
            primaryBox1.Name = "primaryBox1";
            primaryBox1.Size = new Size(121, 23);
            primaryBox1.TabIndex = 8;
            // 
            // primaryBox2
            // 
            primaryBox2.Font = new Font("Arial Rounded MT Bold", 9.75F);
            primaryBox2.FormattingEnabled = true;
            primaryBox2.Location = new System.Drawing.Point(471, 310);
            primaryBox2.Name = "primaryBox2";
            primaryBox2.Size = new Size(121, 23);
            primaryBox2.TabIndex = 9;
            // 
            // primaryBox3
            // 
            primaryBox3.Font = new Font("Arial Rounded MT Bold", 9.75F);
            primaryBox3.FormattingEnabled = true;
            primaryBox3.Location = new System.Drawing.Point(471, 339);
            primaryBox3.Name = "primaryBox3";
            primaryBox3.Size = new Size(121, 23);
            primaryBox3.TabIndex = 10;
            // 
            // vitalBox1
            // 
            vitalBox1.Font = new Font("Arial Rounded MT Bold", 9.75F);
            vitalBox1.FormattingEnabled = true;
            vitalBox1.Location = new System.Drawing.    Point(635, 281);
            vitalBox1.Name = "vitalBox1";
            vitalBox1.Size = new Size(121, 23);
            vitalBox1.TabIndex = 11;
            // 
            // vitalBox2
            // 
            vitalBox2.Font = new Font("Arial Rounded MT Bold", 9.75F);
            vitalBox2.FormattingEnabled = true;
            vitalBox2.Location =         new System.Drawing.Point(635, 310);
            vitalBox2.Name = "vitalBox2";
            vitalBox2.Size = new Size(121, 23);
            vitalBox2.TabIndex = 12;
            // 
            // vitalBox3
            // 
            vitalBox3.Font = new Font("Arial Rounded MT Bold", 9.75F);
            vitalBox3.FormattingEnabled = true;
            vitalBox3.Location = new System.Drawing.Point(635, 339);
            vitalBox3.Name = "vitalBox3";
            vitalBox3.Size = new Size(121, 23);
            vitalBox3.TabIndex = 13;
            // 
            // primaryEssenceLabel
            // 
            primaryEssenceLabel.AutoSize = true;
            primaryEssenceLabel.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            primaryEssenceLabel.Location = new System.Drawing.Point(471, 260);
            primaryEssenceLabel.Name = "primaryEssenceLabel";
            primaryEssenceLabel.Size = new Size(124, 15);
            primaryEssenceLabel.TabIndex = 14;
            primaryEssenceLabel.Text = "Primary Essences";
            primaryEssenceLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // vitalEssenceLabel
            // 
            vitalEssenceLabel.AutoSize = true;
            vitalEssenceLabel.Font = new Font("Arial Rounded MT Bold", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            vitalEssenceLabel.Location = new System.Drawing.Point(635, 263);
            vitalEssenceLabel.Name = "vitalEssenceLabel";
            vitalEssenceLabel.Size = new Size(102, 15);
            vitalEssenceLabel.TabIndex = 15;
            vitalEssenceLabel.Text = "Vital Essences";
            vitalEssenceLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Peter_Lotro_Tool
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(vitalEssenceLabel);
            Controls.Add(primaryEssenceLabel);
            Controls.Add(vitalBox3);
            Controls.Add(vitalBox2);
            Controls.Add(vitalBox1);
            Controls.Add(primaryBox3);
            Controls.Add(primaryBox2);
            Controls.Add(primaryBox1);
            Controls.Add(classBox);
            Controls.Add(essenceValueText);
            Controls.Add(resetButton);
            Controls.Add(removeButton);
            Controls.Add(addButton);
            Controls.Add(inputField);
            Controls.Add(comboBoxStats);
            Name = "Peter_Lotro_Tool";
            Text = "Peter_Lotro_Tool";
            Load += EV_Tool_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxStats;
        private TextBox inputField;
        private Button addButton;
        private Button removeButton;
        private Button resetButton;
        private Label essenceValueText;
        private ComboBox classBox;
        private ComboBox primaryBox1;
        private ComboBox primaryBox2;
        private ComboBox primaryBox3;
        private ComboBox vitalBox1;
        private ComboBox vitalBox2;
        private ComboBox vitalBox3;
        private Label primaryEssenceLabel;
        private Label vitalEssenceLabel;
    }
}
