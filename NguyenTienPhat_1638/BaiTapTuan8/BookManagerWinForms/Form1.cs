using BookManagerWinForms.Dtos;
using BookManagerWinForms.Services;

namespace BookManagerWinForms;

public partial class Form1 : Form
{
    private readonly BookApiClient _apiClient = new();
    private List<CategoryDto> _categories = new();
    private List<BookDto> _currentBooks = new();

    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        await LoadCategoriesAsync();
        await LoadBooksAsync();
    }

    private async Task LoadCategoriesAsync()
    {
        _categories = await _apiClient.GetCategoriesAsync();

        var searchItems = new List<CategoryComboItem>
        {
            new() { Id = null, Name = "-- Tất cả --" }
        };
        searchItems.AddRange(_categories.Select(x => new CategoryComboItem { Id = x.Id, Name = x.Name }));

        cmbSearchCategory.DataSource = searchItems;
        cmbSearchCategory.DisplayMember = nameof(CategoryComboItem.Name);
        cmbSearchCategory.ValueMember = nameof(CategoryComboItem.Id);

        cmbAddCategory.DataSource = _categories.Select(x => new CategoryComboItem
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
        cmbAddCategory.DisplayMember = nameof(CategoryComboItem.Name);
        cmbAddCategory.ValueMember = nameof(CategoryComboItem.Id);
    }

    private async Task LoadBooksAsync()
    {
        int? categoryId = null;
        if (cmbSearchCategory.SelectedItem is CategoryComboItem item)
        {
            categoryId = item.Id;
        }

        _currentBooks = await _apiClient.SearchBooksAsync(txtKeyword.Text, categoryId);

        dgvBooks.DataSource = _currentBooks.Select(x => new
        {
            x.Id,
            x.Title,
            x.Author,
            x.Year,
            x.Publisher,
            x.Price,
            Category = x.CategoryName
        }).ToList();

        lblCoverStatus.Text = $"Số sách: {_currentBooks.Count}";
        ShowSelectedBookCover();
    }

    private async void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            await LoadBooksAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể tìm kiếm sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnReload_Click(object sender, EventArgs e)
    {
        try
        {
            txtKeyword.Clear();
            cmbSearchCategory.SelectedIndex = 0;
            await LoadBooksAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể tải lại dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnBrowseImage_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Image files|*.jpg;*.jpeg;*.png",
            Title = "Chọn ảnh bìa sách"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtImagePath.Text = dialog.FileName;
        }
    }

    private async void btnAddBook_Click(object sender, EventArgs e)
    {
        if (cmbAddCategory.SelectedItem is not CategoryComboItem selectedCategory || selectedCategory.Id is null)
        {
            MessageBox.Show("Vui lòng chọn category hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
            string.IsNullOrWhiteSpace(txtAuthor.Text) ||
            string.IsNullOrWhiteSpace(txtPublisher.Text))
        {
            MessageBox.Show("Vui lòng nhập đầy đủ Tên sách, Tác giả, Nhà xuất bản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var input = new BookCreateInput
            {
                Title = txtTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                Year = (int)numYear.Value,
                Publisher = txtPublisher.Text.Trim(),
                Price = numPrice.Value,
                CategoryId = selectedCategory.Id.Value,
                ImageFilePath = string.IsNullOrWhiteSpace(txtImagePath.Text) ? null : txtImagePath.Text
            };

            await _apiClient.CreateBookAsync(input);
            MessageBox.Show("Thêm sách thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearAddForm();
            await LoadBooksAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể thêm sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void dgvBooks_SelectionChanged(object sender, EventArgs e)
    {
        ShowSelectedBookCover();
    }

    private void ShowSelectedBookCover()
    {
        if (dgvBooks.CurrentRow is null || dgvBooks.CurrentRow.Index < 0 || dgvBooks.CurrentRow.Index >= _currentBooks.Count)
        {
            picCover.ImageLocation = null;
            lblCoverStatus.Text = "Chưa chọn sách";
            return;
        }

        var book = _currentBooks[dgvBooks.CurrentRow.Index];
        if (string.IsNullOrWhiteSpace(book.ImageUrl))
        {
            picCover.ImageLocation = null;
            lblCoverStatus.Text = $"{book.Title} - Không có ảnh";
            return;
        }

        picCover.ImageLocation = book.ImageUrl;
        lblCoverStatus.Text = $"{book.Title}";
    }

    private void ClearAddForm()
    {
        txtTitle.Clear();
        txtAuthor.Clear();
        txtPublisher.Clear();
        txtImagePath.Clear();
        numYear.Value = 2024;
        numPrice.Value = 0;

        if (cmbAddCategory.Items.Count > 0)
        {
            cmbAddCategory.SelectedIndex = 0;
        }
    }

    private class CategoryComboItem
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
