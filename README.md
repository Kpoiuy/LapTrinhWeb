# BookDB - ASM5 Book Management System

Hệ thống quản lý sách với đầy đủ chức năng CRUD (Create, Read, Update, Delete) được xây dựng bằng ASP.NET Core MVC và Entity Framework Core.

## Tính năng

### Quản lý sách
- ✅ Xem danh sách sách với phân trang (5 sách/trang)
- ✅ Xem chi tiết thông tin sách
- ✅ Thêm sách mới
- ✅ Chỉnh sửa thông tin sách
- ✅ Xóa sách
- ✅ Lọc sách theo danh mục

### Quản lý danh mục
- ✅ Xem danh sách danh mục
- ✅ Hiển thị số lượng sách trong mỗi danh mục
- ✅ Hiển thị sách mẫu cho mỗi danh mục
- ✅ Lọc sách theo danh mục

### Tính năng khác
- ✅ Thêm sách vào giỏ hàng
- ✅ Đặt hàng trực tiếp
- ✅ Quản lý điểm (Grade Management)
- ✅ Validation dữ liệu đầy đủ
- ✅ Error handling và logging
- ✅ Responsive design với Bootstrap

## Công nghệ sử dụng

- **Framework**: ASP.NET Core 8.0 MVC
- **ORM**: Entity Framework Core 8.0
- **Database**: SQLite
- **Frontend**: Bootstrap 5, HTML5, CSS3
- **View Engine**: Razor
- **Validation**: Data Annotations

## Cấu trúc dự án

```
BookDB/
├── Controllers/          # Controllers xử lý request
│   ├── BooksController.cs
│   ├── CategoriesController.cs
│   ├── CartController.cs
│   └── GradeManagementController.cs
├── Data/                # Database context
│   └── BookDbContext.cs
├── Models/              # Entity models
│   ├── Book.cs
│   └── Category.cs
├── ViewModels/          # View models
│   ├── BookListViewModel.cs
│   └── CategoryViewModel.cs
├── Views/               # Razor views
│   ├── Books/
│   ├── Categories/
│   └── Shared/
├── Migrations/          # EF Core migrations
└── wwwroot/            # Static files
```

## Cài đặt và chạy

### Yêu cầu
- .NET 8.0 SDK
- Visual Studio 2022 hoặc VS Code

### Các bước cài đặt

1. Clone repository:
```bash
git clone <repository-url>
cd BookDB
```

2. Restore packages:
```bash
dotnet restore
```

3. Chạy migrations (database đã được tạo sẵn):
```bash
dotnet ef database update
```

4. Chạy ứng dụng:
```bash
dotnet run
```

5. Mở trình duyệt và truy cập:
```
https://localhost:5001
hoặc
http://localhost:5000
```

## Database Schema

### Books Table
- Id (int, PK)
- Title (string, required, max 200)
- Author (string, required, max 100)
- Year (int, required, 1000-2027)
- Publisher (string, required, max 100)
- CategoryId (int, FK)
- ImageUrl (string, optional, max 500)
- Price (decimal, optional)
- Rating (decimal, optional, 0-5)

### Categories Table
- Id (int, PK)
- Name (string, required, max 100, unique)

## Dữ liệu mẫu

Hệ thống đã được seed với:
- 5 danh mục: Văn học, Khoa học, Lịch sử, Kinh tế, Công nghệ
- 10 sách mẫu với đầy đủ thông tin

## Validation Rules

### Book
- Title: Bắt buộc, tối đa 200 ký tự
- Author: Bắt buộc, tối đa 100 ký tự
- Year: Bắt buộc, từ 1000 đến năm hiện tại + 1
- Publisher: Bắt buộc, tối đa 100 ký tự
- Category: Bắt buộc
- Price: Tùy chọn, >= 0
- Rating: Tùy chọn, 0-5

### Category
- Name: Bắt buộc, tối đa 100 ký tự, duy nhất

## Tác giả

Dự án ASM5 - Book Database Management System

## License

This project is for educational purposes.
