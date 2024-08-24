using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;

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
            InitializeLogFile();
            LoadDataAsync();
            InitializeComponent();
        }
       
        private async Task LoadDataAsync()
        {
            ApplicationData.Instance.Settings = Utility.LoadSettings();
            ApplicationData.Instance.StatConfig = Utility.LoadStatConfigs();
            ApplicationData.Instance.PlayerStatsPerClass = Utility.LoadClass();
            ApplicationData.Instance.itemProgressions = await Utility.LoadProgressionsAsync();
            ApplicationData.Instance.itemDb = await Utility.LoadItemsAsync();
            ApplicationData.Instance.essenceValues = await Utility.LoadEssenceValuesAsync();
            InitializeTabs();
            AddInitialTab();
        }
        private void InitializeLogFile()
        {
            try
            {
                FileInfo logFile = new FileInfo(ApplicationData.logFilePath);

                if (logFile.Exists)
                {
                    File.WriteAllText(ApplicationData.logFilePath, $"Log-File created: " + DateTime.Now + Environment.NewLine);
                }
                else
                {
                    using (FileStream fs = logFile.Create())
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes("Log-File created: " + DateTime.Now + Environment.NewLine);
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Initialisieren der Log-Datei: {ex.Message}");
            }
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
    public class ApplicationData
    {
        private static readonly Lazy<ApplicationData> _instance = new Lazy<ApplicationData>(() => new ApplicationData());

        public static ApplicationData Instance => _instance.Value;
        public Settings Settings { get; set; }
        public Stats PlayerStatsPerClass { get; set; }
        public StatConfigs StatConfig { get; set; }

        public Items itemDb { get; set; }

        public Progressions itemProgressions { get; set; }

        public List<Item> essenceValues { get; set; }


        public const string logFilePath = "log.txt";
        public const string essenceFilePath = "xmls/essence_values.xml";
        public const string characterStatDerivationFilePath = "xmls/classStatDerivations.xml";
        public const string settingsFilePath = "xmls/settings.xml";
        public const string statConfigFilePath = "xmls/statConfigs.xml";
        public const string progressionFilePath = "xmls/progressions.xml";
        public const string itemsFilePath = "xmls/items.xml";
        public const string iconFolder = "items";


        public static HashSet<StatEnum> viableStats = new HashSet<StatEnum> {

            StatEnum.Might,
            StatEnum.Agility,
            StatEnum.Vitality,
            StatEnum.Will,
            StatEnum.Fate,
            StatEnum.Critical_Rating,
            StatEnum.Physical_Mastery,
            StatEnum.Tactical_Mastery,
            StatEnum.Physical_Mitigation,
            StatEnum.Tactical_Mitigation,
            StatEnum.Critical_Defence,
            StatEnum.Finesse,
            StatEnum.Block,
            StatEnum.Parry,
            StatEnum.Evade,
            StatEnum.Outgoing_Healing,
            StatEnum.Incoming_Healing,
            StatEnum.Resistance,
            StatEnum.Morale,
            StatEnum.Power,
            StatEnum.Armour,
            StatEnum.Basic_EssenceSlot

        };

        public static HashSet<StatEnum> statsToValue = new HashSet<StatEnum> {


            StatEnum.Critical_Rating,
            StatEnum.Physical_Mastery,
            StatEnum.Tactical_Mastery,
            StatEnum.Physical_Mitigation,
            StatEnum.Tactical_Mitigation,
            StatEnum.Critical_Defence,
            StatEnum.Finesse,
            StatEnum.Block,
            StatEnum.Parry,
            StatEnum.Evade,
            StatEnum.Outgoing_Healing,
            StatEnum.Incoming_Healing,
            StatEnum.Resistance,
            StatEnum.Morale,
            StatEnum.Power
        };

        private ApplicationData() { }
    }
}