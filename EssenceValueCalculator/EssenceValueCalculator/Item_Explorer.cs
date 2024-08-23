using System;
using System.Collections;
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
        public Item_Explorer()
        {
            InitializeComponent();
            itemDb = Utility.LoadItems(itemsFilePath);
            itemProgressions = Utility.LoadProgressions(progressionFilePath);


            LoadItems();

        }
        private async Task LoadItems()
        {
            const int batchSize = 10; // Anzahl der maximalen gleichzeitigen Aufgaben
            var count = 0;

            foreach (Item item in itemDb.ItemList)
            {
                iconImages.Clear();
                iconIdSaves = Utility.GetIconIDsFromString(iconIdSaves, item.Icon, "-");
                iconIdSaves.Reverse();

                var imageTasks = new List<Task<Image>>();

                foreach (int id in iconIdSaves)
                {
                    if (id != 0)
                    {
                        string path = $"{iconFolder}/{id}.png";
                        imageTasks.Add(LoadImageAsync(path));
                    }
                }

                // Verarbeite die Bilder in Batches
                await BatchProcessAsync(imageTasks, batchSize);

                Image fullIcon = await Utility.OverlayIconsAsync(iconImages);

                itemDatabaseGrid.Invoke(new Action(() => itemDatabaseGrid.Rows.Add(fullIcon, item.Name)));
                numberText.Invoke(new Action(() => numberText.Text = $"Loaded: {count}"));
                count++;
            }
        }

        private async Task BatchProcessAsync(List<Task<Image>> tasks, int batchSize)
        {
            var taskIndex = 0;
            while (taskIndex < tasks.Count)
            {
                var batchTasks = tasks.Skip(taskIndex).Take(batchSize).ToList();
                var loadedImages = await Task.WhenAll(batchTasks);
                iconImages.AddRange(loadedImages);
                taskIndex += batchSize;
            }
        }
        private Task<Image> LoadImageAsync(string path)
        {
            return Task.Run(() =>
            {
                return Image.FromFile(path);
            });
        }

    }
}
