/* =========================================================
   COURSE REGISTRATION APPLICATION - DATA + VIEW/FUNCTION/PROCEDURE
   SQL Server / ASP.NET Core Identity
   KHONG TAO BANG MOI - CHI RESET DATA + INSERT + VIEW + FUNCTION + PROCEDURE
   ========================================================= */

USE [CourseRegistrationDb];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
GO

/* =========================================================
   0) DROP CAC OBJECT CU (NEU TON TAI)
   ========================================================= */
IF OBJECT_ID(N'dbo.sp_CourseEnrollmentSummary', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_CourseEnrollmentSummary;
IF OBJECT_ID(N'dbo.sp_SearchCourses', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_SearchCourses;
IF OBJECT_ID(N'dbo.sp_GetMyCourses', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetMyCourses;
IF OBJECT_ID(N'dbo.sp_CancelEnrollment', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_CancelEnrollment;
IF OBJECT_ID(N'dbo.sp_EnrollCourse', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EnrollCourse;
IF OBJECT_ID(N'dbo.sp_Logout', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_Logout;
IF OBJECT_ID(N'dbo.sp_Login', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_Login;
IF OBJECT_ID(N'dbo.sp_RegisterStudent', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegisterStudent;
IF OBJECT_ID(N'dbo.sp_GetCoursesPaged', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetCoursesPaged;
IF OBJECT_ID(N'dbo.sp_DeleteCourse', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_DeleteCourse;
IF OBJECT_ID(N'dbo.sp_UpdateCourse', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_UpdateCourse;
IF OBJECT_ID(N'dbo.sp_CreateCourse', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_CreateCourse;

IF OBJECT_ID(N'dbo.fn_TotalCreditsByUser', N'FN') IS NOT NULL DROP FUNCTION dbo.fn_TotalCreditsByUser;
IF OBJECT_ID(N'dbo.fn_CountMyCourses', N'FN') IS NOT NULL DROP FUNCTION dbo.fn_CountMyCourses;
IF OBJECT_ID(N'dbo.fn_SearchCourses', N'IF') IS NOT NULL DROP FUNCTION dbo.fn_SearchCourses;

IF OBJECT_ID(N'dbo.vw_CourseEnrollmentCount', N'V') IS NOT NULL DROP VIEW dbo.vw_CourseEnrollmentCount;
IF OBJECT_ID(N'dbo.vw_MyCourses', N'V') IS NOT NULL DROP VIEW dbo.vw_MyCourses;
IF OBJECT_ID(N'dbo.vw_CourseList', N'V') IS NOT NULL DROP VIEW dbo.vw_CourseList;
GO

/* =========================================================
   1) RESET DU LIEU + SEED IDENTITY USER/ROLE
   - Chi 2 role: ADMIN, STUDENT
   - Chi 2 user mau: admin01, student01
   ========================================================= */
DECLARE @AdminHash NVARCHAR(MAX) =
(
    SELECT TOP (1) PasswordHash
    FROM dbo.AspNetUsers
    WHERE UserName IN (N'admin01', N'admin')
    ORDER BY CASE WHEN UserName = N'admin01' THEN 0 ELSE 1 END
);

DECLARE @StudentHash NVARCHAR(MAX) =
(
    SELECT TOP (1) PasswordHash
    FROM dbo.AspNetUsers
    WHERE UserName IN (N'student01', N'student')
    ORDER BY CASE WHEN UserName = N'student01' THEN 0 ELSE 1 END
);

/* fallback hash de script van chay duoc lan dau */
IF @AdminHash IS NULL
    SET @AdminHash = N'AQAAAAIAAYagAAAAEGSHLkWz/bXzhxGmVeIpWCD/pIJECz8fyj0JjD3SPw4mlxsQQ5FSVo4NkrM6G/yHSg==';
IF @StudentHash IS NULL
    SET @StudentHash = N'AQAAAAIAAYagAAAAEBq/5dqqd2ltYPh0r6XqBc91gDHnPh+2hRbN3Kc11wiyzcetrARPF8vgTaSgHmp31A==';

DELETE FROM dbo.Enrollments;
DELETE FROM dbo.Courses;
DELETE FROM dbo.Categories;

DBCC CHECKIDENT ('dbo.Enrollments', RESEED, 0);
DBCC CHECKIDENT ('dbo.Courses', RESEED, 0);
DBCC CHECKIDENT ('dbo.Categories', RESEED, 0);

DELETE FROM dbo.AspNetUserTokens;
DELETE FROM dbo.AspNetUserLogins;
DELETE FROM dbo.AspNetUserClaims;
DELETE FROM dbo.AspNetUserRoles;
DELETE FROM dbo.AspNetRoleClaims;
DELETE FROM dbo.AspNetUsers;
DELETE FROM dbo.AspNetRoles;

INSERT INTO dbo.AspNetRoles (Id, [Name], NormalizedName, ConcurrencyStamp)
VALUES
    (N'ROLE_ADMIN_001', N'ADMIN', N'ADMIN', CONVERT(NVARCHAR(36), NEWID())),
    (N'ROLE_STUDENT_001', N'STUDENT', N'STUDENT', CONVERT(NVARCHAR(36), NEWID()));

INSERT INTO dbo.AspNetUsers
(
    Id, UserName, NormalizedUserName, Email, NormalizedEmail,
    EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
    PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount
)
VALUES
(
    N'USR_ADMIN_001',
    N'admin01',
    N'ADMIN01',
    N'admin01@university.edu.vn',
    N'ADMIN01@UNIVERSITY.EDU.VN',
    1,
    @AdminHash,
    CONVERT(NVARCHAR(36), NEWID()),
    CONVERT(NVARCHAR(36), NEWID()),
    0, 0, 1, 0
),
(
    N'USR_STUDENT_001',
    N'student01',
    N'STUDENT01',
    N'student01@university.edu.vn',
    N'STUDENT01@UNIVERSITY.EDU.VN',
    1,
    @StudentHash,
    CONVERT(NVARCHAR(36), NEWID()),
    CONVERT(NVARCHAR(36), NEWID()),
    0, 0, 1, 0
);

INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
VALUES
    (N'USR_ADMIN_001', N'ROLE_ADMIN_001'),
    (N'USR_STUDENT_001', N'ROLE_STUDENT_001');

/* =========================================================
   2) CATEGORY - 6 MAU
   ========================================================= */
SET IDENTITY_INSERT dbo.Categories ON;
INSERT INTO dbo.Categories (Id, [Name])
VALUES
    (1, N'CÃ´ng nghá»‡ thÃ´ng tin'),
    (2, N'Kinh táº¿'),
    (3, N'Ngoáº¡i ngá»¯'),
    (4, N'Ká»¹ nÄƒng má»m'),
    (5, N'ToÃ¡n á»©ng dá»¥ng'),
    (6, N'TrÃ¬nh Ä‘á»™ cÆ¡ sá»Ÿ');
SET IDENTITY_INSERT dbo.Categories OFF;

/* =========================================================
   3) COURSE - 22 MAU
   ========================================================= */
SET IDENTITY_INSERT dbo.Courses ON;
INSERT INTO dbo.Courses (Id, [Name], Image, Credits, Lecturer, CategoryId)
VALUES
    (1,  N'Láº­p trÃ¬nh C# cÆ¡ báº£n',                 N'images/csharp-basic\.svg',       3, N'TS. Nguyá»…n VÄƒn An',    1),
    (2,  N'ASP.NET Core MVC',                    N'images/aspnet-core-mvc\.svg',    3, N'TS. Tráº§n Minh Khoa',   1),
    (3,  N'Entity Framework Core',               N'images/ef-core\.svg',            3, N'ThS. LÃª Quang Huy',    1),
    (4,  N'CÆ¡ sá»Ÿ dá»¯ liá»‡u SQL Server',            N'images/sql-server\.svg',         3, N'TS. Pháº¡m Thu HÃ ',      1),
    (5,  N'Láº­p trÃ¬nh Web Frontend',              N'images/frontend\.svg',           3, N'ThS. Äáº·ng Háº£i Nam',    1),
    (6,  N'PhÃ¢n tÃ­ch thiáº¿t káº¿ há»‡ thá»‘ng',         N'images/system-analysis\.svg',    3, N'TS. VÃµ Thanh BÃ¬nh',    1),
    (7,  N'TrÃ­ tuá»‡ nhÃ¢n táº¡o nháº­p mÃ´n',           N'images/ai-intro\.svg',           3, N'TS. HoÃ ng Gia Báº£o',    1),
    (8,  N'Quáº£n trá»‹ doanh nghiá»‡p',               N'images/business-admin\.svg',     2, N'ThS. Nguyá»…n Thá»‹ Lan',  2),
    (9,  N'NguyÃªn lÃ½ marketing',                 N'images/marketing\.svg',          2, N'TS. Phan Äá»©c TÃ i',     2),
    (10, N'Káº¿ toÃ¡n cÄƒn báº£n',                     N'images/accounting\.svg',         2, N'ThS. Há»“ Thá»‹ Háº¡nh',     2),
    (11, N'Tiáº¿ng Anh giao tiáº¿p 1',               N'images/english-1\.svg',          2, N'Ms. Sarah Tran',       3),
    (12, N'Tiáº¿ng Anh giao tiáº¿p 2',               N'images/english-2\.svg',          2, N'Mr. John Nguyen',      3),
    (13, N'Tiáº¿ng Nháº­t sÆ¡ cáº¥p N5',                N'images/japanese-n5\.svg',        2, N'ThS. Mai KhÃ¡nh Ly',    3),
    (14, N'Ká»¹ nÄƒng thuyáº¿t trÃ¬nh',                N'images/presentation-skill\.svg', 2, N'ThS. BÃ¹i Ngá»c Ãnh',    4),
    (15, N'Ká»¹ nÄƒng lÃ m viá»‡c nhÃ³m',               N'images/teamwork-skill\.svg',     2, N'ThS. Trá»‹nh Quá»‘c DÅ©ng', 4),
    (16, N'Ká»¹ nÄƒng quáº£n lÃ½ thá»i gian',           N'images/time-management\.svg',    2, N'ThS. Pháº¡m Má»¹ Linh',    4),
    (17, N'ToÃ¡n cao cáº¥p A1',                     N'images/math-a1\.svg',            3, N'TS. LÆ°u VÄƒn Háº£i',      5),
    (18, N'XÃ¡c suáº¥t thá»‘ng kÃª',                   N'images/probability\.svg',        3, N'TS. Nguyá»…n XuÃ¢n PhÃºc', 5),
    (19, N'ToÃ¡n rá»i ráº¡c',                        N'images/discrete-math\.svg',      3, N'TS. Tráº§n Há»¯u NghÄ©a',   5),
    (20, N'Tin há»c Ä‘áº¡i cÆ°Æ¡ng',                   N'images/basic-it\.svg',           3, N'ThS. VÅ© Quá»‘c KhÃ¡nh',   6),
    (21, N'PhÃ¡p luáº­t Ä‘áº¡i cÆ°Æ¡ng',                 N'images/basic-law\.svg',          2, N'ThS. Äá»— Ngá»c Mai',     6),
    (22, N'PhÆ°Æ¡ng phÃ¡p há»c Ä‘áº¡i há»c',             N'images/university-method\.svg',  2, N'ThS. LÃª Thá»‹ BÃ­ch',     6);
SET IDENTITY_INSERT dbo.Courses OFF;

/* =========================================================
   4) ENROLLMENT - 12 MAU
   ========================================================= */
SET IDENTITY_INSERT dbo.Enrollments ON;
INSERT INTO dbo.Enrollments (Id, UserId, CourseId, EnrollDate)
VALUES
    (1,  N'USR_STUDENT_001', 1,  '2026-02-10'),
    (2,  N'USR_STUDENT_001', 2,  '2026-02-10'),
    (3,  N'USR_STUDENT_001', 3,  '2026-02-11'),
    (4,  N'USR_STUDENT_001', 4,  '2026-02-12'),
    (5,  N'USR_STUDENT_001', 11, '2026-02-12'),
    (6,  N'USR_STUDENT_001', 14, '2026-02-13'),
    (7,  N'USR_STUDENT_001', 15, '2026-02-13'),
    (8,  N'USR_STUDENT_001', 17, '2026-02-14'),
    (9,  N'USR_STUDENT_001', 18, '2026-02-14'),
    (10, N'USR_STUDENT_001', 20, '2026-02-15'),
    (11, N'USR_STUDENT_001', 21, '2026-02-16'),
    (12, N'USR_STUDENT_001', 22, '2026-02-17');
SET IDENTITY_INSERT dbo.Enrollments OFF;
GO

/* =========================================================
   5) VIEW
   ========================================================= */
CREATE VIEW dbo.vw_CourseList
AS
SELECT
    c.Id,
    c.[Name]       AS CourseName,
    c.Image,
    c.Credits,
    c.Lecturer,
    c.CategoryId,
    cat.[Name]     AS CategoryName
FROM dbo.Courses c
INNER JOIN dbo.Categories cat ON c.CategoryId = cat.Id;
GO

CREATE VIEW dbo.vw_MyCourses
AS
SELECT
    e.Id           AS EnrollmentId,
    e.UserId,
    u.UserName,
    c.Id           AS CourseId,
    c.[Name]       AS CourseName,
    c.Credits,
    c.Lecturer,
    c.Image,
    cat.[Name]     AS CategoryName,
    e.EnrollDate
FROM dbo.Enrollments e
INNER JOIN dbo.AspNetUsers u ON e.UserId = u.Id
INNER JOIN dbo.Courses c ON e.CourseId = c.Id
INNER JOIN dbo.Categories cat ON c.CategoryId = cat.Id;
GO

CREATE VIEW dbo.vw_CourseEnrollmentCount
AS
SELECT
    c.Id AS CourseId,
    c.[Name] AS CourseName,
    COUNT(e.Id) AS TotalEnrollments
FROM dbo.Courses c
LEFT JOIN dbo.Enrollments e ON c.Id = e.CourseId
GROUP BY c.Id, c.[Name];
GO

/* =========================================================
   6) FUNCTION
   ========================================================= */
CREATE FUNCTION dbo.fn_SearchCourses
(
    @Keyword NVARCHAR(200)
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        c.Id,
        c.[Name]       AS CourseName,
        c.Image,
        c.Credits,
        c.Lecturer,
        cat.[Name]     AS CategoryName
    FROM dbo.Courses c
    INNER JOIN dbo.Categories cat ON c.CategoryId = cat.Id
    WHERE c.[Name] LIKE N'%' + @Keyword + N'%'
);
GO

CREATE FUNCTION dbo.fn_CountMyCourses
(
    @UserId NVARCHAR(450)
)
RETURNS INT
AS
BEGIN
    DECLARE @Total INT;

    SELECT @Total = COUNT(*)
    FROM dbo.Enrollments
    WHERE UserId = @UserId;

    RETURN ISNULL(@Total, 0);
END;
GO

CREATE FUNCTION dbo.fn_TotalCreditsByUser
(
    @UserId NVARCHAR(450)
)
RETURNS INT
AS
BEGIN
    DECLARE @TotalCredits INT;

    SELECT @TotalCredits = SUM(c.Credits)
    FROM dbo.Enrollments e
    INNER JOIN dbo.Courses c ON e.CourseId = c.Id
    WHERE e.UserId = @UserId;

    RETURN ISNULL(@TotalCredits, 0);
END;
GO

/* =========================================================
   7) PROCEDURE - COURSE CRUD + PAGING
   ========================================================= */
CREATE PROCEDURE dbo.sp_CreateCourse
    @Name NVARCHAR(255),
    @Image NVARCHAR(2000),
    @Credits INT,
    @Lecturer NVARCHAR(255),
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Courses ([Name], Image, Credits, Lecturer, CategoryId)
    VALUES (@Name, @Image, @Credits, @Lecturer, @CategoryId);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewCourseId;
END;
GO

CREATE PROCEDURE dbo.sp_UpdateCourse
    @Id INT,
    @Name NVARCHAR(255),
    @Image NVARCHAR(2000),
    @Credits INT,
    @Lecturer NVARCHAR(255),
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Courses
    SET
        [Name] = @Name,
        Image = @Image,
        Credits = @Credits,
        Lecturer = @Lecturer,
        CategoryId = @CategoryId
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE dbo.sp_DeleteCourse
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Enrollments WHERE CourseId = @Id;
    DELETE FROM dbo.Courses WHERE Id = @Id;
END;
GO

CREATE PROCEDURE dbo.sp_GetCoursesPaged
    @PageNumber INT = 1,
    @PageSize INT = 5
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id,
        c.[Name] AS CourseName,
        c.Image,
        c.Credits,
        c.Lecturer,
        cat.[Name] AS CategoryName
    FROM dbo.Courses c
    INNER JOIN dbo.Categories cat ON c.CategoryId = cat.Id
    ORDER BY c.Id
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

/* =========================================================
   8) PROCEDURE - REGISTER / LOGIN / LOGOUT (SQL LAYER DEMO)
   ========================================================= */
CREATE PROCEDURE dbo.sp_RegisterStudent
    @UserId NVARCHAR(450),
    @Username NVARCHAR(256),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AspNetUsers
        WHERE NormalizedUserName = UPPER(@Username)
           OR NormalizedEmail = UPPER(@Email)
    )
    BEGIN
        RAISERROR (N'TÃ i khoáº£n hoáº·c email Ä‘Ã£ tá»“n táº¡i.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.AspNetUsers
    (
        Id, UserName, NormalizedUserName, Email, NormalizedEmail,
        EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
        PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount
    )
    VALUES
    (
        @UserId,
        @Username,
        UPPER(@Username),
        @Email,
        UPPER(@Email),
        1,
        @PasswordHash,
        CONVERT(NVARCHAR(36), NEWID()),
        CONVERT(NVARCHAR(36), NEWID()),
        0, 0, 1, 0
    );

    DECLARE @StudentRoleId NVARCHAR(450);
    SELECT TOP (1) @StudentRoleId = Id
    FROM dbo.AspNetRoles
    WHERE NormalizedName = N'STUDENT';

    IF @StudentRoleId IS NULL
    BEGIN
        RAISERROR (N'KhÃ´ng tÃ¬m tháº¥y role STUDENT.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
    VALUES (@UserId, @StudentRoleId);
END;
GO

CREATE PROCEDURE dbo.sp_Login
    @Username NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        u.Id,
        u.UserName,
        u.Email,
        r.[Name] AS RoleName
    FROM dbo.AspNetUsers u
    LEFT JOIN dbo.AspNetUserRoles ur ON u.Id = ur.UserId
    LEFT JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id
    WHERE u.NormalizedUserName = UPPER(@Username);
END;
GO

CREATE PROCEDURE dbo.sp_Logout
    @UserId NVARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        @UserId AS LoggedOutUserId,
        N'ÄÄƒng xuáº¥t thÃ nh cÃ´ng (mÃ´ phá»ng á»Ÿ táº§ng SQL).' AS Message;
END;
GO

/* =========================================================
   9) PROCEDURE - ENROLL / CANCEL / MY COURSES / SEARCH / SUMMARY
   ========================================================= */
CREATE PROCEDURE dbo.sp_EnrollCourse
    @UserId NVARCHAR(450),
    @CourseId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.AspNetUserRoles ur
        INNER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id
        WHERE ur.UserId = @UserId
          AND r.NormalizedName = N'STUDENT'
    )
    BEGIN
        RAISERROR (N'Chá»‰ STUDENT Ä‘Æ°á»£c phÃ©p Ä‘Äƒng kÃ½ há»c pháº§n.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Courses WHERE Id = @CourseId)
    BEGIN
        RAISERROR (N'Há»c pháº§n khÃ´ng tá»“n táº¡i.', 16, 1);
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM dbo.Enrollments
        WHERE UserId = @UserId AND CourseId = @CourseId
    )
    BEGIN
        RAISERROR (N'Sinh viÃªn Ä‘Ã£ Ä‘Äƒng kÃ½ há»c pháº§n nÃ y rá»“i.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.Enrollments (UserId, CourseId, EnrollDate)
    VALUES (@UserId, @CourseId, GETDATE());
END;
GO

CREATE PROCEDURE dbo.sp_CancelEnrollment
    @UserId NVARCHAR(450),
    @CourseId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Enrollments
    WHERE UserId = @UserId
      AND CourseId = @CourseId;
END;
GO

CREATE PROCEDURE dbo.sp_GetMyCourses
    @UserId NVARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.Id AS EnrollmentId,
        c.Id AS CourseId,
        c.[Name] AS CourseName,
        c.Credits,
        c.Lecturer,
        c.Image,
        cat.[Name] AS CategoryName,
        e.EnrollDate
    FROM dbo.Enrollments e
    INNER JOIN dbo.Courses c ON e.CourseId = c.Id
    INNER JOIN dbo.Categories cat ON c.CategoryId = cat.Id
    WHERE e.UserId = @UserId
    ORDER BY e.EnrollDate DESC, c.[Name];
END;
GO

CREATE PROCEDURE dbo.sp_SearchCourses
    @Keyword NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.fn_SearchCourses(@Keyword)
    ORDER BY Id;
END;
GO

CREATE PROCEDURE dbo.sp_CourseEnrollmentSummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.vw_CourseEnrollmentCount
    ORDER BY TotalEnrollments DESC, CourseId;
END;
GO

/* =========================================================
   10) QUICK CHECK
   ========================================================= */
SELECT
    (SELECT COUNT(*) FROM dbo.AspNetRoles)      AS RolesCount,
    (SELECT COUNT(*) FROM dbo.AspNetUsers)      AS UsersCount,
    (SELECT COUNT(*) FROM dbo.AspNetUserRoles)  AS UserRolesCount,
    (SELECT COUNT(*) FROM dbo.Categories)       AS CategoriesCount,
    (SELECT COUNT(*) FROM dbo.Courses)          AS CoursesCount,
    (SELECT COUNT(*) FROM dbo.Enrollments)      AS EnrollmentsCount;
GO

