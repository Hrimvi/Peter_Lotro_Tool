using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace EssenceValueCalculator
{
    public partial class Item_Explorer : Form
    {
       

        private Items itemDb;
        private Progressions itemProgressions;

        private List<int> iconIdSaves = new List<int>();
        private List<Image> iconImages = new List<Image>();

        private int count = 1;
        private TextBox searchTextBox;

        private TextBox levelInputTextBox;
        private Button scaleButton;

        private Item currentSelected;
        public Item_Explorer()
        {
            InitializeComponent();
            InitializeAsync();

        }
        private async void InitializeAsync()
        {
            itemDb = await Utility.LoadItemsAsync();
            itemProgressions = await Utility.LoadProgressionsAsync();

            searchTextBox = new TextBox();
            searchTextBox.Width = 200;
            searchTextBox.Location = new Point(10, 10);
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            this.Controls.Add(searchTextBox);
            levelInputTextBox = null;
            scaleButton = null;
            await LoadItems();

        }
        private async void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();
            int batchSize = 10;

            var visibilityResults = new ConcurrentDictionary<int, bool>();
            var rows = itemDatabaseGrid.Rows.Cast<DataGridViewRow>().ToList();
            var batches = rows.Select((row, index) => new { row, index })
                              .GroupBy(x => x.index / batchSize)
                              .Select(g => g.Select(x => x.row).ToList())
                              .ToList();

            await Task.Run(() =>
            {
                Parallel.ForEach(batches, batch =>
                {
                    foreach (var row in batch)
                    {
                        if (row.Cells["itemName"].Value != null)
                        {
                            string itemName = row.Cells["itemName"].Value.ToString().ToLower();
                            bool isVisible = itemName.Contains(searchText);
                            visibilityResults[row.Index] = isVisible;
                        }
                    }
                });
            });

            itemDatabaseGrid.SuspendLayout();

            itemDatabaseGrid.Invoke(new Action(() =>
            {
                foreach (var result in visibilityResults)
                {
                    itemDatabaseGrid.Rows[result.Key].Visible = result.Value;
                }

                itemDatabaseGrid.ResumeLayout();
            }));
        }
        private async Task LoadItems()
        {
            var allTasks = new List<Task>();
            foreach (Item item in itemDb.ItemList)
            {
                if (item.minLevel != null) if (item.minLevel < 120) continue;

                iconImages.Clear();
                iconIdSaves = Utility.GetIconIDsFromString(iconIdSaves, item.Icon, "-");
                iconIdSaves.Reverse();

                var imageTasks = new List<Task<Image>>();

                foreach (int id in iconIdSaves)
                {
                    if (id != 0)
                    {
                        string path = $"{Utility.iconFolder}/{id}.png";
                        imageTasks.Add(LoadImageAsync(path, id));
                    }
                }

                var loadedImages = await Task.WhenAll(imageTasks);

                iconImages.AddRange(loadedImages);

                Image fullIcon = await Utility.OverlayIconsAsync(iconImages);

                string armourType;
                if (item.ArmourType == null) armourType = "";
                else armourType = item.ArmourType;

                int itemLevel;
                if (item.itemLevel == null) itemLevel = 0;
                else itemLevel = item.itemLevel;
                itemDatabaseGrid.Invoke(new Action(() => itemDatabaseGrid.Rows.Add(fullIcon, item.Name, armourType, itemLevel)));
                numberText.Invoke(new Action(() => numberText.Text = $"Loaded: {count}"));
                count++;
            }
        }
        private Dictionary<int, Image> imageCache = new Dictionary<int, Image>();

        private Task<Image> LoadImageAsync(string path, int id)
        {
            if (imageCache.TryGetValue(id, out Image cachedImage))
            {
                return Task.FromResult(cachedImage);
            }

            return Task.Run(() =>
            {
                var image = Image.FromFile(path);
                imageCache[id] = image;
                return image;
            });
        }

        private async void itemDatabaseGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedName = itemDatabaseGrid.Rows[e.RowIndex].Cells["itemName"].Value.ToString();
                var selectedItem = itemDb.ItemList.FirstOrDefault(item => item.Name == selectedName);

                if (selectedItem != null)
                {
                    currentSelected = selectedItem;
                    await DisplayItemDetailsAsync(selectedItem);

                    if (levelInputTextBox == null || scaleButton == null)
                    {
                        levelInputTextBox = new TextBox
                        {
                            Width = 100,
                            Location = new Point(selectedItemPanel.Left, selectedItemPanel.Bottom + 10)
                        };
                        levelInputTextBox.KeyPress += LevelInputTextBox_KeyPress;

                        scaleButton = new Button
                        {
                            Text = "Scale Item",
                            Location = new Point(selectedItemPanel.Left + 100, selectedItemPanel.Bottom + 10),
                            Width = 100
                        };
                        scaleButton.Click += ScaleButton_Click;

                        this.Controls.Add(levelInputTextBox);
                        this.Controls.Add(scaleButton);
                    }
                    else
                    {
                        levelInputTextBox.Visible = true;
                        scaleButton.Visible = true;

                        levelInputTextBox.Location = new Point(selectedItemPanel.Left, selectedItemPanel.Bottom + 10);
                        scaleButton.Location = new Point(selectedItemPanel.Left + 100, selectedItemPanel.Bottom + 10);
                    }
                }
            }
        }
        private async void ScaleButton_Click(object sender, EventArgs e)
        {
            Utility.Log("ScaleButton_Click wurde aufgerufen.");

            if (int.TryParse(levelInputTextBox.Text, out int newLevel) && currentSelected != null)
            {
                var selectedItem = currentSelected;
                selectedItem.itemLevel = newLevel;

                Utility.Log("Item gefunden, aktualisiere die Stats.");
                try
                {
                    await DisplayItemDetailsAsync(selectedItem);
                    Utility.Log($"Item details updated for {selectedItem.Name} at level {newLevel}.");
                }
                catch (Exception ex)
                {
                    Utility.Log($"Fehler beim Aktualisieren der Item-Details: {ex.Message}");
                }
            }
            else
            {
                Utility.Log("Ungültige Eingabe für Item Level oder kein Item ausgewählt.");
            }
        }
        private void HideLevelControls()
        {
            if (levelInputTextBox != null && scaleButton != null)
            {
                levelInputTextBox.Visible = false;
                scaleButton.Visible = false;
            }
        }
        private async Task DisplayItemDetailsAsync(Item item)
        {
            selectedItemPanel.Controls.Clear();

            Panel nameAndIconPanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(400, 60)
            };

            PictureBox iconPictureBox = new PictureBox
            {
                Size = new Size(50, 50),
                Location = new Point(0, 5),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            iconImages.Clear();
            iconIdSaves = Utility.GetIconIDsFromString(iconIdSaves, item.Icon, "-");
            iconIdSaves.Reverse();

            var imageTasks = new List<Task<Image>>();
            foreach (int id in iconIdSaves)
            {
                if (id != 0)
                {
                    string path = $"{Utility.iconFolder}/{id}.png";
                    imageTasks.Add(LoadImageAsync(path, id));
                }
            }

            var loadedImages = await Task.WhenAll(imageTasks);
            iconImages.AddRange(loadedImages);

            iconPictureBox.Image = await Utility.OverlayIconsAsync(iconImages);

            nameAndIconPanel.Controls.Add(iconPictureBox);

            Label nameLabel = new Label
            {
                Text = $"{item.Name}",
                Location = new Point(iconPictureBox.Right + 10, 10),
                AutoSize = true
            };
            nameAndIconPanel.Controls.Add(nameLabel);

            selectedItemPanel.Controls.Add(nameAndIconPanel);


            Label levelLabel = new Label
            {
                Text = $"Item Level: {item.itemLevel}",
                Location = new Point(10, nameAndIconPanel.Bottom + 10),
                AutoSize = true
            };
            selectedItemPanel.Controls.Add(levelLabel);

            if (item.Dps != null && item.Dps > 0)
            {
                Label dpsLabel = new Label
                {
                    Text = $"DPS: {item.Dps}",
                    Location = new Point(10, (selectedItemPanel.Controls.OfType<Label>().LastOrDefault()?.Bottom ?? nameAndIconPanel.Bottom) + 10),
                    AutoSize = true
                };
                selectedItemPanel.Controls.Add(dpsLabel);
            }

            if (item.Stats != null && item.Stats.StatList != null && item.Stats.StatList.Any())
            {
                Label statsLabel = new Label
                {
                    Text = "Stats:",
                    Location = new Point(10, (selectedItemPanel.Controls.OfType<Label>().LastOrDefault()?.Bottom ?? (selectedItemPanel.Controls.OfType<PictureBox>().LastOrDefault()?.Bottom ?? nameAndIconPanel.Bottom)) + 10),
                    AutoSize = true
                };
                selectedItemPanel.Controls.Add(statsLabel);

                int statsY = statsLabel.Bottom + 10;
                var Stats = ConvertStatsToDictionary(item.Stats.StatList, item.itemLevel);
                foreach (var stat in Stats)
                {
                    Label statLabel = new Label
                    {
                        Text = $"{stat.Key.ToString().Replace("_", " ")}: {stat.Value}",
                        Location = new Point(10, statsY),
                        AutoSize = true
                    };
                    selectedItemPanel.Controls.Add(statLabel);
                    statsY += 20;

                }
            }
        }


        private Dictionary<StatEnum, float> ConvertStatsToDictionary(List<ItemStat> itemStats, int itemLevel)
        {
            var statCalc = new Dictionary<StatEnum, float>();

            foreach (var stat in itemStats)
            {
                try
                {
                    var statEnum = ParseStatEnum(stat.Name);
                    statCalc[statEnum] = (float)Math.Round(Utility.GetStatsFromProgressions(itemProgressions, stat.Scaling, itemLevel), 2);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return statCalc;
        }
        private void LevelInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private StatEnum ParseStatEnum(string statString)
        {
            statString = statString.Trim().ToUpper().Replace(" ", "_");

            if (Enum.TryParse<StatEnum>(statString, ignoreCase: true, out StatEnum result))
            {
                return result;
            }
            Utility.Log($"Ungültiger Stat-String: {statString}");
            return StatEnum.TBD;
        }

        private void Item_Explorer_Load(object sender, EventArgs e)
        {

        }
    }
}
