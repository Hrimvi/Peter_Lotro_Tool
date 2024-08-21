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

        public MainMenu()
        {
            InitializeComponent();
            InitializeTabs();
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
            // Button zum Hinzufügen von Tabs
            var addTabButton = new Button
            {
                Text = "New Tab",
                Dock = DockStyle.Top,
                Height = 30
            };
            addTabButton.Click += AddNewTab;
            this.Controls.Add(addTabButton);
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
                    formToLoad = new EV_Tool();
                    break;
                case "Settings":
                    formToLoad = new Form2();
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
    }


}