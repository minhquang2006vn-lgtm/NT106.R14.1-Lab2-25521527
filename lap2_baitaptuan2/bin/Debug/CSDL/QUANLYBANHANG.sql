CREATE DATABASE QLBH
USE QLBH

CREATE TABLE KHACHHANG(
MAKH CHAR(4),
HOTEN VARCHAR(40),
DCHI  VARCHAR(50),
SODT VARCHAR(20),
NGSINH SMALLDATETIME,
DOANHNHSO MONEY,
NGDK SMALLDATETIME,
CONSTRAINT PK_MAKH PRIMARY KEY (MAKH),
)

CREATE TABLE NHANVIEN(
MANV CHAR(4),
HOTEN VARCHAR(40),
NGVL SMALLDATETIME,
SODT VARCHAR(20),
CONSTRAINT PK_MANV PRIMARY KEY (MANV),
)

CREATE TABLE SANPHAM(
MASP CHAR(4),
TENSP VARCHAR (40),
DVT VARCHAR(20),
NUOCSX VARCHAR(40),
GIA MONEY,
CONSTRAINT PK_MASP PRIMARY KEY (MASP),
)

CREATE TABLE HOADON(
SOHD INT,
NGHD SMALLDATETIME ,
MAKH CHAR(4) REFERENCES KHACHHANG (MAKH),
MANV CHAR(4) REFERENCES NHANVIEN (MANV),
TRIGIA MONEY,
CONSTRAINT PK_SOHD PRIMARY KEY (SOHD),
)

CREATE TABLE CTHD(
SOHD INT REFERENCES HOADON (SOHD),
MASP CHAR(4) REFERENCES SANPHAM(MASP),
SL INT,
CONSTRAINT PK_CTHD PRIMARY KEY (SOHD,MASP),
)

-- câu 2:thêm vào thuộc tính ghi chú cho quan hệ SANPHAM
ALTER TABLE SANPHAM
ADD GHICHU VARCHAR(20)

-- câu 3: thêm vào thuộc tính LOAIKH cho quan hệ KHACHHANG
ALTER TABLE KHACHHANG
ADD LOAIKH TINYINT

--câU 4: sửa kiểu dữ liệu thuộc tính GHICHU => varchar(100)
ALTER TABLE SANPHAM
ALTER COLUMN GHICHU VARCHAR(100)

--câu 5: Xóa  thuộc tính GHICHU
ALTER TABLE SANPHAM
DROP COLUMN GHICHU

-- câu 6:
--chuyển kiểu dữ liệu của LOAIKHACH hàng để lưu chuỗi
ALTER TABLE KHACHHANG
ALTER COLUMN LOAIKH VARCHAR(20)

-- sau đó thêm ràng buộc để có thể lưu các giá trị 'Vang lai', 'Thuong xuyen', 'Vip'..
ALTER TABLE KHACHHANG
ADD CONSTRAINT KIEMTRA_LOAIKH
CHECK (LOAIKH IN ('Vang lai', 'Thuong xuyen', 'Vip'))

--câu7: thêm đơn vị tính cho sản phẩm'
ALTER TABLE SANPHAM
ADD CONSTRAINT KIEMTRA_DVT
CHECK (DVT IN('cay', 'hop', 'cai', 'quyen', 'chuc'))

--câu 8: giá bán của sản phẩm từ 500đ trở lên
ALTER TABLE SANPHAM
ADD CONSTRAINT KIEMTRA_GIA
CHECK(GIA >=500)

--câu 9: khách hàng phải mua ít nhất 1 sản phẩm
ALTER TABLE CTHD
ADD CONSTRAINT KIEMTRA_SL
CHECK(SL >=1)

--câu 10: 
ALTER TABLE KHACHHANG
ADD CONSTRAINT KIEMTRA_NGDK_NGSINH
CHECK (NGDK > NGSINH)


INSERT INTO NHANVIEN (MANV, HOTEN, NGVL, SODT) VALUES
	 ('NV01', 'Nguyen Nhu Nhut', '2006-04-13', '0927345678'),
	 ('NV02', 'Le Thi Phi Yen', '2006-04-21', '0987567390'),
	 ('NV03', 'Nguyen Van B', '2006-04-27', '0997047382'),
	 ('NV04', 'Ngo Thanh Tuan', '2006-06-24', '0913758498'),
	 ('NV05', 'Nguyen Thi Truc Thanh', '2006-07-20', '0918590387');

INSERT INTO KHACHHANG (MAKH, HOTEN, DCHI, SODT, NGSINH, DOANHNHSO, NGDK) VALUES
	 ('KH01', 'Nguyen Van A', '731 Tran Hung Dao, Q5, TpHCM', '08823451', '1960-10-22', 13060000.00, '2006-07-22'),
	 ('KH02', 'Tran Ngoc Han', '23/5 Nguyen Trai, Q5, TpHCM', '0908256478', '1974-04-03', 280000.00, '2006-07-30'),
	 ('KH03', 'Tran Ngoc Linh', '45 Nguyen Canh Chan, Q1, TpHCM', '0938776266', '1980-06-12', 3860000.00, '2006-08-05'),
	 ('KH04', 'Tran Minh Long', '50/34 Le Dai Hanh, Q10, TpHCM', '0917325476', '1965-03-09', 250000.00, '2006-10-02'),
	 ('KH05', 'Le Nhat Minh', '34 Truong Dinh, Q3, TpHCM', '08246108', '1950-03-10', 21000.00, '2006-10-28'),
	 ('KH06', 'Le Hoai Thuong', '227 Nguyen Van Cu, Q5, TpHCM', '08631738', '1981-12-31', 915000.00, '2006-11-24'),
	 ('KH07', 'Nguyen Van Tam', '32/3 Tran Binh Trong, Q5, TpHCM', '0916783565', '1971-04-06', 12500.00, '2006-12-01'),
	 ('KH08', 'Phan Thi Thanh', '45/2 An Duong Vuong, Q5, TpHCM', '0938435756', '1971-01-10', 365000.00, '2006-12-13'),
	 ('KH09', 'Le Ha Vinh', '873 Le Hong Phong, Q5, TpHCM', '08654763', '1979-09-03', 70000.00, '2007-01-14'),
	 ('KH10', 'Ha Duy Lap', '34/34B Nguyen Trai, Q1, TpHCM', '08768904', '1983-05-02', 67500.00, '2007-01-16');

INSERT INTO SANPHAM (MASP, TENSP, DVT, NUOCSX, GIA) VALUES
	 ('BB01', 'But bi', 'cay', 'Viet Nam', 5000.00),
	 ('BB02', 'But bi', 'cay', 'Trung Quoc', 7000.00),
	 ('BB03', 'But bi', 'hop', 'Thai Lan', 100000.00),
	 ('BC01', 'But chi', 'cay', 'Singapore', 3000.00),
	 ('BC02', 'But chi', 'cay', 'Singapore', 5000.00),
	 ('BC03', 'But chi', 'cay', 'Viet Nam', 3500.00),
	 ('BC04', 'But chi', 'hop', 'Viet Nam', 30000.00),
	 ('ST01', 'So tay 500 trang', 'quyen', 'Trung Quoc', 40000.00),
	 ('ST02', 'So tay loai 1', 'quyen', 'Viet Nam', 55000.00),
	 ('ST03', 'So tay loai 2', 'quyen', 'Viet Nam', 51000.00),
	 ('ST04', 'So tay', 'quyen', 'Thai Lan', 55000.00),
	 ('ST05', 'So tay mong', 'quyen', 'Thai Lan', 20000.00),
	 ('ST06', 'Phan viet bang', 'hop', 'Viet Nam', 5000.00),
	 ('ST07', 'Phan khong bui', 'hop', 'Viet Nam', 7000.00),
	 ('ST08', 'Bong bang', 'cai', 'Viet Nam', 1000.00),
	 ('ST09', 'But long', 'cay', 'Viet Nam', 5000.00),
	 ('ST10', 'But long', 'cay', 'Trung Quoc', 7000.00),
	 ('TV01', 'Tap 100 giay mong', 'quyen', 'Trung Quoc', 2500.00),
	 ('TV02', 'Tap 200 giay mong', 'quyen', 'Trung Quoc', 4500.00),
	 ('TV03', 'Tap 100 giay tot', 'quyen', 'Viet Nam', 3000.00),
	 ('TV04', 'Tap 200 giay tot', 'quyen', 'Viet Nam', 5500.00),
	 ('TV05', 'Tap 100 trang', 'chuc', 'Viet Nam', 23000.00),
	 ('TV06', 'Tap 200 trang', 'chuc', 'Viet Nam', 53000.00),
	 ('TV07', 'Tap 100 trang', 'chuc', 'Trung Quoc', 34000.00);

INSERT INTO HOADON (SOHD, NGHD, MAKH, MANV, TRIGIA) VALUES
	 (1001, '2006-07-23', 'KH01', 'NV01', 320000.00),
	 (1002, '2006-08-12', 'KH01', 'NV02', 840000.00),
	 (1003, '2006-08-23', 'KH02', 'NV01', 100000.00),
	 (1004, '2006-09-01', 'KH02', 'NV01', 180000.00),
	 (1005, '2006-10-20', 'KH01', 'NV02', 3800000.00),
	 (1006, '2006-10-16', 'KH01', 'NV03', 2430000.00),
	 (1007, '2006-10-28', 'KH03', 'NV03', 510000.00),
	 (1008, '2006-10-28', 'KH01', 'NV03', 440000.00),
	 (1009, '2006-10-28', 'KH03', 'NV04', 200000.00),
	 (1010, '2006-11-01', 'KH01', 'NV01', 5200000.00),
	 (1011, '2006-11-04', 'KH04', 'NV03', 250000.00),
	 (1012, '2006-11-30', 'KH05', 'NV03', 21000.00),
	 (1013, '2006-12-12', 'KH06', 'NV01', 5000.00),
	 (1014, '2006-12-31', 'KH03', 'NV02', 3150000.00),
	 (1015, '2007-01-01', 'KH06', 'NV01', 910000.00),
	 (1016, '2007-01-01', 'KH07', 'NV02', 12500.00),
	 (1017, '2007-01-02', 'KH08', 'NV03', 35000.00),
	 (1018, '2007-01-13', 'KH08', 'NV03', 330000.00),
	 (1019, '2007-01-13', 'KH01', 'NV03', 30000.00),
	 (1020, '2007-01-14', 'KH09', 'NV04', 70000.00),
	 (1021, '2007-01-16', 'KH10', 'NV03', 67500.00),
	 (1022, '2007-01-16', NULL,   'NV03', 7000.00),
	 (1023, '2007-01-17', NULL,   'NV01', 330000.00);

INSERT INTO CTHD (SOHD, MASP, SL) VALUES
	 (1001, 'BC01', 5),
	 (1001, 'BC02', 10),
	 (1001, 'ST01', 5),
	 (1001, 'ST08', 10),
	 (1001, 'TV02', 10),
	 (1002, 'BB01', 20),
	 (1002, 'BB02', 20),
	 (1002, 'BC04', 20),
	 (1003, 'BB03', 10),
	 (1004, 'TV01', 20),
	 (1004, 'TV02', 10),
	 (1004, 'TV03', 10),
	 (1004, 'TV04', 10),
	 (1005, 'TV05', 50),
	 (1005, 'TV06', 50),
	 (1006, 'ST01', 30),
	 (1006, 'ST02', 10),
	 (1006, 'TV07', 20),
	 (1007, 'ST03', 10),
	 (1008, 'ST04', 8),
	 (1009, 'ST05', 10),
	 (1010, 'ST04', 50),
	 (1010, 'ST07', 50),
	 (1010, 'ST08', 100),
	 (1010, 'TV03', 100),
	 (1010, 'TV07', 50),
	 (1011, 'ST06', 50),
	 (1012, 'ST07', 3),
	 (1013, 'ST08', 5),
	 (1014, 'BB01', 50),
	 (1014, 'BB02', 100),
	 (1014, 'BC02', 80),
	 (1014, 'BC04', 60),
	 (1015, 'BB02', 30),
	 (1015, 'BB03', 7),
	 (1016, 'TV01', 5),
	 (1017, 'TV02', 1),
	 (1017, 'TV03', 1),
	 (1017, 'TV04', 5),
	 (1018, 'ST04', 6),
	 (1019, 'ST05', 1),
	 (1019, 'ST06', 2),
	 (1020, 'ST07', 10),
	 (1021, 'ST08', 5),
	 (1021, 'TV01', 7),
	 (1021, 'TV02', 10),
	 (1022, 'ST07', 1),
	 (1023, 'ST04', 6);
