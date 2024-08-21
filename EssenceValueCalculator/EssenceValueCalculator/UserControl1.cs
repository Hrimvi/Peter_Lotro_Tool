﻿using System;
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

            var otherButton = new Button
            {
                Text = "Settings",
                Dock = DockStyle.Top,
                Height = 50
            };
            otherButton.Click += (sender, e) => FunctionSelected?.Invoke(this, "Settings");

            this.Controls.Add(otherButton);
            this.Controls.Add(evButton);
        }
    }
}