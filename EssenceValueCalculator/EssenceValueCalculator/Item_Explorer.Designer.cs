namespace EssenceValueCalculator
{
    partial class Item_Explorer
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
            selectedItemPanel = new Panel();
            itemDatabaseGrid = new DataGridView();
            icons = new DataGridViewImageColumn();
            itemName = new DataGridViewTextBoxColumn();
            ArmourType = new DataGridViewTextBoxColumn();
            ItemLevel = new DataGridViewTextBoxColumn();
            numberText = new Label();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)itemDatabaseGrid).BeginInit();
            SuspendLayout();
            // 
            // selectedItemPanel
            // 
            selectedItemPanel.Location = new Point(529, 42);
            selectedItemPanel.Name = "selectedItemPanel";
            selectedItemPanel.Size = new Size(259, 300);
            selectedItemPanel.TabIndex = 0;
            // 
            // itemDatabaseGrid
            // 
            itemDatabaseGrid.AllowUserToAddRows = false;
            itemDatabaseGrid.AllowUserToOrderColumns = true;
            itemDatabaseGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            itemDatabaseGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemDatabaseGrid.Columns.AddRange(new DataGridViewColumn[] { icons, itemName, ArmourType, ItemLevel });
            itemDatabaseGrid.Location = new Point(12, 69);
            itemDatabaseGrid.Name = "itemDatabaseGrid";
            itemDatabaseGrid.ReadOnly = true;
            itemDatabaseGrid.RowHeadersVisible = false;
            itemDatabaseGrid.Size = new Size(511, 273);
            itemDatabaseGrid.TabIndex = 1;
            itemDatabaseGrid.CellContentClick += itemDatabaseGrid_CellContentClick;
            // 
            // icons
            // 
            icons.HeaderText = "Icon";
            icons.Name = "icons";
            icons.ReadOnly = true;
            icons.Width = 36;
            // 
            // itemName
            // 
            itemName.HeaderText = "itemName";
            itemName.Name = "itemName";
            itemName.ReadOnly = true;
            itemName.Width = 88;
            // 
            // ArmourType
            // 
            ArmourType.HeaderText = "Armour Type";
            ArmourType.Name = "ArmourType";
            ArmourType.ReadOnly = true;
            // 
            // ItemLevel
            // 
            ItemLevel.HeaderText = "Item Level";
            ItemLevel.Name = "ItemLevel";
            ItemLevel.ReadOnly = true;
            ItemLevel.Width = 86;
            // 
            // numberText
            // 
            numberText.AutoSize = true;
            numberText.Location = new Point(78, 374);
            numberText.Name = "numberText";
            numberText.Size = new Size(38, 15);
            numberText.TabIndex = 2;
            numberText.Text = "label1";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(127, 40);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(396, 23);
            textBox1.TabIndex = 3;
            // 
            // Item_Explorer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox1);
            Controls.Add(numberText);
            Controls.Add(itemDatabaseGrid);
            Controls.Add(selectedItemPanel);
            Name = "Item_Explorer";
            Text = "Item_Explorer";
            ((System.ComponentModel.ISupportInitialize)itemDatabaseGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel selectedItemPanel;
        private DataGridView itemDatabaseGrid;
        private Label numberText;
        private DataGridViewImageColumn icons;
        private DataGridViewTextBoxColumn itemName;
        private DataGridViewTextBoxColumn ArmourType;
        private DataGridViewTextBoxColumn ItemLevel;
        private TextBox textBox1;
    }
}