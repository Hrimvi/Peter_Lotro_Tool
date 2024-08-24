using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EssenceValueCalculator
{
    public partial class StartScreen : UserControl
    {
        public event EventHandler<string> FunctionSelected;

        public StartScreen()
        {
            InitializeComponent();

            var settingsButton = new Button
            {
                Text = "Settings",
                Dock = DockStyle.Top,
                Height = 50
            };
            settingsButton.Click += (sender, e) => FunctionSelected?.Invoke(this, "Settings");
            this.Controls.Add(settingsButton);


           
            var itemExplorerButton = new Button
            {
                Text = "Item Explorer",
                Dock = DockStyle.Top,
                Height = 50
            };
            itemExplorerButton.Click += (sender, e) => FunctionSelected?.Invoke(this, "Item Explorer");
            this.Controls.Add(itemExplorerButton);



            var evButton = new Button
            {
                Text = "EV Calculator",
                Dock = DockStyle.Top,
                Height = 50
            };
            evButton.Click += (sender, e) => FunctionSelected?.Invoke(this, "EV Calculator");
            this.Controls.Add(evButton);










        }

        private void StartScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
