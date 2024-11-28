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


        private List<int> iconIdSaves = new List<int>();
        private List<Image> iconImages = new List<Image>();

        private int count = 1;
        private TextBox searchTextBox;

        private TextBox levelInputTextBox;
        private Button scaleButton;

        private Item currentSelected;

        private TextBox minLevelTextBox;
        private TextBox maxLevelTextBox;
        private Button applyFilterButton;
        private int? minLevelFilter = null;
        private int? maxLevelFilter = null;
        private string currentSearchText = "";
        public Item_Explorer()
        {
            InitializeComponent();
            InitializeAsync();

        }
        private async void InitializeAsync()
        {
            searchTextBox = new TextBox
            {
                Width = 200,
                Location = new Point(10, 10)
            };
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            this.Controls.Add(searchTextBox);

            minLevelTextBox = new TextBox
            {
                Width = 100,
                Location = new Point(searchTextBox.Right + 20, 10),
                PlaceholderText = "Min Level"
            };
            this.Controls.Add(minLevelTextBox);

            maxLevelTextBox = new TextBox
            {
                Width = 100,
                Location = new Point(minLevelTextBox.Right + 10, 10),
                PlaceholderText = "Max Level"
            };
            this.Controls.Add(maxLevelTextBox);

            applyFilterButton = new Button
            {
                Text = "Apply Filter",
                Location = new Point(maxLevelTextBox.Right + 10, 10)
            };
            applyFilterButton.Click += ApplyFilterButton_Click;
            this.Controls.Add(applyFilterButton);

            levelInputTextBox = null;
            scaleButton = null;
            await LoadItems();

        }
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();
            if (searchText.Length < 3)
            {
                return;
            }

            currentSearchText = searchText;
        }
        private async void ApplyFilterButton_Click(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text;
            int? minLevel = string.IsNullOrEmpty(minLevelTextBox.Text) ? null : int.Parse(minLevelTextBox.Text);
            int? maxLevel = string.IsNullOrEmpty(maxLevelTextBox.Text) ? null : int.Parse(maxLevelTextBox.Text);

            await ApplyFiltersAsync(searchText, minLevel, maxLevel);
        }
        private async Task LoadItems()
        {
            var allTasks = new List<Task>();
            foreach (Item item in ApplicationData.Instance.itemDb.ItemList)
            {
                if (item.minLevel != null) if (item.minLevel < 150) continue;

                iconImages.Clear();
                iconIdSaves = Utility.GetIconIDsFromString(iconIdSaves, item.itemIcon, "-");
                iconIdSaves.Reverse();

                var imageTasks = new List<Task<Image>>();

                foreach (int id in iconIdSaves)
                {
                    if (id != 0)
                    {
                        string path = $"{ApplicationData.iconFolder}/{id}.png";
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
                itemDatabaseGrid.Invoke(new Action(() => itemDatabaseGrid.Rows.Add(fullIcon, item.itemName, armourType, itemLevel)));
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
                var selectedItem = ApplicationData.Instance.itemDb.ItemList.FirstOrDefault(item => item.itemName == selectedName);

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
                    Utility.Log($"Item details updated for {selectedItem.itemName} at level {newLevel}.");
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
            iconIdSaves = Utility.GetIconIDsFromString(iconIdSaves, item.itemIcon, "-");
            iconIdSaves.Reverse();

            var imageTasks = new List<Task<Image>>();
            foreach (int id in iconIdSaves)
            {
                if (id != 0)
                {
                    string path = $"{ApplicationData.iconFolder}/{id}.png";
                    imageTasks.Add(LoadImageAsync(path, id));
                }
            }

            var loadedImages = await Task.WhenAll(imageTasks);
            iconImages.AddRange(loadedImages);

            iconPictureBox.Image = await Utility.OverlayIconsAsync(iconImages);

            nameAndIconPanel.Controls.Add(iconPictureBox);

            Label nameLabel = new Label
            {
                Text = $"{item.itemName}",
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
                int statsY = (selectedItemPanel.Controls.OfType<Label>().LastOrDefault()?.Bottom ?? nameAndIconPanel.Bottom) + 10;
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
                    var statEnum = Utility.ParseStatEnum(stat.Name);
                    statCalc[statEnum] = (float)Utility.GetStatsFromProgressions(ApplicationData.Instance.itemProgressions, stat.Scaling, itemLevel);
                }
                catch (ArgumentException ex)
                {
                    Utility.Log(ex.ToString());
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


        private void Item_Explorer_Load(object sender, EventArgs e)
        {

        }
        private async Task ApplyFiltersAsync(string searchText, int? minLevel = null, int? maxLevel = null)
        {
            // Maximale Anzahl anzuzeigender Items
            const int maxItems = 3000;

            // Filterbedingungen festlegen
            Func<Item, bool> filterCondition = item =>
            {
                bool matchesSearchText = string.IsNullOrEmpty(searchText) || item.itemName.ToLower().Contains(searchText.ToLower());
                bool matchesLevel = (!minLevel.HasValue || item.itemLevel >= minLevel) &&
                                    (!maxLevel.HasValue || item.itemLevel <= maxLevel);

                return matchesSearchText && matchesLevel;
            };

            // Items filtern und begrenzen
            var filteredItems = ApplicationData.Instance.itemDb.ItemList
                .Where(filterCondition)
                .Take(maxItems)
                .ToList();

            // UI-Update auf dem Hauptthread ausführen
            await UpdateGridAsync(filteredItems);
        }

        private async Task UpdateGridAsync(List<Item> items)
        {
            // Grid leeren
            itemDatabaseGrid.Invoke(new Action(() => itemDatabaseGrid.Rows.Clear()));

            // Asynchron die Items in Batches hinzufügen
            int batchSize = 100;
            int currentIndex = 0;

            while (currentIndex < items.Count)
            {
                var batch = items.Skip(currentIndex).Take(batchSize);

                // Items im aktuellen Batch verarbeiten
                foreach (var item in batch)
                {
                    // Icon laden
                    List<int> iconIdSaves = Utility.GetIconIDsFromString(new List<int>(), item.itemIcon, "-");
                    iconIdSaves.Reverse();
                    var imageTasks = iconIdSaves
                        .Where(id => id != 0)
                        .Select(id => LoadImageAsync($"{ApplicationData.iconFolder}/{id}.png", id))
                        .ToList();

                    // Bilder laden
                    var loadedImages = await Task.WhenAll(imageTasks);

                    // Icons kombinieren
                    var fullIcon = await Utility.OverlayIconsAsync(loadedImages.ToList());

                    // UI-Update auf Hauptthread
                    itemDatabaseGrid.Invoke(new Action(() =>
                    {
                        itemDatabaseGrid.Rows.Add(fullIcon, item.itemName, item.ArmourType, item.itemLevel);
                    }));
                }

                currentIndex += batchSize;
            }
        }
    }
}
