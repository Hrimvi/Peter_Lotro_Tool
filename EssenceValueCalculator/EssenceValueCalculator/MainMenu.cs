using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace EssenceValueCalculator
{
    public partial class MainMenu : Form
    {
        private TabControl tabControl;
        private ContextMenuStrip tabContextMenu;
        private ToolStripMenuItem closeTabMenuItem;
        private ToolStrip toolStrip;
        private ToolStripDropDownButton optionsDropDown;

        public MainMenu()
        {
            InitializeComponent();
            InitializeTabs();
            AddInitialTab();
        }

        private void InitializeTabs()
        {
            tabControl = new TabControl { Dock = DockStyle.Fill };
            this.Controls.Add(tabControl);

            tabContextMenu = new ContextMenuStrip();
            closeTabMenuItem = new ToolStripMenuItem("Close Tab");
            closeTabMenuItem.Click += CloseTabMenuItem_Click;
            tabContextMenu.Items.Add(closeTabMenuItem);

            tabControl.MouseUp += TabControl_MouseUp;

            toolStrip = new ToolStrip();
            optionsDropDown = new ToolStripDropDownButton("Options");
            optionsDropDown.DropDownItems.Add("New Tab", null, NewTabMenuItem_Click);
            optionsDropDown.DropDownItems.Add("Close App", null, CloseAppMenuItem_Click);


            toolStrip.Items.Add(optionsDropDown);

            this.Controls.Add(toolStrip);
        }

        private void AddInitialTab()
        {
            AddNewTab(null, EventArgs.Empty);
        }

        private void AddNewTab(object sender, EventArgs e)
        {
            var newTabPage = new TabPage("New Tab");
            tabControl.TabPages.Add(newTabPage);
            tabControl.SelectedTab = newTabPage;

            var startScreen = new StartScreen();
            startScreen.FunctionSelected += (s, functionName) => LoadFunction(newTabPage, functionName);
            newTabPage.Controls.Add(startScreen);
        }

        private void LoadFunction(TabPage tabPage, string functionName)
        {
            tabPage.Controls.Clear();

            Form formToLoad = null;

            switch (functionName)
            {
                case "EV Calculator":
                    formToLoad = new Peter_Lotro_Tool();
                    break;
                case "Item Explorer":
                    formToLoad = new Item_Explorer();
                    break;
                case "Settings":
                    formToLoad = new SettingsForm();
                    break;
            }

            if (formToLoad != null)
            {
                formToLoad.TopLevel = false;
                formToLoad.FormBorderStyle = FormBorderStyle.None;
                formToLoad.Dock = DockStyle.Fill;
                tabPage.Controls.Add(formToLoad);
                formToLoad.Show();

                tabPage.Text = functionName;
            }
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
        }

        private void TabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    if (tabControl.GetTabRect(i).Contains(e.Location))
                    {
                        tabControl.SelectedIndex = i;
                        tabContextMenu.Show(tabControl, e.Location);
                        break;
                    }
                }
            }
        }

        private void CloseTabMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }

        private void NewTabMenuItem_Click(object sender, EventArgs e)
        {
            AddNewTab(sender, e);
        }

        private void CloseAppMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}