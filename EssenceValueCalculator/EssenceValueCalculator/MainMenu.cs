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
        private EV_Tool evTool;
        private Form2? form2;
        public MainMenu()
        {
            InitializeComponent();
            InitializeTabs();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }
        private void InitializeTabs()
        {
            // TabControl initialisieren
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);

            // Tab für EV-Rechner erstellen
            var evTabPage = new TabPage("EV Calculator");
            tabControl.TabPages.Add(evTabPage);

            // Tab für Einstellungen erstellen
            var settingsTabPage = new TabPage("Settings");
            tabControl.TabPages.Add(settingsTabPage);

            // Andere Tabs hinzufügen (weitere Funktionen)
            var otherTabPage = new TabPage("Other Function");
            tabControl.TabPages.Add(otherTabPage);

            // Hier kannst du spezifische UserControls oder Funktionen in die Tabs einfügen
            evTabPage.Controls.Add(InitializeEvCalculatorPanel());
            settingsTabPage.Controls.Add(InitializeSettingsPanel());
            otherTabPage.Controls.Add(InitializeOtherFunctionPanel());

            evTool = new EV_Tool();
            evTool.TopLevel = false; // Muss False sein, um es in ein anderes Control einzubetten
            evTool.FormBorderStyle = FormBorderStyle.None;
            evTool.Dock = DockStyle.Fill;
            evTabPage.Controls.Add(evTool);
            evTool.Show();

            // Erstelle ein neues Form2 Objekt und bette es in den Tab ein
            form2 = new Form2();
            form2.TopLevel = false; // Muss False sein, um es in ein anderes Control einzubetten
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.Dock = DockStyle.Fill;
            settingsTabPage.Controls.Add(form2);
            form2.Show();
        }
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Text == "EV Calculator")
            {
                // Berechne oder lade die EV Daten neu
            }
            else if (tabControl.SelectedTab.Text == "Settings")
            {
                // Lade oder speichere Einstellungen
            }
        }
        private Panel InitializeEvCalculatorPanel()
        {
            // Erstelle und konfiguriere hier dein Panel für den EV-Rechner
            var panel = new Panel();
            // Füge deine Steuerelemente und Logik hinzu
            return panel;
        }

        private Panel InitializeSettingsPanel()
        {
            // Erstelle und konfiguriere hier dein Panel für die Einstellungen
            var panel = new Panel();
            // Füge deine Steuerelemente und Logik hinzu
            return panel;
        }

        private Panel InitializeOtherFunctionPanel()
        {
            // Erstelle und konfiguriere hier dein Panel für weitere Funktionen
            var panel = new Panel();
            // Füge deine Steuerelemente und Logik hinzu
            return panel;
        }
    }

   
}
