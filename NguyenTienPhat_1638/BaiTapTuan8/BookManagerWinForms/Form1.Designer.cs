namespace BookManagerWinForms;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlSearch = new Panel();
        btnReload = new Button();
        cmbSearchCategory = new ComboBox();
        lblSearchCategory = new Label();
        btnSearch = new Button();
        txtKeyword = new TextBox();
        lblKeyword = new Label();
        dgvBooks = new DataGridView();
        grpAddBook = new GroupBox();
        numPrice = new NumericUpDown();
        lblPrice = new Label();
        btnBrowseImage = new Button();
        txtImagePath = new TextBox();
        lblImage = new Label();
        cmbAddCategory = new ComboBox();
        lblAddCategory = new Label();
        numYear = new NumericUpDown();
        lblYear = new Label();
        txtPublisher = new TextBox();
        lblPublisher = new Label();
        txtAuthor = new TextBox();
        lblAuthor = new Label();
        txtTitle = new TextBox();
        lblTitle = new Label();
        btnAddBook = new Button();
        grpCover = new GroupBox();
        lblCoverStatus = new Label();
        picCover = new PictureBox();
        pnlSearch.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
        grpAddBook.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numYear).BeginInit();
        grpCover.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picCover).BeginInit();
        SuspendLayout();
        // 
        // pnlSearch
        // 
        pnlSearch.Controls.Add(btnReload);
        pnlSearch.Controls.Add(cmbSearchCategory);
        pnlSearch.Controls.Add(lblSearchCategory);
        pnlSearch.Controls.Add(btnSearch);
        pnlSearch.Controls.Add(txtKeyword);
        pnlSearch.Controls.Add(lblKeyword);
        pnlSearch.Location = new Point(12, 12);
        pnlSearch.Name = "pnlSearch";
        pnlSearch.Size = new Size(1220, 74);
        pnlSearch.TabIndex = 0;
        // 
        // btnReload
        // 
        btnReload.Location = new Point(849, 22);
        btnReload.Name = "btnReload";
        btnReload.Size = new Size(95, 29);
        btnReload.TabIndex = 5;
        btnReload.Text = "Tải lại";
        btnReload.UseVisualStyleBackColor = true;
        btnReload.Click += btnReload_Click;
        // 
        // cmbSearchCategory
        // 
        cmbSearchCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSearchCategory.FormattingEnabled = true;
        cmbSearchCategory.Location = new Point(428, 24);
        cmbSearchCategory.Name = "cmbSearchCategory";
        cmbSearchCategory.Size = new Size(204, 28);
        cmbSearchCategory.TabIndex = 2;
        // 
        // lblSearchCategory
        // 
        lblSearchCategory.AutoSize = true;
        lblSearchCategory.Location = new Point(316, 28);
        lblSearchCategory.Name = "lblSearchCategory";
        lblSearchCategory.Size = new Size(106, 20);
        lblSearchCategory.TabIndex = 3;
        lblSearchCategory.Text = "Lọc Category:";
        // 
        // btnSearch
        // 
        btnSearch.Location = new Point(748, 22);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(95, 29);
        btnSearch.TabIndex = 4;
        btnSearch.Text = "Tìm kiếm";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        // 
        // txtKeyword
        // 
        txtKeyword.Location = new Point(82, 24);
        txtKeyword.Name = "txtKeyword";
        txtKeyword.Size = new Size(214, 27);
        txtKeyword.TabIndex = 1;
        // 
        // lblKeyword
        // 
        lblKeyword.AutoSize = true;
        lblKeyword.Location = new Point(3, 28);
        lblKeyword.Name = "lblKeyword";
        lblKeyword.Size = new Size(73, 20);
        lblKeyword.TabIndex = 0;
        lblKeyword.Text = "Từ khóa:";
        // 
        // dgvBooks
        // 
        dgvBooks.AllowUserToAddRows = false;
        dgvBooks.AllowUserToDeleteRows = false;
        dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvBooks.Location = new Point(12, 92);
        dgvBooks.MultiSelect = false;
        dgvBooks.Name = "dgvBooks";
        dgvBooks.ReadOnly = true;
        dgvBooks.RowHeadersWidth = 51;
        dgvBooks.RowTemplate.Height = 29;
        dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBooks.Size = new Size(960, 320);
        dgvBooks.TabIndex = 1;
        dgvBooks.SelectionChanged += dgvBooks_SelectionChanged;
        // 
        // grpAddBook
        // 
        grpAddBook.Controls.Add(numPrice);
        grpAddBook.Controls.Add(lblPrice);
        grpAddBook.Controls.Add(btnBrowseImage);
        grpAddBook.Controls.Add(txtImagePath);
        grpAddBook.Controls.Add(lblImage);
        grpAddBook.Controls.Add(cmbAddCategory);
        grpAddBook.Controls.Add(lblAddCategory);
        grpAddBook.Controls.Add(numYear);
        grpAddBook.Controls.Add(lblYear);
        grpAddBook.Controls.Add(txtPublisher);
        grpAddBook.Controls.Add(lblPublisher);
        grpAddBook.Controls.Add(txtAuthor);
        grpAddBook.Controls.Add(lblAuthor);
        grpAddBook.Controls.Add(txtTitle);
        grpAddBook.Controls.Add(lblTitle);
        grpAddBook.Controls.Add(btnAddBook);
        grpAddBook.Location = new Point(12, 418);
        grpAddBook.Name = "grpAddBook";
        grpAddBook.Size = new Size(1220, 220);
        grpAddBook.TabIndex = 2;
        grpAddBook.TabStop = false;
        grpAddBook.Text = "Thêm sách mới từ Web API";
        // 
        // numPrice
        // 
        numPrice.Location = new Point(622, 120);
        numPrice.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        numPrice.Name = "numPrice";
        numPrice.Size = new Size(168, 27);
        numPrice.TabIndex = 13;
        // 
        // lblPrice
        // 
        lblPrice.AutoSize = true;
        lblPrice.Location = new Point(507, 122);
        lblPrice.Name = "lblPrice";
        lblPrice.Size = new Size(66, 20);
        lblPrice.TabIndex = 12;
        lblPrice.Text = "Giá tiền:";
        // 
        // btnBrowseImage
        // 
        btnBrowseImage.Location = new Point(861, 74);
        btnBrowseImage.Name = "btnBrowseImage";
        btnBrowseImage.Size = new Size(83, 29);
        btnBrowseImage.TabIndex = 11;
        btnBrowseImage.Text = "Chọn...";
        btnBrowseImage.UseVisualStyleBackColor = true;
        btnBrowseImage.Click += btnBrowseImage_Click;
        // 
        // txtImagePath
        // 
        txtImagePath.Location = new Point(622, 75);
        txtImagePath.Name = "txtImagePath";
        txtImagePath.ReadOnly = true;
        txtImagePath.Size = new Size(233, 27);
        txtImagePath.TabIndex = 10;
        // 
        // lblImage
        // 
        lblImage.AutoSize = true;
        lblImage.Location = new Point(507, 78);
        lblImage.Name = "lblImage";
        lblImage.Size = new Size(84, 20);
        lblImage.TabIndex = 9;
        lblImage.Text = "Hình ảnh:";
        // 
        // cmbAddCategory
        // 
        cmbAddCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbAddCategory.FormattingEnabled = true;
        cmbAddCategory.Location = new Point(622, 31);
        cmbAddCategory.Name = "cmbAddCategory";
        cmbAddCategory.Size = new Size(322, 28);
        cmbAddCategory.TabIndex = 8;
        // 
        // lblAddCategory
        // 
        lblAddCategory.AutoSize = true;
        lblAddCategory.Location = new Point(507, 34);
        lblAddCategory.Name = "lblAddCategory";
        lblAddCategory.Size = new Size(76, 20);
        lblAddCategory.TabIndex = 7;
        lblAddCategory.Text = "Category:";
        // 
        // numYear
        // 
        numYear.Location = new Point(116, 161);
        numYear.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
        numYear.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
        numYear.Name = "numYear";
        numYear.Size = new Size(367, 27);
        numYear.TabIndex = 6;
        numYear.Value = new decimal(new int[] { 2024, 0, 0, 0 });
        // 
        // lblYear
        // 
        lblYear.AutoSize = true;
        lblYear.Location = new Point(23, 163);
        lblYear.Name = "lblYear";
        lblYear.Size = new Size(68, 20);
        lblYear.TabIndex = 5;
        lblYear.Text = "Năm XB:";
        // 
        // txtPublisher
        // 
        txtPublisher.Location = new Point(116, 118);
        txtPublisher.Name = "txtPublisher";
        txtPublisher.Size = new Size(367, 27);
        txtPublisher.TabIndex = 4;
        // 
        // lblPublisher
        // 
        lblPublisher.AutoSize = true;
        lblPublisher.Location = new Point(23, 121);
        lblPublisher.Name = "lblPublisher";
        lblPublisher.Size = new Size(97, 20);
        lblPublisher.TabIndex = 3;
        lblPublisher.Text = "Nhà xuất bản:";
        // 
        // txtAuthor
        // 
        txtAuthor.Location = new Point(116, 74);
        txtAuthor.Name = "txtAuthor";
        txtAuthor.Size = new Size(367, 27);
        txtAuthor.TabIndex = 2;
        // 
        // lblAuthor
        // 
        lblAuthor.AutoSize = true;
        lblAuthor.Location = new Point(23, 77);
        lblAuthor.Name = "lblAuthor";
        lblAuthor.Size = new Size(59, 20);
        lblAuthor.TabIndex = 1;
        lblAuthor.Text = "Tác giả:";
        // 
        // txtTitle
        // 
        txtTitle.Location = new Point(116, 31);
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(367, 27);
        txtTitle.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(23, 34);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(71, 20);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Tên sách:";
        // 
        // btnAddBook
        // 
        btnAddBook.Location = new Point(833, 173);
        btnAddBook.Name = "btnAddBook";
        btnAddBook.Size = new Size(111, 31);
        btnAddBook.TabIndex = 14;
        btnAddBook.Text = "Thêm sách";
        btnAddBook.UseVisualStyleBackColor = true;
        btnAddBook.Click += btnAddBook_Click;
        // 
        // grpCover
        // 
        grpCover.Controls.Add(lblCoverStatus);
        grpCover.Controls.Add(picCover);
        grpCover.Location = new Point(978, 92);
        grpCover.Name = "grpCover";
        grpCover.Size = new Size(254, 320);
        grpCover.TabIndex = 3;
        grpCover.TabStop = false;
        grpCover.Text = "Ảnh bìa";
        // 
        // lblCoverStatus
        // 
        lblCoverStatus.Location = new Point(10, 287);
        lblCoverStatus.Name = "lblCoverStatus";
        lblCoverStatus.Size = new Size(236, 26);
        lblCoverStatus.TabIndex = 1;
        lblCoverStatus.Text = "Chưa chọn sách";
        // 
        // picCover
        // 
        picCover.BorderStyle = BorderStyle.FixedSingle;
        picCover.Location = new Point(10, 28);
        picCover.Name = "picCover";
        picCover.Size = new Size(236, 250);
        picCover.SizeMode = PictureBoxSizeMode.Zoom;
        picCover.TabIndex = 0;
        picCover.TabStop = false;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1244, 650);
        Controls.Add(grpCover);
        Controls.Add(grpAddBook);
        Controls.Add(dgvBooks);
        Controls.Add(pnlSearch);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Book Manager - BaiTapTuan8";
        Load += Form1_Load;
        pnlSearch.ResumeLayout(false);
        pnlSearch.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
        grpAddBook.ResumeLayout(false);
        grpAddBook.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numYear).EndInit();
        grpCover.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picCover).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlSearch;
    private Button btnReload;
    private ComboBox cmbSearchCategory;
    private Label lblSearchCategory;
    private Button btnSearch;
    private TextBox txtKeyword;
    private Label lblKeyword;
    private DataGridView dgvBooks;
    private GroupBox grpAddBook;
    private NumericUpDown numYear;
    private Label lblYear;
    private TextBox txtPublisher;
    private Label lblPublisher;
    private TextBox txtAuthor;
    private Label lblAuthor;
    private TextBox txtTitle;
    private Label lblTitle;
    private ComboBox cmbAddCategory;
    private Label lblAddCategory;
    private Button btnBrowseImage;
    private TextBox txtImagePath;
    private Label lblImage;
    private NumericUpDown numPrice;
    private Label lblPrice;
    private Button btnAddBook;
    private GroupBox grpCover;
    private Label lblCoverStatus;
    private PictureBox picCover;
}
