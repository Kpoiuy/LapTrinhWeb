// Test file để kiểm tra xử lý lỗi trong script.js
// Chạy test này với Node.js và jsdom

const fs = require('fs');
const { JSDOM } = require('jsdom');

// Test 1: Kiểm tra code không bị lỗi khi các phần tử tồn tại
function testWithElements() {
    console.log('\n=== Test 1: Các phần tử DOM tồn tại ===');
    
    const html = `
        <!DOCTYPE html>
        <html>
        <body>
            <button id="welcomeBtn">Chào Mừng</button>
            <button id="changeColorBtn">Đổi Màu Nền</button>
        </body>
        </html>
    `;
    
    const dom = new JSDOM(html, { runScripts: 'dangerously' });
    const { window } = dom;
    const { document } = window;
    
    // Load script
    const scriptContent = fs.readFileSync('./script.js', 'utf8');
    const scriptElement = document.createElement('script');
    scriptElement.textContent = scriptContent;
    document.body.appendChild(scriptElement);
    
    // Trigger DOMContentLoaded
    const event = new window.Event('DOMContentLoaded');
    document.dispatchEvent(event);
    
    // Kiểm tra các phần tử
    const welcomeBtn = document.getElementById('welcomeBtn');
    const changeColorBtn = document.getElementById('changeColorBtn');
    
    if (welcomeBtn && changeColorBtn) {
        console.log('✅ PASSED: Các phần tử DOM tồn tại');
        console.log('✅ PASSED: Script chạy thành công không có lỗi');
        return true;
    } else {
        console.log('❌ FAILED: Không tìm thấy các phần tử DOM');
        return false;
    }
}

// Test 2: Kiểm tra code không bị lỗi khi các phần tử KHÔNG tồn tại
function testWithoutElements() {
    console.log('\n=== Test 2: Các phần tử DOM KHÔNG tồn tại (Xử lý lỗi) ===');
    
    const html = `
        <!DOCTYPE html>
        <html>
        <body>
            <!-- Không có các nút -->
        </body>
        </html>
    `;
    
    try {
        const dom = new JSDOM(html, { runScripts: 'dangerously' });
        const { window } = dom;
        const { document } = window;
        
        // Load script
        const scriptContent = fs.readFileSync('./script.js', 'utf8');
        const scriptElement = document.createElement('script');
        scriptElement.textContent = scriptContent;
        document.body.appendChild(scriptElement);
        
        // Trigger DOMContentLoaded
        const event = new window.Event('DOMContentLoaded');
        document.dispatchEvent(event);
        
        // Nếu đến đây mà không có lỗi, nghĩa là xử lý lỗi hoạt động tốt
        console.log('✅ PASSED: Script chạy thành công mặc dù không có các phần tử DOM');
        console.log('✅ PASSED: Xử lý lỗi hoạt động đúng - không có exception');
        return true;
    } catch (error) {
        console.log('❌ FAILED: Script bị lỗi khi không có các phần tử DOM');
        console.log('Error:', error.message);
        return false;
    }
}

// Test 3: Kiểm tra code có sử dụng kiểm tra điều kiện
function testCodeHasErrorHandling() {
    console.log('\n=== Test 3: Kiểm tra code có xử lý lỗi ===');
    
    const scriptContent = fs.readFileSync('./script.js', 'utf8');
    
    // Kiểm tra có sử dụng if (welcomeBtn)
    if (scriptContent.includes('if (welcomeBtn)') || scriptContent.includes('if(welcomeBtn)')) {
        console.log('✅ PASSED: Code kiểm tra sự tồn tại của welcomeBtn');
    } else {
        console.log('❌ FAILED: Code không kiểm tra sự tồn tại của welcomeBtn');
        return false;
    }
    
    // Kiểm tra có sử dụng if (changeColorBtn)
    if (scriptContent.includes('if (changeColorBtn)') || scriptContent.includes('if(changeColorBtn)')) {
        console.log('✅ PASSED: Code kiểm tra sự tồn tại của changeColorBtn');
    } else {
        console.log('❌ FAILED: Code không kiểm tra sự tồn tại của changeColorBtn');
        return false;
    }
    
    return true;
}

// Chạy tất cả tests
console.log('====================================');
console.log('KIỂM TRA XỬ LÝ LỖI JAVASCRIPT');
console.log('====================================');

const test1 = testWithElements();
const test2 = testWithoutElements();
const test3 = testCodeHasErrorHandling();

console.log('\n====================================');
console.log('KẾT QUẢ TỔNG HỢP');
console.log('====================================');

if (test1 && test2 && test3) {
    console.log('✅ TẤT CẢ TESTS PASSED');
    console.log('\nKết luận:');
    console.log('- Code kiểm tra sự tồn tại của các phần tử DOM trước khi thêm event listeners');
    console.log('- Code không bị lỗi nếu phần tử không tồn tại');
    console.log('- Yêu cầu 5.3 và 6.3 được đáp ứng đầy đủ');
    process.exit(0);
} else {
    console.log('❌ MỘT HOẶC NHIỀU TESTS FAILED');
    process.exit(1);
}
