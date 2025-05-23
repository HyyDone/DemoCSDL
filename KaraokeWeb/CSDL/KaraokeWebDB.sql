CREATE DATABASE KaraokeWeb;
USE KaraokeWeb;

INSERT INTO RoomsTypes (MaLoaiPhong, LoaiPhong) VALUES
('VIP34', 'Ph�ng VIP 3-4'),
('VIP7',  'Ph�ng VIP 7'),
('VIP19', 'Ph�ng VIP 1-9'),
('VIPVV', 'Ph�ng VIP VV');

INSERT INTO Rooms (MaPhong, TenPhong, MaLoaiPhong, ImageUrl, Gia, SucChua, TinhTrang) VALUES
-- Ph�ng VIP 3-4 (400.000?/gi?, 12 ng??i)
('VIP34_1', 'Ph�ng 3-4 01', 'VIP34', '/ImagesRoom/room1_vip34.jpg', 400000, 12, 0),
('VIP34_2', 'Ph�ng 3-4 02', 'VIP34', '/ImagesRoom/room2_vip34.jpg', 400000, 12, 0),
('VIP34_3', 'Ph�ng 3-4 03', 'VIP34', '/ImagesRoom/room3_vip34.jpg', 400000, 12, 0),
('VIP34_4', 'Ph�ng 3-4 04', 'VIP34', '/ImagesRoom/room4_vip34.jpg', 400000, 12, 0),
('VIP34_5', 'Ph�ng 3-4 05', 'VIP34', '/ImagesRoom/room5_vip34.jpg', 400000, 12, 0),

-- Ph�ng VIP 7 (600.000?/gi?, 25 ng??i)
('VIP7_1', 'Ph�ng 7 01', 'VIP7', '/ImagesRoom/room1_vip7.jpg', 600000, 25, 0),
('VIP7_2', 'Ph�ng 7 02', 'VIP7', '/ImagesRoom/room2_vip7.jpg', 600000, 25, 0),
('VIP7_3', 'Ph�ng 7 03', 'VIP7', '/ImagesRoom/room3_vip7.jpg', 600000, 25, 0),
('VIP7_4', 'Ph�ng 7 04', 'VIP7', '/ImagesRoom/room4_vip7.jpg', 600000, 25, 0),
('VIP7_5', 'Ph�ng 7 05', 'VIP7', '/ImagesRoom/room5_vip7.jpg', 600000, 25, 0),

-- Ph�ng VIP 1-9 (500.000?/gi?, 12 ng??i)
('VIP19_1', 'Ph�ng 1-9 01', 'VIP19', '/ImagesRoom/room1_vip9.jpg', 500000, 12, 0),
('VIP19_2', 'Ph�ng 1-9 02', 'VIP19', '/ImagesRoom/room2_vip9.jpg', 500000, 12, 0),
('VIP19_3', 'Ph�ng 1-9 03', 'VIP19', '/ImagesRoom/room3_vip9.jpg', 500000, 12, 0),
('VIP19_4', 'Ph�ng 1-9 04', 'VIP19', '/ImagesRoom/room4_vip9.jpg', 500000, 12, 0),
('VIP19_5', 'Ph�ng 1-9 05', 'VIP19', '/ImagesRoom/room5_vip9.jpg', 500000, 12, 0),

-- Ph�ng VIP VV (700.000?/gi?, 30 ng??i)
('VIPVV_1', 'Ph�ng VV 01', 'VIPVV', '/ImagesRoom/room1_vipvv.jpg', 700000, 30, 0),
('VIPVV_2', 'Ph�ng VV 02', 'VIPVV', '/ImagesRoom/room2_vipvv.jpg', 700000, 30, 0),
('VIPVV_3', 'Ph�ng VV 03', 'VIPVV', '/ImagesRoom/room3_vipvv.jpg', 700000, 30, 0),
('VIPVV_4', 'Ph�ng VV 04', 'VIPVV', '/ImagesRoom/room4_vipvv.jpg', 700000, 30, 0),
('VIPVV_5', 'Ph�ng VV 05', 'VIPVV', '/ImagesRoom/room5_vipvv.jpg', 700000, 30, 0);

INSERT INTO RoomsImages (Url, MaPhong) VALUES
-- Ph�ng VIP 3-4
('/ImagesRoom/room1_vip34.jpg', 'VIP34_1'),
('/ImagesRoom/room2_vip34.jpg', 'VIP34_2'),
('/ImagesRoom/room3_vip34.jpg', 'VIP34_3'),
('/ImagesRoom/room4_vip34.jpg', 'VIP34_4'),
('/ImagesRoom/room5_vip34.jpg', 'VIP34_5'),

-- Ph�ng VIP 7
('/ImagesRoom/room1_vip7.jpg', 'VIP7_1'),
('/ImagesRoom/room2_vip7.jpg', 'VIP7_2'),
('/ImagesRoom/room3_vip7.jpg', 'VIP7_3'),
('/ImagesRoom/room4_vip7.jpg', 'VIP7_4'),
('/ImagesRoom/room5_vip7.jpg', 'VIP7_5'),

-- Ph�ng VIP 1-9
('/ImagesRoom/room1_vip9.jpg', 'VIP19_1'),
('/ImagesRoom/room2_vip9.jpg', 'VIP19_2'),
('/ImagesRoom/room3_vip9.jpg', 'VIP19_3'),
('/ImagesRoom/room4_vip9.jpg', 'VIP19_4'),
('/ImagesRoom/room5_vip9.jpg', 'VIP19_5'),

-- Ph�ng VIP VV
('/ImagesRoom/room1_vipvv.jpg', 'VIPVV_1'),
('/ImagesRoom/room2_vipvv.jpg', 'VIPVV_2'),
('/ImagesRoom/room3_vipvv.jpg', 'VIPVV_3'),
('/ImagesRoom/room4_vipvv.jpg', 'VIPVV_4'),
('/ImagesRoom/room5_vipvv.jpg', 'VIPVV_5');

DECLARE @sql NVARCHAR(MAX) = N'';

-- X�a t?t c? c�c r�ng bu?c kh�a ngo?i tr??c
SELECT @sql += 'ALTER TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + '];' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'FOREIGN KEY';

-- Th?c thi c�u l?nh x�a kh�a ngo?i
EXEC sp_executesql @sql;

-- X�a t?t c? c�c b?ng
SET @sql = '';
SELECT @sql += 'DROP TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '];' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

-- Th?c thi c�u l?nh x�a b?ng
EXEC sp_executesql @sql;