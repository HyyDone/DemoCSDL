CREATE DATABASE KaraokeWeb;
USE KaraokeWeb;

INSERT INTO RoomsTypes (MaLoaiPhong, LoaiPhong) VALUES
('VIP34', 'Phòng VIP 3-4'),
('VIP7',  'Phòng VIP 7'),
('VIP19', 'Phòng VIP 1-9'),
('VIPVV', 'Phòng VIP VV');

INSERT INTO Rooms (MaPhong, TenPhong, MaLoaiPhong, ImageUrl, Gia, SucChua, TinhTrang) VALUES
-- Phòng VIP 3-4 (400.000?/gi?, 12 ng??i)
('VIP34_1', 'Phòng 3-4 01', 'VIP34', '/ImagesRoom/room1_vip34.jpg', 400000, 12, 0),
('VIP34_2', 'Phòng 3-4 02', 'VIP34', '/ImagesRoom/room2_vip34.jpg', 400000, 12, 0),
('VIP34_3', 'Phòng 3-4 03', 'VIP34', '/ImagesRoom/room3_vip34.jpg', 400000, 12, 0),
('VIP34_4', 'Phòng 3-4 04', 'VIP34', '/ImagesRoom/room4_vip34.jpg', 400000, 12, 0),
('VIP34_5', 'Phòng 3-4 05', 'VIP34', '/ImagesRoom/room5_vip34.jpg', 400000, 12, 0),

-- Phòng VIP 7 (600.000?/gi?, 25 ng??i)
('VIP7_1', 'Phòng 7 01', 'VIP7', '/ImagesRoom/room1_vip7.jpg', 600000, 25, 0),
('VIP7_2', 'Phòng 7 02', 'VIP7', '/ImagesRoom/room2_vip7.jpg', 600000, 25, 0),
('VIP7_3', 'Phòng 7 03', 'VIP7', '/ImagesRoom/room3_vip7.jpg', 600000, 25, 0),
('VIP7_4', 'Phòng 7 04', 'VIP7', '/ImagesRoom/room4_vip7.jpg', 600000, 25, 0),
('VIP7_5', 'Phòng 7 05', 'VIP7', '/ImagesRoom/room5_vip7.jpg', 600000, 25, 0),

-- Phòng VIP 1-9 (500.000?/gi?, 12 ng??i)
('VIP19_1', 'Phòng 1-9 01', 'VIP19', '/ImagesRoom/room1_vip9.jpg', 500000, 12, 0),
('VIP19_2', 'Phòng 1-9 02', 'VIP19', '/ImagesRoom/room2_vip9.jpg', 500000, 12, 0),
('VIP19_3', 'Phòng 1-9 03', 'VIP19', '/ImagesRoom/room3_vip9.jpg', 500000, 12, 0),
('VIP19_4', 'Phòng 1-9 04', 'VIP19', '/ImagesRoom/room4_vip9.jpg', 500000, 12, 0),
('VIP19_5', 'Phòng 1-9 05', 'VIP19', '/ImagesRoom/room5_vip9.jpg', 500000, 12, 0),

-- Phòng VIP VV (700.000?/gi?, 30 ng??i)
('VIPVV_1', 'Phòng VV 01', 'VIPVV', '/ImagesRoom/room1_vipvv.jpg', 700000, 30, 0),
('VIPVV_2', 'Phòng VV 02', 'VIPVV', '/ImagesRoom/room2_vipvv.jpg', 700000, 30, 0),
('VIPVV_3', 'Phòng VV 03', 'VIPVV', '/ImagesRoom/room3_vipvv.jpg', 700000, 30, 0),
('VIPVV_4', 'Phòng VV 04', 'VIPVV', '/ImagesRoom/room4_vipvv.jpg', 700000, 30, 0),
('VIPVV_5', 'Phòng VV 05', 'VIPVV', '/ImagesRoom/room5_vipvv.jpg', 700000, 30, 0);

INSERT INTO RoomsImages (Url, MaPhong) VALUES
-- Phòng VIP 3-4
('/ImagesRoom/room1_vip34.jpg', 'VIP34_1'),
('/ImagesRoom/room2_vip34.jpg', 'VIP34_2'),
('/ImagesRoom/room3_vip34.jpg', 'VIP34_3'),
('/ImagesRoom/room4_vip34.jpg', 'VIP34_4'),
('/ImagesRoom/room5_vip34.jpg', 'VIP34_5'),

-- Phòng VIP 7
('/ImagesRoom/room1_vip7.jpg', 'VIP7_1'),
('/ImagesRoom/room2_vip7.jpg', 'VIP7_2'),
('/ImagesRoom/room3_vip7.jpg', 'VIP7_3'),
('/ImagesRoom/room4_vip7.jpg', 'VIP7_4'),
('/ImagesRoom/room5_vip7.jpg', 'VIP7_5'),

-- Phòng VIP 1-9
('/ImagesRoom/room1_vip9.jpg', 'VIP19_1'),
('/ImagesRoom/room2_vip9.jpg', 'VIP19_2'),
('/ImagesRoom/room3_vip9.jpg', 'VIP19_3'),
('/ImagesRoom/room4_vip9.jpg', 'VIP19_4'),
('/ImagesRoom/room5_vip9.jpg', 'VIP19_5'),

-- Phòng VIP VV
('/ImagesRoom/room1_vipvv.jpg', 'VIPVV_1'),
('/ImagesRoom/room2_vipvv.jpg', 'VIPVV_2'),
('/ImagesRoom/room3_vipvv.jpg', 'VIPVV_3'),
('/ImagesRoom/room4_vipvv.jpg', 'VIPVV_4'),
('/ImagesRoom/room5_vipvv.jpg', 'VIPVV_5');

DECLARE @sql NVARCHAR(MAX) = N'';

-- Xóa t?t c? các ràng bu?c khóa ngo?i tr??c
SELECT @sql += 'ALTER TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + '];' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'FOREIGN KEY';

-- Th?c thi câu l?nh xóa khóa ngo?i
EXEC sp_executesql @sql;

-- Xóa t?t c? các b?ng
SET @sql = '';
SELECT @sql += 'DROP TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '];' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

-- Th?c thi câu l?nh xóa b?ng
EXEC sp_executesql @sql;