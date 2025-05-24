CREATE DATABASE KaraokeWeb;
USE KaraokeWeb;

INSERT INTO RoomsTypes (MaLoaiPhong, LoaiPhong) VALUES
('VIP34', 'Phòng VIP 3-4'),
('VIP7',  'Phòng VIP 7'),
('VIP19', 'Phòng VIP 1-9'),
('VIPVV', 'Phòng VIP VV');

INSERT INTO Rooms (MaPhong, TenPhong, MaLoaiPhong, ImageUrl, Gia, SucChua, TinhTrang) VALUES
('VIP34_1', 'Phòng 3-4 01', 'VIP34', '/ImagesRoom/room1_vip34.jpg', 400000, 12, 0),
('VIP34_2', 'Phòng 3-4 02', 'VIP34', '/ImagesRoom/room2_vip34.jpg', 400000, 12, 0),
('VIP34_3', 'Phòng 3-4 03', 'VIP34', '/ImagesRoom/room3_vip34.jpg', 400000, 12, 0),
('VIP34_4', 'Phòng 3-4 04', 'VIP34', '/ImagesRoom/room4_vip34.jpg', 400000, 12, 0),
('VIP34_5', 'Phòng 3-4 05', 'VIP34', '/ImagesRoom/room5_vip34.jpg', 400000, 12, 0),

('VIP7_1', 'Phòng 7 01', 'VIP7', '/ImagesRoom/room1_vip7.jpg', 600000, 25, 0),
('VIP7_2', 'Phòng 7 02', 'VIP7', '/ImagesRoom/room2_vip7.jpg', 600000, 25, 0),
('VIP7_3', 'Phòng 7 03', 'VIP7', '/ImagesRoom/room3_vip7.jpg', 600000, 25, 0),
('VIP7_4', 'Phòng 7 04', 'VIP7', '/ImagesRoom/room4_vip7.jpg', 600000, 25, 0),
('VIP7_5', 'Phòng 7 05', 'VIP7', '/ImagesRoom/room5_vip7.jpg', 600000, 25, 0),

('VIP19_1', 'Phòng 1-9 01', 'VIP19', '/ImagesRoom/room1_vip9.jpg', 500000, 12, 0),
('VIP19_2', 'Phòng 1-9 02', 'VIP19', '/ImagesRoom/room2_vip9.jpg', 500000, 12, 0),
('VIP19_3', 'Phòng 1-9 03', 'VIP19', '/ImagesRoom/room3_vip9.jpg', 500000, 12, 0),
('VIP19_4', 'Phòng 1-9 04', 'VIP19', '/ImagesRoom/room4_vip9.jpg', 500000, 12, 0),
('VIP19_5', 'Phòng 1-9 05', 'VIP19', '/ImagesRoom/room5_vip9.jpg', 500000, 12, 0),

('VIPVV_1', 'Phòng VV 01', 'VIPVV', '/ImagesRoom/room1_vipvv.jpg', 700000, 30, 0),
('VIPVV_2', 'Phòng VV 02', 'VIPVV', '/ImagesRoom/room2_vipvv.jpg', 700000, 30, 0),
('VIPVV_3', 'Phòng VV 03', 'VIPVV', '/ImagesRoom/room3_vipvv.jpg', 700000, 30, 0),
('VIPVV_4', 'Phòng VV 04', 'VIPVV', '/ImagesRoom/room4_vipvv.jpg', 700000, 30, 0),
('VIPVV_5', 'Phòng VV 05', 'VIPVV', '/ImagesRoom/room5_vipvv.jpg', 700000, 30, 0);

INSERT INTO RoomsImages (Url, MaPhong) VALUES
('/ImagesRoom/room1_vip34.jpg', 'VIP34_1'),
('/ImagesRoom/room2_vip34.jpg', 'VIP34_2'),
('/ImagesRoom/room3_vip34.jpg', 'VIP34_3'),
('/ImagesRoom/room4_vip34.jpg', 'VIP34_4'),
('/ImagesRoom/room5_vip34.jpg', 'VIP34_5'),

('/ImagesRoom/room1_vip7.jpg', 'VIP7_1'),
('/ImagesRoom/room2_vip7.jpg', 'VIP7_2'),
('/ImagesRoom/room3_vip7.jpg', 'VIP7_3'),
('/ImagesRoom/room4_vip7.jpg', 'VIP7_4'),
('/ImagesRoom/room5_vip7.jpg', 'VIP7_5'),

('/ImagesRoom/room1_vip9.jpg', 'VIP19_1'),
('/ImagesRoom/room2_vip9.jpg', 'VIP19_2'),
('/ImagesRoom/room3_vip9.jpg', 'VIP19_3'),
('/ImagesRoom/room4_vip9.jpg', 'VIP19_4'),
('/ImagesRoom/room5_vip9.jpg', 'VIP19_5'),

('/ImagesRoom/room1_vipvv.jpg', 'VIPVV_1'),
('/ImagesRoom/room2_vipvv.jpg', 'VIPVV_2'),
('/ImagesRoom/room3_vipvv.jpg', 'VIPVV_3'),
('/ImagesRoom/room4_vipvv.jpg', 'VIPVV_4'),
('/ImagesRoom/room5_vipvv.jpg', 'VIPVV_5');

INSERT INTO Warehouses (MaNL, TenNL, Gia, SL, MaMon) VALUES
('NL001', N'Thịt Bò Tươi', 90000, 50, 'MON001'),     
('NL002', N'Nước Ngọt Coca', 10000, 200, 'MON002'), 
('NL003', N'Cánh Gà', 75000, 60, 'MON003'),      
('NL004', N'Hạt Dẻ Sấy', 15000, 40, 'MON004'),      
('NL005', N'Hạt Điều Rang', 25000, 35, 'MON005'),     
('NL006', N'Bia Heiniken', 17500, 100, 'MON006'),     
('NL007', N'Thịt Bò Khô', 35000, 30, 'MON007'),       
('NL008', N'Thịt Gà Xé', 34000, 30, 'MON008'),       
('NL009', N'Mực Khô', 40000, 20, 'MON009'),           
('NL010', N'Mít Chín', 20000, 50, 'MON010'),         
('NL011', N'Mực Tươi Khô', 70000, 25, 'MON011'),      
('NL012', N'Nước Suối Đóng Chai', 7500, 300, 'MON012'),
('NL013', N'Pepsi Lon', 10000, 150, 'MON013'),       
('NL014', N'Nước Tăng Lực Redbull', 12500, 100, 'MON014'),
('NL015', N'Bia Sài Gòn Xanh', 15000, 100, 'MON015'), 
('NL016', N'Snack Tổng Hợp', 15000, 100, 'MON016'),    
('NL017', N'Nước Tăng Lực Sting', 10000, 120, 'MON017'),  
('NL018', N'Strongbow Lon', 17500, 80, 'MON018'),      
('NL019', N'Bia Tiger Lon', 16000, 90, 'MON019'),     
('NL020', N'Trái Cây Tươi', 75000, 30, 'MON020');    

INSERT INTO Menus (MaMon, TenMon, MoTa, Gia, LoaiMon, ImageUrl, MaNL) VALUES
('MON001', N'Bò Lúc Lắc', N'Bò lúc lắc – Mềm ngon từng miếng, đậm đà từng vị!', 180000, 'Food', 'ImageFood/boluclac.jpg', 'NL001'),
('MON002', N'Coca', N'Coca - Uống cảm xúc, thưởng vị Coca-Cola', 20000, 'Beverage', 'ImageFood/coca.jpg', 'NL002'),
('MON003', N'Gà Chiên Mắm', N'Gà chiên mắm – Ăn một lần, nhớ cả đời!', 150000, 'Food', 'ImageFood/gachienmam.jpg', 'NL003'),
('MON004', N'Hạt Dẻ', N'Hạt dẻ – Ngọt bùi từ thiên nhiên ban tặng!', 30000, 'Food', 'ImageFood/hatde.jpg', 'NL004'),
('MON005', N'Hạt Điều', N'Hạt Điều - Tự Nhiên là Ngon!', 50000, 'Food', 'ImageFood/hatdieu.jpg', 'NL005'),
('MON006', N'Heiniken', N'Heiniken - Cho đời thêm vui!', 35000, 'Beverage', 'ImageFood/heiniken.jpg', 'NL006'),
('MON007', N'Khô Bò Sấy', N'Khô bò sấy – Cay đúng điệu, ngon đúng gu!', 70000, 'Food', 'ImageFood/khobosay.jpg', 'NL007'),
('MON008', N'Khô Gà Sấy', N'Khô gà sấy - Thèm là xé, ngon là hết!', 68000, 'Food', 'ImageFood/khogasay.jpg', 'NL008'),
('MON009', N'Khô Mực Nướng', N'Khô mực nướng - Mực ngon nướng tới, Là quên cả lối về!', 80000, 'Food', 'ImageFood/khomucnuong.jpg', 'NL009'),
('MON010', N'Mít Sấy', N'Mít chín tự nhiên, sấy giòn thơm, ngọt bùi khó cưỡng!', 40000, 'Food', 'ImageFood/mitsay.jpg', 'NL010'),
('MON011', N'Mực Nướng', N'Mực khô nướng thơm lừng, dai ngọt, cực hợp nhậu!', 140000, 'Food', 'ImageFood/mucnuong.jpg', 'NL011'),
('MON012', N'Nước Suối', N'Nước suối tinh khiết – Giải khát nhẹ nhàng mỗi ngày!', 15000, 'Beverage', 'ImageFood/nuocsuoi.jpg', 'NL012'),
('MON013', N'Pepsi', N'Pepsi mát lạnh – Bùng nổ vị cola sảng khoái!', 20000, 'Beverage', 'ImageFood/pepsi.jpg', 'NL013'),
('MON014', N'Redbull', N'Redbull – Tăng lực bền bỉ, tỉnh táo mỗi ngày!', 25000, 'Beverage', 'ImageFood/redbull.jpg', 'NL014'),
('MON015', N'Sài Gòn Xanh', N'Bia Sài Gòn Xanh – Vị nhẹ dễ uống, chuẩn gu Việt!', 30000, 'Beverage', 'ImageFood/saigonxanh.jpg', 'NL015'),
('MON016', N'Snack', N'Snack giòn tan, đa vị – Ăn là ghiền!', 30000, 'Food', 'ImageFood/snack.jpg', 'NL016'),
('MON017', N'Sting', N'Sting dâu đỏ – Năng lượng bật tung ngày mới!', 20000, 'Beverage', 'ImageFood/sting.jpg', 'NL017'),
('MON018', N'Strongbow', N'Thức uống táo lên men nhẹ – Ngon mát, hợp chill!', 35000, 'Beverage', 'ImageFood/strongbow.jpg', 'NL018'),
('MON019', N'Tiger Lon', N'Bia Tiger lon – Đậm vị bia, sảng khoái đậm chất!', 32000, 'Beverage', 'ImageFood/tigerlon.jpg', 'NL019'),
('MON020', N'Trái Cây', N'Trái cây tươi ngon, đầy màu sắc – Bổ dưỡng & mát lành!', 150000, 'Food', 'ImageFood/traicay.jpg', 'NL020');

 












DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += 'ALTER TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + '];' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'FOREIGN KEY';

EXEC sp_executesql @sql;

SET @sql = '';
SELECT @sql += 'DROP TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '];' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

EXEC sp_executesql @sql;