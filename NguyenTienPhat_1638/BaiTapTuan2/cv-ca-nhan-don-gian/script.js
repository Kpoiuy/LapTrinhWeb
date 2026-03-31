// Đợi DOM load xong
document.addEventListener('DOMContentLoaded', function() {
    
    // Lấy các phần tử DOM
    const welcomeBtn = document.getElementById('welcomeBtn');
    const changeColorBtn = document.getElementById('changeColorBtn');
    const body = document.body;
    
    // Mảng màu nền để đổi
    const colors = ['#f4f4f4', '#e8f4f8', '#fff4e6', '#f0f8e8', '#fce8f3'];
    let currentColorIndex = 0;
    
    // Xử lý nút chào mừng
    if (welcomeBtn) {
        welcomeBtn.addEventListener('click', function() {
            alert('Chào mừng bạn đến với CV của tôi! 👋');
        });
    }
    
    // Xử lý nút đổi màu nền
    if (changeColorBtn) {
        changeColorBtn.addEventListener('click', function() {
            currentColorIndex = (currentColorIndex + 1) % colors.length;
            body.style.backgroundColor = colors[currentColorIndex];
        });
    }
    
});
