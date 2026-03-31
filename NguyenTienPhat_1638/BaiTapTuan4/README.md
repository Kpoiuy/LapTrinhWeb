# Bài Tập Tuần 4

Thư mục này chứa 2 bài tập ASP.NET Core MVC:

## Bài Tập 1: Form Đăng Ký Người Dùng
- **Thư mục**: `bai-tap-1/`
- **Chức năng**: Form đăng ký với validation
  - Tên đăng nhập: 3-30 ký tự
  - Email: định dạng hợp lệ
  - Mật khẩu: tối thiểu 6 ký tự
  - Xác nhận mật khẩu: phải khớp với mật khẩu

### Cách chạy Bài Tập 1:
```bash
cd bai-tap-1
dotnet run
```
Truy cập: https://localhost:5001 hoặc http://localhost:5000

## Bài Tập 2: Quản Lý Công Việc (TodoList)
- **Thư mục**: `bai-tap-2/`
- **Chức năng**: CRUD đầy đủ cho công việc
  - Danh sách công việc
  - Thêm công việc mới (với mã công việc tự nhập)
  - Xem chi tiết công việc
  - Sửa công việc
  - Xoá công việc (có trang xác nhận)
  - Đánh dấu trạng thái hoàn thành

### Cách chạy Bài Tập 2:
```bash
cd bai-tap-2
dotnet run
```
Truy cập: https://localhost:5001 hoặc http://localhost:5000

## Yêu cầu hệ thống:
- .NET 8.0 SDK
- Windows/Linux/macOS

## Lưu ý:
- Dữ liệu được lưu trong file JSON (Data/users.json và Data/todos.json)
- UI có viền màu tím và header màu đỏ theo yêu cầu
- Mỗi action có trang riêng theo mô hình MVC
