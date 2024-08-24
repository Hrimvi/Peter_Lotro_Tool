using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace EssenceValueCalculator
{
    public partial class Item_Explorer : Form
    {
        private const string progressionFilePath = "xmls/progressions.xml";
        private const string itemsFilePath = "xmls/items.xml";
        private const string iconFolder = "items";

        private Items itemDb;
        private Progressions itemProgressions;

        private List<int> iconIdSaves = new List<int>();
        private List<Image> iconImages = new List<Image>();

        private int count = 1;
        private TextBox searchTextBox;
        public Item_Explorer()
        {
            InitializeComponent();
            InitializeAsync();

        }
        private async void InitializeAsync()
        {
            itemDb = await Utility.LoadItemsAsync(itemsFilePath);
            itemProgressions = await Utility.LoadProgressionsAsync(progressionFilePath);

            searchTextBox = new TextBox();
            searchTextBox.Width = 200;
            searchTextBox.Location = new Point(10, 10); // Adjust location as needed
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            this.Controls.Add(searchTextBox);

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
                        string path = $"{iconFolder}/{id}.png";
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

        private void itemDatabaseGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
