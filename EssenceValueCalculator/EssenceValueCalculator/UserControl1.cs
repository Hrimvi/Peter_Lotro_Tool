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

            var evButton = new Button
            {
                Text = "EV Calculator",
                Dock = DockStyle.Top,
                Height = 50
            };
            evButton.Click += (sender, e) => FunctionSelected?.Invoke(this, "EV Calculator");

            var itemExplorerButton = new Button
            {
                Text = "Item Explorer",
                Dock = DockStyle.Top,
                Height = 50
            };
            itemExplorerButton.Click += (sender, e) => FunctionSelected?.Invoke(this, "Item Explorer");

            var otherButton = new Button
            {
                Text = "Settings",
                Dock = DockStyle.Top,
                Height = 50
            };
            otherButton.Click += (sender, e) => FunctionSelected?.Invoke(this, "Settings");

            this.Controls.Add(evButton);
            this.Controls.Add(itemExplorerButton);
            this.Controls.Add(otherButton);
            
        }
    }
}
