using System.Data.OleDb;
using System.Globalization;

namespace morente_sample
{
    public partial class inventory : Form
    {
        private const string CategoryPlaceholder = "Select Category";
        private const string UnitPlaceholder = "Select Unit";
        private const string StatusPlaceholder = "Select Status";
        private bool loggingOut;

        public inventory()
        {
            InitializeComponent();

            InitializeInputOptions();
            WireControlEvents();
            ConfigureDataGridView();
            LoadProductsFromDatabase();
            UpdateDashboardCounts();
            ShowAddItemSection();
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.RowHeadersWidth = 40;
            dataGridView1.AllowUserToResizeColumns = true;

            Column1.FillWeight = 45;
            Column2.FillWeight = 150;
            Column3.FillWeight = 120;
            Column4.FillWeight = 130;
            Column5.FillWeight = 75;
            Column6.FillWeight = 90;
            Column7.FillWeight = 110;
        }

        private void InitializeInputOptions()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[]
            {
                "Grocery",
                "School Supplies"
            });

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new object[]
            {
                "In Stock",
                "Low Stock",
                "Out of Stock"
            });

            comboBox1.Text = CategoryPlaceholder;
            comboBox3.Items.Clear();
            comboBox3.Text = UnitPlaceholder;
            comboBox2.Text = StatusPlaceholder;
        }

        private void UpdateUnitOptions()
        {
            string category = comboBox1.Text.Trim();
            comboBox3.Items.Clear();

            if (category.Equals("Grocery", StringComparison.OrdinalIgnoreCase))
            {
                comboBox3.Items.AddRange(new object[]
                {
                    "Kilogram (kg)",
                    "Gram (g)",
                    "Liter (L)",
                    "Milliliter (mL)",
                    "Pack",
                    "Bottle",
                    "Can"
                });
            }
            else if (category.Equals("School Supplies", StringComparison.OrdinalIgnoreCase))
            {
                comboBox3.Items.AddRange(new object[]
                {
                    "Pieces (pcs)",
                    "Pack",
                    "Box",
                    "Ream",
                    "Bundle",
                    "Set"
                });
            }

            comboBox3.SelectedIndex = -1;
            comboBox3.Text = UnitPlaceholder;
        }

        private void WireControlEvents()
        {
            button22.Click += button22_InventoryClick;
            button24.Click += button24_AddItemClick;
            button23.Click += button23_SalesClick;
            button25.Click += button25_ReportsClick;
            button19.Click += button19_AddClick;
            button2.Click += button2_UpdateClick;
            button1.Click += button1_DeleteClick;

            dataGridView1.RowsAdded += dataGridView1_DataChanged;
            dataGridView1.RowsRemoved += dataGridView1_DataChanged;
            dataGridView1.CellValueChanged += dataGridView1_DataChanged;
            dataGridView1.DataSourceChanged += dataGridView1_DataChanged;
            dataGridView1.CellClick += dataGridView1_CellClick;

            FormClosing += inventory_FormClosing;
        }

        private void dataGridView1_DataChanged(object? sender, EventArgs e)
        {
            UpdateDashboardCounts();
        }

        private void LoadProductsFromDatabase()
        {
            try
            {
                Database.EnsureProductUnitColumn();

                using OleDbConnection connection = Database.GetConnection();
                connection.Open();

                using OleDbCommand command = new(
                    "SELECT ID, productName, productCategory, productUnit, quantity, unitPrice, status " +
                    "FROM products ORDER BY ID",
                    connection);
                using OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["ID"],
                        reader["productName"],
                        reader["productCategory"],
                        reader["productUnit"],
                        reader["quantity"],
                        reader["unitPrice"],
                        reader["status"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load inventory data: " + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void UpdateDashboardCounts()
        {
            int totalItems = 0;
            int inStock = 0;
            int lowStock = 0;
            int outOfStock = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow || !HasProductData(row))
                {
                    continue;
                }

                totalItems++;

                string status = Convert.ToString(row.Cells[Column7.Name].Value)?.Trim() ?? string.Empty;
                if (status.Equals("In Stock", StringComparison.OrdinalIgnoreCase) ||
                    status.Equals("Available", StringComparison.OrdinalIgnoreCase))
                {
                    inStock++;
                }
                else if (status.Equals("Low Stock", StringComparison.OrdinalIgnoreCase))
                {
                    lowStock++;
                }
                else if (status.Equals("Out of Stock", StringComparison.OrdinalIgnoreCase))
                {
                    outOfStock++;
                }
            }

            label93.Text = totalItems.ToString();
            label94.Text = inStock.ToString();
            label95.Text = lowStock.ToString();
            label96.Text = outOfStock.ToString();
        }

        private static bool HasProductData(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(cell.Value)))
                {
                    return true;
                }
            }

            return false;
        }

        private void button19_AddClick(object? sender, EventArgs e)
        {
            if (!TryReadProductInput(out string itemName, out string category,
                out string unit, out string quantity, out string price, out string status))
            {
                return;
            }

            int productId = GetNextProductId();

            try
            {
                using OleDbConnection connection = Database.GetConnection();
                connection.Open();

                using OleDbCommand command = new(
                    "INSERT INTO products (ID, productName, productCategory, productUnit, quantity, unitPrice, status) " +
                    "VALUES (?, ?, ?, ?, ?, ?, ?)",
                    connection);
                command.Parameters.AddWithValue("@ID", productId);
                command.Parameters.AddWithValue("@productName", itemName);
                command.Parameters.AddWithValue("@productCategory", category);
                command.Parameters.AddWithValue("@productUnit", unit);
                command.Parameters.AddWithValue("@quantity", int.Parse(quantity, CultureInfo.CurrentCulture));
                command.Parameters.AddWithValue("@unitPrice", decimal.Parse(price, CultureInfo.CurrentCulture));
                command.Parameters.AddWithValue("@status", status);
                command.ExecuteNonQuery();

                dataGridView1.Rows.Add(productId, itemName, category, unit, quantity, price, status);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add the product: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateDashboardCounts();
            ClearEditorFields();
            MessageBox.Show("Product added successfully.", "Add Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_UpdateClick(object? sender, EventArgs e)
        {
            DataGridViewRow? selectedRow = GetSelectedProductRow();
            if (selectedRow == null)
            {
                MessageBox.Show("Select a product to edit first.", "Edit Product", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryReadProductInput(out string itemName, out string category,
                out string unit, out string quantity, out string price, out string status))
            {
                return;
            }

            if (!int.TryParse(Convert.ToString(selectedRow.Cells[Column1.Name].Value), out int productId))
            {
                MessageBox.Show("The selected product has an invalid ID.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using OleDbConnection connection = Database.GetConnection();
                connection.Open();

                using OleDbCommand command = new(
                    "UPDATE products SET productName = ?, productCategory = ?, productUnit = ?, quantity = ?, " +
                    "unitPrice = ?, status = ? WHERE ID = ?",
                    connection);
                command.Parameters.AddWithValue("@productName", itemName);
                command.Parameters.AddWithValue("@productCategory", category);
                command.Parameters.AddWithValue("@productUnit", unit);
                command.Parameters.AddWithValue("@quantity", int.Parse(quantity, CultureInfo.CurrentCulture));
                command.Parameters.AddWithValue("@unitPrice", decimal.Parse(price, CultureInfo.CurrentCulture));
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@ID", productId);
                command.ExecuteNonQuery();

                selectedRow.Cells[Column2.Name].Value = itemName;
                selectedRow.Cells[Column3.Name].Value = category;
                selectedRow.Cells[Column4.Name].Value = unit;
                selectedRow.Cells[Column5.Name].Value = quantity;
                selectedRow.Cells[Column6.Name].Value = price;
                selectedRow.Cells[Column7.Name].Value = status;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update the product: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateDashboardCounts();
            ClearEditorFields();
            MessageBox.Show("Product updated successfully.", "Edit Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_DeleteClick(object? sender, EventArgs e)
        {
            DataGridViewRow? selectedRow = GetSelectedProductRow();
            if (selectedRow == null)
            {
                MessageBox.Show("Select a product to delete first.", "Delete Product", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string itemName = Convert.ToString(selectedRow.Cells[Column2.Name].Value) ?? "this product";
            DialogResult confirmation = MessageBox.Show(
                $"Are you sure you want to delete {itemName}?",
                "Delete Product",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            if (!int.TryParse(Convert.ToString(selectedRow.Cells[Column1.Name].Value), out int productId))
            {
                MessageBox.Show("The selected product has an invalid ID.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using OleDbConnection connection = Database.GetConnection();
                connection.Open();

                using OleDbCommand command = new("DELETE FROM products WHERE ID = ?", connection);
                command.Parameters.AddWithValue("@ID", productId);
                command.ExecuteNonQuery();

                dataGridView1.Rows.Remove(selectedRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete the product: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ClearEditorFields();
            UpdateDashboardCounts();
            MessageBox.Show("Product deleted successfully.", "Delete Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool TryReadProductInput(out string itemName, out string category,
            out string unit, out string quantity, out string price, out string status)
        {
            itemName = textBox1.Text.Trim();
            category = comboBox1.Text.Trim();
            unit = comboBox3.Text.Trim();
            quantity = textBox4.Text.Trim();
            price = textBox5.Text.Trim();
            status = comboBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(itemName) || itemName.Equals("Enter Item Name", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Enter a product name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(category) || category.Equals(CategoryPlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Select a product category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(unit) || unit.Equals(UnitPlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Enter a product unit.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(quantity, out int quantityValue) || quantityValue < 0)
            {
                MessageBox.Show("Enter a valid non-negative quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(price, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal priceValue) || priceValue < 0)
            {
                MessageBox.Show("Enter a valid non-negative price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(status) || status.Equals(StatusPlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Select a product status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            quantity = quantityValue.ToString();
            price = priceValue.ToString("0.##", CultureInfo.CurrentCulture);
            return true;
        }

        private int GetNextProductId()
        {
            int nextId = 1;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow || !int.TryParse(Convert.ToString(row.Cells[Column1.Name].Value), out int productId))
                {
                    continue;
                }

                nextId = Math.Max(nextId, productId + 1);
            }

            return nextId;
        }

        private DataGridViewRow? GetSelectedProductRow()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return null;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            return selectedRow.IsNewRow || !HasProductData(selectedRow) ? null : selectedRow;
        }

        private void dataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            if (row.IsNewRow || !HasProductData(row))
            {
                return;
            }

            textBox1.Text = Convert.ToString(row.Cells[Column2.Name].Value) ?? string.Empty;
            comboBox1.Text = Convert.ToString(row.Cells[Column3.Name].Value) ?? string.Empty;
            UpdateUnitOptions();
            comboBox3.Text = Convert.ToString(row.Cells[Column4.Name].Value) ?? string.Empty;
            textBox4.Text = Convert.ToString(row.Cells[Column5.Name].Value) ?? string.Empty;
            textBox5.Text = Convert.ToString(row.Cells[Column6.Name].Value) ?? string.Empty;
            comboBox2.Text = Convert.ToString(row.Cells[Column7.Name].Value) ?? string.Empty;
        }

        private void ClearEditorFields()
        {
            textBox1.Clear();
            comboBox3.Items.Clear();
            comboBox3.Text = UnitPlaceholder;
            textBox4.Clear();
            textBox5.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox1.Text = CategoryPlaceholder;
            comboBox2.SelectedIndex = -1;
            comboBox2.Text = StatusPlaceholder;
            dataGridView1.ClearSelection();
        }

        private void ActivateNavigationButton(Button selectedButton)
        {
            Button[] navigationButtons =
            {
                button21,
                button22,
                button23,
                button24,
                button25,
                button26
            };

            foreach (Button button in navigationButtons)
            {
                button.UseVisualStyleBackColor = true;
                button.BackColor = SystemColors.Control;
                button.ForeColor = SystemColors.ControlText;
            }

            selectedButton.UseVisualStyleBackColor = false;
            selectedButton.BackColor = Color.FromArgb(221, 232, 208);
            selectedButton.ForeColor = Color.FromArgb(111, 78, 55);
        }

        private void ShowDashboardSection()
        {
            ActivateNavigationButton(button21);
            label2.Text = "Manage Stock, Track Items";
            UpdateDashboardCounts();
            label67.Text = "Inventory Dashboard";
        }

        private void ShowInventorySection()
        {
            ActivateNavigationButton(button22);
            label2.Text = "View and manage inventory records";
            label67.Text = "Inventory Records";
            dataGridView1.ClearSelection();
            UpdateDashboardCounts();
            dataGridView1.Focus();
        }

        private void ShowAddItemSection()
        {
            ActivateNavigationButton(button24);
            label2.Text = "Add and manage inventory items";
            label67.Text = "Add New Item";
            ClearEditorFields();
            textBox1.Focus();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ShowDashboardSection();
        }

        private void button22_InventoryClick(object? sender, EventArgs e)
        {
            ShowInventorySection();
        }

        private void button24_AddItemClick(object? sender, EventArgs e)
        {
            ShowAddItemSection();
        }

        private void button23_SalesClick(object? sender, EventArgs e)
        {
            ActivateNavigationButton(button23);
            label2.Text = "Record and review sales transactions";
            label67.Text = "Sales";
            MessageBox.Show("Sales functionality is not configured yet.", "Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button25_ReportsClick(object? sender, EventArgs e)
        {
            ActivateNavigationButton(button25);
            label2.Text = "View inventory and sales reports";
            label67.Text = "Reports";
            MessageBox.Show("Reports functionality is not configured yet.", "Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            ActivateNavigationButton(button26);
            label2.Text = "Manage application settings";
            label67.Text = "Settings";
            MessageBox.Show("Settings functionality is not configured yet.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            DialogResult confirmation = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            login loginForm = new login();
            loginForm.Show();
            loggingOut = true;
            Close();
        }

        private void inventory_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (loggingOut)
            {
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Are you sure you want to exit the application?",
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label83_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUnitOptions();
        }

        private void label94_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {

        }
    }
}